using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(TimeController))]
public class EnemyStats : MonoBehaviour
{
    private const int HEALTH = 0;
    private const int DEFENSE = 1;
    private const int DAMAGE = 2;
    private const int DEFENSE_SCALE = 10;

    private int[] statCount;
    private int currHealth;

    [SerializeField] int enemyHealth = 50;
    [SerializeField] int healthIncrement = 10;
    [SerializeField] int enemyDefense = 0;
    [SerializeField] int defenseIncrement = 1;
    [SerializeField] int enemyDamage = 20;
    [SerializeField] int damageIncrement = 10;

    private float enemyUpgrade = 50.0f;


    void Awake() {
         statCount = new int[3];
         currHealth = enemyHealth;
    }

    public int getScaledDamage(int damage) {
        return (int) (damage * ((float) DEFENSE_SCALE / (DEFENSE_SCALE + statCount[DEFENSE])));
    }

    public void hit(int damage) {
        currHealth -= getScaledDamage(damage);
    }

    public void incrementHealth() {
        statCount[HEALTH]++;
        currHealth += healthIncrement;
    }

    public int getHealth() {
        return currHealth;
    }

    public int getMaxHealth() {
        return enemyHealth + (statCount[HEALTH] * healthIncrement);
    }

    public void setHealthStat(int health) {
        statCount[HEALTH] = health;
    }

    public void setDefenseStat(int defense) {
        statCount[DEFENSE] = defense;
    }

    public void incrementDefense() {
        statCount[DEFENSE]++;
    }

    public int getDefense() {
        return enemyDefense + (statCount[DEFENSE] * defenseIncrement);
    }

    public void setDamageStat(int damage) {
        statCount[DAMAGE] = damage;
    }

    public void incrementDamage() {
       statCount[DAMAGE]++; 
    }

    public int getDamage() {
        return enemyDamage + (statCount[DAMAGE] * damageIncrement);
    }

    void Update() {
       for(int i = 0; i < 3; i++) {
           statCount[i] = Math.Max(0, (int) ((TimeController.clock.timeElapsedSeconds - i * enemyUpgrade) / (3 * enemyUpgrade)));
       }
    }
}
