using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float timeSpan;

    void Start()
    {
        Invoke(nameof(SelfDestroy), timeSpan);

        ServiceLocator.Current.Get<LevelManager>().OnLevelStateChanged +=
            OnLevelStateChange;
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);    
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

    void OnDisable()
    {
        if(ServiceLocator.Current != null)
            ServiceLocator.Current.Get<LevelManager>().OnLevelStateChanged -=
                OnLevelStateChange;
    }
}
