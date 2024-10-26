using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public Sprite LockedIcon;
    public Sprite AbilityIcon;
    public string AbilityName;
    public GameObject AbilityGameProp;

    public bool IsUnlocked;

    void Start()
    {
        IsUnlocked = false;
        GetComponent<Image>().sprite = IsUnlocked ? AbilityIcon : LockedIcon;
        GetComponentInChildren<TextMeshProUGUI>().text = IsUnlocked ? AbilityName : "Locked";
    }
}
