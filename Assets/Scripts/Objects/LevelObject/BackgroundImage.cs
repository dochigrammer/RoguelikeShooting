using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundImage : MonoBehaviour
{
    public Material _BackgroundMaterial;
    public float _ScrollSpeed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = Vector2.up;

        if( _BackgroundMaterial != null)
        {
            _BackgroundMaterial.mainTextureOffset += direction * _ScrollSpeed * Time.deltaTime;
        }
    }
}
