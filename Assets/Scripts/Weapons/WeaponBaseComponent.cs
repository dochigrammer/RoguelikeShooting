using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBaseComponent : MonoBehaviour
{
    protected CharaBaseComponent _CharaOwner = null;



    public virtual EPrepareMotion GetPrepareMotion() { return EPrepareMotion.NA; }

    public void BindOwner( CharaBaseComponent _owner)
    {
        _CharaOwner = _owner;
    }

    public CharaBaseComponent GetOwner() { return _CharaOwner; }

    public virtual bool IsExpiredCoolTime()
    {
        return true;
    }


    public virtual CrosshairInfo GetCrosshairInfo()
    {
        return new CrosshairInfo()
        {
            CrossHairType = ECrossHairType.Dot,
            CrossHairSize = 0.0f,
            CurrentDeviation = 0.0f
        };
    }

    #region Attack Action
    protected abstract bool OnAttack( Vector3 attack_location);

    public virtual float GetWeaponDamage() { return 0.0f; }

    public virtual bool IsAttackable()
    {
        return IsExpiredCoolTime();
    }

    public bool ExecuteAttack( Vector3 _attack_location)
    {
        // 공격이 가능할경우
        if( IsAttackable())
        {
            // 공격을 시작
            return OnAttack(_attack_location);
        }

        return false;
    }
    #endregion


}
