using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private float CurrentTime = 0.0f;
    private float SpawnIntervalTime = 0.0f;

    public float SpawnIntervalMinTime = 0.5f;
    public float SpawnIntervalMaxTime = 1.5f;
    public GameObject EnemyGameObject = null;

    public int PoolSize = 10;
    public List<GameObject> EnemyObjectPool;

    public Transform[] SpawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        EnemyObjectPool = new List<GameObject>();

        for( int i = 0; i < PoolSize; ++i)
        {
            var enemy = Instantiate(EnemyGameObject);
            enemy.SetActive(false);

            EnemyObjectPool.Add(enemy);
        }

        SpawnIntervalTime = Random.Range(SpawnIntervalMinTime, SpawnIntervalMaxTime);
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTime += Time.deltaTime;

        if( CurrentTime >= SpawnIntervalTime)
        {
            CurrentTime = 0.0f;

            SpawnEnemy();
        }
    }


    protected GameObject GetDeactivatedEnemy()
    {
        foreach( var enemy in EnemyObjectPool)
        {
            if(!enemy.activeSelf)
            {
                EnemyObjectPool.Remove(enemy);
                return enemy;
            }
        }
        return null;
    }

    protected void SpawnEnemy()
    {
        GameObject enemy = GetDeactivatedEnemy();

        if( enemy != null)
        {
            enemy.SetActive(true);

            if( SpawnPoints.Length > 0 )
            {
                int index = Random.Range(0, SpawnPoints.Length);

                enemy.transform.position = SpawnPoints[index].transform.position;
            }
            else
            {
                enemy.transform.position = transform.position;
            }
        }
    }


}
