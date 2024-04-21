using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float _spawnRate = 2.0f;

    private float _nextSpawnTime;

    public bool _isSpawning = true;

    public GameObject bossPrefab;

    private float _timer;

    private void Update()
    {
        if (!GameManagerGP.Instance._isPlaying) { return; }


        _timer += Time.deltaTime;

        if (_timer > _nextSpawnTime && _isSpawning)
        {

            SpawnEnemy();
            _nextSpawnTime = _timer + _spawnRate;
        }

        if(_timer > 60f)
        {
            if (_isSpawning)
            {
                GameManagerGP.Instance._isAttackable = false;
                Instantiate(bossPrefab, new Vector3(0, 7, 0), Quaternion.identity);
                _isSpawning = false;
            }
            if (_timer > 65f)
            {
                GameManagerGP.Instance._isAttackable = true;
            }
        }
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }

    public void Init()
    {
        _isSpawning = true;
        _timer = 0;
        _nextSpawnTime = 0;
    }
}
