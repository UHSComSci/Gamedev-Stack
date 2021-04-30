using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    GradientBackground gradient;
    ColorManager colorManager;

    public float colorChangeSpeed = 2f;

    void Start()
    {
        gradient = FindObjectOfType<GradientBackground>();
        colorManager = FindObjectOfType<ColorManager>();
    }

    float progress = 0;
    // Update is called once per frame
    void Update()
    {
        progress += Time.deltaTime * colorChangeSpeed;
        gradient.SetColor(colorManager.getColor(progress), colorManager.getColor(progress - colorManager.colorPeriod * 2));
    }
}
