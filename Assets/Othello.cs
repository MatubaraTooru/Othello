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
    private int _rows = 8; // 行
    private int _columns = 8; // 列
    [SerializeField]
    private Cell _cellPrefab = null;
    [SerializeField]
    private GameObject _stonePrefab = null;
    private Dictionary<string, Cell> _cells = new Dictionary<string, Cell>();
    [SerializeField]
    private StoneColor _nowTurnColor = StoneColor.None; // 現在のターンの石の色を示すフラグ（初期値はNone)
    List<Stack<Tuple<char, char>>> reverseList = new List<Stack<Tuple<char, char>>>();


    void Start()
    {
        // グリッドの設定
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = _columns;
        var parent = _gridLayoutGroup.gameObject.transform;

        // オセロ盤を初期化
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

                // 初期配置の設定（中央に4つの石を置く）
                if (address == "D4" || address == "E5") // 白
                {
                    stone.SetActive(true);
                    cell.Color = StoneColor.White;
                }
                else if (address == "D5" || address == "E4") // 黒
                {
                    stone.SetActive(true);
                    stone.transform.rotation = Quaternion.Euler(180, 0, 0);
                    cell.Color = StoneColor.Black;
                }
            }
        }

        _nowTurnColor = StoneColor.Black; // 最初のターンは黒石から始まります
        SearchCells(); // 石を置けるセルを検索し、ハイライトする
    }

    private void Update()
    {
        // ゲームの進行状況を管理するための処理（ゲームロジックを追加）
        // 石を置くタイミングやルールをここに追加する
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
                            if (dx == 0 && dy == 0) continue; // 現在のセルは除外

                            // 現在の方向に対して、相手の石を挟むかどうかを判定
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
        // プレイヤーのクリック入力を処理するためのメソッド
        var targetCell = eventData.pointerCurrentRaycast.gameObject.GetComponent<Cell>();

        // （ここにクリックされたセルに対する処理を追加する）
    }
}

public enum StoneColor
{
    None = 0, // 石が置かれていない状態
    White = 1, // 白石
    Black = 2 // 黒石
}
