using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public bool IsStaggered { get; }

    public virtual void ChangeHealth(int value)
    {

    }

    public virtual void Stagger()
    {

    }
}
