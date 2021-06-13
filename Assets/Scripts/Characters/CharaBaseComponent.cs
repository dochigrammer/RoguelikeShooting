using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaBaseComponent : MonoBehaviour
{
    protected CharacterController _CharaController = null;
    protected WeaponBaseComponent _Weapon = null;
    protected Animator _Animator = null;

    protected float _CurrentHP = 100.0f;
    protected Vector3 _LastMoveDirection = Vector3.zero;

    public CharaStats _CharaStats;

    protected CharaAdditionalStats _CharaAdditionalStats;

    protected float _Gravity_scale = 10.0f;

    public WeaponBaseComponent GetWeaponComponent() { return _Weapon; }

    public void IncreaseStats(CharaAdditionalStats _stats)
    {
        if( _stats.IncreasedHP > 0.0f )
        {
            _CurrentHP = Mathf.Min(_CurrentHP +_stats.IncreasedHP, _CharaStats.MaxHP);
        }

        _CharaAdditionalStats += _stats;

        if (_stats.IncreasedDamage > 0.0f)
        {
            var gun = _Weapon as GunBaseComponent;
            if( gun !=null)
            {
                gun.AdditionalDamage = (int)_CharaAdditionalStats.IncreasedDamage;
            }
        }

    }

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

    public virtual void ResetForRespawn()
    {
        _CurrentHP = 100.0f;
    }


    public virtual void OnDied()
    {
        Destroy( this.gameObject );
    }

    public virtual void Start()
    {
        _CharaController = GetComponent<CharacterController>();
        _Weapon = GetComponentInChildren<WeaponBaseComponent>();
        _Animator = GetComponent<Animator>();

        _CurrentHP = _CharaStats.MaxHP;

        OnChangeWeapon();
    }

    public virtual void Update()
    {
        DriveGravity();
        DriveAnimation();
    }


    protected void DriveAnimation()
    {
        if (_Animator != null)
        {
            int direction_x = Mathf.RoundToInt(_LastMoveDirection.x);
            int direction_z = Mathf.RoundToInt(_LastMoveDirection.z);

            _Animator.SetInteger("Velocity_X", direction_x);
            _Animator.SetInteger("Velocity_Z", direction_z);
        }
    }


    protected void DriveGravity()
    {
        // Gravity 
        Vector3 gravity_vector = Vector3.down * Time.deltaTime * _Gravity_scale;
        _CharaController.Move(gravity_vector);
    }
}
