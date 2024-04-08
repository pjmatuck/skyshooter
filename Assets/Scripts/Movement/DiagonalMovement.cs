using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
        var currentZRotation = transform.localRotation.z;
        transform.Rotate(new Vector3(0f, 0f, currentZRotation + CalculateRotation()));
    }

    void FixedUpdate()
    {
        transform.Translate(Vector2.up * speed * Time.fixedDeltaTime);
    }

    float CalculateRotation()
    {
        var position = new Vector2(_startPosition.x, _startPosition.y);
        var target = new Vector2(_directionPivot.x, _directionPivot.y);
        var hypo = Vector2.Distance(target,position);
        var opposite = target.x - position.x;
        var angle = Mathf.Sin(opposite / hypo);
        return angle * Mathf.Rad2Deg;
    }
}
