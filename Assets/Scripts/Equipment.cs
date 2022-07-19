using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WeaponType
{
    Fists = 0,
    Sword = 2,
    GreatAxe = 4,
    Spell = 6
}

public class Equipment : MonoBehaviour
{
    public Sprite imageDisplay;
    public WeaponType weaponType;
    public float damage;
    public float block;
    public Vector3 rotationInHand;
    public Collider weaponCollider;

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Enter called");

        if (PlayerScript.playerInstance.gameObject == collider.gameObject &&
        PlayerScript.playerInstance.heldItem == this) // Если оружие столкнулось с игроком, и игрок дежит его
            return;

        if (PlayerScript.playerInstance.gameObject == collider.gameObject &&
        PlayerScript.playerInstance.heldItem != this) // Если оружие столкнулось с игроком, и игрок дежит не его
        {
            if (PlayerScript.playerInstance.playerState == PlayerScript.PlayerState.Blocking) // Если игрок блокирует
                PlayerScript.playerInstance.TakeDamage(damage - PlayerScript.playerInstance.heldItem.block); // Игрок получает уменьшенный урон
            else
                PlayerScript.playerInstance.TakeDamage(damage); // Игрок получает полный

            return;
        }

        if (collider.GetComponent<EnemyScript>() == null)
            return;

        if (collider.GetComponent<EnemyScript>().anchorR.GetChild(0).gameObject != gameObject && PlayerScript.playerInstance.playerState == PlayerScript.PlayerState.Attacking)
        {
            collider.GetComponent<EnemyScript>().TakeDamage(damage);
            PlayerScript.playerInstance.playerState = PlayerScript.PlayerState.AFK;
        }


    }
}
