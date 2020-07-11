using UnityEngine;
using DG.Tweening;

public class CamShake : MonoBehaviour
{
    public static CamShake _staticShake;
    Transform _cam = default;

    private void Awake()
    {
        if(_staticShake == null)
            _staticShake = this;
        else
            Destroy(gameObject);

        _cam = transform;
    }

    public void Shake(float _duration, float _strenght)
    {
        _cam.DOShakeRotation(_duration, _strenght,10, 180, true);
    }
}
