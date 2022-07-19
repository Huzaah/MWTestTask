using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlotScript : MonoBehaviour
{
    public Equipment weapon;
    public Image icon;

    public void SetItem(Equipment eq)
    {
        if (weapon != null)
            return;

        weapon = eq;
        icon.sprite = eq.imageDisplay;
        icon.gameObject.SetActive(true);
    }
    public void RemoveItem()
    {
        weapon = null;
        icon.gameObject.SetActive(false);
    }
}
