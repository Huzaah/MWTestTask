using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager mainCanvas;
    public GameObject playerHealthBar;
    public GameObject itemPickUpMsg;
    public InventoryScript inventory;
    public GameObject gameOverScreen;
    private void Awake()
    {
        if (mainCanvas == null)
        {
            mainCanvas = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool TogglePickUpMsg(bool isOn)
    {
        itemPickUpMsg.SetActive(isOn);
        Debug.Log("Pick message toggled " + isOn);
        return isOn;
    }
}
