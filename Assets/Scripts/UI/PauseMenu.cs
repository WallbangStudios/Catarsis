using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public GameObject WonMenuUI;
    public GameObject DieMenuUI;
    public GameObject PauseMenuUI;

    public static bool Won = false;
    public static bool Died = false;
    public static bool GameIsPaused = false;

    void Awake() {
        Won = false;
        Died = false;
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (Won) {
            Win();
            return;
        }
        else if (Died){
            Diedly();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameIsPaused){
                Resume();
            }
            else {
                Pause();
            }
        }
	}

    public void Win() {
        WonMenuUI.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;
    }

    public void Diedly() {
        DieMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void PlayAgain() {
        Time.timeScale = 1f;
        GameIsPaused = false;
        DieMenuUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Resume() {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause() {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("menu");
    }

    public void QuitGame() {
        Application.Quit();
    }

}
