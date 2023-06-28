using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class DayNightCycle : MonoBehaviourPun
{
    [SerializeField]
    private Gradient gradient;

    [SerializeField]
    private float darkness = 0.5f;

    [SerializeField]
    private Light2D globalLight;

    // Start is called before the first frame update
    void Start()
    {
        globalLight = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        DoDayNightCycle();
    }
    private void DoDayNightCycle()
    {
        globalLight.color = gradient.Evaluate(CalculatePercentage());

        if (CalculatePercentage() >= 0.5)
            globalLight.intensity = 2 * (1 - darkness) * CalculatePercentage() + (1 - 2 * (1 - darkness));
        else
            globalLight.intensity = 2 * (darkness - 1) * CalculatePercentage() + 1;
    }

    private float CalculatePercentage()
    {
        float percentage = WorldTime.instance.GetCurrentTime() / WorldTime.instance.GetCycle();
        return percentage;
    }
}
