using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseController : MonoBehaviour
{
    public HUD_HP HUD_HP;

    protected PlayerCharaComponent _PlayerChara;
    protected Vector3 _ControlRotation = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        InitailizeComponents();

        if( HUD_HP != null)
        {
            HUD_HP.BindChara(_PlayerChara);
        }
    }


    // Update is called once per frame
    void Update()
    {
        DriveMove();
        DriveRotation();
        DriveAttack();
    }

    public PlayerCharaComponent GetOwnerChara()
    {
        return _PlayerChara;
    }

    public Vector3 GetControlRotation()
    {
        return _ControlRotation;
    }

    public float GetCurrentCameraRecoil()
    {
        if(_PlayerChara != null)
        {
            var weapon = _PlayerChara.GetWeaponComponent();

            var gun = weapon as GunBaseComponent;

            if( gun != null)
            {
                return gun.GetCurrentRecoil();
            }
        }
        return 0.0f;
    }

    protected void InitailizeComponents()
    {
        if (_PlayerChara == null)
        {
            _PlayerChara = GetComponent<PlayerCharaComponent>();
        }

        var main_camera_game_object = GameObject.FindGameObjectWithTag("MainCamera");

        if (main_camera_game_object != null)
        {
            var player_camera = main_camera_game_object.GetComponent<PlayerCameraComponent>();
            if (player_camera != null)
            {
                player_camera.BindController(this);
            }
        }
    }


    protected void DriveRotation()
    {
        float mouse_input_x = Input.GetAxis("Mouse X");
        float mouse_input_y = Input.GetAxis("Mouse Y");

        Vector3 input_rotation = new Vector3(-mouse_input_y, mouse_input_x, 0.0f);

        _ControlRotation += input_rotation * Time.deltaTime * 300.0f;

        _ControlRotation.x = Mathf.Clamp(_ControlRotation.x, -45.0f, 45.0f);
        // transform.rotation += Quaternion.Euler();

        _ControlRotation.y = _ControlRotation.y % 360.0f;

        if (_PlayerChara != null )
        {
            _PlayerChara.PerformRotate(_ControlRotation);
        }
    }

    protected void DriveMove()
    {
        float horizontal_input = Input.GetAxis("Horizontal");
        float vertical_input = Input.GetAxis("Vertical");

        Vector3 input_direction = Quaternion.Euler(0.0f, _ControlRotation.y, 0.0f) * new Vector3(horizontal_input, 0.0f, vertical_input);

        if ( _PlayerChara != null && !input_direction.IsNearlyEqual( Vector3.zero))
        {
            _PlayerChara.PerfromMove(input_direction);
        }
    }

    protected void DriveAttack()
    {
        // ?????? ??????
        if(Input.GetButtonDown("Fire1") )
        {
            if( _PlayerChara != null ) // ???????? ???????? ????????
            {
                // ???????? ?????? ???????? ???? ???????? ?????? ???????? ?????? ???? ?????????? ????.
                Ray _attack_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                Vector3 attack_location = Vector3.zero;

                RaycastHit hit;
                if( Physics.Raycast(_attack_ray, out hit))
                {
                    // ???????? ?????? ???? ???????? ????
                    attack_location = hit.point;
                }
                else
                {
                    // ?????????? ???????? ???? ??, ???????? ?????? ???????? ?????? ???? ????
                    attack_location = _attack_ray.origin + _attack_ray.direction * 1000.0f;
                }

                // ???? ?????? ?????? ????
                _PlayerChara.DoAttack(attack_location);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _PlayerChara.DoReload();
        }
    }
}
