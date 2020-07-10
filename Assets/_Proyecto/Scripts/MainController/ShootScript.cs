using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    ObjectPooler miPool = default;
    public Transform puntoSpawn = default;
    public float shootDelay = 0.5f;
    bool isShooting = false, canShoot = true;
    WaitForSeconds seconds;

    private void Start() {
        miPool = ObjectPooler.SharedInstance;
        seconds = new WaitForSeconds(shootDelay);
    }

    public void HoldDisparo() => isShooting = true;

    public void ReleaseDisparo() => isShooting = false;

    private void Update() 
    {
        if (isShooting)
            Disparar();
    }

    void Disparar() 
    {
        if (canShoot)    
        {
            canShoot = false;
            Bullet bullet = miPool.GetPooledObject("bullet").GetComponent<Bullet>();
            bullet.Disparar(puntoSpawn.position, puntoSpawn.forward, 25f);
            StartCoroutine(waitToShoot());
        }
    }

    IEnumerator waitToShoot() 
    {
        yield return seconds;
        canShoot = true;
    }
}
