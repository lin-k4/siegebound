using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class MainMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Button gameStartButton;
    public Button statisticsButton;
    public Button tutorialButton;
    public Button exitButton;

    [Header("LevelSelector")]
    public GameObject LevelSelector;
    public Button[] levels;
    public Sprite lockIcon;

    [Header("StatisticsPanel")]
    public GameObject StatisticsPanel;
    public TMP_Text systemName;
    public TMP_Text timePlayed;
    public TMP_Text timeSpentInMainMenu;
    public TMP_Text highestCompletedLevel;
    public TMP_Text timesOpened;

    [Header("TutorialPanel")]
    public GameObject TutorialPanel;

    float openTime;

    void Start()
    {
        if(PlayerPrefs.HasKey("TimesOpened"))
        {
            PlayerPrefs.SetInt("TimesOpened", PlayerPrefs.GetInt("TimesOpened") + 1);
        }
        else
        {
            PlayerPrefs.SetInt("TimesOpened", 1);
        }

        if(!PlayerPrefs.HasKey("TimePlayed"))
        {
            PlayerPrefs.SetInt("TimePlayed", 0);
        }
        openTime = Time.time;

        if (!PlayerPrefs.HasKey("TimeSpentInMainMenu"))
        {
            PlayerPrefs.SetInt("TimeSpentInMainMenu", 0);
        } // opentime also works for this

        if (!PlayerPrefs.HasKey("HighestCompletedLevel"))
        {
            PlayerPrefs.SetInt("HighestCompletedLevel", 0);
        }

    }

    //opens level selesctor panel
    public void OpenLevelSelector()
    {
        CloseStatisticsPanel();
        CloseTutorialPanel();

        int highestCompleted = PlayerPrefs.GetInt("HighestCompletedLevel", 0);

        //handles unlocked levels
        for (int i = 0; i < levels.Length; i++)
        {
            TMP_Text buttonText = levels[i].GetComponentInChildren<TMP_Text>();
            Image buttonImage = levels[i].GetComponent<Image>();

            if (i > highestCompleted)
            {
                levels[i].interactable = false;
                if (buttonImage != null)
                {
                    buttonImage.sprite = lockIcon;
                    buttonImage.enabled = true;
                    
                }
                if (buttonText != null)
                {
                    buttonText.text = "";
                    buttonText.enabled = false;
                }
            }
            else
            {
                levels[i].interactable = true;
                if (buttonImage != null)
                {
                    //buttonImage.sprite = null;
                    buttonImage.enabled = true;
                }
                if (buttonText != null)
                {
                    buttonText.text = "Level: " + (i + 1);
                    buttonText.enabled = true;
                }
            }
        }
        LevelSelector.SetActive(true);
    }

    //opens a specific level
    public void OpenLevel(string name)
    {
        PlayerPrefs.SetInt("TimePlayed", PlayerPrefs.GetInt("TimePlayed") + (int)(Time.time - openTime));
        PlayerPrefs.SetInt("TimeSpentInMainMenu", PlayerPrefs.GetInt("TimeSpentInMainMenu") + (int)(Time.time - openTime));
        SceneManager.LoadScene(name);
    }


    //opens statistics panel
    public void OpenStatisticsPanel()
    {
        CloseLevelSelector();
        CloseTutorialPanel();
        if(SystemInfo.deviceName == "n/a")
        {
            systemName.text = "N/A";
        }
        else
        {
            systemName.text = SystemInfo.deviceName;
        }
        
        timePlayed.text = PlayerPrefs.GetInt("TimePlayed") + " seconds";
        timeSpentInMainMenu.text = PlayerPrefs.GetInt("TimeSpentInMainMenu") + " seconds";
        if(PlayerPrefs.GetInt("HighestCompletedLevel") == 0)
        {
            highestCompletedLevel.text = "N/A";
        } 
        else
        {
            highestCompletedLevel.text = "Level: " + PlayerPrefs.GetInt("HighestCompletedLevel");
        }
        timesOpened.text = PlayerPrefs.GetInt("TimesOpened") + " times";
        StatisticsPanel.SetActive(true);
    }

    //opens tutorial panel
    public void OpenTutorialPanel()
    {
        CloseLevelSelector();
        CloseStatisticsPanel();
        TutorialPanel.SetActive(true);
    }

    //closes level selector panel
    public void CloseLevelSelector()
    {
        LevelSelector.SetActive(false);
    }

    //closes statistics panel
    public void CloseStatisticsPanel()
    {
        StatisticsPanel.SetActive(false);
    }

    //closes tutorial panel
    public void CloseTutorialPanel()
    {
        TutorialPanel.SetActive(false);
    }

    //exits the game
    public void ExitGame()
    {
        PlayerPrefs.SetInt("TimePlayed", PlayerPrefs.GetInt("TimePlayed") + (int)(Time.time - openTime));
        PlayerPrefs.SetInt("TimeSpentInMainMenu", PlayerPrefs.GetInt("TimeSpentInMainMenu") + (int)(Time.time - openTime));

        CloseLevelSelector();
        CloseStatisticsPanel();
        CloseTutorialPanel();
        Application.Quit();
    }
}
