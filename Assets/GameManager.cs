using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera camera;

    int height = 0;
    float l, r, f, b;
    public Transform block;
    public Transform basePlatform;

    public Transform slidelr;
    public Transform slidefb;
    float lrDir = 1f, fbDir = 1f;

    public float slideSpeed = 1f;
    public float moveRange = 1.5f;

    Transform cur;
    bool gameOver = false;

    public Color[] colorPalette;
    public int colorPeriod = 8;

    public GradientBackground gradient;

    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer gameObjectRenderer = basePlatform.GetComponent<MeshRenderer>();
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.color = getColor(height);
        gameObjectRenderer.material = newMaterial;
        NextBlock();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver) return;

        float difficulty = Mathf.Pow(0.005f * height, 2) + 1;


        //Camera Movement
        Vector3 c = camera.transform.position;
        c.y = 7 + height * 0.1f;
        camera.transform.position = Vector3.Lerp(camera.transform.position, c, 0.1f);

        //Sliding Blocks around
        Vector3 lr = slidelr.position;
        lr.y = height * 0.1f;
        lr.x += slideSpeed * Time.deltaTime * lrDir * difficulty;
        slidelr.position = lr;
        if(slidelr.position.x > moveRange && lrDir == 1f)
            lrDir = -1f;
        else if(slidelr.position.x < -l && lrDir == -1f)
            lrDir = 1f;

        Vector3 fb = slidefb.position;
        fb.y = height * 0.1f;
        fb.z += slideSpeed * Time.deltaTime * fbDir * difficulty;
        slidefb.position = fb;
        if (slidefb.position.z > moveRange && fbDir == 1f)
            fbDir = -1f;
        else if (slidefb.position.z < -b && fbDir == -1f)
            fbDir = 1f;

        //User Input
        if (Input.GetKeyDown(KeyCode.Space))
            NextBlock();
    }
    
    void NextBlock()
    {
        float oldL = l, oldR = r, oldB = b, oldF = f;
        if (cur != null)
        {
            if (height % 2 == 1)
            {
                //Cube moving lr
                float cur = slidelr.position.x;
                if (cur > 0)
                    oldR += cur;
                else
                    oldL -= cur;
            }
            else
            {
                //Cube moving fb
                float cur = slidefb.position.z;
                if (cur > 0)
                    oldF += cur;
                else
                    oldB -= cur;
            }

            cur.gameObject.GetComponent<BlockScript>().ChangeSize(oldL, oldR, oldF, oldB);
            cur.SetParent(null, true);
        }

        if (height%2 == 1)
        {
            //Cube moving lr
            float cur = slidelr.position.x;
            if(cur > 0)
                l += cur;
            else
                r -= cur;
        }
        else
        {
            //Cube moving fb
            float cur = slidefb.position.z;
            if (cur > 0)
                b += cur;
            else
                f -= cur;
        }


        //Spawn Block with width and length
        if (height % 2 == 0)
            cur = Instantiate(block, slidelr);
        else
            cur = Instantiate(block, slidefb);
        cur.gameObject.GetComponent<BlockScript>().ChangeSize(l, r, f, b);


        //Reset Moving Platform Position
        Vector3 lr = slidelr.position;
        lr.x = moveRange;
        slidelr.position = lr;

        Vector3 fb = slidefb.position;
        fb.z = moveRange;
        slidefb.position = fb;

        height++;

        //Colors
        MeshRenderer gameObjectRenderer = cur.GetComponent<MeshRenderer>();
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.color = getColor(height);
        gameObjectRenderer.material = newMaterial;

        gradient.SetColor(getColor(height),getColor(height - colorPeriod * 2));
    }

    private Color getColor(int progress)
    {
        if (progress < colorPeriod) progress += colorPeriod * colorPalette.Length;
        float colorT = (progress % colorPeriod) / (float)colorPeriod;
        int curIdx = (progress / colorPeriod) % colorPalette.Length;
        int nextIdx = (curIdx + 1) % colorPalette.Length;
        return Color.Lerp(colorPalette[curIdx], colorPalette[nextIdx], colorT);
    }

    public void GameOver()
    {
        gameOver = true;
    }

}
