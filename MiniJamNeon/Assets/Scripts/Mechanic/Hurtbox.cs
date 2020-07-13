using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Hurtbox : MonoBehaviour
{
	// Attached to a status
	BoxCollider2D box;	
	public Collider2D Bounds {
		get { return box; }
	}

	PlayerStats statusP;
  EnemyStats statusE; 

	void Awake() {
		box = GetComponent<BoxCollider2D>();
		statusP = GetComponentInParent<PlayerStats>();
    statusE = GetComponentInParent<EnemyStats>();
	}

	public void RegisterHit(int damage) {
     if(statusP != null) {
        statusP.hit(damage);
        if(statusP.getHealth() <= 0) {
            // Game Over, Player has died
        }
     } else if(statusE != null) {
       statusE.hit(damage);
       if(statusE.getHealth() <= 0) {
           Destroy(this.transform.parent);
       }
     } else {
       // Something is wrong
     }
	}
}