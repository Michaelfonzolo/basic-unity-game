using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour {

    private Rigidbody2D myRigidbody;
    private Animator myAnimator;

    /// <summary>
    /// How quickly the player moves.
    /// </summary>
    [SerializeField]
    private float Speed;

    /// <summary>
    /// Whether or not the player is facing right.
    /// </summary>
    [SerializeField]
    private bool FacingRight = true;

    /// <summary>
    /// Whether or not the player is on the ground.
    /// </summary>
    [SerializeField]
    private bool OnGround = false;

    /// <summary>
    /// The amount by which we multiply the force applied to the player upon jumping.
    /// </summary>
    [SerializeField]
    private float JumpHeight;

    [SerializeField]
    private float BulletSpeed;

    public Rigidbody2D Bullet;

    private string[] ThingsThatKillRobots =
    {
        "Water",
        "Death"
    };

	// Use this for initialization
	void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        float horizontalMovement = Input.GetAxis("Horizontal");

        Move(horizontalMovement);

        Flip(horizontalMovement);

        Jump();

        Shoot();
	}

    private void Move(float horizontalMovement)
    {
        myAnimator.SetFloat("speed", Mathf.Abs(horizontalMovement));

        myRigidbody.velocity = new Vector2(Speed * horizontalMovement, myRigidbody.velocity.y);
    }

    private void Flip(float horizontalMovement)
    {
        if ((horizontalMovement > 0 && !FacingRight) || 
            (horizontalMovement < 0 && FacingRight))
        {
            FacingRight = !FacingRight;

            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            OnGround = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (ThingsThatKillRobots.Contains(other.tag))
        {
            Debug.Log("Congratulations! The robot is dead.");
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && OnGround)
        {
            myRigidbody.AddForce(Vector2.up * JumpHeight, ForceMode2D.Impulse);
            OnGround = false;
        }
    }

    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Rigidbody2D bulletClone = (Rigidbody2D)Instantiate(Bullet, transform.position, transform.rotation);
            bulletClone.velocity = (FacingRight ? Vector2.right : Vector2.left) * BulletSpeed;
        }
    }
}
