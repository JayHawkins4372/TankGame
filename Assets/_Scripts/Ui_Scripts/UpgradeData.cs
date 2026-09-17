//Author: Wade lawler
//Last Modified: 9/17/26
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrades/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    [TextArea]
    public string description;

    // An identifier so the player script knows what stat to change when clicked
    // naming format: "SHELL_SPEED", "FIRE_RATE", "MAX_AMMO"
    public string upgradeID;

    public float modifierValue;
}
