using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryScript : MonoBehaviour
{
    public List<WeaponSlotScript> weaponSlots;
    public List<Equipment> weapons;
    private KeyCode[] keyCodes = {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4
    };

    void Update()
    {
        foreach (KeyCode code in keyCodes)
        {
            if (Input.GetKey(code))
            {
                string slotString = code.ToString();
                int slotId = slotString[5] - '0';
                slotId--;

                if (weaponSlots[slotId].weapon != null && weaponSlots[slotId].weapon != PlayerScript.playerInstance.heldItem)
                    PlayerScript.playerInstance.EquipWeapon(weaponSlots[slotId].weapon);

                return;
            }
        }

    }

    public void AddItemToInventory(Equipment eq)
    {
        var query = from slot in weaponSlots where slot.weapon == null select slot;
        query.First().SetItem(eq);
    }
    public void RemoveItemFromInventory(Equipment eq)
    {
        var query = from slot in weaponSlots where slot.weapon == eq select slot;
        query.First().RemoveItem();
    }
}
