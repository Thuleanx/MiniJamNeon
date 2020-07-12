using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Timers))]
public class EnemyStats : MonoBehaviour
{
    private const int HEALTH = 0;
    private const int DEFENSE = 1;
    private const int DAMAGE = 2;
    private const int DEFENSE_SCALE = 10;

    private int[] statCount;
    private int currHealth;

    private const int ENEMY_HEALTH = 50;
    private const int HEALTH_INCREMENT = 10;
    private const int ENEMY_DEFENSE = 0;
    private const int DEFENSE_INCREMENT = 1;
    private const int ENEMY_DAMAGE = 20;
    private const int DAMAGE_INCREMENT = 10;

    private float enemyUpgrade = 50.0f;
    private int currUpgrade;

    Timers timers;

    void Awake() {
         statCount = new int[3];
         currHealth = ENEMY_HEALTH;
         timers = GetComponent<Timers>();
    }

    void Start() {
         currUpgrade = 0;
         timers.RegisterTimer("upgrade", enemyUpgrade);
         timers.StartTimer("upgrade");
    }

    public int getScaledDamage(int damage) {
        return (int) (damage * ((float) DEFENSE_SCALE / (DEFENSE_SCALE + statCount[DEFENSE])));
    }

    public void hit(int damage) {
        currHealth -= getScaledDamage(damage);
    }

    public void incrementHealth() {
        statCount[HEALTH]++;
        currHealth += HEALTH_INCREMENT;
    }

    public int getHealth() {
        return currHealth;
    }

    public int getMaxHealth() {
        return ENEMY_HEALTH + (statCount[HEALTH] * HEALTH_INCREMENT);
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
        return ENEMY_DEFENSE + (statCount[DEFENSE] * DEFENSE_INCREMENT);
    }

    public void setDamageStat(int damage) {
        statCount[DAMAGE] = damage;
    }

    public void incrementDamage() {
       statCount[DAMAGE]++; 
    }

    public int getDamage() {
        return ENEMY_DAMAGE + (statCount[DAMAGE] * DAMAGE_INCREMENT);
    }

    void Update() {
        // Enemy Stat Boost
        if (timers.Expired("upgrade")) {
           statCount[currUpgrade]++;
           currUpgrade = (currUpgrade + 1) % 3;
           timers.StartTimer("upgrade");
        }
    }
}
