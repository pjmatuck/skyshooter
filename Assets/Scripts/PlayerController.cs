using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject bullet;

    PlayerInput _playerInput;
    Rigidbody2D _rigidbody2D;
    //float moveX, moveY;
    Vector2 direction;

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //moveX = Input.GetAxis("Horizontal");
        //moveY = Input.GetAxis("Vertical");

        direction = _playerInput.actions["Move"].ReadValue<Vector2>();
        Debug.Log(direction.x + ", " + direction.y);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    public void Move(Vector2 direction)
    {
        _rigidbody2D.velocity = direction * speed;
    }

    public void Shoot()
    {
        Instantiate(
            bullet, 
            new Vector3(transform.position.x, transform.position.y, transform.position.z), 
            Quaternion.identity);
    }

    void FixedUpdate()
    {
        Move(direction);
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
