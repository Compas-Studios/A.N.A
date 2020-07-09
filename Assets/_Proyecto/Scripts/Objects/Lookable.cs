using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lookable : MonoBehaviour
{
    public int _priority = 1;
    public Transform _trans;

    private void Awake() 
    {
        _trans = transform;
    }
}
