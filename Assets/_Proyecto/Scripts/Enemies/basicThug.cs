using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicThug : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform _ShootPoint = default;
    public float moveRadius = 5.0f;
    public float moveSpeed = 3.0f;
    public float detectRadius = 5.0f;
    public float secondsToShoot = 1.0f;
    Vector3 endPos = Vector3.zero, direccion, _transPos, playerDirection;
    Transform _trans, _playerTrans;
    Rigidbody _rb;
    GameObject _gobj;
    ObjectPooler miPool;
    bool jugadorVisto = false, canShoot = true;

    WaitForSeconds seconds;

    private void Awake() 
    {
        _trans = transform;
        _playerTrans = FindObjectOfType<PlayerMovement>().transform; //Encontrar Transform del Jugador
        _gobj = gameObject;
        _rb = GetComponent<Rigidbody>();
        seconds = new WaitForSeconds(secondsToShoot);
    }

    private void Start() 
    {
        endPos = GetMovePos();
        miPool = ObjectPooler.SharedInstance;
    }

    private void Update() 
    {
        _transPos = new Vector3(_trans.position.x, 0, _trans.position.z); //posicion de transform
        playerDirection = _playerTrans.position - _transPos; //direccion hacia el jugador

        if (!jugadorVisto && playerDirection.sqrMagnitude < detectRadius * detectRadius) //si esta en radio de deteccion
            jugadorVisto = true; //detectao
        
        if (!jugadorVisto) 
            MoveBehaviour();
        else 
            Atacar();
    }

    private void Atacar() 
    {
        if (playerDirection.sqrMagnitude > (detectRadius * detectRadius) * 1.2f) { //si esta fuera del radio de deteccion con 20% mas
            endPos = _playerTrans.position; //ve al ultimo punto
            jugadorVisto = false; //detectao
            return;
        }

        Disparar();

        _trans.rotation = Quaternion.LookRotation(playerDirection);
    }

    void Disparar() {
        if (canShoot) {
            canShoot = false;
            Bullet bullet = miPool.GetPooledObject("bullet").GetComponent<Bullet>();
            bullet.Disparar(_ShootPoint.position, _ShootPoint.forward, 10.0f);
            StartCoroutine(waitToShoot());
        }
    }

    void MoveBehaviour() {

        if ((endPos - _transPos).sqrMagnitude < 1.0f) { //si ya llegaste obten nueva posicion y muevete
            endPos = GetMovePos();
            return;
        }

        direccion = Vector3.ClampMagnitude((endPos - _transPos), 1.0f);
        _trans.rotation = direccion.sqrMagnitude > 0.01f ? Quaternion.LookRotation(direccion) : _trans.rotation;
    }

    private void FixedUpdate() 
    {
        if(!jugadorVisto)
            _rb.MovePosition(_trans.position + (direccion * moveSpeed * Time.deltaTime));
    }

    Vector3 GetMovePos() 
    {
        Vector2 moveVector = UnityEngine.Random.insideUnitCircle * moveRadius;
        return new Vector3(moveVector.x, 0, moveVector.y);
    }

    IEnumerator waitToShoot() {
        yield return seconds;
        canShoot = true;
    }

    public void TakeDmg() {
        print("auch");
    }
}
