using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour, IGameService
{
    [SerializeField] GameObject joystickImageGameObject;

    public void EnableJoystick()
    {
        joystickImageGameObject.SetActive(true);
    }

    public void DisableJoystick()
    {
        joystickImageGameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (ServiceLocator.Current != null)
            ServiceLocator.Current.Register(this);
    }

    private void OnDisable()
    {
        if (ServiceLocator.Current != null)
            ServiceLocator.Current.Unregister(this);
    }
}
