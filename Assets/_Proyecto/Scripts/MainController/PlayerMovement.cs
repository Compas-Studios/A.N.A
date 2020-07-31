using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /*[SerializeField] private Transform _camSpace = default;*/
    [SerializeField] private float _rotSpeed = 2.0f;
    [SerializeField] private float _lookDist = 4.5f, looseRadius = 6.5f;
    [SerializeField] private Joystick _joystick = default;
    [SerializeField] bool useKeyboard = default;
    public float _moveSpeed = 5.0f;
    Vector3 _direccion = default, _lastdir = default, _inputfinal = default;
    List<Lookable> misLookables;
    GameObject _currentLookGo;
    Lookable _currLook;
    Rigidbody _rBody = default;
    Transform _tr = default;
    Quaternion desiredRot;
    bool hasTarget;

    private void Awake() 
    {
        _tr = transform;
        _rBody = GetComponent<Rigidbody>();
        _lastdir = Vector3.forward; //mira al frente
        misLookables = new List<Lookable>();
        foreach(Lookable l in FindObjectsOfType<Lookable>()) {
            misLookables.Add(l);
        }
    }

    public void updateLookables(List<Lookable> nuevosLookables)
    {
        foreach (var l in nuevosLookables)
            misLookables.Add(l);
    }

    #region GetInputs
    /// <summary>
    /// Small function that returns a normalized vector for input
    /// </summary>
    /// <returns>normalized Vector2</returns>
    void InputsMovil() 
    {
        _inputfinal = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
    }

    /// <summary>
    /// Small function that returns a vector for input based on camSpace
    /// </summary>
    /// <returns>normalized Vector2</returns>
    void InputsTeclas() 
    {

        _inputfinal = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _inputfinal = Vector3.ClampMagnitude(_inputfinal, 1);
        /*if (_camSpace) 
        {
            Vector3 forward = _camSpace.forward;
            forward.y = 0f;
            forward.Normalize();
            Vector3 right = _camSpace.right;
            right.y = 0f;
            right.Normalize();
            
            dire3D = 
                (forward * dire3D.z + right * dire3D.x);
        }*/
    }
    #endregion


    private void Update() 
    {
        if(useKeyboard)
            InputsTeclas();
        else
            InputsMovil();
        
        _direccion = _inputfinal;
        _lastdir = _direccion.sqrMagnitude > 0.01f ? _direccion : _lastdir;

        if (_currLook)
            desiredRot = Quaternion.LookRotation(_currLook._trans.position - _tr.position);
        else 
            desiredRot = Quaternion.LookRotation(_lastdir);

        _tr.rotation = Quaternion.Slerp(_tr.rotation, desiredRot, Time.deltaTime * _rotSpeed);

        if (_currLook == null) {
            _currLook = getNearestTarget();
            _currentLookGo = _currLook? _currLook.gameObject : null;
        } 
        else 
        {
            if (_currentLookGo.activeSelf == false)
            {
                _currLook = null;
                _currentLookGo = null;
                return;
            }
                
            if ((_currLook._trans.position - _tr.position).sqrMagnitude > looseRadius * looseRadius) 
            {
                _currLook = null;
            }
        }
    }

    private void FixedUpdate() {
        _rBody.MovePosition(_tr.position + (_direccion * Time.fixedDeltaTime * _moveSpeed));
    }

    Lookable getNearestTarget() 
    {
        Lookable nearest = null;
        float nearDist = float.MaxValue;
        float distance;
        foreach (Lookable _l in misLookables) {
            if (_l._priority > 0) 
            {
                distance = (_l._trans.position - _tr.position).sqrMagnitude;
                if (distance < _lookDist*_lookDist) 
                {
                    if (distance < nearDist) {
                        nearDist = distance;
                        nearest = _l;
                    }
                }
            } 
        }
        return nearest;
    }

    public void AddToLookable(Lookable _newLookable)
    {
        if (!misLookables.Contains(_newLookable))
        {
            misLookables.Add(_newLookable);
        }
    }

    public Vector3 getDireccion() //obtener la direccion a la que va o sino el frente del personaje
    {
        Vector3 direc = _direccion.sqrMagnitude > 0.01f? _direccion.normalized : _tr.forward;
        return direc;
    }
}
