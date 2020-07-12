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

    [SerializeField] int enemyHealth = 50;
    [SerializeField] int healthIncrement = 10;
    [SerializeField] int enemyDefense = 0;
    [SerializeField] int defenseIncrement = 1;
    [SerializeField] int enemyDamage = 20;
    [SerializeField] int damageIncrement = 10;

    private float enemyUpgrade = 50.0f;
    private int currUpgrade;

    Timers timers;

    void Awake() {
         statCount = new int[3];
         currHealth = enemyHealth;
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
        // Enemy Stat Boost
        if (timers.Expired("upgrade")) {
           statCount[currUpgrade]++;
           currUpgrade = (currUpgrade + 1) % 3;
           timers.StartTimer("upgrade");
        }
    }
}
