using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        AudioManager.audiomanager.Play("Transition");
    }

    void Update()
    {
        
    }

    public void LoadScene()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(1);
            AudioManager.audiomanager.Play("Transition");
        }

        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            SceneManager.LoadScene(0);
            AudioManager.audiomanager.Play("Transition");
        }
    }

    public void SetTrigger()
    {
        animator.SetTrigger("FadeOut");
    }

    public void TransitionSound()
    {
        AudioManager.audiomanager.Play("Transition");
    }

    public void RateButtonPressed()
    {
        Application.OpenURL("market://details?id=.com.FratNightGaming.FlappyBird");
    }

}
