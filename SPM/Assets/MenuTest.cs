using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTest : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    public string MainMenuSceneName;

    void Start()
    {
        menu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if(!menu.activeInHierarchy)
            {
                PauseGame();
            }else 
            
            if(menu.activeInHierarchy)
            {
                UnpauseGame();
            }
            
        }
    }

    public void OnClose()
    {
        UnpauseGame();
    }

    private void PauseGame()
    {
        Cursor.ActivateCursor(true, CursorLockMode.Confined);
        menu.SetActive(true);
        Time.timeScale = 0;
        AudioListener.pause = true;


        GameObject playerObj = GameObject.FindWithTag("Player");
        playerObj.GetComponent<PlayerController>().enabled = false;

        GameObject camObj = GameObject.FindWithTag("MainCamera");
        camObj.GetComponent<ThirdPersonCamera>().enabled = false;
    }

    private void UnpauseGame()
    {
        Cursor.ActivateCursor(false, CursorLockMode.Locked);
        menu.SetActive(false);
        Time.timeScale = 1;
        AudioListener.pause = false;


        GameObject playerObj = GameObject.FindWithTag("Player");
        playerObj.GetComponent<PlayerController>().enabled = true;

        GameObject camObj = GameObject.FindWithTag("MainCamera");
        camObj.GetComponent<ThirdPersonCamera>().enabled = true;

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        print("load main menu scene");
    }

}
