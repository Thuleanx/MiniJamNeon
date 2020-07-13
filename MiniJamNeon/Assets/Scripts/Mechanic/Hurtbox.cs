using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class Hurtbox : MonoBehaviour
{
	// Attached to a status
	BoxCollider2D box;	
	public Collider2D Bounds {
		get { return box; }
	}
  CharacterAnimationController anim;

	PlayerStats statusP;
  EnemyStats statusE; 

	void Awake() {
		box = GetComponent<BoxCollider2D>();
		statusP = GetComponentInParent<PlayerStats>();
    statusE = GetComponentInParent<EnemyStats>();
    anim = GetComponentInParent<CharacterAnimationController>();
	}

	public void RegisterHit(int damage) {
    if (canBeHit()) {
     if(statusP != null) {
        statusP.hit(damage);
        anim?.SetState(AnimState.Hit); 

        if(statusP.getHealth() <= 0) {
            // Game Over, Player has died
            AudioManager.Instance.Play("lose");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
     } else if(statusE != null) {
       statusE.hit(damage);
       if(statusE.getHealth() <= 0) {
           Destroy(this.transform.parent.gameObject);
       }
     } else {
       // Something is wrong
       print("oof");
     }
    }
	}
  public bool canBeHit() {
    return anim == null || anim.State != AnimState.Hit; 
  }
}