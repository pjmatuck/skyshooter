using System;
using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField][Tooltip("In seconds")] float minTimeToShoot;
    [SerializeField][Tooltip("In seconds")] float maxTimeToShoot;
    [SerializeField] float startShootingCooldown;
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] LootController lootController;

    GunController _gun;
    UIController _uiController;
    AudioManager _audioManager;
    Animator _animator;

    EnemyState _state;

    public event Action OnDestruction;

    void Start()
    {
        _gun = GetComponent<GunController>();
        _animator = GetComponent<Animator>();

        _uiController = ServiceLocator.Current.Get<UIController>();
        _audioManager = ServiceLocator.Current.Get<AudioManager>();

        ServiceLocator.Current.Get<LevelManager>().OnLevelStateChanged +=
            OnLevelStateChange;

        StartCoroutine(Shoot());
    }

    private void OnLevelStateChange(LevelState state)
    {
        switch (state)
        {
            case LevelState.COMPLETE:
                SelfDestroy();
                break;
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.down * speed * Time.fixedDeltaTime);

        if(transform.position.y < -10)
            SelfDestroy();
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(startShootingCooldown);

        while(_state == EnemyState.OK)
        {
            _gun.Shoot();
            float timeToShoot = UnityEngine.Random.Range(minTimeToShoot, maxTimeToShoot);
            yield return new WaitForSeconds(timeToShoot);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet") && _state != EnemyState.EXPLODING)
        {
            _audioManager.PlaySFX(explosionSFX);
            Destroy(collision.gameObject);
            _animator.SetTrigger("explode");
            gameObject.tag = "Untagged";
            _uiController.IncreaseKillCount();
            StopAllCoroutines();
            _state = EnemyState.EXPLODING;
            DropItem();
            OnDestruction();
        }
    }

    private void OnDisable()
    {
        ServiceLocator.Current.Get<LevelManager>().OnLevelStateChanged -=
            OnLevelStateChange;
        StopAllCoroutines();
    }

    public void AnimExplosionEnds()
    {
        SelfDestroy();
    }

    void DropItem()
    {
        var item = lootController.Drop();

        if (item == null) return;

        Instantiate(item, 
            new Vector3(transform.position.x, transform.position.y, transform.position.z), 
            Quaternion.identity);
    }
}

public enum EnemyState
{
    OK,
    EXPLODING
}
