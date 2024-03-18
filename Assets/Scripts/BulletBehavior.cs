using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float timeSpan;

    void Start()
    {
        Invoke(nameof(SelfDestoy), timeSpan);
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);    
    }

    void SelfDestoy()
    {
        Destroy(gameObject);
    }
}
