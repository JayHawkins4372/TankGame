//Author: Wade Lawler
//Last modified: 9/30/2026
using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradeMenu : MonoBehaviour
{

    public GameObject menuPanel;

    private bool isMenuOpen = false;

    public WaveManager waveManager;

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
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

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
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //if (waveManager != null)
           // {
            //    waveManager.ResumeNextWave();
           // }
        }
    }
}
