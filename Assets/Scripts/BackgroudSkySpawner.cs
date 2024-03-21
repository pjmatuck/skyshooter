using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroudSkySpawner : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] List<Transform> tilePoints;

    void FixedUpdate()
    {
        foreach (Transform t in tilePoints)
        {
            t.Translate(Vector2.down * speed * Time.deltaTime);

            if (t.localPosition.y <= -18)
            {
                t.position = new Vector3(t.position.x, transform.position.y, t.position.z);
            }
        }
    }
}
