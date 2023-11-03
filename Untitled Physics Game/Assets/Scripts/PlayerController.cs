using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isPlayer1;

    public Rigidbody2D backTire;
    public Rigidbody2D frontTire;
    public float speed = 50;

    private float movement;

    private void Update()
    {
        if (isPlayer1)
            movement = Input.GetAxis("Horizontal_P1");
        else
            movement = Input.GetAxis("Horizontal_P2");
    }

    private void FixedUpdate()
    {
        backTire.AddTorque(-movement * speed * Time.fixedDeltaTime);
        frontTire.AddTorque(-movement * speed * Time.fixedDeltaTime);
    }
}
