using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraComponent : MonoBehaviour
{
    protected Camera _MainCameraComponent = null;
    protected GameObject _PossessedPlayerGameObject = null;

    public Vector3 CameraLocationOffset = new Vector3(0.0f, 3.0f, -4.0f);
    public Quaternion CameraRotationOffset = new Quaternion(-20.0f, 0.0f, 0.0f, 1.0f);

    public float CameraSmoothMinVelocity = 5.0f;
    public float CameraSmoothMaxVelocity = 10.0f;
    public float CameraMaxDistance = 1.0f;

    protected float CameraMaxDistance_DivideFactor = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        CameraMaxDistance_DivideFactor = 1.0f / CameraMaxDistance;

        _MainCameraComponent = GetComponent<Camera>();
        _PossessedPlayerGameObject = GameObject.FindGameObjectWithTag("Player");

        Debug.Assert(_PossessedPlayerGameObject != null);
        Debug.Assert(_MainCameraComponent != null);
    }

    // Update is called once per frame
    void Update()
    {
        DriveCamera();
    }

    protected void DriveCamera()
    {
        Vector3 target_location = _PossessedPlayerGameObject.transform.position + CameraLocationOffset;
        Vector3 location_distance =  target_location - transform.position;

        float distance_factor = Mathf.Clamp(location_distance.magnitude * CameraMaxDistance_DivideFactor, 0.0f, 1.0f);

        transform.position += location_distance * Mathf.Clamp(EasingUtil.EaseOutSine(CameraSmoothMinVelocity, CameraSmoothMaxVelocity, distance_factor) * Time.deltaTime, 0.0f, 1.0f );

        transform.rotation = CameraRotationOffset;
    }
}
