using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationCamera : MonoBehaviour
{
    protected float _RotationSpeed = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        var rotate_angle = transform.rotation.eulerAngles + new Vector3(0.0f, _RotationSpeed, 0.0f) * Time.deltaTime;

        transform.rotation = Quaternion.Euler(rotate_angle);
    }
}
