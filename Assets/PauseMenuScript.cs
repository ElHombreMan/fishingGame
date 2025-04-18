using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject PausePane1;
    private bool GameIsPaused = false;

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
        Time.timeScale = 0;
        GameIsPaused = true;
    
    
    }

    public void Continue() {
        PausePane1.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;
    
    
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
