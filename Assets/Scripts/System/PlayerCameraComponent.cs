using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraComponent : MonoBehaviour
{
    protected Camera _MainCameraComponent = null;
    protected GameObject    _CameraParentGameObject = null;
    protected BaseController _PossessedController = null;

    const float rotation_pitch_limit_angle = 45.0f;

    public float CameraSmoothMinVelocity = 15.0f;
    public float CameraSmoothMaxVelocity = 30.0f;
    public float CameraMaxDistance = 0.5f;
    public float CameraRotationVelocity = 16.0f;

    protected float CameraMaxDistance_DivideFactor = 0.0f;

    public bool BindController(BaseController _controller)
    {
        if( _PossessedController == null)
        {
            _PossessedController = _controller;
            InitializeCameraInfo();
            return true;
        }

        return false;
    }

    public void InitializeCameraInfo()
    {
        CameraMaxDistance_DivideFactor = 1.0f / CameraMaxDistance;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_MainCameraComponent == null)
            _MainCameraComponent = GetComponent<Camera>();

        if (_CameraParentGameObject == null)
            _CameraParentGameObject = gameObject.transform.parent.gameObject;

        Debug.Assert(_MainCameraComponent != null);

        InitializeCameraInfo();
    }

    // Update is called once per frame
    void Update()
    {
        DriveCamera();
    }

    protected void DriveCamera()
    {
        if (_PossessedController == null || _CameraParentGameObject == null)
            return;

        var chara_component = _PossessedController.GetOwnerChara();

        Vector3 target_location = _PossessedController.gameObject.transform.position;
        
        // 무기사용으로 증가한 Recoil + 마우스 컨트롤로 인한 Rotation 값을 계산하여 Pitch값 계산
        var control_rot_x = Mathf.Clamp(_PossessedController.GetControlRotation().x - _PossessedController.GetCurrentCameraRecoil(),
            -rotation_pitch_limit_angle, -rotation_pitch_limit_angle); // 회전값 제한

        // 위로 일정 값으로 결정된 Pitch값과 유저의 Input으로 변한 Yaw값으로 목표 회전값을 정함
        Quaternion target_rotation = Quaternion.Euler(control_rot_x, _PossessedController.GetControlRotation().y, 0.0f);
        
        if (chara_component != null)
        {
            // 현재 소유중인 캐릭터의 위치가 타겟
            target_location = chara_component.gameObject.transform.position;
        }

        // 카메라와 캐릭터의 거리를 구함
        Vector3 location_distance =  target_location - _CameraParentGameObject.transform.position;

        // 그 거리에 비례하여 카메라 속도를 조정하기 위한 값을 구함
        float distance_factor = Mathf.Clamp(location_distance.magnitude * CameraMaxDistance_DivideFactor, 0.0f, 1.0f);

        // 위의 값을 활용하여 카메라의 부드러운 이동 제공 ( 거리가 짧으면 느리게 멀면 빠르게 )
        _CameraParentGameObject.transform.position += location_distance * 
            Mathf.Clamp(EasingUtil.EaseOutSine(CameraSmoothMinVelocity, CameraSmoothMaxVelocity, distance_factor) * Time.deltaTime, 0.0f, 1.0f );

        // 위의 결정된 회전값으로 일정 보간하여 현재값에 대입
        _CameraParentGameObject.transform.rotation = Quaternion.Slerp(_CameraParentGameObject.transform.rotation, target_rotation, CameraRotationVelocity * Time.deltaTime);
    }
}
