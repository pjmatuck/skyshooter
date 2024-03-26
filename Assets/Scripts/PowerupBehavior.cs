using System;
using UnityEngine;

public class PowerupBehavior : MonoBehaviour
{
    [SerializeField] float lifeSpan;
    [SerializeField] float speed;

    Vector2 direction = Vector2.zero;

    int[] xDirections = new int[] { -1, 1 };

    public event Action OnPowerupCollected;

    void Start()
    {
        direction = new Vector2(
            xDirections[UnityEngine.Random.Range(0, 2)],
            -1);

        Invoke(nameof(SelfDestroy), lifeSpan);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if (transform.position.x < -2.5f || transform.position.x > 2.5f)
            direction.x *= -1;

        if(transform.position.y > 4.8)
            direction.y *= -1;
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnPowerupCollected();
        }
    }
}
