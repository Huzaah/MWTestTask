using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStandScript : MonoBehaviour
{
    [SerializeField] private Equipment heldItem;
    public float degreesPerSecond = 15f;
    public float amplitude = 0.2f;
    public float frequency = 0.5f;
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();
    public Transform itemAnchor;
    private Transform itemTransform;
    private Transform playerTransform;
    public float viewDistance;
    public bool playerInRange;
    public LayerMask mask;

    void Start()
    {
        playerTransform = PlayerScript.playerInstance.transform;
        GetComponent<BoxCollider>().enabled = true;

        if (itemAnchor.childCount > 0)
        {
            SetItem(itemAnchor.GetChild(0).GetComponent<Equipment>());
        }
        else
        {
            Debug.Log("No weapon in stand!");
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (heldItem != null)
            SpinItem();

    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.E) && playerInRange)
        {
            PlayerScript.playerInstance.PickUpItem(heldItem.gameObject);
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        if (playerTransform == null)
            return;
        if (collider.gameObject.layer == playerTransform.gameObject.layer)
        {
            playerInRange = true;
            UIManager.mainCanvas.TogglePickUpMsg(playerInRange);
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject == playerTransform.gameObject)
        {
            playerInRange = false;
            UIManager.mainCanvas.TogglePickUpMsg(playerInRange);
        }
    }
    public void SetItem(Equipment itemEq)
    {
        itemTransform = itemEq.transform;
        heldItem = itemEq;
        posOffset = itemTransform.localPosition;

        itemTransform.SetParent(itemAnchor);
        itemTransform.localPosition = Vector3.zero;
        //Debug.Log(posOffset);
    }
    void SpinItem()
    {
        itemTransform.Rotate(new Vector3(0, Time.deltaTime * degreesPerSecond, 0), Space.World);

        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        itemTransform.localPosition = tempPos;
    }
    void OnDestroy()
    {
        playerInRange = false;
        try
        {
            UIManager.mainCanvas.TogglePickUpMsg(playerInRange);
        }
        catch { Debug.Log("OnDestroy called after play mode end."); }

    }
}
