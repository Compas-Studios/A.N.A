using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DashHandler : MonoBehaviour
{
    [SerializeField] Renderer[] DashRenderers = default;
    [SerializeField] int dashModels = 5;
    public float dashMaxDistance = 3f;
    PlayerMovement playerMove;
    Vector3 newPos = default;
    Vector3 heading = default;
    Transform _tr = default;
    Transform[] parentTransRend = default;
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
        parentTransRend = new Transform[dashModels];
        DashRenderers = new Renderer[dashModels * 2];
        mtpBlock = new MaterialPropertyBlock();
    }

    private void Start()
    {
        for (int i = 0; i < dashModels; i++)
        {
            parentTransRend[i] = ObjectPooler.SharedInstance.GetPooledObject("DashModel").transform;
            Renderer[] renderers = parentTransRend[i].GetComponentsInChildren<Renderer>();
            DashRenderers[i * 2] = renderers[0];
            DashRenderers[(i * 2) + 1] = renderers[1];
        }

        disableMat(); //apagalos a todos

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

        float dist = 1.0f / dashModels;
        float t = 0.0f;

        for (int i = 0; i < parentTransRend.Length; i++)
        {
            parentTransRend[i].gameObject.SetActive(true); //prendelos a todos 
            parentTransRend[i].rotation = _tr.rotation;
            parentTransRend[i].position = Vector3.Lerp(_tr.position, newPos, t);
            t += dist;
        }   

        DOTween.To(updateMaterial, 1, 0, 1.0f).OnComplete(disableMat);//Funcion encargada de hacerlos transparentes y desactivarlos al terminar
        
        _tr.position = newPos; //movemos

        if (cam != null) cam.Shake(0.1f, 1.0f); //sacude la camara papi

    }

    void updateMaterial(float _tValue)
    {
        /*===========================EFECTOS===========================*/
        foreach (Renderer r in DashRenderers)
        {
            mtpBlock.SetFloat(propertyId, _tValue);
            r.SetPropertyBlock(mtpBlock);
        }
        /*============================================================*/
    }

    void disableMat()
    {
        foreach(Transform t in parentTransRend)
        {
            t.gameObject.SetActive(false);
        }
    }

    IEnumerator waitToDash()
    {
        yield return seconds;
        canDash = true;
    }
}
