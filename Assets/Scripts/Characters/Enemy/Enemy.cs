using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : CharaBaseComponent
{
    protected const string PLAYER_NAME = "Player";
    protected const float ACTION_TICK_INTERVAL = 2.0f;

    protected float _ProperDistance = 25.0f;

    protected EEnemyState _CurrentEnemyState;
    protected CharaBaseComponent _PerceivedPlayer;
    protected float _DecideNewActionRemainTime = 0.0f;
    protected Vector3 _GoalLocation = Vector3.zero;
    protected NavMeshAgent _NavAgent;

    public GameObject _ExplosionFactory;

    // Start is called before the first frame update

    public override void Start()
    {
        base.Start();
        _NavAgent = GetComponent<NavMeshAgent>();

        _NavAgent.updatePosition = false;
        _NavAgent.updateRotation = true;
    }

    public override void Update()
    {
        base.Update();
        DriveEnemyBehavior();
    }

    protected bool CanExecuteNewAction() { return _DecideNewActionRemainTime <= 0.0f; }

    public override void OnHit(WeaponBaseComponent _weapon)
    {
        base.OnHit(_weapon);

        if( _weapon != null && _PerceivedPlayer == null)
        {
            _PerceivedPlayer = _weapon.GetOwner();
            ChangeState(EEnemyState.Idle);
            Debug.Log("OnHit");
        }
    }

    public override void OnDied()
    {
        ScoreManager.Instance.AddScore(50);
        base.OnDied();
    }


    protected void DriveEnemyBehavior()
    {
        _DecideNewActionRemainTime -= Time.deltaTime;

        DirveRotation();

        switch (_CurrentEnemyState)
        {
            case EEnemyState.NearbySerach: // 주위를 살핌
                NearbySerachPlayer();
                break;

            case EEnemyState.ProperLocationMove: // 적을 발견했을때 적절한 위치로 이동
                ProperLocationMove();
                break;

            case EEnemyState.Attack: // 적에게 공격 행동을 가함
                DriveAttack();
                break;

            case EEnemyState.Idle: // 일정시간 쉬면서 다음 행동을 결정함
                DriveIdle();
                break;
        }
    }

    protected void ChangeState( EEnemyState _state)
    {
        _CurrentEnemyState = _state;
        _DecideNewActionRemainTime = 0.0f;
    }

    protected void NearbySerachPlayer()
    {
        const float max_distance = 20.0f;

        if (CanExecuteNewAction())
        {
            _GoalLocation = transform.position + new Vector3(Random.Range(-max_distance, max_distance), 0.0f, Random.Range(-max_distance, max_distance));

            if( _NavAgent.SetDestination(_GoalLocation) )
            {
                _DecideNewActionRemainTime = ACTION_TICK_INTERVAL;
            }
        }

        MoveToGoalLocation();
        using ( var player_enumerator = BattleGameMnager.Instance.GetPlayerCharas() )
        {
            // 모든 플레이어의 캐릭터 순회
            while(player_enumerator.MoveNext())
            {
                var player_chara = player_enumerator.Current;

                if(player_chara != null)
                {
                    Vector3 distance_direction =  (player_chara.transform.position - transform.position).normalized;

                    // 플레이어의 거리와 현재 AI의 방향이 90도 이내일때
                    if( Vector3.Dot(transform.forward, distance_direction) >= 0.0f )
                    {
                        
                        Ray to_player_ray = new Ray()
                        {
                            origin = transform.position + Vector3.up,
                            direction = distance_direction
                        };
                        // 현재위치에서 목표지점으로 레이를 쏴서 보이는지 확인
                        RaycastHit hit;
                        if (Physics.Raycast(to_player_ray, out hit, 1000.0f))
                        {
                            if (hit.collider != null)
                            {
                                var hit_object = hit.collider.gameObject;

                                player_chara = hit_object.GetComponentInParent<PlayerCharaComponent>();
                                if(player_chara != null )
                                {
                                    // 충돌검사로 해당 플레이어가 보였다고 판단되면 해당 플레이어를 경계할 수 있도록 값을 저장
                                    _PerceivedPlayer = player_chara;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }


        if (_PerceivedPlayer != null)
        {
            ChangeState(EEnemyState.ProperLocationMove);
        }
    }

    protected void ProperLocationMove()
    {
        if ( _PerceivedPlayer != null)
        {
            if (CanExecuteNewAction())
            {
                _GoalLocation = GetProperLocationByPlayer();
                if (_NavAgent.SetDestination(_GoalLocation))
                {
                    _DecideNewActionRemainTime = ACTION_TICK_INTERVAL;
                }
            }

            MoveToGoalLocation();

            float goal_distance = Vector3.Distance(transform.position, _GoalLocation);
            
            if( goal_distance <= 10.0f)
            {
                ChangeState(EEnemyState.Idle);
            }

        }

    }

    protected void DriveAttack()
    {
        Debug.Log("Attack");
        if (CanExecuteNewAction())
        {
            Vector3 random_deviation = new Vector3( Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f) );
            
            GetWeaponComponent().ExecuteAttack(_PerceivedPlayer.transform.position + Vector3.up + random_deviation);

            ChangeState(EEnemyState.Idle);

            _DecideNewActionRemainTime = ACTION_TICK_INTERVAL;
        }
    }

    protected void DirveRotation()
    {
        if (_PerceivedPlayer != null)
        {
            Vector3 look_direction = _PerceivedPlayer.transform.position - transform.position;
            look_direction.y = 0.0f;
            look_direction.Normalize();

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(look_direction), Time.deltaTime * 32.0f);
        }
    }

    protected void DriveIdle()
    {
        MoveToGoalLocation();

        if (CanExecuteNewAction())
        {
            if (_PerceivedPlayer == null)
            {
                ChangeState(EEnemyState.NearbySerach);
            }
            else
            {
                float distance = Vector3.Distance(_PerceivedPlayer.transform.position, _GoalLocation);

                bool is_force_move = Random.Range(0, 4) == 0;

                if ( is_force_move)
                {
                    ChangeState(EEnemyState.ProperLocationMove);
                }
                else
                {
                    ChangeState(EEnemyState.Attack);
                }
            }
        }
    }

    protected void MoveToGoalLocation()
    {
        Vector3 goal_distance = transform.position - _GoalLocation;

        if (!goal_distance.IsNearlyEqual(Vector3.zero, 1.0f))
        {
            _CharaController.Move(_NavAgent.velocity * Time.deltaTime);

            transform.position = _NavAgent.nextPosition;
        }
    }


    protected Vector3 GetProperLocationByPlayer()
    {
        Vector3 target_location = transform.position;

        if (_PerceivedPlayer != null)
        {
            target_location = _PerceivedPlayer.transform.position;
        }

        float proper_distance_half = _ProperDistance * 0.25f;

        return target_location + new Vector3(Random.Range(-proper_distance_half, proper_distance_half), 0.0f, Random.Range(-proper_distance_half, proper_distance_half));
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

            GunBaseComponent player_gun = GameObject.Find("Player").GetComponent<GunBaseComponent>();

            player_gun.BulletObjectPool.Add(other.gameObject);
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
