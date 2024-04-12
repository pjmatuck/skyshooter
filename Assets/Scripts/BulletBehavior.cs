using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float timeSpan;
    [SerializeField] bool randomizeSpeed;
    [SerializeField][Range(1,2)] float randomFactor;

    void Start()
    {
        Invoke(nameof(SelfDestroy), timeSpan);

        ServiceLocator.Current.Get<LevelManager>().OnLevelStateChanged +=
            OnLevelStateChange;

        speed = randomizeSpeed
            ? Random.Range(speed, speed * randomFactor)
            : speed;
    }

    void FixedUpdate()
    {
        transform.Translate(Vector2.up * speed * Time.fixedDeltaTime);    
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

    void SelfDestroy()
    {
        if(gameObject != null)
            Destroy(gameObject);
    }

    void OnDestroy()
    {
        if(ServiceLocator.Current != null)
            ServiceLocator.Current.Get<LevelManager>().OnLevelStateChanged -=
                OnLevelStateChange;
    }
}
