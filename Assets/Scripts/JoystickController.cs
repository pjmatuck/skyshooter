using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour, IGameService
{
    [SerializeField] GameObject joystickImageGameObject;
    private void Awake()
    {
        ServiceLocator.Current.Register(this);
    }

    public void EnableJoystick()
    {
        joystickImageGameObject.SetActive(true);
    }

    public void DisableJoystick()
    {
        joystickImageGameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        ServiceLocator.Current.Unregister(this);
    }
}
