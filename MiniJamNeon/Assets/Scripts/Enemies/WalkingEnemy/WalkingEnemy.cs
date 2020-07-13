using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : MonoBehaviour
{
    private IEnemyState currentState;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public float minIdleDuration = 3f;
    public float maxIdleDuration = 7f;
    public float minPatrolDuration = 3f;
    public float maxPatrolDuration = 7f;

    private bool facingRight = true;
    public float movementSpeed = 6f;
    public bool isRanged = false;

    [HideInInspector]
    public GameObject Target;

    private bool activated = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated && collision.tag == "Activation")
        {
            activated = true;
            ChangeState(new IdleState());
        }

        if (activated)
        {
            currentState.OnTriggerEnter(collision);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            currentState.Execute();
            if (Target != null)
            {
                LookAtTarget();
            }
        }
        
        
    }

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(this);
    }

    public void SetSpeed(float speed)
    {
        animator.SetFloat("speed", speed);
    }

    public void Move()
    {
        animator.SetFloat("speed", 1);
        
        transform.Translate(GetDirection() * movementSpeed * Time.deltaTime);
    }

    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }

    public void ChangeDirection()
    {
        facingRight = !facingRight;
        Debug.Log("facingRight: " + facingRight);
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
        //spriteRenderer.flipX = !facingRight;
    }

    

    private void LookAtTarget()
    {
        float xDir = Target.transform.position.x - transform.position.x;
        if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
        {
            ChangeDirection();
        }
        
    }

  
}
