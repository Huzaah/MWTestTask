using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControllerScript : MonoBehaviour
{
    public static LevelControllerScript levelController;
    public GameObject enemyPrefab;
    public GameObject weaponStandPrefab;
    public GameObject player;
    public float spawnDistance;
    public float spawnDistanceFromPlayer;
    public float spawnDelay;
    public List<GameObject> weaponsInLevel;
    void Awake()
    {
        if (levelController == null)
        {
            levelController = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        StartCoroutine(EnemySpawner());
    }
    private IEnumerator EnemySpawner()
    {
        while (true)
        {
            float x = Random.Range(-spawnDistance, spawnDistance);
            float z = Random.Range(-spawnDistance, spawnDistance);

            x += x > 0 ? spawnDistanceFromPlayer : -spawnDistanceFromPlayer;
            z += z > 0 ? spawnDistanceFromPlayer : -spawnDistanceFromPlayer;

            GameObject newEnemy = Instantiate(enemyPrefab, player.transform.position + new Vector3(x, 0f, z), Quaternion.identity, transform);
            Debug.Log("enemy spawned");
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
