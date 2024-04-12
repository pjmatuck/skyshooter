using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SimpleGunController : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform[] pivotPoints;
    [SerializeField] AudioClip shootSFX;

    AudioManager _audioManager;

    Dictionary<int, int[]> levelPivotsMap = new Dictionary<int, int[]>
    {
        {1, new int[] {2} },
        {2, new int[] {1,3} },
        {3, new int[] {1,2,3} },
        {4, new int[] {0,1,3,4} },
        {5, new int[] {0,1,2,3,4} }
    };

    int _currentLevel = 1;

    void Awake()
    {
        _audioManager = ServiceLocator.Current.Get<AudioManager>();
    }

    void Start()
    {
        EnablePivot(_currentLevel);    
    }

    public bool LevelUp()
    {
        if (_currentLevel == levelPivotsMap.Count) return false;

        _currentLevel++;
        EnablePivot(_currentLevel);

        return true;
    }

    public void LevelDown()
    {
        if (_currentLevel == 0) return;

        _currentLevel--;
        EnablePivot(_currentLevel);
    }

    public void Restore()
    {
        _currentLevel = 1;
    }

    private void EnablePivot(int level)
    {
        var getMap = levelPivotsMap[level];

        foreach (var p in pivotPoints)
        {
            p.gameObject.SetActive(false);
        }

        foreach (var m in getMap)
        {
            pivotPoints[m].gameObject.SetActive(true);
        }
    }

    public void Shoot()
    {
        _audioManager.PlaySFX(shootSFX);

        foreach(var p in pivotPoints)
        {
            if (p.gameObject.activeSelf)
            {
                Instantiate(bullet, 
                    p.position, 
                    Quaternion.identity, 
                    ServiceLocator.Current.Get<PoolManager>().PoolTransform);
            }
        }
    }
}
