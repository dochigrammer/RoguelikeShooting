using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_HP : MonoBehaviour
{
    protected CharaBaseComponent _CharaOwner;
    protected Slider _Sldier;

    public void BindChara( CharaBaseComponent _chara)
    {
        _CharaOwner = _chara;
    }

    // Start is called before the first frame update
    void Start()
    {
        _Sldier = GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if( _Sldier != null )
        {
            float hp_per = 0.0f;
            if(_CharaOwner != null)
            {
                hp_per = _CharaOwner.GetCurrentHPPercentage();
            }

            _Sldier.value = hp_per;
        }
    }
}
