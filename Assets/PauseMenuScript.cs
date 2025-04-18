using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject PausePane1;
    public static bool GameIsPaused = false; //a global variable for other scripts as well

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Log("Escape pressed"); //to check

            if (GameIsPaused)
            {
                Continue();
            }
            else { 
                Pause();
            }
        
        
        }
        
    }

    public void Pause() { 
        PausePane1.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // Free the cursor
    }

    public void Continue() {
        PausePane1.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; // Re-lock the cursor
    }

    public void quitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game...");
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu"); 
    }

}
