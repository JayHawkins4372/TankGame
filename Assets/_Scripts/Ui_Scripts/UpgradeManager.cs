//Author: Wade Lawler
//last Modified: 9/30/26
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
//using ShellSpawner;
//using Shell;


public class UpgradeManager : MonoBehaviour
{
    [Header("Upgrade Pool")]
    //All upgrades go into this list
    public List<UpgradeData> allPossibleUpgrades;

    [Header("UI Buttons")]
    //Ui Buttons 1-3
    public Button[] upgradeButtons;
    //Text components on the Ui buttons
    public TextMeshProUGUI[] buttonTextLabels;

    //references to other C# scripts
    public ShellSpawner shellSpawner;
    public Shell shell;

    //A temporary internal list to keep track of the 3 upgrades currently drawn
    private List<UpgradeData> activeUpgradesInSlots = new List<UpgradeData>();

    private bool madeSelection = false;

    public void PopulateUpgradeSlots()
    {
        //resets for new upgrade selection
        madeSelection = false;
        Debug.Log($"[UpgradeManager] PopulateUpgradeSlots() was called! Master Pool Count: {allPossibleUpgrades.Count}");
        // Clear out previous cards
        activeUpgradesInSlots.Clear();

        //Stop if there's not at least 3 upgrade cards added
        if (allPossibleUpgrades.Count < 3)
        {
            Debug.LogError("[UpgradeManager] Need at least 3 upgrades in your master pool list!");
            return;
        }

        //Create a temporary copy of your master pool
        List<UpgradeData> temporaryPool = new List<UpgradeData>(allPossibleUpgrades);

        //Run loop 3 times to pick upgrade choices at random
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, temporaryPool.Count);
            UpgradeData selectedUpgrade = temporaryPool[randomIndex];

            activeUpgradesInSlots.Add(selectedUpgrade);
        //Prevents duplicates
            temporaryPool.RemoveAt(randomIndex); 
        }

        Debug.Log($"[UpgradeManager] Successfully generated {activeUpgradesInSlots.Count} random cards!");

        //Loop through UI buttons on screen and update them with the 3 chosen upgrades
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

        //Safety check to ensure clicked is a valid drawn card slot
        if (slotIndex >= activeUpgradesInSlots.Count) return;

        //lock in upgrade has been pressed
        madeSelection = true;

        //Grab card data
        UpgradeData chosenUpgrade = activeUpgradesInSlots[slotIndex];
        string variableName = chosenUpgrade.upgradeID;

        if (Unity.VisualScripting.Variables.Application.IsDefined(variableName))
        {
            //Fetch the variable number from Unity's global Scene variables table
            float currentNumber = Unity.VisualScripting.Variables.Application.Get<float>(variableName);

            //Add upgrade cards modifier value
            float newNumber = currentNumber + chosenUpgrade.modifierValue;

            //Force new number back into the global Scene variables map
            Unity.VisualScripting.Variables.Application.Set(variableName, newNumber);

            Debug.Log($"[UpgradeManager] SUCCESS! Upgraded Scene Variable '{variableName}' from {currentNumber} to {newNumber}!");
        }
        else
        {
            switch (variableName)
            {
                case "damage":
                    shellSpawner.baseShellDamage += chosenUpgrade.modifierValue;
                    Debug.Log($"[UpgradeManager] C# Upgrade SUCCESS! damage is now {shellSpawner.baseShellDamage}");
                    break;

                case "spawnInterval":
                    shellSpawner.spawnInterval -= chosenUpgrade.modifierValue;
                    Debug.Log($"[UpgradeManager] C# Upgrade SUCCESS! fireRate is now {shellSpawner.spawnInterval}");
                    break;
            }
        }
            //If player chose upgrade, disable upgrade buttons
            for (int i = 0; i < upgradeButtons.Length; i++)
            {
                upgradeButtons[i].interactable = false;
            }
    }
}