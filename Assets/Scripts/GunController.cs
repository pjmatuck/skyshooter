using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] Transform gunPivot;
    [SerializeField] GameObject projectile;

    Transform _pool;

    private void Start()
    {
        _pool = ServiceLocator.Current.Get<PoolManager>().PoolTransform;
    }

    public void Shoot()
    {
        Instantiate(
            projectile,
            gunPivot.position,
            transform.rotation,
            _pool);
    }
}
