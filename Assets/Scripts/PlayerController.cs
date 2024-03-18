using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject bullet;

    Rigidbody2D _rigidbody2D;
    float moveX, moveY;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Instantiate(
            bullet, 
            new Vector3(transform.position.x, transform.position.y, transform.position.z), 
            Quaternion.identity);
    }

    void FixedUpdate()
    {
        _rigidbody2D.velocity = new Vector2(moveX, moveY) * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Powerup"))
        {
            //TODO: Powerup collected
            Destroy(collision.gameObject);
        }
    }
}
