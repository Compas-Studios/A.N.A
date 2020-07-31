using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LouisBoss : MonoBehaviour, IDamageable
{
    [SerializeField] Vector4 limitesXY = default;
    [SerializeField] Animator _anim = default;
    [SerializeField] Transform _target = default;
    [SerializeField] SpriteRenderer spRend = default;
    [SerializeField] Renderer rend = default;
    public float health = 0, maxHealth = 100;
    Transform _tr = default, _trPlayer = default;
    ObjectPooler _pool = default;
    bool  isAlive = true, isBusy = false;
    int estado = 0; //idle, atacando, moviendo
    float shootRadius = 5f;

    Image healthImg;
    MaterialPropertyBlock mtpb = default;
    int matProperty;

    private void Awake()
    {
        _tr = transform.parent.parent;
        _trPlayer = FindObjectOfType<PlayerMovement>().transform;

        mtpb = new MaterialPropertyBlock();
        matProperty = Shader.PropertyToID("_transition");
    }

    private void Start()
    {
        _pool = ObjectPooler.SharedInstance;
        _target.parent = null;
        health = maxHealth;
        CanvasGroup cg = GameObject.FindGameObjectWithTag("bgndUI").GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.DOFade(1, 1.0f);
            healthImg = cg.transform.GetChild(0).GetComponent<Image>();
        }
    }

    private void Update()
    {
        if (!isAlive)
            return;

        if (!isBusy)
        {
            estado = Random.Range(0, 3);
            switch (estado)
            {
                case 0:
                    StartCoroutine(waitTillNoBussy(1f));
                    break;
                case 1:
                    Atacar();
                    break;
                case 2:
                    ChangePosition();
                    break;
                default: StartCoroutine(waitTillNoBussy(1f)); break;
            }
            isBusy = true;
        }
        
    }

    void Atacar()
    {
        _anim.SetTrigger("Attack");
        StartCoroutine(AttackRoutine(4.0f));
        StartCoroutine(waitTillNoBussy(4.5f));
    }

    void ChangePosition()
    {
        _anim.SetTrigger("Attack");
        spRend.DOFade(1, 0.5f);
        spRend.DOFade(0, 0.5f).SetDelay(1.1f);
        Vector3 newPos = new Vector3(Random.Range(limitesXY.x, limitesXY.y), _tr.position.y, Random.Range(limitesXY.z, limitesXY.w));
        _target.DOMove(newPos, 0.2f).SetEase(Ease.OutExpo);
        _tr.DOLocalJump(newPos, 5f, 1, 0.15f, false).SetDelay(3.0f);
        StartCoroutine(waitTillNoBussy(4.5f));
    }

    void Morir()
    {
        if (isAlive)
        {
            StopAllCoroutines();
            _anim.SetTrigger("Dead");
            isAlive = false;
        }
    }

    IEnumerator waitTillNoBussy(float _t)
    {
        yield return new WaitForSeconds(_t);
        isBusy = false;
    }

    IEnumerator AttackRoutine(float _l)
    {
        float halfTime = _l / 2;
        
        yield return new WaitForSeconds(halfTime);
        Vector3 direccionPlayer = (_trPlayer.position - _tr.position);
        direccionPlayer = Vector3.ClampMagnitude(direccionPlayer, shootRadius);
        direccionPlayer.y = 0;
        Debug.DrawLine(_tr.position, _tr.position + direccionPlayer, Color.cyan, 5.0f);
        Bullet bullet;

        for (int i = 0; i < 16; i++)
        {
            bullet = _pool.GetPooledObject("bullet").GetComponent<Bullet>();
            //Debug.DrawLine(_tr.position + direccionPlayer , _tr.position + direccionPlayer.normalized , Color.magenta, 3.0f);
            bullet.Disparar(_tr.position + direccionPlayer , direccionPlayer.normalized , 12.0f, true);
            direccionPlayer = Quaternion.Euler(0,-22.5f,0) * direccionPlayer;
        }

        yield return new WaitForSeconds(halfTime);
        direccionPlayer = (_trPlayer.position - _tr.position);
        direccionPlayer = Vector3.ClampMagnitude(direccionPlayer, shootRadius);
        direccionPlayer.y = 0;
        Debug.DrawLine(_tr.position, _tr.position + direccionPlayer, Color.cyan, 5.0f);

        for (int i = 0; i < 16; i++)
        {
            bullet = _pool.GetPooledObject("bullet").GetComponent<Bullet>();
            //Debug.DrawLine(_tr.position + direccionPlayer , _tr.position + direccionPlayer.normalized , Color.magenta, 3.0f);
            bullet.Disparar(_tr.position + direccionPlayer, direccionPlayer.normalized, 16.0f, true);
            direccionPlayer = Quaternion.Euler(0, -22.5f, 0) * direccionPlayer;
        }

    }

    public void TakeDmg(bool _enemie)
    {
        DOTween.To(updateMaterial, 0.75f, 0, 0.1f);

        health -= 1;
        if (health <= 0)
        {
            Morir();
        }

        float convertedvalue = health / maxHealth;

        if (healthImg != null)
        {
            healthImg.DOFillAmount(convertedvalue, 0.01f).SetEase(Ease.InOutExpo);
        }
    }

    void updateMaterial(float _t)
    {
        mtpb.SetFloat(matProperty, _t);
        rend.SetPropertyBlock(mtpb); 
    }

    private void OnDrawGizmos()
    {
        if (_tr != null)
            Gizmos.DrawWireSphere(_tr.position, shootRadius);
    }
}
