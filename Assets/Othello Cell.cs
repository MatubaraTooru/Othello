using System.Collections.Generic;
using UnityEngine;

public class OthelloCell : MonoBehaviour
{
    private StoneColor _stoneColor = StoneColor.None; // セルに置かれた石の色
    public StoneColor StoneColor 
    {
        get => _stoneColor; 
        set => _stoneColor = value; 
    } // プロパティで石の色を取得・設定

    [SerializeField]
    private GameObject _canPlaceMarker = null; // 石が置けることを示すマーカーオブジェクト
    [SerializeField]
    private GameObject _stoneObject = null; // セルに置かれる石のオブジェクト

    private List<string> _flippableCellIdentifiers = new List<string>();

    private void Awake()
    {
        _canPlaceMarker.SetActive(false); // 初期状態では石が置けることを示すマーカーは非表示
        _stoneObject.SetActive(false); // 初期状態ではセルに石は置かれていないため非表示
    }

    void Update()
    {
        // 何か特定の処理を行う場合にここにコードを追加
    }

    public void ReverseStone()
    {
        if (_stoneColor == StoneColor.Black)
            _stoneObject.transform.rotation = Quaternion.Euler(90, 0, 0); // 石を裏返す処理
        else if (_stoneColor == StoneColor.White)
            _stoneObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
    }

    public void Highlight()
    {
        _canPlaceMarker.SetActive(true); // 石が置けることを示すマーカーを表示
    }
    public void HideMarker()
    {
        _canPlaceMarker.SetActive(false);
    }

    public void PlaceStoneOnCell()
    {
        _stoneObject.SetActive(true); // セルに石を置くとき、石のオブジェクトを表示

        if (_stoneColor == StoneColor.Black)
        {
            _stoneObject.transform.rotation = Quaternion.Euler(90, 0, 0); // 黒石を置いた場合、石を裏返す
        }
        if (_canPlaceMarker.activeSelf == true)
        {
            _canPlaceMarker.SetActive(false);
        }
    }


    // フリップ可能なセル情報を追加
    public void AddFlippableCell(string cellIdentifier)
    {
        _flippableCellIdentifiers.Add(cellIdentifier);
    }

    // フリップ可能なセル情報を取得
    public List<string> GetFlippableCells()
    {
        return _flippableCellIdentifiers;
    }
}
