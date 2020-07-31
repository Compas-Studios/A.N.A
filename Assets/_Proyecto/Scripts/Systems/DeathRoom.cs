using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRoom : MonoBehaviour
{
    [SerializeField] int enemigos = 5;
    [SerializeField] Vector3 roomSize = default;
    [SerializeField] GameObject enemiePref = default;
    [SerializeField] Collider[] barreras = default;
    Collider miTrigger = default;
    List<Lookable> enemi = default;

    private void Awake()
    {
        miTrigger = GetComponent<Collider>();
        enemi = new List<Lookable>();
    }

    private void Start()
    {
        foreach(var b in barreras)
            b.enabled = false;
    }

    public void Activar(PlayerMovement pm)
    {
        foreach (var b in barreras)
            b.enabled = true;

        for (int  i = 0;  i < enemigos;  i++)
        {
            Vector3 pos = transform.position;
            //pos *= roomSize.magnitude;
            pos += UnityEngine.Random.insideUnitSphere;
            GameObject go = Instantiate(enemiePref, pos, Quaternion.identity);
            go.GetComponent<basicThug>().mideathRoom = this;
            enemi.Add(go.GetComponent<Lookable>());
        }

        pm.updateLookables(enemi);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            miTrigger.enabled = false;
            Activar(other.GetComponent<PlayerMovement>());
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, roomSize);
    }

    internal void EnemigoMuerto()
    {
        enemigos--;
        if (enemigos <= 0)
        {
            foreach (var b in barreras)
            {
                b.enabled = false;
            }
            Destroy(gameObject);
        }
    }
}
