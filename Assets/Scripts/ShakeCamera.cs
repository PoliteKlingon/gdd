
using Cinemachine;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    [SerializeField] private float shakeIntensity = 0.25f;
    public void Shake()
    {
        GetComponent<CinemachineImpulseSource>()
            .GenerateImpulse(
                new Vector3(
                    Random.Range(-shakeIntensity, shakeIntensity),
                    Random.Range(-shakeIntensity, shakeIntensity),
                    Random.Range(-shakeIntensity, shakeIntensity)
                    )
            );
    }
}
