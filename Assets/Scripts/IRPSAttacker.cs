using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRPSAttacker
{
    public GameManager.AttackType CurrentAttackType { get; }

    public virtual void EndAttack()
    {

    }
}
