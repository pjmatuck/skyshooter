using UnityEngine;

public static class Bootstraper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        ServiceLocator.Initialize();

        //Register non monobehavior services

    }
}
