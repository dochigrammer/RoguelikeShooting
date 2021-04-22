using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBaseComponent : MonoBehaviour
{
    public GameObject Bullet;
    public GameObject Muzzle;

    public int PoolSize = 10;
    public List<GameObject> BulletObjectPool;

    public bool IsAttackable()
    {
        return IsExpiredCoolTime() && BulletObjectPool.Count > 0;
    }
       
    public bool IsExpiredCoolTime()
    {
        return true;
    }

    public void Attack()
    {
        GameObject bullet = GetDeactivatedBullet();

        if ( bullet != null)
        {
            Vector3 initial_location = gameObject.transform.position;

            if(Muzzle != null)
            {
                initial_location = Muzzle.transform.position;
            }

            bullet.SetActive(true);
            bullet.transform.position = initial_location;
        }
    }

    public GameObject GetDeactivatedBullet()
    {
        foreach( var bullet in BulletObjectPool)
        {
            if( bullet.activeSelf == false)
            {
                BulletObjectPool.Remove(bullet);
                return bullet;
            }

        }

        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        BulletObjectPool = new List<GameObject>();

        for( int i = 0; i < PoolSize; ++i)
        {
            GameObject bullet = Instantiate(Bullet);

            bullet.SetActive(false);

            BulletObjectPool.Add(bullet);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
