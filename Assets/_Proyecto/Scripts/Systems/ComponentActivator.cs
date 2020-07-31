using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentActivator : MonoBehaviour
{
    public MonoBehaviour cosaAActivar = default;

    private void OnTriggerEnter(Collider other)
    {
        cosaAActivar.enabled = true;
    }
}
