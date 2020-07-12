using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private const int HEALTH = 0;
    private const int DEFENSE = 1;
    private const int DAMAGE = 2;
    private const int DEFENSE_SCALE = 10;

    private int[] statCount = new int[3];

    private const int PLAYER_HEALTH = 100;
    private const int PLAYER_DEFENSE = 0;
    private const int PLAYER_DAMAGE = 10;

    void Awake() {
         setHealth(PLAYER_HEALTH);
         setDefense(PLAYER_DEFENSE);
         setDamage(PLAYER_DAMAGE);
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

    }
}
