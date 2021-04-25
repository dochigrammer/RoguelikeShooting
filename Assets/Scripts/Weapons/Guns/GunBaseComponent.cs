using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GunBaseComponent : WeaponBaseComponent
{
    public GunConfig GunConfig;

    public GameObject Bullet;
    public GameObject Muzzle;

    public int PoolSize = 10;
    public List<GameObject> BulletObjectPool;

    protected float _CurrentDeviation = 5.0f;
    protected float _CurrentRecoil = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        BulletObjectPool = new List<GameObject>();

        for (int i = 0; i < PoolSize; ++i)
        {
            GameObject bullet = Instantiate(Bullet);

            bullet.SetActive(false);

            BulletObjectPool.Add(bullet);
        }
    }

    private void Update()
    {
        DriveFireSystem();
    }

    public float GetCurrentRecoil() { return _CurrentRecoil; }

    protected void DriveFireSystem()
    {
        if(_CurrentRecoil >= GunConfig.Recoil_Min)
        {
            _CurrentRecoil -= GunConfig.RecoveryRecoilSpeed * Time.deltaTime;

            if( _CurrentRecoil <= GunConfig.Recoil_Min)
            {
                _CurrentDeviation = GunConfig.Recoil_Min;
            }
        }

        if( _CurrentDeviation >= GunConfig.Deviation_Min)
        {
            _CurrentDeviation -= GunConfig.RecoveryDeviationSpeed * Time.deltaTime;

            if (_CurrentDeviation <= GunConfig.Deviation_Min)
            {
                _CurrentDeviation = GunConfig.Deviation_Min;
            }
        }
    }


    public override CrosshairInfo GetCrosshairInfo()
    {
        return new CrosshairInfo()
        {
            CrossHairType = ECrossHairType.Reticle,
            CrossHairSize = 20.0f,
            CurrentDeviation = _CurrentDeviation
        };
    }

    public override float GetWeaponDamage() { return GunConfig.Damage; }

    public override bool IsAttackable()
    {
        return IsExpiredCoolTime() && BulletObjectPool.Count > 0;
    }

    public override bool IsExpiredCoolTime()
    {
        return true;
    }

    protected virtual Transform GetMuzzleTransform()
    {
        if( Muzzle != null)
        {
            return Muzzle.transform;
        }

        return transform;
    }

    protected override bool OnAttack(Vector3 attack_location)
    {
        Transform muzzle_transform = GetMuzzleTransform();

        // 총기 화염 이펙트
        Instantiate(GunConfig.MuzzleFireEffect, muzzle_transform);

        // 총기 사격 사운드
        if(GunConfig.FireSound != null)
        {
            Instantiate(GunConfig.FireSound, transform);
        }

        // 총기 반동
        _CurrentRecoil = Mathf.Min( _CurrentRecoil + GunConfig.IncreasedRecoilPerShot, GunConfig.Recoil_Max );
        _CurrentDeviation = Mathf.Min( _CurrentDeviation + GunConfig.IncreasedDeviationPerShot, GunConfig.Deviation_Max );

        // 총기의 총구부분에서 도착지점까지의 방향계산
        var direction = (attack_location - muzzle_transform.position).normalized;

        RaycastHit hit_result;
        // 총기의 총구부분에서 도착지점으로 충돌하는 물체가 있는지 레이 검사
        if( Physics.Raycast(muzzle_transform.position, direction, out hit_result, GunConfig.FireDistance) )
        {
            // 충돌한 물체가 있을때
            if( hit_result.collider != null )
            {
                // 총알 피격 이펙트
                Instantiate(GunConfig.TrajectileHitEffect, hit_result.point, new Quaternion(0, 0, 0, 0));

                CharaBaseComponent chara = hit_result.collider.gameObject.GetComponent<CharaBaseComponent>();

                // 해당 물체가 캐릭터라면
                if ( chara != null)
                {
                    // 대미지를 줍니다.
                    chara.OnHit(this);

                    return true;
                }
            }
        }

        return false;
    }

    public GameObject GetDeactivatedBullet()
    {
        foreach (var bullet in BulletObjectPool)
        {
            if (bullet.activeSelf == false)
            {
                BulletObjectPool.Remove(bullet);
                return bullet;
            }

        }

        return null;
    }

}
