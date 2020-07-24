using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_DialogTrigger : MonoBehaviour
{
    [SerializeField] Vector3 _camPos = default;
    [SerializeField] Quaternion _camRot = default;
    [SerializeField] bool show3D = false;
    bool hasPlayer = false;

    private void OnDrawGizmos()
    {
        if (show3D)
        {
            Gizmos.DrawWireSphere(_camPos, 1);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_camPos, _camRot * Vector3.forward);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        hasPlayer = other.CompareTag("Player"); //activamos si es el player
    }

    private void OnTriggerExit(Collider other)
    {
        hasPlayer = !other.CompareTag("Player"); //sino pos no nmms
    }
}
