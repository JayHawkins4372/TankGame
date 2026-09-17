//Author: Wade Lawler
//Last modified: 9/16/2026
using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradeMenu : MonoBehaviour
{

    public GameObject menuPanel;

    private bool isMenuOpen = false;

    public WaveManager waveManager;

    void Start()
    {
        // menu is hidden when the game first starts
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
    }


    void Update()
    {
        // Toggle the menu overlay when pressing the Escape key (Temporary until waves and enemies are added)
        if (Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame)
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        menuPanel.SetActive(isMenuOpen);

        // Pause game while menu is open
        if (isMenuOpen)
        {
            Time.timeScale = 0f; // Pauses gameplay
            Cursor.lockState = CursorLockMode.None; // Unlocks mouse cursor (temp until touch controls)
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f; // Resumes gameplay
            Cursor.lockState = CursorLockMode.Locked; // Relocks mouse cursor (temp until touch controls)
            Cursor.visible = false;

            if (waveManager != null)
            {
                waveManager.ResumeNextWave();
            }
        }
    }
}
