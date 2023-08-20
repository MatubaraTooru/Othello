using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField]
    private bool _isBlack = false;

    public bool IsBlack 
    { 
        get => _isBlack; 
        set
        {
            _isBlack = value;
            _animator.SetBool("IsBlack", _isBlack);
        }

    }
    private Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }
}
