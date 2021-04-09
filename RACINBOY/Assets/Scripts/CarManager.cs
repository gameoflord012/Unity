using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CarManager : MonoBehaviour
{
    Rigidbody2D rb;
    public float carSpeed;
    public float tiltSpeed;
    public float rotateSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        float ang = (transform.rotation.eulerAngles.z + 90) * Mathf.PI / 180f;
        Vector2 direction = new Vector2(Mathf.Cos(ang), Mathf.Sin(ang));
        direction = direction.normalized;

        float inpV = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(inpV) > 0.5f)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, direction * carSpeed * inpV, tiltSpeed * Time.deltaTime);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, tiltSpeed * Time.deltaTime);
        }

        float inpH = Input.GetAxisRaw("Horizontal");
        if(Mathf.Abs(inpH) > 0.5f) 
        {
            Debug.Log("Rotate");
            transform.Rotate(new Vector3(0, 0, rotateSpeed * -inpH * Time.deltaTime * rb.velocity.magnitude));
        }
    }
}
