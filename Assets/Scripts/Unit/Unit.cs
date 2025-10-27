using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    protected float _hp;
    protected int _attackPoint;
    protected int _defensePoint;
    public abstract void Attack();
    public abstract void TakenDamage(float Damage);
}
