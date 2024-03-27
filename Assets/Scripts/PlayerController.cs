
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootSpeedPerSec;
    [SerializeField] float blinkingTotalTime;
    [SerializeField] float blinkingInterval;
    [SerializeField] Transform gunPivot;
    [SerializeField] SimpleGunController gunController;
    [SerializeField] PlayerStartEndAnimations startEndAnims;

    PlayerInput _playerInput;
    Rigidbody2D _rigidbody2D;
    float moveX, moveY;
    Vector2 direction;
    JoystickController _joystickController;
    UIController _uiController;
    AchievementsManager _achievementsManager;
    int _playerHP = 3;
    PlayerState _currentState;
    SpriteRenderer _spriteRenderer;
    Animator _animator;

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();       

        _joystickController = ServiceLocator.Current.Get<JoystickController>();
        _uiController = ServiceLocator.Current.Get<UIController>();

        ServiceLocator.Current.Get<LevelManager>().OnLevelStateChanged +=
            OnLevelStateChange;

        gunController = Instantiate(gunController, gunPivot);
    }

    void Update()
    {
        if (_currentState != PlayerState.NORMAL) return;

        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        direction = new Vector2(moveX, moveY);

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
        if(_currentState != PlayerState.NORMAL) return;

        gunController.Shoot();
    }

    void FixedUpdate()
    {
        Move(direction);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Powerup"))
        {
            gunController.LevelUp();
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
        _currentState = PlayerState.BLINK;

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
        {
            GameOver();
        }
        else
        {
            _uiController.DecreaseHP();
            StartCoroutine(Blink());
        }
    }

    private void GameOver()
    {
        StopAllCoroutines();
        CancelInvoke();
        _uiController.GameOver();
        _animator.SetTrigger("explode");
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    public void AnimExplosionEnds()
    {
        Debug.Log("Explosion ends");
        gameObject.SetActive(false);
    }

    private void OnLevelStateChange(LevelState state)
    {
        switch (state)
        {
            case LevelState.SETUP:
                _currentState = PlayerState.ARRIVE;
                startEndAnims.EntranceAnim();
            break;
            case LevelState.RUN:
                _currentState = PlayerState.NORMAL;
                InvokeRepeating(nameof(Shoot), 0f, 1 / shootSpeedPerSec);
            break;
            case LevelState.PAUSE:
            break;
            case LevelState.FINISH:
                RestorePlayer();
            break;
        }

    }

    void RestorePlayer()
    {
        gunController.Restore();
    }

    enum PlayerState
    {
        ARRIVE,
        BLINK,
        NORMAL
    }
}
