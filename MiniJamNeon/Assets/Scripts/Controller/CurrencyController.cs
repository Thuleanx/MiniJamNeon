
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyController : MonoBehaviour
{
	public static CurrencyController Instance;

	int currency = 1000;

	PlayerStats stat;
	CharacterAnimationController anim;

	[SerializeField] GameObject DamageUpgrade;
	[SerializeField] GameObject ArmourUpgrade;
	[SerializeField] GameObject HealthUpgrade;
	Money money;

	void Awake() {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		stat = player.GetComponent<PlayerStats>();
		anim = player.GetComponent<CharacterAnimationController>();
		money = GetComponent<Money>();
		if (Instance != null)
			Instance = this;
	}

	public void GainCurrency(int amount) {
		currency += amount;
		AudioManager.Instance.Play("currency");
	}

	// option either Damage, Health, or Armour
	public void TryUpdgrade(string option) {
		int amountRequired = 0;
		if (option == "Damage")
			amountRequired = stat.getDamageUpgradeCost();
		else if (option == "Armour")
			amountRequired = stat.getDefenseUpgradeCost();
		else
			amountRequired = stat.getHealthUpgradeCost();


		if (currency >= amountRequired) {
			print("HELLOOOOO");
			currency -= amountRequired;
			if (option == "Damage")
				stat.incrementDamage();
			else if (option == "Armour")
				stat.incrementDefense();
			else
				stat.incrementHealth();
			anim.SetState(AnimState.Shine);
			AudioManager.Instance.Play("lvl");
		}
	}

	public bool CanUpgrade(string option) {
		if (option == "Damage")
			return  stat.getDamageUpgradeCost() <= currency;
		else if (option == "Armour")
			return  stat.getDefenseUpgradeCost() <= currency;
		else
			return  stat.getHealthUpgradeCost() <= currency;
	}

	void Update() {
		DamageUpgrade.SetActive(CanUpgrade("Damage"));
		ArmourUpgrade.SetActive(CanUpgrade("Armour"));
		HealthUpgrade.SetActive(CanUpgrade("Health"));
		money.SetAmount(currency);
	}
}