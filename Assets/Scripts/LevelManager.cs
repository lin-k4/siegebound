using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public bool isPlaying;
    [Header("Resources")]
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int totalHealth = 100;
    public int money = 500;
    public float sellValue = .75f;

    [Header("Placement Logic")]
    public GameObject selectedTower = null;
    public Button cannonButton;
    public Button archerButton;
    public Button mageButton;
    public TMP_Text cannonPrice;
    public TMP_Text archerPrice;
    public TMP_Text magePrice;

    [Header("Towers")]
    public GameObject cannonLevel1;
    public GameObject archerLevel1;
    public GameObject mageLevel1;

    [Header("TowerManagement")]
    public GameObject selectedPlacedTower;

    [Header("UI")]
    public TMP_Text healthText;
    public Slider healthSlider;
    public TMP_Text moneyText;
    public TMP_Text levelLost;
    public TMP_Text levelWonText;
    public Button nextLevelButton;
    public Button replayLevelButton;
    public Button mainMenuButton;
    public TMP_Text waveText;
    public Slider waveSlider;

    [Header("Side Panel")]
    public GameObject sidePanel;


    [Header("Enemies")]
    public GameObject spawner;
    public GameObject lastNode;

    [Header("Wave Management")]
    int currentHealth;

    [Header("Level Number")]
    public int levelNumber = 1;
    public string nextLevel = "";

    float startTime;

    void Start()
    {
        isPlaying = true;
        currentHealth = totalHealth;
        if (cannonButton != null)
        {
            cannonButton.onClick.AddListener(OnCannonButtonPressed);
            archerButton.onClick.AddListener(OnArcherButtonPressed);
            mageButton.onClick.AddListener(OnMageButtonPressed);
        }

        if(moneyText != null)
        {
            UpdateMoneyText();
        }

        if(sidePanel != null)
        {
            sidePanel.SetActive(false);
        }

        cannonPrice.text = GetCannonPrice();
        archerPrice.text = GetArcherPrice();
        magePrice.text = GetMagePrice();

        startTime = Time.time;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleGlobalClick();
        }
    }

    // Handles level loss
    void LevelLost()
    {
        Debug.Log("Level Lost! Implement endgame screen here.");
        isPlaying = false;
        levelLost.gameObject.SetActive(true);
        replayLevelButton.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);

        cannonButton.interactable = false;
        mageButton.interactable = false;
        archerButton.interactable = false;
    }

    // Handles level win
    public void LevelWon()
    {
        Debug.Log("Level Won!");
        isPlaying = false;
        levelWonText.gameObject.SetActive(true);
        if(nextLevel.Length > 0) 
        { 
            nextLevelButton.gameObject.SetActive(true); 
        }
        else
        {
            replayLevelButton.gameObject.SetActive(true);
        }

        mainMenuButton.gameObject.SetActive(true);
        cannonButton.interactable = false;
        mageButton.interactable = false;
        archerButton.interactable = false;

        PlayerPrefs.SetInt("HighestCompletedLevel", Mathf.Max(PlayerPrefs.GetInt("HighestCompletedLevel"), levelNumber));
    }

    // Handles global clicks for selecting towers and enabling side panel
    private void HandleGlobalClick()
    {
        if (IsMouseOverCanvasElement()) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject hitObj = hit.collider.gameObject;
            if (hitObj.CompareTag("Tower") && selectedTower == null)
            {
                if (hitObj == selectedPlacedTower)
                {
                    HideSelectedTowerVisualizer();
                    sidePanel.SetActive(false);
                    return;
                }

                if (selectedPlacedTower != null)
                {
                    HideSelectedTowerVisualizer();
                }

                if (hitObj.GetComponentInChildren<ArcheryTowerBehavior>() != null)
                {
                    hitObj.GetComponentInChildren<ArcheryTowerBehavior>().IsSelected();
                }
                else if (hitObj.GetComponentInChildren<CannonBehavior>() != null)
                {
                    hitObj.GetComponentInChildren<CannonBehavior>().IsSelected();
                }
                else if (hitObj.GetComponentInChildren<MageTowerBehavior>() != null)
                {
                    hitObj.GetComponentInChildren<MageTowerBehavior>().IsSelected();
                }

                selectedPlacedTower = hitObj;
                sidePanel.SetActive(true);
                sidePanel.GetComponent <PanelBehavior>().RefreshPanel();


                return;
            }
        }

        if (selectedPlacedTower != null)
        {
            HideSelectedTowerVisualizer();
            sidePanel.SetActive(false);
        }
    }

    //handles hiding the visualizer for the selected tower
    public void HideSelectedTowerVisualizer()
    {
        if (selectedPlacedTower != null)
        {
            if (selectedPlacedTower.GetComponentInChildren<ArcheryTowerBehavior>() != null)
                selectedPlacedTower.GetComponentInChildren<ArcheryTowerBehavior>().IsDeselected();
            else if (selectedPlacedTower.GetComponentInChildren<CannonBehavior>() != null)
                selectedPlacedTower.GetComponentInChildren<CannonBehavior>().IsDeselected();
            else if (selectedPlacedTower.GetComponentInChildren<MageTowerBehavior>() != null)
                selectedPlacedTower.GetComponentInChildren<MageTowerBehavior>().IsDeselected();

            selectedPlacedTower = null;
        }
    }

    // checks if the mouse is currently over a UI element to prevent interactions with the game world
    private bool IsMouseOverCanvasElement()
    {
        if (EventSystem.current == null) return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.GetComponentInParent<Canvas>() != null)
            {
                return true;
            }
        }
        return false;
    }

    // handles cannon button press and selects the cannon tower for placement
    void OnCannonButtonPressed()
    {
        if (selectedTower != cannonLevel1)
        {
            selectedTower = cannonLevel1;
        }
        else
        {
            selectedTower = null;
        }
    }

    // handles archer button press and selects the archer tower for placement
    void OnArcherButtonPressed()
    {
        if (selectedTower != archerLevel1)
        {
            selectedTower = archerLevel1;
        }
        else
        {
            selectedTower = null;
        }
    }

    // handles mage button press and selects the mage tower for placement
    void OnMageButtonPressed()
    {
        if (selectedTower != mageLevel1)
        {
            selectedTower = mageLevel1;
        }
        else
        {
            selectedTower = null;
        }
    }

    // Updates the money text UI element to reflect the current amount of money
    public void UpdateMoneyText()
    {
        moneyText.text = "Money: " + money.ToString();
    }

    // updates the health to reflect damage taken, updates the health text UI element to reflect the current health of the player
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took damage! Current health: " + currentHealth);
        if(healthText != null)
        {
            healthText.text = currentHealth.ToString();
        }
        if(healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / totalHealth;
        }
        if(currentHealth <= 0)
        {
            LevelLost();
        }
    }

    // loads the next level when the next level button is pressed
    public void LoadNextLevel()
    {
        if(nextLevel.Length != 0)
        {
            PlayerPrefs.SetInt("TimePlayed", PlayerPrefs.GetInt("TimePlayed") + (int)(Time.time - startTime));
            SceneManager.LoadScene(nextLevel);
        }
    }

    // loads the main menu
    public void LoadMainMenu()
    {
        PlayerPrefs.SetInt("TimePlayed", PlayerPrefs.GetInt("TimePlayed") + (int)(Time.time - startTime));
        SceneManager.LoadScene("MainMenu");
    }

    // reloads the current level
    public void ReplayLevel()
    {
        PlayerPrefs.SetInt("TimePlayed", PlayerPrefs.GetInt("TimePlayed") + (int)(Time.time - startTime));
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // gets the price of the lvl 1 cannon tower
    public string GetCannonPrice()
    {
        return cannonLevel1.GetComponent<CannonBehavior>().totalCost.ToString();
    }

    // gets the price of the lvl 1 archer tower
    public string GetArcherPrice()
    {
        return archerLevel1.GetComponent<ArcheryTowerBehavior>().totalCost.ToString();
    }

    // gets the price of the lvl 1 mage tower
    public string GetMagePrice()
    {
        return mageLevel1.GetComponent<MageTowerBehavior>().totalCost.ToString();
    }
}