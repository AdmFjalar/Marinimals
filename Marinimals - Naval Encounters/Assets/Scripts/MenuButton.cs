using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public GameObject joinMenu;
    public GameObject startMenu;
    public GameObject settingsMenu;

    public void StartGame()
    {
        joinMenu.SetActive(true);
        startMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void Settings()
    {
        joinMenu.SetActive(false);
        startMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
