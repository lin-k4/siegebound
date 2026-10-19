using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PanelBehavior : MonoBehaviour
{
    public TMP_Text towerTypeText;
    public TMP_Text towerStatsText;
    public Button sellButton;
    public TMP_Text sellPriceText;
    public Button upgradeButton;
    public TMP_Text upgradeCostText;
    public TMP_Text statsCompareText;
    public Button exitButton;

    public GameObject upgradePrefab;

    public LevelManager levelManager;

    public GameObject currentSelectedTower;

    private void OnEnable()
    {
        upgradeButton.onClick.AddListener(OnUpgradeClicked);
        sellButton.onClick.AddListener(OnSellClicked);

        RefreshPanel();
    }

    private void OnDisable()
    {
        upgradeButton.onClick.RemoveListener(OnUpgradeClicked);
        sellButton.onClick.RemoveListener(OnSellClicked);

        towerStatsText.text = "";
        towerStatsText.text= "";
        sellPriceText.text = "";
        upgradeCostText.text = "";
        statsCompareText.text = ""; 

        currentSelectedTower = null;
    }

    // listens for the upgrade button click and handles the upgrade logic
    private void OnUpgradeClicked()
    {
        if (currentSelectedTower.GetComponent<ArcheryTowerBehavior>())
        {
            if (currentSelectedTower.GetComponent<ArcheryTowerBehavior>().upgradePrefab)
            {
                GameObject newTower = currentSelectedTower.GetComponent<ArcheryTowerBehavior>().AttemptUpgrade();
                if (newTower != currentSelectedTower)
                {
                    levelManager.selectedPlacedTower = newTower;
                    currentSelectedTower = newTower;

                    if (currentSelectedTower.GetComponent<ArcheryTowerBehavior>() != null)
                    {
                        currentSelectedTower.GetComponent<ArcheryTowerBehavior>().IsSelected();
                    }

                    Invoke("SetInformation", 0.1f); //Needs delay or won't update panel correctly
                }
            }

        }
        else if (currentSelectedTower.GetComponent<MageTowerBehavior>())
        {
            if (currentSelectedTower.GetComponent<MageTowerBehavior>().upgradePrefab)
            {
                GameObject newTower = currentSelectedTower.GetComponent<MageTowerBehavior>().AttemptUpgrade();
                if (newTower != currentSelectedTower)
                {
                    levelManager.selectedPlacedTower = newTower;
                    currentSelectedTower = newTower;

                    if (currentSelectedTower.GetComponent<MageTowerBehavior>() != null)
                    {
                        currentSelectedTower.GetComponent<MageTowerBehavior>().IsSelected();
                    }

                    Invoke("SetInformation", 0.1f); //Needs delay or won't update panel correctly
                }
            }
        }
        else if (currentSelectedTower.GetComponent<CannonBehavior>())
        {
            if (currentSelectedTower.GetComponent<CannonBehavior>().upgradePrefab)
            {
                GameObject newTower = currentSelectedTower.GetComponent<CannonBehavior>().AttemptUpgrade();
                if (newTower != currentSelectedTower)
                {
                    levelManager.selectedPlacedTower = newTower;
                    currentSelectedTower = newTower;

                    if (currentSelectedTower.GetComponent<CannonBehavior>() != null)
                    {
                        currentSelectedTower.GetComponent<CannonBehavior>().IsSelected();
                    }

                    Invoke("SetInformation", 0.1f); //Needs delay or won't update panel correctly
                }
            }
        }
    }


    // listens for the sell button click and handles the sell logic
    private void OnSellClicked()
    {
        if (currentSelectedTower == null) return;

        int refundAmount = 0;

        if (currentSelectedTower.GetComponent<ArcheryTowerBehavior>())
        {
            refundAmount = Mathf.RoundToInt(currentSelectedTower.GetComponent<ArcheryTowerBehavior>().totalCost * levelManager.sellValue);
        }
        else if (currentSelectedTower.GetComponent<MageTowerBehavior>())
        {
            refundAmount = Mathf.RoundToInt(currentSelectedTower.GetComponent<MageTowerBehavior>().totalCost * levelManager.sellValue);
        }
        else if (currentSelectedTower.GetComponent<CannonBehavior>())
        {
            refundAmount = Mathf.RoundToInt(currentSelectedTower.GetComponent<CannonBehavior>().totalCost * levelManager.sellValue);
        }

        levelManager.money += refundAmount;
        levelManager.moneyText.text = "Money: " + levelManager.money.ToString();

        Destroy(currentSelectedTower);
        gameObject.SetActive(false);
    }

    // sets the information on the panel based on the selected tower
    void SetInformation()
    {
        if (currentSelectedTower == null)
        {
            return;
        }

        if (currentSelectedTower.GetComponent<ArcheryTowerBehavior>())
        {
            ArcheryTowerBehavior archeryTower = currentSelectedTower.GetComponent<ArcheryTowerBehavior>();
            towerTypeText.text = archeryTower.GetTowerTypeText();
            towerStatsText.text = archeryTower.GetStats();
            sellPriceText.text = archeryTower.GetSellValue();
            upgradeCostText.text = archeryTower.GetUpgradeCost();
            statsCompareText.text = archeryTower.GetUpgradeStats();
        }
        else if (currentSelectedTower.GetComponent<MageTowerBehavior>())
        {
            MageTowerBehavior mageTower = currentSelectedTower.GetComponent<MageTowerBehavior>();
            towerTypeText.text = mageTower.GetTowerTypeText();
            towerStatsText.text = mageTower.GetStats();
            sellPriceText.text = mageTower.GetSellValue();
            upgradeCostText.text = mageTower.GetUpgradeCost();
            statsCompareText.text = mageTower.GetUpgradeStats();
        }
        else if (currentSelectedTower.GetComponent<CannonBehavior>())
        {
            CannonBehavior cannonTower = currentSelectedTower.GetComponent<CannonBehavior>();
            towerTypeText.text = cannonTower.GetTowerTypeText();
            towerStatsText.text = cannonTower.GetStats();
            sellPriceText.text = cannonTower.GetSellValue();
            upgradeCostText.text = cannonTower.GetUpgradeCost();
            statsCompareText.text = cannonTower.GetUpgradeStats();
        }
    }

    // refreshes the panel information
    public void RefreshPanel()
    {
        if (levelManager == null) return;

        currentSelectedTower = levelManager.selectedPlacedTower;
        SetInformation();
    }


}