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
    public float speed = 100;
    public float carTorque = 30;

    //ABHI: added int variale to keep track of score
    public int score = 0;
    public static event Action<bool> playerDead;

    private float movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float oldMovement = movement;

        if (isPlayer1)
            movement = Input.GetAxisRaw("Horizontal_P1");
        else
            movement = Input.GetAxisRaw("Horizontal_P2");

        if (oldMovement != movement)
        {
            rb.totalTorque = 0;
            backTire.totalTorque = 0;
            frontTire.totalTorque = 0;
        }
    }

    private void FixedUpdate()
    {
        backTire.AddTorque(-movement * speed * Time.fixedDeltaTime);
        frontTire.AddTorque(-movement * speed * Time.fixedDeltaTime);
        rb.AddTorque(movement * carTorque * Time.fixedDeltaTime);
    }

    public void Die()
    {
        StartCoroutine(DieTimer());
    }

    IEnumerator DieTimer()
    {
        Time.timeScale = 0.25f;

        yield return new WaitForSeconds(1f);

        Time.timeScale = 1f;
        playerDead?.Invoke(isPlayer1);
    }
}
