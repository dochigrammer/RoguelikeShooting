using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class GunBaseComponent : WeaponBaseComponent
{
    public GunConfig GunConfig;

    public GameObject Bullet;
    public GameObject Muzzle;

    public int AdditionalDamage = 0;

    public int MaxMagazine = 5;
    public int MaxBulletCount = 30;

    public int PoolSize = 10;

    protected int _CurrentMagazine = 5;
    protected int _BulletCount = 30;

    public int GetBulletCount() { return _BulletCount; }

    protected float _CurrentDeviation = 5.0f;
    protected float _CurrentRecoil = 0.0f;

    protected bool _IsReloading = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _CurrentMagazine = MaxMagazine;
        _BulletCount = MaxBulletCount;
    }

    private void Update()
    {
        DriveFireSystem();
    }

    public override EPrepareMotion GetPrepareMotion()
    {
        if( _BulletCount > 0)
        {
            return EPrepareMotion.NA;
        }
        else
        {
            return EPrepareMotion.Reload;
        }
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

    public void Reload()
    {
        if (!_IsReloading)
        {
            _IsReloading = true;

            StartCoroutine("ReloadWait");
            
            if( GunConfig.ReloadSound != null)
            {
                Instantiate(GunConfig.ReloadSound, gameObject.transform);
            }
        }
    }

    public IEnumerator ReloadWait()
    {
        yield return new WaitForSeconds(1.0f);

        _IsReloading = false;
        _BulletCount = MaxBulletCount;
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

    public override float GetWeaponDamage() { return GunConfig.Damage + AdditionalDamage; }

    public override bool IsAttackable()
    {
        return IsExpiredCoolTime() && _BulletCount > 0 && _IsReloading == false;
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

        --_BulletCount;

        // �ѱ� ȭ�� ����Ʈ
        Instantiate(GunConfig.MuzzleFireEffect, muzzle_transform);

        // �ѱ� ��� ����
        if(GunConfig.FireSound != null)
        {
            Instantiate(GunConfig.FireSound, transform);
        }

        // �ѱ� �ݵ�
        _CurrentRecoil = Mathf.Min( _CurrentRecoil + GunConfig.IncreasedRecoilPerShot, GunConfig.Recoil_Max );
        _CurrentDeviation = Mathf.Min( _CurrentDeviation + GunConfig.IncreasedDeviationPerShot, GunConfig.Deviation_Max );

        // �ѱ��� �ѱ��κп��� �������������� ������
        var direction = (attack_location - muzzle_transform.position).normalized;

        RaycastHit hit_result;
        // �ѱ��� �ѱ��κп��� ������������ �浹�ϴ� ��ü�� �ִ��� ���� �˻�
        if( Physics.Raycast(muzzle_transform.position, direction, out hit_result, GunConfig.FireDistance) )
        {
            // �浹�� ��ü�� ������
            if( hit_result.collider != null )
            {
                // �Ѿ� �ǰ� ����Ʈ
                Instantiate(GunConfig.TrajectileHitEffect, hit_result.point, new Quaternion(0, 0, 0, 0));

                CharaBaseComponent chara = hit_result.collider.gameObject.GetComponent<CharaBaseComponent>();

                // �ش� ��ü�� ĳ���Ͷ��
                if ( chara != null)
                {
                    // ������� �ݴϴ�.
                    chara.OnHit(this);
                }
                return true;
            }
        }

        return false;
    }



}
