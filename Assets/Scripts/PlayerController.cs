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
    [SerializeField] float blinkingTotalTime;
    [SerializeField] float blinkingInterval;

    PlayerInput _playerInput;
    Rigidbody2D _rigidbody2D;
    float moveX, moveY;
    Vector2 direction;
    JoystickController _joystickController;
    UIController _uiController;
    GameManager _gameManager;
    int _playerHP = 3;
    PlayerState _currentState;
    SpriteRenderer _spriteRenderer;

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _joystickController = ServiceLocator.Current.Get<JoystickController>();
        _uiController = ServiceLocator.Current.Get<UIController>();
        _gameManager = ServiceLocator.Current.Get<GameManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _gameManager.OnGameStateChanged += OnGameStateChange;

        _currentState = PlayerState.NORMAL;

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

        if (_currentState != PlayerState.NORMAL)
            return;

        if (collision.CompareTag("Enemy"))
        {
            GetDamage();
        }

        if (collision.CompareTag("EnemyBullet"))
        {
            GetDamage();
        }
    }

    IEnumerator Blink()
    {
        _currentState = PlayerState.BLINKING;

        float timer = 0f;
        while(timer < blinkingTotalTime)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkingInterval);
            timer += blinkingInterval;
        }

        _spriteRenderer.enabled = true;

        _currentState = PlayerState.NORMAL;
    }

    private void GetDamage()
    {
        _playerHP--;
        if (_playerHP == 0)
            GameOver();
        _uiController.DecreaseHP();
        StartCoroutine(Blink());
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

    enum PlayerState
    {
        BLINKING,
        NORMAL
    }
}
