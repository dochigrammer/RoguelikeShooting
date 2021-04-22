using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    CharaComponent Chara;
    // Start is called before the first frame update
    void Start()
    {
        if(Chara == null)
        {
            Chara = GetComponent<CharaComponent>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        DriveMove();
        DriveAttack();
    }

    protected void DriveMove()
    {
        float horizontal_input = Input.GetAxis("Horizontal");
        float vertical_input = Input.GetAxis("Vertical");

        Vector3 input_direction = new Vector3(horizontal_input, 0.0f, vertical_input);

        if (Chara != null)
        {
            Chara.PerfromMove(input_direction);
        }
    }

    protected void DriveAttack()
    {
        if(Input.GetButtonDown("Fire1") )
        {
            if( Chara != null )
            {
                Chara.DoAttack();
            }
        }
    }
}
