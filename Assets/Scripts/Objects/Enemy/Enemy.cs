using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected const string PLAYER_NAME = "Player";

    protected Vector3 MoveDirection = Vector3.zero;
    public float Velocity = 5;

    public GameObject _ExplosionFactory;

    // Start is called before the first frame update

    public void OnEnable()
    {
        int rand = Random.Range(0, 10);

        if (rand < 3)
        {
            var target_player = GameObject.Find(PLAYER_NAME);

            if (target_player != null)
            {
                MoveDirection = target_player.transform.position - transform.position;
                MoveDirection.Normalize();
            }
        }

        if (MoveDirection.Equals(Vector3.zero))
        {
            MoveDirection = Vector3.down;
        }
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += MoveDirection * Velocity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject explosion = Instantiate(_ExplosionFactory);

        if( explosion == null)
        {
            Debug.LogWarning("explosion Instantiate Fail");
        }

        if( explosion != null)
        {
            explosion.transform.position = transform.position;
        }

        if( other.gameObject.name.Contains("Bullet"))
        {
            other.gameObject.SetActive(false);

            WeaponBaseComponent player_weapon = GameObject.Find("Player").GetComponent<WeaponBaseComponent>();

            player_weapon.BulletObjectPool.Add(other.gameObject);
        }
        else
        {
            Destroy(other.gameObject);
        }

        gameObject.SetActive(false);

        GameObject go_enemy_manager = GameObject.Find("EnemyManager");
        var enemy_manager = go_enemy_manager.GetComponent<EnemyManager>();

        enemy_manager.EnemyObjectPool.Add(other.gameObject);

        ScoreManager.Instance.AddScore(1);
    }
}
