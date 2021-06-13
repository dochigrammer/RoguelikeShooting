using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    protected GameObject _TargetObject;
    public GameObject Effect;
    public CharaAdditionalStats Stats;

    public void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if( player != null)
        {
            _TargetObject = player;
        }
    }

    public void Update()
    {
        DriveMovement();
    }

    protected void DriveMovement()
    {
        const float collision_enter_distance = 1.0f;
        const float move_speed = 2.0f;

        if( _TargetObject != null)
        {
            Vector3 dest_location = _TargetObject.transform.position;
            Vector3 current_location = transform.position;

            Vector3 result_location =  Vector3.Lerp(current_location, dest_location, move_speed * Time.deltaTime);

            transform.position = result_location;

            Vector3 distance = dest_location - result_location;

            Debug.Log(distance.sqrMagnitude);
            if ( distance.magnitude <= collision_enter_distance * collision_enter_distance)
            {
                OnApplyEffects();
                Destroy(gameObject);
            }

        }
    }


    public virtual void OnApplyEffects()
    {
        GameObject effect = Instantiate(Effect, gameObject.transform.position,new Quaternion());

        var player = _TargetObject.GetComponent<PlayerCharaComponent>();

        if ( player != null)
        {
            player.OnReceiveItem(this);
            player.IncreaseStats(Stats);
        }
    }


}
