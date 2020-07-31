using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ParticulaSeguirJugador : MonoBehaviour
{
    Transform bar;
    [SerializeField] Vector3 offset = default;
    void Start()
    {
        bar = GameObject.Find("Player").transform;
    }

    void Update()
    {
        transform.position = bar.position+offset;
    }
}