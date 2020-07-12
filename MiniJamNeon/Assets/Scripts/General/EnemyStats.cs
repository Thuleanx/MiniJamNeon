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

    private const int ENEMY_HEALTH = 50;
    private const int ENEMY_DEFENSE = 0;
    private const int ENEMY_DAMAGE = 10;

    private float enemyUpgrade = 50.0f;
    private int currUpgrade;

    Timers timers;

    void Awake() {
         statCount = new int[3];
         setHealth(ENEMY_HEALTH);
         setDefense(ENEMY_DEFENSE);
         setDamage(ENEMY_DAMAGE);
         timers = GetComponent<Timers>();
    }

    void Start() {
         currUpgrade = 0;
         timers.RegisterTimer("upgrade", enemyUpgrade);
         timers.StartTimer("upgrade");
    }

    public float getScaledDamage(float damage) {
        return damage * ((float) DEFENSE_SCALE / (DEFENSE_SCALE + statCount[DEFENSE]));
    }

    public void setHealth(int health) {
        statCount[HEALTH] = health;
    }

    public void incrementHealth() {
        statCount[HEALTH]++;
    }

    public int getHealth() {
        return statCount[HEALTH];
    }

    public void setDefense(int defense) {
        statCount[DEFENSE] = defense;
    }

    public void incrementDefense() {
        statCount[DEFENSE]++;
    }

    public int getDefense() {
        return statCount[DEFENSE];
    }

    public void setDamage(int damage) {
        statCount[DAMAGE] = damage;
    }

    public void incrementDamage() {
       statCount[DAMAGE]++; 
    }

    public int getDamage() {
        return statCount[DAMAGE];
    }

    void Update() {
        // Enemy Stat Boost
        if(timers.Expire("upgrade")) {
           statCount[currUpgrade]++;
           currUpgrade = (currUpgrade + 1) % 3;
           timers.StartTimer("upgrade");
        }
    }
}
