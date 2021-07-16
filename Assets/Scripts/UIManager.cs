using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    private GameObject MainMenu;
    private GameObject OptionsMenu;
    private GameObject TitleUI;
    private GameObject StartUI;
    private GameObject fade;
    

    public void Start()
    {
        // Initialize GameObjects
        MainMenu = GameObject.FindGameObjectWithTag("MainMenu");
        OptionsMenu = GameObject.FindGameObjectWithTag("OptionsMenu");
        StartUI = GameObject.FindGameObjectWithTag("StartUI");
        TitleUI = GameObject.FindGameObjectWithTag("TitleUI");
        fade = GameObject.FindGameObjectWithTag("crossfade");

        StartUI.SetActive(false);
        OptionsMenu.SetActive(false);

        // Start of game fade in
        LeanTween.alpha(fade.GetComponent<RectTransform>(), 0, 1f).setOnComplete(() =>
        {
            fade.SetActive(false);
        });
    }

    public void DisplayStartUI()
    {
        fade.SetActive(true);
        LeanTween.alpha(fade.GetComponent<RectTransform>(), 1, 2f).setOnComplete(() =>
        {
            TitleUI.SetActive(false);
            StartUI.SetActive(true);
            LeanTween.alpha(fade.GetComponent<RectTransform>(), 0, 2f).setOnComplete(() =>
            {
                fade.SetActive(false);
            });
        });
    }

    public void DisplayOptionsMenu()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    public void DisplayMainMenu()
    {
        OptionsMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}


// SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
