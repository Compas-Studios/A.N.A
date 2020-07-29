using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    Transform tr = default;
    [SerializeField] float interacionRadius = 2.0f;
    [SerializeField] LayerMask lm;
    int layerindex;
    private void Awake()
    {
        tr = transform;
        layerindex = lm.value;
    }

    public void TryInteract()
    {
        Collider[] colliders = Physics.OverlapSphere(tr.position, interacionRadius, layerindex);
        if (colliders.Length > 0)
        {
            IInteractuable interactable = default;
            float closestOne = (colliders[0].transform.position - tr.position).sqrMagnitude;
            foreach (Collider c in colliders)
            {
                float distancia = (c.transform.position - tr.position).sqrMagnitude;
                if (distancia <= closestOne)//es el mas cercano actualmente
                {
                    interactable = c.GetComponent<IInteractuable>();// lo guardo en mi variable
                    closestOne = distancia;
                }
            }

            if (interactable != null)
            {
                interactable.Interactuar(this);
            }
        }
    }

}
