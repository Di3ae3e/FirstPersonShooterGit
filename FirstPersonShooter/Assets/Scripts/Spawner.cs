using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    void Start()
    {
       StartCoroutine(EnemySpawn());
    }

    IEnumerator EnemySpawn()
    {
        yield return new WaitForSeconds(3f);
        Instantiate(enemyPrefab, gameObject.transform);
        StartCoroutine(EnemySpawn());
    }
}
