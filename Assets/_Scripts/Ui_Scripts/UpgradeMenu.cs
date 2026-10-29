//Author: Wade Lawler
//Last modified: 9/30/2026
using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradeMenu : MonoBehaviour
{

    public GameObject menuPanel;

    private bool isMenuOpen = false;

    public WaveManager waveManager;

    //for upgradeUi
    [SerializeField] private GameObject upgradePopupPrefab;
    [SerializeField] private Transform playerTransform;

    void Start()
    {
        //menu is hidden when the game first starts
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
    }


    void Update()
    {
        //Toggle the menu overlay when pressing the Escape key (for testing)
        if (Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame)
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        menuPanel.SetActive(isMenuOpen);

        //Pause game while menu is open
        if (isMenuOpen)
        {
        //Pauses gameplay
            Time.timeScale = 0f; 
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;

            //get the manager within upgradescreen
            UpgradeManager manager = menuPanel.GetComponentInChildren<UpgradeManager>();
            if (manager != null)
            {
                manager.PopulateUpgradeSlots();
            }
        }
        else
        {
        //Resume gameplay
            Time.timeScale = 1f; 
            //Cursor.lockState = CursorLockMode.Locked;
           // Cursor.visible = false;

            //if (waveManager != null)
           // {
            //    waveManager.ResumeNextWave();
           // }
        }
    }

    public void TriggerUpgradePopup(string cardText)
    {
        // 1. Close the card UI panel and resume Time.timeScale back to 1f FIRST
        ToggleMenu();

        // 2. Now that time is running, spawn the visual text over the player
        if (upgradePopupPrefab != null && playerTransform != null)
        {
            Vector3 spawnPos = playerTransform.position + Vector3.up * 3f;
            GameObject popup = Instantiate(upgradePopupPrefab, spawnPos, Quaternion.identity);

            UpgradeTextEffect effectScript = popup.GetComponent<UpgradeTextEffect>();
            if (effectScript != null)
            {
                effectScript.SetupUpgradePopup(cardText, playerTransform);
            }
        }
    }
}
