using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoWepon : Weapon
{

    public NoWepon()
    {
        lockDirectionDuringAttack = true;
        lockMovementDuringAttack = true;
    }
    public override void updateDelay()
    {

    }
    public override void normalHold(PlayerMovementScript player)
    {

    }



    public override void altAttack(PlayerMovementScript player)
    {

    }

    public override bool attacking()
    {
        return false;
    }
}

