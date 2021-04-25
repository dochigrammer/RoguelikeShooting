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
        
        // ���������� ������ Recoil + ���콺 ��Ʈ�ѷ� ���� Rotation ���� ����Ͽ� Pitch�� ���
        var control_rot_x = Mathf.Clamp(_PossessedController.GetControlRotation().x - _PossessedController.GetCurrentCameraRecoil(),
            -rotation_pitch_limit_angle, -rotation_pitch_limit_angle); // ȸ���� ����

        // ���� ���� ������ ������ Pitch���� ������ Input���� ���� Yaw������ ��ǥ ȸ������ ����
        Quaternion target_rotation = Quaternion.Euler(control_rot_x, _PossessedController.GetControlRotation().y, 0.0f);
        
        if (chara_component != null)
        {
            // ���� �������� ĳ������ ��ġ�� Ÿ��
            target_location = chara_component.gameObject.transform.position;
        }

        // ī�޶�� ĳ������ �Ÿ��� ����
        Vector3 location_distance =  target_location - _CameraParentGameObject.transform.position;

        // �� �Ÿ��� ����Ͽ� ī�޶� �ӵ��� �����ϱ� ���� ���� ����
        float distance_factor = Mathf.Clamp(location_distance.magnitude * CameraMaxDistance_DivideFactor, 0.0f, 1.0f);

        // ���� ���� Ȱ���Ͽ� ī�޶��� �ε巯�� �̵� ���� ( �Ÿ��� ª���� ������ �ָ� ������ )
        _CameraParentGameObject.transform.position += location_distance * 
            Mathf.Clamp(EasingUtil.EaseOutSine(CameraSmoothMinVelocity, CameraSmoothMaxVelocity, distance_factor) * Time.deltaTime, 0.0f, 1.0f );

        // ���� ������ ȸ�������� ���� �����Ͽ� ���簪�� ����
        _CameraParentGameObject.transform.rotation = Quaternion.Slerp(_CameraParentGameObject.transform.rotation, target_rotation, CameraRotationVelocity * Time.deltaTime);
    }
}
