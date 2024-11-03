using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;

    [SerializeField] private List<Enemy> enemyPrefabs;
    [SerializeField] private int poolSize = 100;
    [SerializeField] private Transform enemiesParent;

    private List<Enemy> enemyPool = new List<Enemy>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Enemy enemy = Instantiate(GetRandomEnemyPrefab(), enemiesParent);
            enemy.gameObject.SetActive(false);
            enemyPool.Add(enemy);
            Debug.Log($"Враг #{i + 1} добавлен в пул.");
        }
    }

    private Enemy GetRandomEnemyPrefab()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Count);
        return enemyPrefabs[randomIndex];
    }

    public Enemy GetEnemy()
    {
        if (enemyPool.Count > 0)
        {
            Enemy enemy = enemyPool[0];
            enemyPool.RemoveAt(0);
            Debug.Log("Враг получен из пула.");
            return enemy;
        }
        else
        {
            Debug.LogWarning("Пул врагов исчерпан.");
            return null;
        }
    }

    public void ReturnEnemy(Enemy enemy)
    {
        if (enemyPool.Count < poolSize)
        {
            enemy.gameObject.SetActive(false);
            enemyPool.Add(enemy);
            Debug.Log("Враг возвращён в пул.");
        }
        else
        {
            Destroy(enemy.gameObject);
        }
    }
}
