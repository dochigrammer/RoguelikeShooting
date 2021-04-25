using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    protected RectTransform _RectTransform_Crosshair = null;

    public delegate CrosshairInfo FUNC_GetCrosshairInfo();

    protected FUNC_GetCrosshairInfo _Func_GetCrosshairInfo;

    public void Start()
    {
        _RectTransform_Crosshair = GetComponent<RectTransform>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void Update()
    {
        UpdateCrosshair();
    }

    public void BindCrosshairInfo(FUNC_GetCrosshairInfo _func_get_crosshair_info)
    {
        if( _func_get_crosshair_info != null )
        {
            _Func_GetCrosshairInfo = _func_get_crosshair_info;
        }
    }

    public void UpdateCrosshair()
    {
        if ( _Func_GetCrosshairInfo != null)
        {
            var cross_hair_info = _Func_GetCrosshairInfo.Invoke();

            switch( cross_hair_info.CrossHairType)
            {
                case ECrossHairType.Reticle:
                    UpdateReticle(cross_hair_info);
                    break;
            }
        }
    }

    protected void UpdateReticle( in CrosshairInfo _info)
    {
        float cross_hair_size = (_info.CrossHairSize + _info.CurrentDeviation);
        _RectTransform_Crosshair.sizeDelta = new Vector2(cross_hair_size, cross_hair_size);
    }
}