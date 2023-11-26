using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerMenu : MonoBehaviour
{
    public GameObject mainMenu;

    public GameObject controlPanel;

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Controls()
    {
        controlPanel.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void ExitControls()
    {
        controlPanel.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void Quit()
    {        
        Application.Quit();
    }
}
