using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.gameObject.name.Contains("Bullet"))
        {
            other.gameObject.SetActive(false);

            WeaponBaseComponent player_weapon = GameObject.Find("Player").GetComponent<WeaponBaseComponent>();

            player_weapon.BulletObjectPool.Add(other.gameObject);

        }
        else if( other.gameObject.name.Contains("Enemy"))
        {
            other.gameObject.SetActive(false);

            GameObject go_enemy_manager = GameObject.Find("EnemyManager");
            var enemy_manager = go_enemy_manager.GetComponent<EnemyManager>();

            enemy_manager.EnemyObjectPool.Add(other.gameObject);
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
