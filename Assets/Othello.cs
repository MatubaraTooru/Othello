using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Othello : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup = null;
    private int _rows = 8; // �s
    private int _columns = 8; // ��
    [SerializeField]
    private Cell _cellPrefab = null;
    [SerializeField]
    private GameObject _stonePrefab = null;
    private Dictionary<string, Cell> _cells = new Dictionary<string, Cell>();
    [SerializeField]
    private StoneColor _nowTurnColor = StoneColor.None; // ���݂̃^�[���̐΂̐F�������t���O�i�����l��None)
    List<Stack<Tuple<char, char>>> reverseList = new List<Stack<Tuple<char, char>>>();


    void Start()
    {
        // �O���b�h�̐ݒ�
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = _columns;
        var parent = _gridLayoutGroup.gameObject.transform;

        // �I�Z���Ղ�������
        for (var r = 'A'; r < 'A' + _rows; r++)
        {
            for (var c = 1; c <= 8; c++)
            {
                var address = $"{r}{c}";
                Cell cell = Instantiate(_cellPrefab);
                cell.transform.SetParent(parent);
                cell.name = address;
                cell.Color = StoneColor.None;
                _cells.Add(address, cell);
                var stone = Instantiate(_stonePrefab, cell.transform);
                stone.SetActive(false);

                // �����z�u�̐ݒ�i������4�̐΂�u���j
                if (address == "D4" || address == "E5") // ��
                {
                    stone.SetActive(true);
                    cell.Color = StoneColor.White;
                }
                else if (address == "D5" || address == "E4") // ��
                {
                    stone.SetActive(true);
                    stone.transform.rotation = Quaternion.Euler(180, 0, 0);
                    cell.Color = StoneColor.Black;
                }
            }
        }

        _nowTurnColor = StoneColor.Black; // �ŏ��̃^�[���͍��΂���n�܂�܂�
        SearchCells(); // �΂�u����Z�����������A�n�C���C�g����
    }

    private void Update()
    {
        // �Q�[���̐i�s�󋵂��Ǘ����邽�߂̏����i�Q�[�����W�b�N��ǉ��j
        // �΂�u���^�C�~���O�⃋�[���������ɒǉ�����
    }

    private void SearchCells()
    {
        for (var r = 'A'; r <= 'H'; r++)
        {
            for (var c = 1; c <= 8; c++)
            {
                var currentCell = _cells[$"{r}{c}"];

                if (currentCell.Color == StoneColor.None)
                {
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            if (dx == 0 && dy == 0) continue; // ���݂̃Z���͏��O

                            // ���݂̕����ɑ΂��āA����̐΂����ނ��ǂ����𔻒�
                            SearchCell((char)(r + dx), (char)(c + dy), dx, dy);
                        }
                    }
                }
            }
        }
    }

    bool SearchCell(char row, char column, int x, int y)
    {
        if (!_cells.ContainsKey($"{row}{column}"))
            return false;
        if (_cells[$"{row}{column}"].Color != StoneColor.None && _cells[$"{row}{column}"].Color != _nowTurnColor)
        {
            return SearchCell((char)(row + x), (char)(column + y), x, y);
        }
        else if (_cells[$"{row}{column}"].Color == _nowTurnColor)
        {
            return true;
        }

        return false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // �v���C���[�̃N���b�N���͂��������邽�߂̃��\�b�h
        var targetCell = eventData.pointerCurrentRaycast.gameObject.GetComponent<Cell>();

        // �i�����ɃN���b�N���ꂽ�Z���ɑ΂��鏈����ǉ�����j
    }
}

public enum StoneColor
{
    None = 0, // �΂��u����Ă��Ȃ����
    White = 1, // ����
    Black = 2 // ����
}
