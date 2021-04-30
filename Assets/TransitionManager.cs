using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public Animator animator;
    public float transitionTime = 1f;
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void ReloadLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator LoadLevel(int levelIdx)
    {
        animator.SetTrigger("Fade");
        //Wait 1 second
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIdx);
    }
}
