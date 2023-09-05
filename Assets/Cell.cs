using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField]
    private bool _isBlack = false;

    public bool IsBlack { get => _isBlack; set => _isBlack = !_isBlack; }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
