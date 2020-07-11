using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DashHandler : MonoBehaviour
{
    [SerializeField] Renderer[] beginDashRenderers = default;
    [SerializeField] Renderer[] endDashRenderers = default;
    [SerializeField] TrailRenderer trail = default;
    public float dashMaxDistance = 3f;
    PlayerMovement playerMove;
    Vector3 newPos = default;
    Vector3 heading = default;
    Transform _tr = default, _parentBeginObj = default, _parentEndObj;
    bool canDash = true;

    WaitForSeconds seconds, trailseconds;
    int propertyId;
    MaterialPropertyBlock mtpBlock;

    CamShake cam;

    private void Awake()
    {
        playerMove = GetComponent<PlayerMovement>();
        _tr = transform;
        seconds = new WaitForSeconds(0.9f);
        trailseconds = new WaitForSeconds(0.1f);
        propertyId = Shader.PropertyToID("_transitionValue");

    }

    private void Start()
    {
        if (beginDashRenderers[0] != null)
            _parentBeginObj = beginDashRenderers[0].transform.parent;

        if (endDashRenderers[0] != null)
            _parentEndObj = endDashRenderers[0].transform.parent;

        cam = CamShake._staticShake;
    }

    public void Dash() // el dash te transporta a la direccion que vayas o al frente si estas quieto
    {
        //Checa si puede hacer dash
        if (!canDash)
            return;

        canDash = false; //ya no puede hacer dash
        StartCoroutine(waitToDash()); //espere un momento para dashear

        heading = playerMove.getDireccion(); //obtenemos la direccion a la que va el jugador
        
        if(Physics.Raycast(_tr.position,heading,out RaycastHit hit, dashMaxDistance)) //checamos si hay algo en esa direccion
            newPos = hit.point - heading; //si hay algo ese sera nuestro limite
        else
            newPos = _tr.position + (heading * dashMaxDistance); //sino nos moveremos la distancia maxima de nuestro dash

        if (_parentBeginObj != null)
        {
            _parentBeginObj.position = _tr.position; //pongo el modelo del fade en el lugar  (si existe)
            _parentBeginObj.rotation = transform.rotation;
        }

        if(trail!=null) trail.emitting = true;

        /*===========================EFECTOS===========================*/
        foreach (Renderer r in beginDashRenderers)
        {
            r.material.SetFloat(propertyId, 1);
            r.material.DOFloat(0, propertyId, 1.0f);
        }

        foreach (Renderer r in endDashRenderers)
        {
            r.material.SetFloat(propertyId, 1);
            r.material.DOFloat(0, propertyId, 1.0f);
        }
        /*============================================================*/

        _tr.position = newPos; //movemos

        if (cam != null) cam.Shake(0.1f, 1.0f); //sacude la camara papi

        if (_parentEndObj != null)
        {
            _parentEndObj.position = _tr.position; //pongo el modelo del fade en el lugar  (si existe)
            _parentEndObj.rotation = transform.rotation;
        }
        
    }

    IEnumerator waitToDash()
    {
        yield return trailseconds;
        if (trail != null) trail.emitting = false;

        yield return seconds;
        canDash = true;
        
    }
}
