using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] Transform gunPivot;
    [SerializeField] GameObject projectile;
    [SerializeField] bool inverseDirection;

    Transform _pool;

    private void Start()
    {
        _pool = GameObject.Find("-- Pool --").transform;
    }

    public void Shoot()
    {
        Instantiate(
            projectile,
            new Vector3(gunPivot.position.x, gunPivot.position.y, gunPivot.position.z),
            inverseDirection ? Quaternion.Euler(0f, 0f, 180f) : Quaternion.identity,
            _pool);
    }
}
