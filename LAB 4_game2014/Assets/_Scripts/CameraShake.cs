using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    [Header("Shake Settings")]
    public CinemachineVirtualCamera virtualCamera;
    public float shakeIntensity = 3f;
    public float shakeDuration = 0.25f;

    private CinemachineBasicMultiChannelPerlin perlin;
    private float shakeTimer;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (virtualCamera == null)
        {
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        if (virtualCamera != null)
        {
            perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (perlin != null)
            {
                perlin.m_AmplitudeGain = 0f;
            }
        }
    }

    void Update()
    {
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f && perlin != null)
            {
                perlin.m_AmplitudeGain = 0f;
            }
        }
    }

    public void Shake()
    {
        Shake(shakeIntensity, shakeDuration);
    }

    public void Shake(float intensity, float duration)
    {
        if (perlin == null) return;

        perlin.m_AmplitudeGain = intensity;
        shakeTimer = duration;
    }
}
