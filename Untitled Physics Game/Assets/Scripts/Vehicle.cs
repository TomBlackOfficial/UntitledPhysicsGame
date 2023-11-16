using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    private Rigidbody2D parentRB;
    public Rigidbody2D backTire;
    public Rigidbody2D frontTire;
    public float speed = 150;
    public float carTorque = 150;

    private float movement;

    private void Awake()
    {
        parentRB = transform.parent.GetComponent<Rigidbody2D>();
    }

    public void SetMovement(float newMovement)
    {
        movement = newMovement;
    }

    private void FixedUpdate()
    {
        backTire.AddTorque(-movement * speed * Time.fixedDeltaTime);
        frontTire.AddTorque(-movement * speed * Time.fixedDeltaTime);
        parentRB.AddTorque(movement * carTorque * Time.fixedDeltaTime);
    }
}
