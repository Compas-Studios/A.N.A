using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameObject _yo;
    Transform _tr;
    Rigidbody _rg;
    [SerializeField] float velocidad = 5.0f;

    private void Awake() 
    {
        _yo = gameObject;
        _tr = transform;
        _rg = GetComponent<Rigidbody>();
    }

    public void Disparar(Vector3 pos,Vector3 direccion) 
    {
        _tr.position = pos;
        _rg.velocity = Vector3.zero;
        _tr.rotation = Quaternion.LookRotation(direccion);
        _rg.AddForce(direccion * velocidad,ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other) 
    {
        /*if (other.CompareTag("Damageable")) 
        {
            
        }*/
        _yo.SetActive(false);
    }
}
