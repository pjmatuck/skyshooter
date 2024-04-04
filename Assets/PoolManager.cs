using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private void OnDisable()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }
}
