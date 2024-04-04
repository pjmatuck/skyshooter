using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalMovement : MonoBehaviour
{
    [SerializeField] float speed;

    Vector3 _startPosition;
    Vector3 _directionPivot;
    Vector3 _direction;
    private void Start()
    {
        _startPosition = transform.position;
        _directionPivot = new Vector3(0f, -4f, 0f);
        _direction = _directionPivot - _startPosition;
    }

    void FixedUpdate()
    {
        transform.Translate(_direction.normalized * speed * Time.fixedDeltaTime);
    }
}
