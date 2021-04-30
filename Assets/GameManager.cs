using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    Camera camera;

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

    public float snapzone = 0.1f;
    public float layerHeight = 0.2f;

    public Text scoreText;

    GradientBackground gradient;
    ColorManager colorManager;
    TransitionManager transitionManager;

    // Start is called before the first frame update
    void Start()
    {
        colorManager = FindObjectOfType<ColorManager>();
        gradient = FindObjectOfType<GradientBackground>();
        camera = FindObjectOfType<Camera>();
        transitionManager = FindObjectOfType<TransitionManager>();

        MeshRenderer gameObjectRenderer = basePlatform.GetComponent<MeshRenderer>();
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.color = colorManager.getColor(height);
        gameObjectRenderer.material = newMaterial;
        basePlatform.position = new Vector3(0, -0.5f + layerHeight / 2, 0);
        NextBlock();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver) return;

        float difficulty = Mathf.Pow(0.005f * height, 2) + 1;


        //Camera Movement
        Vector3 c = camera.transform.position;
        c.y = 7 + height * layerHeight;
        camera.transform.position = Vector3.Lerp(camera.transform.position, c, layerHeight);

        //Sliding Blocks around
        Vector3 lr = slidelr.position;
        lr.y = height * layerHeight;
        lr.x += slideSpeed * Time.deltaTime * lrDir * difficulty;
        slidelr.position = lr;
        if(slidelr.position.x > moveRange && lrDir == 1f)
            lrDir = -1f;
        else if(slidelr.position.x < -l && lrDir == -1f)
            lrDir = 1f;

        Vector3 fb = slidefb.position;
        fb.y = height * layerHeight;
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
        Vector3 lr = slidelr.position;
        Vector3 fb = slidefb.position;

        if (cur != null)
        {
            if (height % 2 == 1)
            {
                //Cube moving lr
                if (Mathf.Abs(lr.x) < snapzone)
                    lr.x = 0;
                slidelr.position = lr;
                if (lr.x > 0)
                    oldR += lr.x;
                else
                    oldL -= lr.x;
            }
            else
            {
                //Cube moving fb
                if (Mathf.Abs(fb.z) < snapzone)
                    fb.z = 0;
                slidefb.position = fb;
                if (fb.z > 0)
                    oldF += fb.z;
                else
                    oldB -= fb.z;
            }

            cur.gameObject.GetComponent<BlockScript>().ChangeSize(oldL, oldR, oldF, oldB, layerHeight);
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
        cur.gameObject.GetComponent<BlockScript>().ChangeSize(l, r, f, b, layerHeight);


        //Reset Moving Platform Position
        lr.x = moveRange;
        slidelr.position = lr;

        fb.z = moveRange;
        slidefb.position = fb;

        height++;

        scoreText.text = (height - 1).ToString();

        //Colors
        MeshRenderer gameObjectRenderer = cur.GetComponent<MeshRenderer>();
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.color = colorManager.getColor(height);
        gameObjectRenderer.material = newMaterial;

        gradient.SetColor(colorManager.getColor(height), colorManager.getColor(height - colorManager.colorPeriod * 2));
    }

    public void GameOver()
    {
        gameOver = true;
        transitionManager.ReloadLevel();
    }

}
