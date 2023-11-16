using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 200f;

    private void Update()
    {
        transform.RotateAround(transform.position, -Vector3.forward, rotateSpeed * Time.deltaTime);
    }
}
