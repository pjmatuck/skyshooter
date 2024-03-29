using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartEndAnimations : MonoBehaviour
{
    [SerializeField] Transform startPivot;
    [SerializeField] Transform entranceEndPivot;
    [SerializeField] Transform endPivot;

    [SerializeField] float timeToEnter;
    [SerializeField] float timeToExit;

    public void EntranceAnim()
    {
        StartCoroutine(MoveLerp(timeToEnter, startPivot.position, entranceEndPivot.position));
    }

    public void ExitAnim()
    {
        StartCoroutine(MoveLerp(timeToExit, transform.position, endPivot.position));
    }

    IEnumerator MoveLerp(float time, Vector3 startPoint, Vector3 endPoint)
    {
        var t = 0f;
        
        while(t <= 1)
        {
            t += Time.deltaTime/time;
            transform.position = Vector3.Lerp(startPoint, endPoint, t);
            yield return null;
        }
    }

    
}
