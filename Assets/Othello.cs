using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Othello : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup = null;
    private int _rows = 8;
    private int _columns = 8;
    [SerializeField]
    private Cell _cellPrefab = null;
    [SerializeField]
    private GameObject _stonePrefab = null;
    private Dictionary<string, Cell> _cells = new Dictionary<string, Cell>();
    [SerializeField]
    private bool _isWhite = false;
    void Start()
    {
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = _columns;
        var parent = _gridLayoutGroup.gameObject.transform;

        for (var c = 1; c <= 8; c++)
        {
            for (var r = 'A'; r <= 'H'; r++)
            {
                var address = $"{r}{c}";
                Cell cell = Instantiate(_cellPrefab);
                cell.transform.SetParent(parent);
                cell.name = address;
                _cells.Add(address, cell);
                var stone = Instantiate(_stonePrefab, cell.transform);
                stone.SetActive(false);

                if (address == "D4" || address == "E5")
                {
                    stone.SetActive(true);
                }
                else if (address == "D5" || address == "E4")
                {
                    stone.SetActive(true);
                    stone.transform.rotation = Quaternion.Euler(180, 0, 0);
                }
            }
        }
    }

    private void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var targetcell = eventData.pointerCurrentRaycast.gameObject.GetComponent<Cell>();

    }
}
enum IsTurn
{
    None = 0,
    White = 1,
    Black = 2
}
