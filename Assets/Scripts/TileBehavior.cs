using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileBehavior : MonoBehaviour
{
    public GameObject towerPrefab;
    public GameObject hasTower = null;
    private LevelManager levelManager;

    private void Start()
    {
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    // handles the logic for placing a tower on the tile when clicked
    private void OnMouseDown()
    {
        if (hasTower != null || IsMouseOverCanvasElement() || levelManager.selectedTower == null)
        {
            return;
        }
        else
        {
            if(levelManager.selectedTower.GetComponent<CannonBehavior>())
            {
                CannonBehavior cannon = levelManager.selectedTower.GetComponent<CannonBehavior>();
                if(levelManager.money < cannon.totalCost)
                {
                    return;
                }
                levelManager.money -= cannon.totalCost;
            } 
            else if (levelManager.selectedTower.GetComponent<ArcheryTowerBehavior>())
            {
                ArcheryTowerBehavior archerTower = levelManager.selectedTower.GetComponent<ArcheryTowerBehavior>();
                if (levelManager.money < archerTower.totalCost)
                {
                    return;
                }
                levelManager.money -= archerTower.totalCost;
            }
            else if (levelManager.selectedTower.GetComponent<MageTowerBehavior>())
            {
                MageTowerBehavior mageTower = levelManager.selectedTower.GetComponent<MageTowerBehavior>();
                if (levelManager.money < mageTower.totalCost)
                {
                    return;
                }
                levelManager.money -= mageTower.totalCost;
            }
            levelManager.UpdateMoneyText();
            GameObject tower = Instantiate(levelManager.selectedTower, transform.position + Vector3.up, transform.rotation);
            hasTower = tower;
            HighlightLogic highlight = GetComponent<HighlightLogic>();
            if (highlight != null)
            {
                highlight.ResetHighlightAndGhost();
            }
            levelManager.selectedTower = null;
            if(levelManager.selectedPlacedTower)
            {
                levelManager.HideSelectedTowerVisualizer();
            }
            levelManager.selectedPlacedTower = hasTower;
        }
    }

    // Check if the mouse is over a UI element
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
}