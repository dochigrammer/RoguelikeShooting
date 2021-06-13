using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharaComponent : CharaBaseComponent
{
    protected BaseController _Controller = null;
    protected Crosshair _CrosshairUI = null;

    public GameObject ResultPopup = null;
    public Text BulletsText = null;

    protected bool IsContrllable()
    {
        return true;
    }


    public override void OnDied()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if(ResultPopup != null)
        {
            ResultPopup.SetActive(true);
        }
        base.OnDied();
    }

    protected bool IsAttackable()
    {
        return _Weapon != null && _Weapon.IsAttackable();
    }

    public void DoReload()
    {
        var gun = _Weapon as GunBaseComponent;

        if (gun != null)
        {
            _Animator.SetTrigger("ReloadAction");
            gun.Reload();
        }
    }

    public void DoAttack(Vector3 _attack_location)
    {
        switch( _Weapon.GetPrepareMotion() )
        {
            case EPrepareMotion.NA:
                break;

            case EPrepareMotion.Reload:
                DoReload();
                return;
        }


        // 해당무기로 공격 시작
        if ( _Weapon.ExecuteAttack(_attack_location))
        {
            // 공격이 가능할경우 해당 방향으로 회전
            Vector3 look_at_location = _attack_location;

            // Y값을 현재 위치로 대입함으로써 Up벡터의 축을 꺠지않음.
            look_at_location.y = transform.position.y;

            transform.LookAt( look_at_location);

            if( _Animator != null)
            {
                _Animator.SetTrigger("FireAction");
            }
        }
    }

    public void OnReceiveItem( ItemBase _item)
    {
        if( _item.GetType() == typeof(HealingItem))
        {
            ScoreManager.Instance.ShowNotify("Heal !!");
        }

        if (_item.GetType() == typeof(PowerUpItem))
        {
            ScoreManager.Instance.ShowNotify("Power Up !!");
        }

        if (_item.GetType() == typeof(SpeedUpItem))
        {
            ScoreManager.Instance.ShowNotify("Speed Up !!");
        }
    }

    public void PerfromMove( Vector3 _move_direction)
    {
        if( IsContrllable())
        {
            _CharaController.Move(_move_direction * (_CharaStats.MoveVelocity + _CharaAdditionalStats.IncreasedVelocity )* Time.deltaTime);

            _LastMoveDirection = _move_direction;
        }
    }

    public void PerformRotate( Vector3 _control_rotation )
    {
        const float rotation_speed = 10.0f;

        if (IsContrllable())
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0.0f, _control_rotation.y, 0.0f), Time.deltaTime * rotation_speed);
        }
    }

    public override void Start() 
    {
        InitializeComponents();

        base.Start();
        ScoreManager.Instance.ShowNotify("Game Start!");
    }

    public override void Update()
    {
        base.Update();

        DriveAnimation();

        var gun = _Weapon as GunBaseComponent;

        if ( BulletsText != null && gun != null)
        {
            BulletsText.text = string.Format("{0} / {1}", gun.GetBulletCount(), gun.MaxBulletCount);
        }
    }


    protected void InitializeComponents()
    {
        var main_canvas = GameObject.FindGameObjectWithTag("MainCanvas");

        if (main_canvas != null)
        {
            _CrosshairUI = main_canvas.GetComponentInChildren<Crosshair>();
        }
    }

    protected override void OnChangeWeapon()
    {
        base.OnChangeWeapon();

        if (_Weapon != null && _CrosshairUI != null)
        {
            _CrosshairUI.BindCrosshairInfo( new Crosshair.FUNC_GetCrosshairInfo( _Weapon.GetCrosshairInfo ) );
        }
    }


}
