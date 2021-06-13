using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharaStats
{
    public float MaxHP;
    public float MoveVelocity;
}

[System.Serializable]
public struct CharaAdditionalStats
{
    public float IncreasedHP;
    public float IncreasedVelocity;
    public float IncreasedDamage;


    public static CharaAdditionalStats operator +(CharaAdditionalStats _dst,CharaAdditionalStats _src )
    {
        _dst.IncreasedHP += _src.IncreasedHP;
        _dst.IncreasedVelocity += _src.IncreasedVelocity;
        _dst.IncreasedDamage += _src.IncreasedDamage;

        return _dst;
    }
}