using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    public GameObject spellEffectPrefab;
    public LayerMask mask;
    public void StartSpell()
    {
        GameObject spellEffect = Instantiate(spellEffectPrefab, Vector3.zero, Quaternion.Euler(Vector3.forward), PlayerScript.playerInstance.transform);
        spellEffect.transform.localRotation = Quaternion.Euler(Vector3.forward);
        spellEffect.transform.localPosition = new Vector3(0f, 1.07f, 0.75f);
    }

}
