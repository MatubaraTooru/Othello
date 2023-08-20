using UnityEngine;
using UnityEngine.UI;

public class Othello : MonoBehaviour
{
    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup = null;
    [SerializeField]
    private int _rows = 1;
    [SerializeField]
    private int _columns = 1;
    [SerializeField]
    private Cell _cellPrefab = null;
    private Cell[,] _cells;
    [SerializeField]
    private Material _selectedMaterial = null;
    [SerializeField]
    private Material _nonSelectedMaterial = null;
    private int _selectedRow = 0;
    private int _selectedColumn = 0;
    void Start()
    {
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = _columns;

        _cells = new Cell[_rows, _columns];
        var parent = _gridLayoutGroup.gameObject.transform;
        for (var r = 0; r < _rows; r++)
        {
            for (var c = 0; c < _columns; c++)
            {
                var cell = Instantiate(_cellPrefab);
                cell.transform.SetParent(parent);
                cell.name = $"Cell({r}, {c})";
                _cells[r, c] = cell;
            }
        }

        _cells[0, 0].GetComponentInChildren<MeshRenderer>().material = _selectedMaterial;
    }

    private void Update()
    {
        // キー入力判定
        if (Input.GetKeyDown(KeyCode.LeftArrow)) // 左キーを押した
        {
            TryMoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) // 右キーを押した
        {
            TryMoveRight();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) // 上キーを押した
        {
            TryMoveUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) // 下キーを押した
        {
            TryMoveDown();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RemoveCell();
        }
    }

    private bool TryMoveLeft() // 左に移動
    {
        for (var c = _selectedColumn - 1; c >= 0; c--)
        {
            if (TrySelectCell(_selectedRow, c)) { return true; }
        }
        return false;
    }
    private bool TryMoveRight() // 右に移動
    {
        for (var c = _selectedColumn + 1; c < _cells.GetLength(1); c++)
        {
            if (TrySelectCell(_selectedRow, c)) { return true; }
        }
        return false;
    }
    private bool TryMoveUp() // 上に移動
    {
        for (var r = _selectedRow - 1; r >= 0; r--)
        {
            if (TrySelectCell(r, _selectedColumn)) { return true; }
        }
        return false;
    }
    private bool TryMoveDown() // 下に移動
    {
        for (var r = _selectedRow + 1; r < _cells.GetLength(0); r++)
        {
            if (TrySelectCell(r, _selectedColumn)) { return true; }
        }
        return false;
    }

    private void RemoveCell() // 選択中のセルを消す
    {
        var cell = _cells[_selectedRow, _selectedColumn];
        cell.enabled = false;
    }

    private bool TrySelectCell(int row, int column) // セルを選択する
    {
        if (row < 0 || row >= _cells.GetLength(0) || column < 0 || column >= _cells.GetLength(1))
        {
            return false;
        }

        var newCell = _cells[row, column]; // 選択されるセル
        if (!newCell.enabled) { return false; } // 非表示のセルは選択できないので失敗

        var oldCell = _cells[_selectedRow, _selectedColumn]; // 選択されていたセル
        oldCell.GetComponentInChildren<MeshRenderer>().material = _nonSelectedMaterial;
        newCell.GetComponentInChildren<MeshRenderer>().material = _selectedMaterial;

        _selectedRow = row;
        _selectedColumn = column;
        return true;
    }
}
