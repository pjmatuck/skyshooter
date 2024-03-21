using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField][Tooltip("In seconds")] float minTimeToShoot;
    [SerializeField][Tooltip("In seconds")] float maxTimeToShoot;
    [SerializeField] float startShootingCooldown;

    GunController _gun;
    UIController _uiController;

    void Start()
    {
        _gun = GetComponent<GunController>();
        _uiController = ServiceLocator.Current.Get<UIController>();
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
            Destroy(collision.gameObject);
            SelfDestroy();
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
        StopAllCoroutines();
    }
}
