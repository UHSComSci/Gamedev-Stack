using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientBackground : MonoBehaviour
{
    public UnityEngine.UI.RawImage img;
    private Texture2D backgroundTexture;

    Color color1, color2;
    Color cur1, cur2;
    bool init = true;

    float colorChangeSpeed = 0.02f;


    void Awake()
    {
        backgroundTexture = new Texture2D(1, 2);
        backgroundTexture.wrapMode = TextureWrapMode.Clamp;
        backgroundTexture.filterMode = FilterMode.Bilinear;
    }

    private void Update()
    {
        cur1 = Color.Lerp(cur1, color1, colorChangeSpeed);
        cur2 = Color.Lerp(cur2, color2, colorChangeSpeed);

        backgroundTexture.SetPixels(new Color[] { cur1, cur2 });
        backgroundTexture.Apply();
        img.texture = backgroundTexture;
    }

    public void SetColor(Color color1, Color color2)
    {
        if (init)
        {
            cur1 = color1;
            cur2 = color2;
            init = false;
        }
        this.color1 = color1;
        this.color2 = color2;
    }
}
