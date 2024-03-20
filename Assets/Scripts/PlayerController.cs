using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootSpeedPerSec;

    PlayerInput _playerInput;
    Rigidbody2D _rigidbody2D;
    float moveX, moveY;
    Vector2 direction;
    JoystickController _joystickController;
    UIController _uiController;
    GameManager _gameManager;

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _joystickController = ServiceLocator.Current.Get<JoystickController>();
        _uiController = ServiceLocator.Current.Get<UIController>();
        _gameManager = ServiceLocator.Current.Get<GameManager>();

        _gameManager.OnGameStateChanged += OnGameStateChange;

        InvokeRepeating(nameof(Shoot), 0f, 1 / shootSpeedPerSec);
    }

    void Update()
    {

#if UNITY_EDITOR
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        direction = new Vector2(moveX, moveY);
#endif

        direction = _playerInput.actions["Move"].ReadValue<Vector2>();

        if (direction.magnitude > 0)
            _joystickController.EnableJoystick();
        else 
            _joystickController.DisableJoystick();
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

        if (collision.CompareTag("Enemy"))
        {
            GameOver();
        }

        if (collision.CompareTag("EnemyBullet"))
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        _uiController.GameOver();
        _gameManager.SetState(GameManager.GameState.GAMEOVER);
    }

    private void OnGameStateChange(GameManager.GameState state)
    {
        if(state == GameManager.GameState.GAMEOVER)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
