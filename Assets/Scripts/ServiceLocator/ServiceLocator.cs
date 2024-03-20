using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ServiceLocator
{
    private ServiceLocator() { }

    private readonly Dictionary<string, IGameService> services =
        new Dictionary<string, IGameService>();
    public static ServiceLocator Current {  get; private set; }
    public static void Initialize()
    {
        Current = new ServiceLocator();
    }

    public T Get<T>() where T : IGameService
    {
        string key = typeof(T).Name;
        if (!services.ContainsKey(key))
        {
            Debug.LogError($"{key} not registered.");
            throw new InvalidOperationException();
        }

        return (T)services[key];
    }

    public void Register<T>(T service) where T : IGameService
    {
        string key = typeof(T).Name;
        if(services.ContainsKey(key))
        {
            Debug.LogError($"Attempted to unregister.");
            return;
        }

        services.Add(key, service);
    }

    public void Unregister<T>(T service) where T : IGameService
    {
        string key = typeof(T).Name;
        if (!services.ContainsKey(key))
        {
            Debug.LogError($"Attempted to unregister.");
            return;
        }

        services.Remove(key);
    }
}
