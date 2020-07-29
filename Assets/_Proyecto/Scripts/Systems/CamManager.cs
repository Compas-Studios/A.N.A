using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CamManager : MonoBehaviour
{
    public static CamManager camanInstance = default;
    SCR_CameraFollow micam = default;
    Transform camTr = default;
    Vector3 lastCamPos = default;
    private void Awake()
    {
        if(camanInstance == null)
            camanInstance = this;
        else
            Destroy(this);
        
        micam = FindObjectOfType<SCR_CameraFollow>();
        camTr = micam.transform;
    }

    public void moveToPos(Vector3 pos, Vector3 camtarget, float duration)
    {
        micam.enabled = false;
        camTr.DOMove(pos, duration).SetUpdate(true);
        Quaternion quat = Quaternion.LookRotation(camtarget - pos, Vector3.up);
        camTr.DORotateQuaternion(quat, duration).SetUpdate(true);
    }

    public void resetCam()
    {
        micam.enabled = true;
        micam.ResetRot();
    }
}
