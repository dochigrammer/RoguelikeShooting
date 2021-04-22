using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaComponent : MonoBehaviour
{
    public CharaStats _CharaStats;

    protected WeaponBaseComponent _Weapon = null;
    protected BaseController _Controller = null;

    protected bool IsContrllable()
    {
        return true;
    }

    protected bool IsAttackable()
    {
        return _Weapon != null && _Weapon.IsAttackable();
    }

    public void DoAttack()
    {
        if( IsAttackable())
        {
            _Weapon.Attack();
        }
    }

    public void PerfromMove( Vector3 _move_direction)
    {
        if( IsContrllable())
        {
            transform.position += _move_direction * _CharaStats.MoveVelocity * Time.deltaTime;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _Weapon = GetComponent<WeaponBaseComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit");
    }

}
