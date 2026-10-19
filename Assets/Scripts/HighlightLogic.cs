using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class HighlightLogic : MonoBehaviour
{
    [Header("Highlight Settings")]
    [ColorUsage(true, true)]
    public Color tealHighlight = new Color(0.0f, 0.4f, 0.4f);

    private Renderer blockRenderer;
    private LevelManager levelManager;
    private TileBehavior tileBehavior;

    private GameObject currentGhost;
    private GameObject lastTrackedTowerPrefab;

    void Start()
    {
        blockRenderer = GetComponent<Renderer>();
        levelManager = FindAnyObjectByType<LevelManager>();
        tileBehavior = GetComponent<TileBehavior>();
    }

    void OnMouseOver()
    {
        if (levelManager == null ||levelManager.selectedTower == null ||IsMouseOverCanvasElement() ||(tileBehavior != null && tileBehavior.hasTower != null))
        {
            ResetHighlightAndGhost();
            return;
        }

        if (blockRenderer != null)
        {
            blockRenderer.material.EnableKeyword("_EMISSION");
            blockRenderer.material.SetColor("_EmissionColor", tealHighlight);
        }

        HandleGhostPreview();
    }

    void OnMouseExit()
    {
        ResetHighlightAndGhost();
    }

    //creates a ghost preview of the tower being placed
    private void HandleGhostPreview()
    {
        GameObject selectedPrefab = levelManager.selectedTower;

        if (currentGhost != null && selectedPrefab != lastTrackedTowerPrefab)
        {
            DestroyGhost();
        }

        if (currentGhost == null)
        {
            Vector3 spawnPosition = transform.position + Vector3.up;
            currentGhost = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

            lastTrackedTowerPrefab = selectedPrefab;
            DisableGhostComponents(currentGhost);
        }
    }

    //erases the ghost preview and resets the highlight
    public void ResetHighlightAndGhost()
    {
        if (blockRenderer != null)
        {
            blockRenderer.material.SetColor("_EmissionColor", Color.black);
        }

        DestroyGhost();
    }

    //destroys the ghost preview
    private void DestroyGhost()
    {
        if (currentGhost != null)
        {
            Destroy(currentGhost);
            currentGhost = null;
            lastTrackedTowerPrefab = null;
        }
    }

    //disables colliders and scripts on the ghost preview to prevent interaction
    private void DisableGhostComponents(GameObject ghost)
    {
        Collider[] colliders = ghost.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        MonoBehaviour[] scripts = ghost.GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }
    }

    //detects if the mouse is currently over a UI element to prevent interactions with the game world
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