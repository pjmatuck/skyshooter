using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] Transform gunPivot;
    [SerializeField] float shootSpeed;
    [SerializeField] GameObject projectile;

    private void Shoot()
    {
        Instantiate(
            projectile,
            new Vector3(transform.position.x, transform.position.y, transform.position.z),
            Quaternion.identity);
    }
}
