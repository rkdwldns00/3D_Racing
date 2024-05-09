using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : Car
{
    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Boost();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Break();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPos();
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Rotate(Input.GetAxis("Horizontal"));
        Move(Input.GetAxis("Vertical"));
    }
}
