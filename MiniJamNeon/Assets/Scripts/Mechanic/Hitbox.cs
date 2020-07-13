using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Hitbox : MonoBehaviour
{
	BoxCollider2D box;	
	CharacterAnimationController anim;

	[SerializeField] LayerMask hurtboxMask;

	static int maxHitboxResults = 10;
  public int damage;

	void Awake() {
		box = GetComponent<BoxCollider2D>();
	}

	public void setDamage(int d)
	{
		damage = d;
	}

	public List<Hurtbox> GetOverlappingHurtbox() {
		List<Hurtbox> results = new List<Hurtbox>();		
		Collider2D[] receiver = new Collider2D[maxHitboxResults];

		ContactFilter2D filter = new ContactFilter2D();
		filter.layerMask = hurtboxMask;
		filter.useLayerMask = true;

		int count = box.OverlapCollider(filter, receiver);

		for (int i = 0; i < count; i++) {
			// guaranteed to have hurtbox
			results.Add(receiver[i].GetComponent<Hurtbox>());
		}

	
		return results;
	}

  void Update() {
	  List<Hurtbox> hurtboxes = GetOverlappingHurtbox();
	  if(hurtboxes.Count > 0) {
		 
		 for(int i = 0; i < hurtboxes.Count; i++) {
			 hurtboxes[i].RegisterHit(damage);
		 }
		 GetComponentInParent<ExplodeOnHit>()?.Explode();
	  }
  }
}
