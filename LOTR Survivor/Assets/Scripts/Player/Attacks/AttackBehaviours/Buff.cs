using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : AttackBehaviour
{
    protected override void Attack()
    {
        if (attackSettings == null)
            return;

        ApplyBuffs();
    }
}
