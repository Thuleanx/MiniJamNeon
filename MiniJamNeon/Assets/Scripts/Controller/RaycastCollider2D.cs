using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastCollider2D : MonoBehaviour
{
	const int DIM = 2;

	#region Components 
	[HideInInspector] public BoxCollider2D body;
	#endregion

	#region Unity constants
	[SerializeField] LayerMask collisionMask;
	[SerializeField] LayerMask platformMask;
	[SerializeField] bool drawGizmos;
	#endregion

	#region Raycasting constants
	const float skinWidth = .05f;

	[SerializeField, Range(2, 32)]
	public int horizontalRayCount = 4;
	[SerializeField, Range(2, 32)]
	public int verticalRayCount = 4;

	float horizontalRaySpacing, verticalRaySpacing;
	#endregion

	#region Raycast Origins
	RaycastOrigin rayCastOrigins;
	public struct RaycastOrigin {
		public Vector2[,] corners;
		public Vector2 Center { get { return (corners[0,0] + corners[1,1]) / 2; }}
		public void Init() {
			corners = new Vector2[DIM, DIM];
		}
	}
	void UpdateRayCastOrigins() {
		Bounds bounds = body.bounds;
		bounds.Expand(2 * -skinWidth);
		for (int i = 0; i < DIM; i++)
			for (int j = 0; j < DIM; j++)
				rayCastOrigins.corners[i, j] = new Vector2(i == 0 ? bounds.min.x : bounds.max.x, j == 0 ? bounds.min.y : bounds.max.y);
	}
	void PrecomputeRaySpacing() {
		Bounds bounds = body.bounds;
		bounds.Expand(2 * -skinWidth);

		horizontalRaySpacing = Mathf.Clamp(horizontalRaySpacing, 2, int.MaxValue);
		verticalRaySpacing = Mathf.Clamp(verticalRaySpacing, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);

		bounds = body.bounds;
		// bodySizeWithSkin = new Vector2(bounds.size.x, bounds.size.y);
	}		 
	#endregion

	#region Collision info 
	public struct CollisionInfo {
		public int collideMaskLeft, collideMaskRight, collideMaskBot, collideMaskTop;

		public RaycastCollider2D parentRaycast;

		public bool AnyLeft 	{ get { return collideMaskLeft != 0; } }
		public bool AnyRight 	{ get { return collideMaskRight != 0; }}
		public bool AnyBot 		{ get { return collideMaskBot != 0; }}
		public bool AnyTop 		{ get { return collideMaskTop != 0; }}

		public bool AllLeft 	{ get { return collideMaskLeft == (1<<parentRaycast.horizontalRayCount) - 1; }}
		public bool AllRight 	{ get { return collideMaskRight == (1<<parentRaycast.horizontalRayCount) - 1; }}
		public bool AllTop 		{ get { return collideMaskTop == (1<<parentRaycast.verticalRayCount) - 1; }}
		public bool AllBot 		{ get { return collideMaskBot == (1<<parentRaycast.verticalRayCount) - 1; }}

		public void Init(RaycastCollider2D raycastCollider) {
			parentRaycast = raycastCollider;
			Reset();
		}
		public void Reset() {
			collideMaskBot = collideMaskLeft = collideMaskTop = collideMaskRight = 0;
		}
	}

	[HideInInspector] public CollisionInfo collisionInfo;
	[HideInInspector] public CollisionInfo platformCollisionInfo;
	#endregion 

	void Awake() {
		body = GetComponent<BoxCollider2D>();	
		collisionInfo.Init(this);
	}

	void Start() {
		rayCastOrigins.Init();
		PrecomputeRaySpacing();

	}

	// faceDir -> -1 if left, 1 if right 
	public void Move(Vector2 velocity, bool platformFallThrough = false, float faceDir = 0) {
		UpdateRayCastOrigins();
		collisionInfo.Reset();
		platformCollisionInfo.Reset();

		Raycast(ref velocity, platformFallThrough, faceDir);		
		// this line is only for ease of writing camera code
		// if (!Mathf.Approximately(velocity.sqrMagnitude, 0))
		transform.Translate(velocity);
	}

	void Raycast(ref Vector2 velocity, bool platformFallThrough, float faceDir) {
		for (int dim = 0; dim < 2; dim++) {
			// Useful variables
			float dir = Mathf.Sign(velocity[dim]);
			float rayLength = Mathf.Abs(velocity[dim]) + skinWidth;

			if (faceDir != 0 && dim == 0) {
				dir = faceDir;
				rayLength = 2 * skinWidth;
			}	
			if (dim == 1 && rayLength <= skinWidth) {
				dir = -1;	
				rayLength = 2 * skinWidth;
			}

			float spacing = dim == 0 ? horizontalRaySpacing : verticalRaySpacing;
			Vector2 dirV = (dim == 0 ? Vector2.right : Vector2.up);
			Vector2 dirVPerp = (dim == 0 ? Vector2.up : Vector2.right);

			if (rayLength > skinWidth) {
				int rayCount = (dim == 0 ? horizontalRayCount : verticalRayCount);
				for (int ray = 0; ray < rayCount; ray++)
				{

					Vector2 rayStart = dim == 0 ? rayCastOrigins.corners[dir == -1 ? 0 : 1, 0] : rayCastOrigins.corners[0, dir == -1 ? 0 : 1];
					rayStart += dirVPerp * (spacing * ray + (dim == 1 ? velocity[dim ^ 1] : 0));

					RaycastHit2D hit = Physics2D.Raycast(rayStart, dirV * dir, rayLength, collisionMask);

					if (drawGizmos)
						Debug.DrawRay(rayStart, dirV * dir * rayLength, Color.magenta);
					
					if (hit) {
						velocity[dim] = Mathf.Max(0, Mathf.Min(Mathf.Abs(velocity[dim]), (hit.distance - skinWidth))) * dir;
						rayLength = Mathf.Max(2*skinWidth, hit.distance);

						// update collisionInfo
						collisionInfo.collideMaskBot |= (dim == 1 && dir < 0 ? 1 : 0) << ray;		
						collisionInfo.collideMaskTop |= (dim == 1 && dir > 0 ? 1 : 0) << ray;		
						collisionInfo.collideMaskLeft |= (dim == 0 && dir < 0 ? 1 : 0) << ray;		
						collisionInfo.collideMaskRight |= (dim == 0 && dir > 0 ? 1 : 0) << ray;		
					} else if (!platformFallThrough) {
						hit = Physics2D.Raycast(rayStart, dirV * dir, rayLength, platformMask);

						if (hit)
						{
							// platform raycast
							if (dir < 0 && dim == 1)
							{
								// velocity[dim] = (hit.distance - skinWidth) * dir;
								velocity[dim] = Mathf.Max(0, Mathf.Min(Mathf.Abs(velocity[dim]), (hit.distance - skinWidth))) * dir;
								rayLength = Mathf.Max(2 * skinWidth, hit.distance);
							}

							platformCollisionInfo.collideMaskBot |= (dim == 1 && dir < 0 ? 1 : 0) << ray;
							platformCollisionInfo.collideMaskTop |= (dim == 1 && dir > 0 ? 1 : 0) << ray;
							platformCollisionInfo.collideMaskLeft |= (dim == 0 && dir < 0 ? 1 : 0) << ray;
							platformCollisionInfo.collideMaskRight |= (dim == 0 && dir > 0 ? 1 : 0) << ray;
						}
					}
				}
			}
		}
	}

}
