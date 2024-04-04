using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMovement : MonoBehaviour
{
    [SerializeField] float waveFrequency;
    [SerializeField] float waveSize;
    
    void FixedUpdate()
    {
        transform.Translate(new Vector3(
            (Mathf.Sin(Time.time * waveFrequency) * Time.deltaTime) / waveSize,
            -1 * Time.deltaTime,
            0f));
    }
}
