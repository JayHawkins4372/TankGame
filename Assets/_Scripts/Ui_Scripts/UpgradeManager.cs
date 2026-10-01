//Author: Wade Lawler
//last Modified: 9/17/26
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;


public class UpgradeManager : MonoBehaviour
{
    [Header("Upgrade Pool")]
    // All upgrades go into this list
    public List<UpgradeData> allPossibleUpgrades;

    [Header("UI Buttons")]
    // Ui Buttons 1-3
    public Button[] upgradeButtons;
    // Text components on the Ui buttons
    public TextMeshProUGUI[] buttonTextLabels;

    // A temporary internal list to keep track of the 3 upgrades currently drawn
    private List<UpgradeData> activeUpgradesInSlots = new List<UpgradeData>();

    private bool madeSelection = false;

    public void PopulateUpgradeSlots()
    {
        //resets for new upgrade selection
        madeSelection = false;
        Debug.Log($"[UpgradeManager] PopulateUpgradeSlots() was called! Master Pool Count: {allPossibleUpgrades.Count}");
        // Clear out previous cards
        activeUpgradesInSlots.Clear();

        // Stop if there's not at least 3 upgrade cards added
        if (allPossibleUpgrades.Count < 3)
        {
            Debug.LogError("[UpgradeManager] Need at least 3 upgrades in your master pool list!");
            return;
        }

        // Create a temporary copy of your master pool so we can cross out cards as we pick them
        List<UpgradeData> temporaryPool = new List<UpgradeData>(allPossibleUpgrades);

        // Run a loop 3 times to pick 3 completely random, unique upgrade choices
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, temporaryPool.Count);
            UpgradeData selectedUpgrade = temporaryPool[randomIndex];

            activeUpgradesInSlots.Add(selectedUpgrade);
            temporaryPool.RemoveAt(randomIndex); // Prevents duplicates
        }

        Debug.Log($"[UpgradeManager] Successfully generated {activeUpgradesInSlots.Count} random cards!");

        // Loop through your physical UI buttons on screen and update them with the 3 chosen upgrades
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            if (i >= activeUpgradesInSlots.Count) break;

            UpgradeData upgrade = activeUpgradesInSlots[i];

            if (buttonTextLabels[i] != null)
            {
                buttonTextLabels[i].text = $"<b>{upgrade.upgradeName}</b>\n{upgrade.description}";
            }
        }

        //rest the upgrade buttons to be clickable again
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].interactable = true;
        }
    }

        public void SelectSlot0() => ExecuteUpgrade(0);
        public void SelectSlot1() => ExecuteUpgrade(1);
        public void SelectSlot2() => ExecuteUpgrade(2);

    private void ExecuteUpgrade(int slotIndex)
    {
        //check if already upgraded
        if (madeSelection) return;

        // Safety check to ensure we clicked a valid drawn card slot
        if (slotIndex >= activeUpgradesInSlots.Count) return;

        //lock in upgrade has been pressed
        madeSelection = true;

        // Grab our card data
        UpgradeData chosenUpgrade = activeUpgradesInSlots[slotIndex];
        string variableName = chosenUpgrade.upgradeID;

        // 1. Fetch the number right out of Unity's global Scene variables table
        float currentNumber = Unity.VisualScripting.Variables.Application.Get<float>(variableName);

        // 2. Add your upgrade card's modifier value to it
        float newNumber = currentNumber + chosenUpgrade.modifierValue;

        // 3. Force the upgraded number straight back into the global Scene variables map
        Unity.VisualScripting.Variables.Application.Set(variableName, newNumber);

        Debug.Log($"[UpgradeManager] SUCCESS! Upgraded Scene Variable '{variableName}' from {currentNumber} to {newNumber}!");

        // 4. If player chose upgrade, disable upgrade buttons
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].interactable = false;
        }
    }
}