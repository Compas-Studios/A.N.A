using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ParticulaSeguirJugador : MonoBehaviour
{
    Transform bar;
    void Start()
    {
        bar = GameObject.Find("Player").transform;
    }

    void Update()
    {
        transform.position = new Vector3(bar.position.x,bar.position.y, bar.position.z);
    }
}