using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField][Tooltip("In seconds")] float minTimeToShoot;
    [SerializeField][Tooltip("In seconds")] float maxTimeToShoot;
    [SerializeField] float startShootingCooldown;
    [SerializeField] AudioClip explosionSFX;

    GunController _gun;
    UIController _uiController;
    AudioManager _audioManager;
    Animator _animator;

    void Start()
    {
        _gun = GetComponent<GunController>();
        _animator = GetComponent<Animator>();

        _uiController = ServiceLocator.Current.Get<UIController>();
        _audioManager = ServiceLocator.Current.Get<AudioManager>();

        StartCoroutine(Shoot());
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

        while(true)
        {
            _gun.Shoot();
            float timeToShoot = Random.Range(minTimeToShoot, maxTimeToShoot);
            yield return new WaitForSeconds(timeToShoot);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            _uiController.IncreaseKillCount();
            _audioManager.PlaySFX(explosionSFX);
            Destroy(collision.gameObject);
            _animator.SetTrigger("explode");
            gameObject.tag = "Untagged";
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
        StopAllCoroutines();
    }

    public void AnimExplosionEnds()
    {
        SelfDestroy();
    }
}
