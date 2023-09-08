using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OthelloGame : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup = null;

    private int Rows = 8; // 行
    private int Columns = 8; // 列

    [SerializeField]
    private OthelloCell _cellPrefab = null;

    private Dictionary<string, OthelloCell> _cells = new Dictionary<string, OthelloCell>();

    [SerializeField]
    private StoneColor _currentTurnColor = StoneColor.None; // 現在のターンの石の色を示すフラグ（初期値はNone)

    void Start()
    {
        // グリッドの設定
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = Columns;
        var parent = _gridLayoutGroup.gameObject.transform;

        // オセロ盤を初期化
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

                // 初期配置の設定（中央に4つの石を置く）
                if (address == "D4" || address == "E5") // 白
                {
                    cell.StoneColor = StoneColor.White;
                    cell.PlaceStoneOnCell();
                }
                else if (address == "D5" || address == "E4") // 黒
                {
                    cell.StoneColor = StoneColor.Black;
                    cell.PlaceStoneOnCell();
                }
            }
        }

        _currentTurnColor = StoneColor.Black; // 最初のターンは黒石から始まります
        SearchPlayableCells(); // 石を置けるセルを検索し、ハイライトする
    }

    private void Update()
    {
        // ゲームの進行状況を管理するための処理（ゲームロジックを追加）
        // 石を置くタイミングやルールをここに追加する
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
                            if (dx == 0 && dy == 0) continue; // 現在のセルは除外
                            // 現在の方向に対して、相手の石を挟めるかどうかを判定
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

        // リストをクリアして反転できるセルの情報をリセット
        flippableCellIdentifiers.Clear();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        // プレイヤーのクリック入力を処理するためのメソッド
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

            // ターンを切り替える
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
    None = 0, // 石が置かれていない状態
    White = 1, // 白石
    Black = 2 // 黒石
}
