
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IGameService
{
    [SerializeField] float speed;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootSpeedPerSec;
    [SerializeField] float blinkingTotalTime;
    [SerializeField] float blinkingInterval;
    [SerializeField] Transform gunPivot;
    [SerializeField] SimpleGunController gunController;
    [SerializeField] PlayerStartEndAnimations startEndAnims;
    [SerializeField] GameObject shield;
    [SerializeField] float shieldTime;
    [SerializeField] float shieldCooldown;

    PlayerInput _playerInput;
    Rigidbody2D _rigidbody2D;
    float moveX, moveY;
    Vector2 direction;
    JoystickController _joystickController;
    UIController _uiController;
    AchievementsManager _achievementsManager;
    int _playerHP = 3;
    LevelState _currentState;
    SpriteRenderer _spriteRenderer;
    Animator _animator;
    bool isInvulnerable = false;
    bool canUseShield = true;

    void Start()
    {
        _currentState = LevelState.ASSEMBLE;

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
        if (_currentState != LevelState.RUN) return;

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
        if(_currentState != LevelState.RUN) return;

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
            if(gunController.LevelUp())
                _uiController.IncreasePowerUP();

            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Heal"))
        {
            if (_playerHP == 3) return;
            _playerHP++;
            _uiController.IncreaseHP();
            Destroy(collision.gameObject);
        }

        if (_currentState != LevelState.RUN)
            return;

        if (isInvulnerable)
            return;

        if (collision.CompareTag("Enemy"))
        {
            GetDamage();
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("EnemyBullet"))
        {
            if(!shield.activeSelf)
                GetDamage();

            Destroy(collision.gameObject);
        }
    }

    IEnumerator Blink()
    {
        isInvulnerable = true;

        float timer = 0f;
        while(timer < blinkingTotalTime)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkingInterval);
            timer += blinkingInterval;
        }

        _spriteRenderer.enabled = true;

        isInvulnerable = false;
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

    void OnEnable()
    {
        ServiceLocator.Current.Register(this);
    }

    void OnDisable()
    {
        ServiceLocator.Current.Unregister(this);
        CancelInvoke();
    }

    public void AnimExplosionEnds()
    {
        Debug.Log("Explosion ends");
        gameObject.SetActive(false);
    }

    private void OnLevelStateChange(LevelState state)
    {
        _currentState = state;

        switch (state)
        {
            case LevelState.ASSEMBLE:
                startEndAnims.EntranceAnim();
            break;
            case LevelState.RUN:
                InvokeRepeating(nameof(Shoot), 0f, 1 / shootSpeedPerSec);
            break;
            case LevelState.PAUSE:
            break;
            case LevelState.COMPLETE:
                CancelInvoke(nameof(Shoot));
                startEndAnims.ExitAnim();
            break;
            case LevelState.DISASSEMBLE:
                RestorePlayer();
            break;
        }
    }

    void RestorePlayer()
    {
        gunController.Restore();
        direction = Vector2.zero;
        _playerHP = 3;
    }

    public void StartShield()
    {
        if (!canUseShield) return;

        canUseShield = false;
        StartCoroutine(EnableShield());
        _uiController.StartShieldCoolDown(shieldCooldown);
        Invoke(nameof(AllowShield), shieldCooldown);
    }

    public IEnumerator EnableShield()
    {
        float time = 0;

        while(time < shieldTime)
        {
            shield.SetActive(true);
            yield return null;
            time += Time.deltaTime;
        }

        shield.SetActive(false);
    }

    void AllowShield()
    {
        canUseShield = true;
    }
}
