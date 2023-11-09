using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isPlayer1;

    private Rigidbody2D rb;
    public Rigidbody2D backTire;
    public Rigidbody2D frontTire;
    public float speed = 100;
    public float carTorque = 30;

    private float movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isPlayer1)
            movement = Input.GetAxisRaw("Horizontal_P1");
        else
            movement = Input.GetAxisRaw("Horizontal_P2");
    }

    private void FixedUpdate()
    {
        backTire.AddTorque(-movement * speed * Time.fixedDeltaTime);
        frontTire.AddTorque(-movement * speed * Time.fixedDeltaTime);
        rb.AddTorque(movement * carTorque * Time.fixedDeltaTime);
    }
}
