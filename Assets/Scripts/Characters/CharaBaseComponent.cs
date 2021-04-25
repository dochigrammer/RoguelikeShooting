using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaBaseComponent : MonoBehaviour
{
    protected CharacterController _CharaController = null;
    protected WeaponBaseComponent _Weapon = null;

    protected float _CurrentHP = 100.0f;

    public CharaStats _CharaStats;

    protected float _Gravity_scale = 10.0f;

    public WeaponBaseComponent GetWeaponComponent() { return _Weapon; }

    public float GetCurrentHPPercentage()
    {
        if ( _CurrentHP <= 0.0f || _CharaStats.MaxHP <= 0.0f )
        {
            return 0.0f;
        }

        return _CurrentHP / _CharaStats.MaxHP;
    }

    public virtual void OnHit( WeaponBaseComponent _weapon )
    {
        if( _weapon != null)
        {
            _CurrentHP -= _weapon.GetWeaponDamage();

            if( _CurrentHP <= float.Epsilon)
            {
                _CurrentHP = 0.0f;

                OnDied();
            }
        }
    }

    protected virtual void OnChangeWeapon()
    {
        _Weapon.BindOwner(this);
    }

    public virtual void OnDied()
    {
        Destroy( this.gameObject );
    }

    public virtual void Start()
    {
        _CharaController = GetComponent<CharacterController>();
        _Weapon = GetComponentInChildren<WeaponBaseComponent>();

        _CurrentHP = _CharaStats.MaxHP;

        OnChangeWeapon();
    }

    public virtual void Update()
    {
        DriveGravity();
    }

    protected void DriveGravity()
    {
        // Gravity 
        Vector3 gravity_vector = Vector3.down * Time.deltaTime * _Gravity_scale;
        _CharaController.Move(gravity_vector);
    }
}
