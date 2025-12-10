using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightEffect : MonoBehaviour
{
    public Light2D light;
    public float maxWaitTime = 1f;
    public float maxFlickerTime = 0.2f;

    float timer;
    float interval;

    public float minIntensity = 1f;
    public float maxIntensity = 2f;
    public float speed = 5f;
    float targetIntensity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetIntensity = maxIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        /*timer += Time.deltaTime;
        if (timer >= interval)
        {
            light.enabled = !light.enabled;
            if (light.enabled)
            {
                interval = Random.Range(0f, maxWaitTime);
            }
            else
            {
                interval = Random.Range(0f, maxFlickerTime);
            }
            timer = 0f;
        }*/
        light.intensity = Mathf.Lerp(light.intensity, targetIntensity, Time.deltaTime * speed);
        if (Mathf.Abs(light.intensity - targetIntensity) < 0.05f)
        {
            if (targetIntensity == maxIntensity)
            {
                targetIntensity = minIntensity;
            }
            else
            {
                targetIntensity = maxIntensity;
            }
        }
    }
}
