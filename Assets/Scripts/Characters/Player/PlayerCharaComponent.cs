using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharaComponent : CharaBaseComponent
{
    
    protected BaseController _Controller = null;
    protected Crosshair _CrosshairUI = null;

    protected bool IsContrllable()
    {
        return true;
    }

    protected bool IsAttackable()
    {
        return _Weapon != null && _Weapon.IsAttackable();
    }

    public void DoAttack(Vector3 _attack_location)
    {
        // �ش繫��� ���� ����
        if( _Weapon.ExecuteAttack(_attack_location))
        {
            // ������ �����Ұ�� �ش� �������� ȸ��
            Vector3 look_at_location = _attack_location;

            // Y���� ���� ��ġ�� ���������ν� Up������ ���� ��������.
            look_at_location.y = transform.position.y;

            transform.LookAt( look_at_location);
        }
    }

    public void PerfromMove( Vector3 _move_direction)
    {
        if( IsContrllable())
        {
            _CharaController.Move(_move_direction * _CharaStats.MoveVelocity * Time.deltaTime);
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
