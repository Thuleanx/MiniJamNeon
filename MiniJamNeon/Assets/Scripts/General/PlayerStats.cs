using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
	private const int HEALTH = 0;
	private const int DEFENSE = 1;
	private const int DAMAGE = 2;
	private const int DEFENSE_SCALE = 10;

	private int[] statCount;
	private int currHealth;

	private const int PLAYER_HEALTH = 100;
	private const int HEALTH_INCREMENT = 20;
	private const int PLAYER_DEFENSE = 0;
	private const int DEFENSE_INCREMENT = 1;
	private const int PLAYER_DAMAGE = 10;
	private const int DAMAGE_INCREMENT = 5;

	[SerializeField] Text debug;

	System.Random random;
	
	void Awake() {
		 statCount = new int[3];
		 currHealth = PLAYER_HEALTH;
		 random = new System.Random();
	}

	private int getStatUpgradeCost(int index) {
		return 100 * (1 + statCount[index]);
	}

	public int getScaledDamage(int damage) {
		return (int) Mathf.Ceil(damage * ((float) DEFENSE_SCALE / (DEFENSE_SCALE + statCount[DEFENSE])));
	}

	public void hit(int damage) {
		currHealth -= getScaledDamage(damage);        
	}

	public void incrementHealth() {
		statCount[HEALTH]++;
		currHealth = getMaxHealth();
	}

	public int getHealth() {
		return currHealth;
	}

	public int getMaxHealth() {
		return PLAYER_HEALTH + (statCount[HEALTH] * HEALTH_INCREMENT);
	}

	public int getHealthUpgradeCost() {
		return getStatUpgradeCost(HEALTH);
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
		return PLAYER_DEFENSE + (statCount[DEFENSE] * DEFENSE_INCREMENT);
	}

	public int getDefenseUpgradeCost() {
		return getStatUpgradeCost(DEFENSE);
	}

	public void incrementDamage() {
	   statCount[DAMAGE]++; 
	}

	public void setDamageStat(int damage) {
	   statCount[DAMAGE] = damage;
	}

	public int getDamage() {
		return PLAYER_DAMAGE + (statCount[DAMAGE] * DAMAGE_INCREMENT);
	}

	public int getDamageUpgradeCost() {
		return getStatUpgradeCost(DAMAGE);
	}

	void Update() {
		// HealthBar.Instance?.SetHealth((float) random.Next() / int.MaxValue);
		HealthBar.Instance?.SetHealth(((float) getHealth()) / getMaxHealth());	 
	}
}
