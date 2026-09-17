//Author: Wade Lawler
//last Modified: 9/17/26
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [Header("Upgrade Pool")]
    // Drag ALL Upgrade Assets into this master list pool
    public List<UpgradeData> allPossibleUpgrades;

    [Header("UI Buttons")]
    // Assign your UI Buttons here
    public Button[] upgradeButtons;
    // Assign the Text component for each of the 3 buttons
    public TextMeshProUGUI[] buttonTextLabels;

    // Tracks the 3 currently chosen upgrades sitting in the layout slots
    private List<UpgradeData> activeUpgradesInSlots = new List<UpgradeData>();

    // Call this method inside your existing UpgradeMenu script right when the panel opens!
    public void PopulateUpgradeSlots()
    {
        activeUpgradesInSlots.Clear();

        // Safety check if you haven't filled your pool yet
        if (allPossibleUpgrades.Count < 3)
        {
            Debug.LogError("[UpgradeManager] You need at least 3 upgrades in your master pool list!");
            return;
        }

        // 1. Clone the master pool list so we can pull from it without altering the original asset pool
        List<UpgradeData> temporaryPool = new List<UpgradeData>(allPossibleUpgrades);

        // 2. Loop 3 times to pick 3 completely unique items
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, temporaryPool.Count);
            UpgradeData selectedUpgrade = temporaryPool[randomIndex];

            activeUpgradesInSlots.Add(selectedUpgrade);

            // Remove it from our temporary list copy so it cannot be selected again this turn
            temporaryPool.RemoveAt(randomIndex);
        }

        // 3. Push the chosen data directly onto your UI elements
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            if (i >= activeUpgradesInSlots.Count) break;

            UpgradeData upgrade = activeUpgradesInSlots[i];

            // Set text label layout (Name on top, description underneath)
            if (buttonTextLabels[i] != null)
            {
                buttonTextLabels[i].text = $"<b>{upgrade.upgradeName}</b>\n{upgrade.description}";
            }

            // Clear any old click links and map the button dynamically to its unique upgrade slot array position
            upgradeButtons[i].onClick.RemoveAllListeners();
            // Cache variable prevents loop closure bugs
            int slotIndex = i; 
            upgradeButtons[i].onClick.AddListener(() => OnUpgradeSelected(slotIndex));
        }
    }

    private void OnUpgradeSelected(int slotIndex)
    {
        UpgradeData chosenUpgrade = activeUpgradesInSlots[slotIndex];
        Debug.Log($"[UpgradeManager] Player picked: {chosenUpgrade.upgradeName} (ID: {chosenUpgrade.upgradeID})");

        // TODO: Apply the chosenUpgrade.modifierValue to the player here based on chosenUpgrade.upgradeID!

        // Close the menu after choice is locked down using your existing toggle setup
        UpgradeMenu menu = GetComponent<UpgradeMenu>();
        if (menu != null)
        {
            menu.ToggleMenu();
        }
    }
}