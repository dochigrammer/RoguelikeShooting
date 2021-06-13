using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private float CurrentTime = 0.0f;
    private float SpawnIntervalTime = 0.0f;
    private float GameElapsedTime = 0.0f;

    public float SpawnIntervalMinTime = 3.0f;
    public float SpawnIntervalMaxTime = 10.0f;
    public GameObject EnemyGameObject = null;

    public int PoolSize = 10;
    public List<GameObject> EnemyObjectPool;

    public GameObject[] DropItems;
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
        GameElapsedTime += Time.deltaTime;

        if ( CurrentTime >= SpawnIntervalTime)
        {
            CurrentTime = 0.0f;

            SpawnEnemy();
        }
    }

    public void ReplaceEnemy( GameObject go)
    {
        EnemyObjectPool.Add(go);
        var enemy = go.GetComponent<Enemy>();
        go.SetActive(false);

        enemy.ResetForRespawn();
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

        
        SpawnIntervalTime = Mathf.Max( SpawnIntervalMinTime, (Random.Range(SpawnIntervalMinTime, SpawnIntervalMaxTime) - (GameElapsedTime * 0.03f)));
    }

    public void OnDropItem(Enemy _enemy)
    {
        Debug.Log("DropItem");
        int index = Random.Range(0, DropItems.Length - 1);

        var go = Instantiate(DropItems[index], _enemy.transform.position, new Quaternion());

        Debug.Log(go);

        go.SetActive(true);

    }
}
