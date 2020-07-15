using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LouisBoss : MonoBehaviour, IDamageable
{
    [SerializeField] Vector2 limitesXY = default;
    [SerializeField] Animator _anim = default;
    [SerializeField] Transform _target = default;
    Transform _tr = default, _trPlayer = default;
    ObjectPooler _pool = default;
    bool  isAlive = true, isBusy = false;
    int estado = 0; //idle, atacando, moviendo

    private void Awake()
    {
        _tr = transform.parent.parent;
        _trPlayer = FindObjectOfType<PlayerMovement>().transform;
    }

    private void Start()
    {
        _pool = ObjectPooler.SharedInstance;
        _target.parent = null;
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
        StartCoroutine(waitTillNoBussy(4.5f));
    }

    void ChangePosition()
    {
        _anim.SetTrigger("Attack");
       // print("startingJump");
        Vector3 newPos = new Vector3(Random.Range(-limitesXY.x, limitesXY.x), _tr.position.y, Random.Range(-limitesXY.y, limitesXY.y));
        _target.DOMove(newPos, 0.2f).SetEase(Ease.OutExpo);
        _tr.DOLocalJump(newPos, 5f, 1, 0.15f, false).SetDelay(3.0f);
        StartCoroutine(waitTillNoBussy(4.5f));
    }

    void Morir()
    {
        _anim.SetTrigger("Dead");
        isAlive = false;
    }

    IEnumerator waitTillNoBussy(float _t)
    {
        yield return new WaitForSeconds(_t);
        isBusy = false;
    }

    public void TakeDmg(bool _enemie)
    {
        
    }
}
