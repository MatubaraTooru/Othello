using System.Collections.Generic;
using UnityEngine;

public class OthelloCell : MonoBehaviour
{
    private StoneColor _stoneColor = StoneColor.None; // �Z���ɒu���ꂽ�΂̐F
    public StoneColor StoneColor 
    {
        get => _stoneColor; 
        set => _stoneColor = value; 
    } // �v���p�e�B�Ő΂̐F���擾�E�ݒ�

    [SerializeField]
    private GameObject _canPlaceMarker = null; // �΂��u���邱�Ƃ������}�[�J�[�I�u�W�F�N�g
    [SerializeField]
    private GameObject _stoneObject = null; // �Z���ɒu�����΂̃I�u�W�F�N�g

    private List<string> _flippableCellIdentifiers = new List<string>();

    private void Awake()
    {
        _canPlaceMarker.SetActive(false); // ������Ԃł͐΂��u���邱�Ƃ������}�[�J�[�͔�\��
        _stoneObject.SetActive(false); // ������Ԃł̓Z���ɐ΂͒u����Ă��Ȃ����ߔ�\��
    }

    void Update()
    {
        // ��������̏������s���ꍇ�ɂ����ɃR�[�h��ǉ�
    }

    public void ReverseStone()
    {
        if (_stoneColor == StoneColor.Black)
            _stoneObject.transform.rotation = Quaternion.Euler(90, 0, 0); // �΂𗠕Ԃ�����
        else if (_stoneColor == StoneColor.White)
            _stoneObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
    }

    public void Highlight()
    {
        _canPlaceMarker.SetActive(true); // �΂��u���邱�Ƃ������}�[�J�[��\��
    }
    public void HideMarker()
    {
        _canPlaceMarker.SetActive(false);
    }

    public void PlaceStoneOnCell()
    {
        _stoneObject.SetActive(true); // �Z���ɐ΂�u���Ƃ��A�΂̃I�u�W�F�N�g��\��

        if (_stoneColor == StoneColor.Black)
        {
            _stoneObject.transform.rotation = Quaternion.Euler(90, 0, 0); // ���΂�u�����ꍇ�A�΂𗠕Ԃ�
        }
        if (_canPlaceMarker.activeSelf == true)
        {
            _canPlaceMarker.SetActive(false);
        }
    }


    // �t���b�v�\�ȃZ������ǉ�
    public void AddFlippableCell(string cellIdentifier)
    {
        _flippableCellIdentifiers.Add(cellIdentifier);
    }

    // �t���b�v�\�ȃZ�������擾
    public List<string> GetFlippableCells()
    {
        return _flippableCellIdentifiers;
    }
}
