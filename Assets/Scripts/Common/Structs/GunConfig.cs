using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GunConfig
{
    [Range(0.0f, float.MaxValue)]
    public float Deviation_Min;

    [Range(0.0f, float.MaxValue)]
    public float Deviation_Max;

    [Range(0.0f, float.MaxValue)]
    public float IncreasedDeviationPerShot;

    [Range(0.0f, float.MaxValue)]
    public float RecoveryDeviationSpeed;

    [Range(0.0f, float.MaxValue)]
    public float Recoil_Min;

    [Range(0.0f, float.MaxValue)]
    public float Recoil_Max;

    [Range(0.0f, float.MaxValue)]
    public float IncreasedRecoilPerShot;

    [Range(0.0f, float.MaxValue)]
    public float RecoveryRecoilSpeed;

    [Range(0, int.MaxValue)]
    public int MaxBulletPerMagazine;

    [Range(0, int.MaxValue)]
    public int  MaxMagazine;

    [Range(1, int.MaxValue)]
    public int ProjectileCountPerShot;

    [Range(1.0f, float.MaxValue)]
    public float FireDistance;

    public float Damage;

    // Sounds
    public GameObject FireSound;

    // Effects
    public GameObject MuzzleFireEffect;
    public GameObject TrajectileHitEffect;

}
