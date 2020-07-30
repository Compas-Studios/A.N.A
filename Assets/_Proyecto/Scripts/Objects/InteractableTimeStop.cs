using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableTimeStop : MonoBehaviour, IInteractuable
{
    public bool canStopTime = true, hasCamPos = false, oneUse = false;
    [SerializeField] Vector3 targetPos = Vector3.zero, camPos = Vector3.zero;
    MonoBehaviour _activador;


    public void Interactuar(MonoBehaviour activador)
    {
        _activador = activador;

        TextRevealer.txtInstance.MostrarInputs(false);
        TextRevealer.txtInstance.SetActivator(this);
        TextRevealer.txtInstance.mostrarCurrentLevel();
        if (hasCamPos)
        CamManager.camanInstance.moveToPos(transform.position + camPos, transform.position + targetPos, 0.5f);

        if (canStopTime)
            Time.timeScale = 0;
    }

    public void FinInteraccion()
    {
        TextRevealer.txtInstance.MostrarInputs(true);

        if (canStopTime)
            Time.timeScale = 1;

        if (hasCamPos)
            CamManager.camanInstance.resetCam();

        _activador.enabled = true;

        if (oneUse)
            this.enabled = false;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(transform.position + camPos, Vector3.one*0.1f);
        Gizmos.DrawWireSphere(transform.position + targetPos, 0.1f);
    }
}
