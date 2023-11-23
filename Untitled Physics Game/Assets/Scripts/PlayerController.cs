using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public bool isPlayer1;

    private Rigidbody2D rb;
    public Rigidbody2D backTire;
    public Rigidbody2D frontTire;
    public float speed = 300;
    public float carTorque = 8;
    public float jumpForce = 25;
    public LayerMask collisionMask;

    public static event Action playerDead;

    private float movement;
    private float defaultCarTorque;
    private bool grounded;
    private int jumpCooldown = 3;
    private float jumpTimer;

    private CircleCollider2D backTireCollider;
    private CircleCollider2D frontTireCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        defaultCarTorque = carTorque;
        jumpTimer = jumpCooldown;

        backTireCollider = backTire.gameObject.GetComponent<CircleCollider2D>();
        frontTireCollider = frontTire.gameObject.GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        CheckWheelCollision();

        jumpTimer += Time.deltaTime;

        float oldMovement = movement;

        if(Input.GetKeyDown(KeyCode.H) && isPlayer1)
        {
            AudioManager.instance.PlayOneShot(AudioManager.AUDIO_CLIPS.HONK);
        }
        else if(Input.GetKeyDown(KeyCode.RightControl) && !isPlayer1)
        {
            AudioManager.instance.PlayOneShot(AudioManager.AUDIO_CLIPS.HONK);
        }

        if (isPlayer1)
            movement = Input.GetAxisRaw("Horizontal_P1");
        else
            movement = Input.GetAxisRaw("Horizontal_P2");

        if (oldMovement != movement)
        {
            backTire.angularVelocity = 0;
            frontTire.angularVelocity = 0;
        }

        if (isPlayer1)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                Jump();
            }
        }
    }

    private void FixedUpdate()
    {
        if (movement!=0)
        {
            backTire.AddTorque(-movement * speed * Time.fixedDeltaTime);
            frontTire.AddTorque(-movement * speed * Time.fixedDeltaTime);
            rb.AddTorque(movement * carTorque * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
        else
        {
            backTire.AddTorque(0);
            frontTire.AddTorque(0);
            rb.AddTorque(0);
        }
    }

    private void Jump()
    {
        if (!grounded)
            return;
        if (jumpTimer < jumpCooldown)
            return;

        jumpTimer = 0;

        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckWheelCollision()
    {
        Collider2D[] backTireCollisions = Physics2D.OverlapCircleAll(backTire.transform.position, backTireCollider.radius + 0.1f, collisionMask);
        Collider2D[] frontTireCollisions = Physics2D.OverlapCircleAll(frontTire.transform.position, frontTireCollider.radius + 0.1f, collisionMask);

        if (backTireCollisions.Length != 0 || frontTireCollisions.Length != 0)
        {
            carTorque = defaultCarTorque;
            grounded = true;
        }
        else
        {
            carTorque = defaultCarTorque * 2.5f;
            grounded = false;
        }
    }

    public void Die()
    {
        if(AudioManager.instance.STATE == AudioManager.GAME_STATES.PLAYING)
        {
            StartCoroutine(DieTimer());

            if (GameManager.instance != null)
                GameManager.instance.PlayerDied(isPlayer1);
        }
        return;
    }

    IEnumerator DieTimer()
    {
        Time.timeScale = 0.25f;

        yield return new WaitForSeconds(1f);

        Time.timeScale = 1f;
        playerDead?.Invoke();
    }
}
