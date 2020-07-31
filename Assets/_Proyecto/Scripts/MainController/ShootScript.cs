using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class ShootScript : MonoBehaviour, IDamageable
{
    [SerializeField] float MaxHealth = 100;
    [SerializeField] Animator shAnim = default;
    public Image healthImg, bckgndhealth, allHealth;
    public Transform puntoSpawn = default;
    public float shootDelay = 0.5f;
    CamShake _cam = default;

    float health;
    ObjectPooler miPool = default;
    bool isShooting = false, canShoot = true, canDamage = true;
    WaitForSeconds shootSeconds, invinSeconds;

    [SerializeField] SpriteRenderer[] muzzleSpRend = default;
    [SerializeField] Sprite[] sprites = default;
    int muzzleLength, muzzleInd = 0;

    private void Awake()
    {
        muzzleLength = sprites.Length;
    }

    private void Start() 
    {
        miPool = ObjectPooler.SharedInstance;
        shootSeconds = new WaitForSeconds(shootDelay);
        invinSeconds = new WaitForSeconds(1.5f);
        health = MaxHealth;

        _cam = CamShake._staticShake;
    }

    public void HoldDisparo()
    {
        isShooting = true;
        shAnim.SetBool("shoot",isShooting);
    }

    public void ReleaseDisparo()
    {
        isShooting = false;
        shAnim.SetBool("shoot", isShooting);
        muzzleSpRend[0].sprite = muzzleSpRend[1].sprite = null;
    }

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
            muzzleInd++;
            if (muzzleInd >= muzzleLength)
                muzzleInd = 0;
            muzzleSpRend[0].sprite = muzzleSpRend[1].sprite = sprites[muzzleInd];
            Bullet bullet = miPool.GetPooledObject("bullet").GetComponent<Bullet>();
            Vector3 variacion = puntoSpawn.right * UnityEngine.Random.Range(-0.5f, 0.5f);
            bullet.Disparar(puntoSpawn.position + variacion, puntoSpawn.forward, 25f, false);
            StartCoroutine(waitToShoot());
        }
    }

    IEnumerator waitToShoot() 
    {
        yield return shootSeconds;
        canShoot = true;
    }

    IEnumerator waitToDamage()
    {
        yield return invinSeconds;
        canDamage = true;
    }

    public void TakeDmg(bool _b) 
    {
        if (!_b  || !canDamage)
            return;

        canDamage = false;
        StartCoroutine(waitToDamage());

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

        if (_cam != null) _cam.Shake(0.2f, 5.0f);
    }

    private void Die() {
        //throw new NotImplementedException();
    }
}
