using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerGP : Singleton<GameManagerGP>
{
    public bool _isGameOver = false;
    public bool _isVictory = false;
    public bool _isPlaying = false;

    [Header("Player Status")]
    public Player player;
    public float _attackDamage = 10f;
    public int _bulletCount = 1;
    public int _currentHP = 3;
    public int _currentLife = 3;
    public bool _isDamageable = true;
    public bool _canShoot = true;
    public bool _hasBomb = false;

    [Header("UI")]
    public UIManager uiManager;

    public int _score = 0;
    public int _highScore = 0;

    public Route[] routes;

    [Header("Bullet")]
    public GameObject bulletExplosionPrefab;

    [Header("Enemy")]
    public EnemySpawner enemySpawner;
    public GameObject explosionPrefab;
    public bool _isAttackable = true;

    [Header("Items")]
    public GameObject[] itemPrefabs;
    public GameObject bombPrefab;
    [Tooltip("아이템 드랍 확율 0~100 (백분율)")]
    [Range(0, 100)] public int _itemDropRate = 30;

    [Header("Follower")]
    public GameObject followerPrefab; // 팔로워 프리팹
    public List<Transform> followers; // 팔로워 목록
    public float _followerDistance = 1.0f; // 팔로워 간의 거리
    public List<Follower> allFollowers = new List<Follower>(); // 모든 팔로워를 관리하는 리스트

    [Header("Test")]
    public bool timeMultiplier = false;

    private void Awake()
    {
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
        Init();
    }

    void Update()
    {
        if (timeMultiplier) { Time.timeScale = 4; }
        else { Time.timeScale = 1; }

        if (_currentHP <= 0)
        {
            _currentLife--;
            player.UnBreakable();
            if (_currentLife <= 0)
            {
                GameOver();
            }
            else
            {
                _currentHP = 3;
            }
        }
    }

    public void UpdateFollowerLists()
    {
        foreach (Follower follower in allFollowers)
        {
            follower.otherFollowers.Clear(); // 기존 리스트를 클리어
            foreach (Follower otherFollower in allFollowers)
            {
                if (otherFollower != follower) // 자기 자신 제외
                {
                    follower.otherFollowers.Add(otherFollower.transform);
                }
            }
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
        uiManager.gameOverPanel.SetActive(true);
        if (_score > _highScore)
        {
            _highScore = _score;
            PlayerPrefs.SetInt("HighScore", _highScore);
        }

        _isPlaying = false;
    }


    public void Restart()
    {
        DestroyAllObjectsWithTag("Enemy");
        DestroyAllObjectsWithTag("Bullet");
        DestroyAllObjectsWithTag("Boss");
        DestroyAllObjectsWithTag("Item");

        Init();
        _isPlaying = true;
    }
    public void Init()
    {
        player.Init();
        _canShoot = true;
        _currentHP = 3;
        _currentLife = 3;
        _score = 0;
        _isVictory = false;
        _isGameOver = false;
        _isPlaying = false;
        _bulletCount = 1;
        _isDamageable = true;
        _isAttackable = true;
        _hasBomb = false;
        _itemDropRate = 30;
        timeMultiplier = false;
        followers.Clear();
        allFollowers.Clear();
        enemySpawner.Init();

    }
    void DestroyAllObjectsWithTag(string tag)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objectsWithTag)
        {
            Destroy(obj);
        }
    }

    public void UseBomb()
    {
        if (_hasBomb)
        {
            _hasBomb = false;
            GameObject bomb = Instantiate(bombPrefab, player.transform.position, Quaternion.identity);
        }
    }
}
