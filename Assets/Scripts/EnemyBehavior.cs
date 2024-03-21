using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField][Tooltip("In seconds")] float timeToShoot;
    
    GunController _gun;
    UIController _uiController;

    void Start()
    {
        _gun = GetComponent<GunController>();
        _uiController = ServiceLocator.Current.Get<UIController>();
        InvokeRepeating(nameof(Shoot), timeToShoot, timeToShoot);    
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

    void Shoot()
    {
        _gun.Shoot();
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
    }
}
