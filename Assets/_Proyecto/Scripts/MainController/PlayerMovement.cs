using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _camSpace = default;
    [SerializeField] private float _rotSpeed = 2.0f;
    [SerializeField] private float _lookDist = 4.5f, looseRadius = 6.5f;
    [SerializeField] private Joystick _joystick = default;
    public float _moveSpeed = 5.0f;
    Vector3 _direccion = default, _lastdir = default;
    List<Lookable> misLookables;
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

    #region GetInputs
    /// <summary>
    /// Small function that returns a normalized vector for input
    /// </summary>
    /// <returns>normalized Vector2</returns>
    Vector3 InputsMovil() 
    {
        Vector3 inputfinal = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
        return inputfinal;
    }

    /// <summary>
    /// Small function that returns a vector for input based on camSpace
    /// </summary>
    /// <returns>normalized Vector2</returns>
    Vector3 InputsTeclas() 
    {
        Vector3 dire3D;
        dire3D = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (_camSpace) 
        {
            Vector3 forward = _camSpace.forward;
            forward.y = 0f;
            forward.Normalize();
            Vector3 right = _camSpace.right;
            right.y = 0f;
            right.Normalize();
            
            dire3D = 
                (forward * dire3D.z + right * dire3D.x);
        }
        
        return Vector3.ClampMagnitude(dire3D,1);
    }
    #endregion


    private void Update() 
    {
        _direccion = InputsMovil();
        _lastdir = _direccion.sqrMagnitude > 0.01f ? _direccion : _lastdir;

        if (_currLook)
            desiredRot = Quaternion.LookRotation(_currLook._trans.position - _tr.position);
        else 
            desiredRot = Quaternion.LookRotation(_lastdir);

        _tr.rotation = Quaternion.Slerp(_tr.rotation, desiredRot, Time.deltaTime * _rotSpeed);

        if (_currLook == null) {
            _currLook = getNearestTarget();
        } 
        else 
        {
            if((_currLook._trans.position - _tr.position).sqrMagnitude > looseRadius * looseRadius) 
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

    /*
      private void OnDrawGizmos() {
        Gizmos.color = Color.white;

        if (_currLook!=null)
        Gizmos.DrawWireSphere(_currLook._trans.position, 1);

        Gizmos.color = Color.blue;
        if(_tr!=null)
        Gizmos.DrawWireSphere(_tr.position, _lookDist);
    }*/
}
