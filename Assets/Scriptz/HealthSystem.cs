/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem {

    public event Action OnHealthChanged;
    public event Action OnDead;

    private int healthMax;
    private int health;

    public HealthSystem(int healthMax) {
        this.healthMax = healthMax;
        health = healthMax;
    }

    public void SetHealthAmount(int health) {
        this.health = health;
        if (OnHealthChanged != null) OnHealthChanged();
    }

    public float GetHealthPercent() {
        return (float)health / healthMax;
    }

    public int GetHealthAmount() {
        return health;
    }

    public void Damage(int amount) {
        health -= amount;
        health = Mathf.Clamp(health, 0, healthMax);
        if (OnHealthChanged != null) OnHealthChanged();

        if (health <= 0) {
            Die();
        }
    }

    public void Die() {
        if (OnDead != null) OnDead();
    }

    public bool IsDead() {
        return health <= 0;
    }

    public void Heal(int amount) {
        health += amount;
        if (health > healthMax) {
            health = healthMax;
        }
        if (OnHealthChanged != null) OnHealthChanged();
    }

}
