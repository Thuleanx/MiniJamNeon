using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public const int HEALTH = 0;
    public const int DEFENSE = 1;
    public const int DAMAGE = 2;
    public const int DEFENSE_SCALE = 10;

    int[] statCount = new int[3];

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
}
