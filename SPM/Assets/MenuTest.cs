using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTest : MonoBehaviour
{
    public GameObject menu;

    void Start()
    {
        menu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            PauseGame();
        }
    }

    public void OnClose()
    {
        UnpauseGame();
    }

    private void PauseGame()
    {
        Cursor.lockState = CursorLockMode.Confined;
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
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        AudioListener.pause = false;


        GameObject playerObj = GameObject.FindWithTag("Player");
        playerObj.GetComponent<PlayerController>().enabled = true;

        GameObject camObj = GameObject.FindWithTag("MainCamera");
        camObj.GetComponent<ThirdPersonCamera>().enabled = true;

    }

}
