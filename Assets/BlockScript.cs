using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    float l, r, f, b;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSize(float l, float r, float f, float b)
    {
        this.l = l;
        this.r = r;
        this.f = f;
        this.b = b;
        float width = 1 - l - r;
        float length = 1 - f - b;

        if (width <= 0 || length <= 0)
            FindObjectOfType<GameManager>().GameOver();

        transform.localScale = new Vector3(width, 0.1f, length);
        transform.localPosition = new Vector3((l - r) / 2f, 0, (b - f) / 2f);
    }
}
