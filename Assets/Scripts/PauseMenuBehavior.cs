using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    bool isGamePaused = false;
    public GameObject pauseMenuPanel;
    float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // resumes game
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        pauseMenuPanel.SetActive(false);
    }

    // pauses game
    public void PauseGame()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
        pauseMenuPanel.SetActive(true);
    }

    // loads main menu scene
    public void LoadMainMenu()
    {
        PlayerPrefs.SetInt("TimePlayed", PlayerPrefs.GetInt("TimePlayed") + (int)(Time.time - startTime));
        ResumeGame();
        SceneManager.LoadScene("MainMenu");
    }

    // exits game
    public void ExitGame()
    {
        PlayerPrefs.SetInt("TimePlayed", PlayerPrefs.GetInt("TimePlayed") + (int)(Time.time - startTime));
        Application.Quit();
    }
}
