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
    }

    void Update()
    {
        
    }
}
