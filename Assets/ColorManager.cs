using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public Color[] colorPalette;
    public int colorPeriod = 4;

    public Color getColor(float progress)
    {
        if (progress < colorPeriod) progress += colorPeriod * colorPalette.Length;
        float colorT = (progress % colorPeriod) / (float)colorPeriod;
        int curIdx = ((int)progress / colorPeriod) % colorPalette.Length;
        int nextIdx = (curIdx + 1) % colorPalette.Length;
        return Color.Lerp(colorPalette[curIdx], colorPalette[nextIdx], colorT);
    }
}
