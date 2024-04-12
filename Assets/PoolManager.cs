using UnityEngine;

public class PoolManager : MonoBehaviour, IGameService
{
    public Transform PoolTransform => transform;

    private void Awake()
    {
        ServiceLocator.Current.Register(this);    
    }

    void Start()
    {
        ServiceLocator.Current.Get<LevelManager>().OnLevelStateChanged +=
            OnLevelStateChange;
    }

    private void OnDisable()
    {
        CleanObjects();
        ServiceLocator.Current.Get<LevelManager>().OnLevelStateChanged -=
            OnLevelStateChange;
    }

    private void OnDestroy()
    {
        ServiceLocator.Current.Unregister(this);
    }

    void CleanObjects()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }

    private void OnLevelStateChange(LevelState state)
    {
        switch (state)
        {
            case LevelState.COMPLETE:
                CleanObjects();
                break;
        }
    }
}
