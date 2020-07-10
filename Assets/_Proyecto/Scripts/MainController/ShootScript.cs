using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class ShootScript : MonoBehaviour, IDamageable
{
    [SerializeField] float MaxHealth = 100;
    public Image healthImg, bckgndhealth, allHealth;
    public Transform puntoSpawn = default;
    public float shootDelay = 0.5f;

    float health;
    ObjectPooler miPool = default;
    bool isShooting = false, canShoot = true;
    WaitForSeconds seconds;

    private void Start() 
    {
        miPool = ObjectPooler.SharedInstance;
        seconds = new WaitForSeconds(shootDelay);
        health = MaxHealth;
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

    public void TakeDmg() 
    {
        
        health -= 10;
        if (health <= 0) {
            health = 0;
            Die();
        }
            
        float convertedvalue = health / MaxHealth;

        if (allHealth != null) 
        {
            healthImg.DOFillAmount(convertedvalue, 0.1f).SetEase(Ease.InOutExpo);
            allHealth.rectTransform.DOShakePosition(0.2f, 10f, 5, 90,false,false);
            bckgndhealth.DOFillAmount(convertedvalue, 1f).SetEase(Ease.InOutExpo);
        }
    }

    private void Die() {
        //throw new NotImplementedException();
    }
}
