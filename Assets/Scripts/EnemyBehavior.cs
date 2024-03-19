using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField][Tooltip("In seconds")] float timeToShoot;
    
    GunController _gun;

    void Start()
    {
        _gun = GetComponent<GunController>();
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
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            SelfDestroy();
        }
    }
}
