using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTest : MonoBehaviour
{
    public GameObject menu;

    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            menu.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
        }
    }

    public void OnClose()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

}
