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
    // Drag ALL Upgrade Assets into this master list pool
    public List<UpgradeData> allPossibleUpgrades;

    [Header("UI Buttons")]
    // Assign your 3 UI Buttons here
    public Button[] upgradeButtons;
    // Assign the Text component for each of the 3 buttons
    public TextMeshProUGUI[] buttonTextLabels;

    // A temporary internal list to keep track of the 3 upgrades currently drawn
    private List<UpgradeData> activeUpgradesInSlots = new List<UpgradeData>();

    public void PopulateUpgradeSlots()
    {
        // Clear out whatever choices were listed from the last layout draw
        activeUpgradesInSlots.Clear();

        // Safety check: Stop the code if you haven't added at least 3 cards to your master pool yet
        if (allPossibleUpgrades.Count < 3)
        {
            Debug.LogError("[UpgradeManager] You need at least 3 upgrades in your master pool list!");
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

        // Loop through your physical UI buttons on screen and update them with the 3 chosen upgrades
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            if (i >= activeUpgradesInSlots.Count) break;

            UpgradeData upgrade = activeUpgradesInSlots[i];

            if (buttonTextLabels[i] != null)
            {
                // Formats the text: The name is displayed in bold, then skips down a line (\n) for the description
                buttonTextLabels[i].text = $"<b>{upgrade.upgradeName}</b>\n{upgrade.description}";
            }

            // Wipe out old button click connections from previous level-ups
            upgradeButtons[i].onClick.RemoveAllListeners();

            // Fixes a Unity loop memory quirk by locking the current index into its own unique temporary integer variable
            int slotIndex = i;

            // Tell the button: "When clicked, run the OnUpgradeSelected function and pass your exact slot placement index"
            upgradeButtons[i].onClick.AddListener(() => OnUpgradeSelected(slotIndex));
        }
    }

    private void OnUpgradeSelected(int slotIndex)
    {
        // Retrieve the exact upgrade card matching the button slot the player just clicked on
        UpgradeData chosenUpgrade = activeUpgradesInSlots[slotIndex];

        // 1. Instantly look through the scene hierarchy to find the GameObject tagged "Player"
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            // 2. Ask the player's S_Player graph: "What number is currently saved inside your variable matching our upgradeID?"
            float currentVal = Variables.Object(player).Get<float>(chosenUpgrade.upgradeID);

            // 3. Take that current number, add your upgrade card's modifier value to it, and inject it straight back into the graph variable
            Variables.Object(player).Set(chosenUpgrade.upgradeID, currentVal + chosenUpgrade.modifierValue);

            Debug.Log($"[UpgradeManager] Successfully upgraded '{chosenUpgrade.upgradeID}' by {chosenUpgrade.modifierValue}!");
        }
        else
        {
            Debug.LogError("[UpgradeManager] Missing Player! Ensure your player object has the 'Player' Tag applied.");
        }

        // Fetch the menu component on this same GameObject to close the UI panel and unpause gameplay
        UpgradeMenu menu = GetComponent<UpgradeMenu>();
        if (menu != null)
        {
            menu.ToggleMenu();
        }
    }
}