using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerScript : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public HealthBarScript healthBar;
    public static PlayerScript playerInstance; // Паттерн Синглтон
    public Animator animator;
    public CharacterController controller;
    public Transform cam;
    public float moveSpeed;
    public float rotateSmoothTime;
    private float rotateSmoothVelocity;
    public Transform anchorL;
    public Transform anchorR;
    float veloccityZ;
    float velocityX;
    public Equipment heldItem;
    public enum PlayerState
    {
        AFK = 0,
        Attacking = 1,
        Blocking = 2
    }
    public PlayerState playerState;
    private void Awake()
    {

        if (playerInstance == null)
        {
            playerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxhealth(maxHealth);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Attack();
        else if (Input.GetKeyDown(KeyCode.Mouse1))
            Block();

        Movement();
    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            if (heldItem != null)
            {
                DropHeldItem();
            }
        }
    }
    private void Movement()
    {
        float hr = Input.GetAxis("Horizontal");
        float ve = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(hr, 0, ve).normalized;
        Vector3 moveDir = new Vector3(0f, 0f, 0f);

        if (dir.magnitude > 0.1)
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotateSmoothVelocity, rotateSmoothTime);
            transform.rotation = Quaternion.Euler(0f, cam.eulerAngles.y, 0f);

            moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.SimpleMove(moveDir.normalized * moveSpeed);

            // Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
            // RaycastHit hitInfo;

            // if (Physics.Raycast(ray, out hitInfo, 10, mask))
            // { 
            //     Debug.DrawLine(ray.origin, hitInfo.point, Color.green);
            //     transform.position = hitInfo.point + Vector3.up * 0.1f;
            // }
        }
        animator.SetFloat("Velocity X", hr);
        animator.SetFloat("Velocity Z", ve);
    }
    public void Attack()
    {
        if (heldItem == null)
            return;
        animator.SetTrigger("Attacked");
        playerState = PlayerState.Attacking;

        if (heldItem.weaponType == WeaponType.Spell)
        {
            StartCoroutine(ShootSpell());
        }
    }
    private IEnumerator ShootSpell()
    {

        yield return new WaitForSeconds(1.04f);
        heldItem.GetComponent<SpellScript>().StartSpell();
    }
    public void Block()
    {
        animator.SetTrigger("Blocked");
        playerState = PlayerState.Blocking;
    }
    public void TakeDamage(float damageTaken)
    {
        if (playerState != PlayerState.Attacking)
            playerState = PlayerState.AFK;
        currentHealth -= damageTaken;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
            Die();
    }
    private void Die()
    {
        Destroy(gameObject);
        UIManager.mainCanvas.gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }
    public void PickUpItem(GameObject weaponObject)
    {
        GameObject newWeapon = Instantiate(weaponObject, Vector3.zero, Quaternion.identity, anchorR);
        Destroy(weaponObject);
        Equipment newWeaponEq = newWeapon.GetComponent<Equipment>();
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.Euler(newWeaponEq.rotationInHand);

        if (heldItem != null)
        {
            UIManager.mainCanvas.inventory.AddItemToInventory(newWeaponEq);
            newWeapon.SetActive(false);

            return;
        }

        UIManager.mainCanvas.inventory.AddItemToInventory(newWeaponEq);

        heldItem = newWeaponEq;
        SetAnimatorLayers(newWeaponEq);
    }
    public void EquipWeapon(Equipment eq)
    {
        if (heldItem != null)
        {
            SetAnimatorLayers(heldItem);
            heldItem.gameObject.SetActive(false);
        }

        heldItem = eq;
        heldItem.gameObject.SetActive(true);
        SetAnimatorLayers(heldItem);
    }
    public void DropHeldItem()
    {
        SetAnimatorLayers(heldItem);
        GameObject newItemStand = Instantiate(LevelControllerScript.levelController.weaponStandPrefab, transform.position + transform.forward + transform.up * 0.1f, Quaternion.identity);

        UIManager.mainCanvas.inventory.RemoveItemFromInventory(heldItem);

        // var query = from slot in UIManager.mainCanvas.inventory.weaponSlots where slot.weapon != null select slot;
        // EquipWeapon(query.First().weapon);

        newItemStand.GetComponent<WeaponStandScript>().SetItem(heldItem);
        heldItem = null;
    }
    private void SetAnimatorLayers(Equipment eq)
    {
        int id1 = (int)eq.weaponType;
        int id2 = (int)eq.weaponType + 1;
        if (id1 == 0)
            return;
        animator.SetLayerWeight(id1, 1f - animator.GetLayerWeight(id1));
        animator.SetLayerWeight(id2, 1f - animator.GetLayerWeight(id2));
    }
}
