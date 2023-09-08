using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OthelloGame : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup = null;

    private int Rows = 8; // �s
    private int Columns = 8; // ��

    [SerializeField]
    private OthelloCell _cellPrefab = null;

    private Dictionary<string, OthelloCell> _cells = new Dictionary<string, OthelloCell>();

    [SerializeField]
    private StoneColor _currentTurnColor = StoneColor.None; // ���݂̃^�[���̐΂̐F�������t���O�i�����l��None)

    void Start()
    {
        // �O���b�h�̐ݒ�
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = Columns;
        var parent = _gridLayoutGroup.gameObject.transform;

        // �I�Z���Ղ�������
        for (char row = 'A'; row < 'A' + Rows; row++)
        {
            for (int col = 1; col <= 8; col++)
            {
                var address = $"{row}{col}";
                OthelloCell cell = Instantiate(_cellPrefab);
                cell.transform.SetParent(parent);
                cell.name = address;
                cell.StoneColor = StoneColor.None;
                _cells.Add(address, cell);

                // �����z�u�̐ݒ�i������4�̐΂�u���j
                if (address == "D4" || address == "E5") // ��
                {
                    cell.StoneColor = StoneColor.White;
                    cell.PlaceStoneOnCell();
                }
                else if (address == "D5" || address == "E4") // ��
                {
                    cell.StoneColor = StoneColor.Black;
                    cell.PlaceStoneOnCell();
                }
            }
        }

        _currentTurnColor = StoneColor.Black; // �ŏ��̃^�[���͍��΂���n�܂�܂�
        SearchPlayableCells(); // �΂�u����Z�����������A�n�C���C�g����
    }

    private void Update()
    {
        // �Q�[���̐i�s�󋵂��Ǘ����邽�߂̏����i�Q�[�����W�b�N��ǉ��j
        // �΂�u���^�C�~���O�⃋�[���������ɒǉ�����
    }

    private void SearchPlayableCells()
    {
        for (char row = 'A'; row <= 'H'; row++)
        {
            for (int col = 1; col <= 8; col++)
            {
                var currentCell = _cells[$"{row}{col}"];

                if (currentCell.StoneColor == StoneColor.None)
                {
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            if (dx == 0 && dy == 0) continue; // ���݂̃Z���͏��O
                            // ���݂̕����ɑ΂��āA����̐΂����߂邩�ǂ����𔻒�
                            if (IsFlippable(row, col, dx, dy, 0, $"{row}{col}"))
                            {
                                _cells[$"{row}{col}"].Highlight();
                            }
                        }
                    }
                }
            }
        }
    }

    bool IsFlippable(char row, int col, int dx, int dy, int moveCount, string originCellID)
    {
        moveCount++;
        char newRow = (char)(row + dx);
        int newCol = col + dy;

        if (!_cells.ContainsKey($"{newRow}{newCol}"))
            return false;

        if (_cells[$"{newRow}{newCol}"].StoneColor == _currentTurnColor && moveCount > 1)
            return true;
        else if (_cells[$"{newRow}{newCol}"].StoneColor == _currentTurnColor)
            return false;

        if (_cells[$"{newRow}{newCol}"].StoneColor == StoneColor.None)
            return false;

        _cells[originCellID].AddFlippableCell($"{newRow}{newCol}");
        return IsFlippable(newRow, newCol, dx, dy, moveCount, originCellID);
    }
    public void FlipCells(string id)
    {
        var flippableCellIdentifiers = _cells[id].GetFlippableCells();

        foreach (var identifier in flippableCellIdentifiers)
        {
            OthelloCell cellToFlip = _cells[identifier];
            cellToFlip.StoneColor = _currentTurnColor;
            cellToFlip.ReverseStone();
        }

        // ���X�g���N���A���Ĕ��]�ł���Z���̏������Z�b�g
        flippableCellIdentifiers.Clear();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        // �v���C���[�̃N���b�N���͂��������邽�߂̃��\�b�h
        var clickedCell = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<OthelloCell>();

        PlaceStone(clickedCell);
    }

    private void PlaceStone(OthelloCell clickedCell)
    {
        if (_cells.ContainsKey(clickedCell.name) && clickedCell.GetFlippableCells().Count >= 1)
        {
            _cells[clickedCell.name].StoneColor = _currentTurnColor;
            _cells[clickedCell.name].PlaceStoneOnCell();
            FlipCells(clickedCell.name);

            // �^�[����؂�ւ���
            if (_currentTurnColor == StoneColor.Black)
            {
                _currentTurnColor = StoneColor.White;
            }
            else if (_currentTurnColor == StoneColor.White)
            {
                _currentTurnColor = StoneColor.Black;
            }

            HideAllMarkers();
            SearchPlayableCells();
        }
    }

    private void HideAllMarkers()
    {
        foreach (var cell in _cells.Values)
        {
            // cell.GetFlippableCells().Clear();
            cell.HideMarker();
        }
    }
}

public enum StoneColor
{
    None = 0, // �΂��u����Ă��Ȃ����
    White = 1, // ����
    Black = 2 // ����
}
