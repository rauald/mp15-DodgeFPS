using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public GameObject GameObject { get; }

    public void TakeDamage(int damage);
}