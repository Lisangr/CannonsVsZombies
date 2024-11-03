using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 3f;
    [SerializeField] private float _minSpawnInterval = 0.5f; // Минимальный интервал спавна, чтобы не снижать интервал бесконечно
    [SerializeField] private int _maxEnemies = 20;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] endPoints;

    private float _timer;
    private float _intervalReductionTimer = 0f; // Таймер для уменьшения интервала спавна
    private const float IntervalReduction = 0.2f;
    private const float IntervalReductionTime = 10f;
    private bool _isInitialized = true;

    private void Update()
    {
        if (_isInitialized)
        {
            _timer += Time.deltaTime;
            _intervalReductionTimer += Time.deltaTime;

            // Уменьшаем интервал спавна каждые 10 секунд
            if (_intervalReductionTimer >= IntervalReductionTime)
            {
                _intervalReductionTimer = 0f;
                _spawnInterval = Mathf.Max(_minSpawnInterval, _spawnInterval - IntervalReduction);
                Debug.Log($"Интервал спавна уменьшен. Новый интервал: {_spawnInterval} сек.");
            }

            // Спавним врагов, если таймер истек и количество врагов меньше максимального
            if (_timer >= _spawnInterval && CountActiveEnemies() < _maxEnemies)
            {
                SpawnEnemy();
                _timer = 0f;
            }
        }
    }

    private void SpawnEnemy()
    {
        Enemy enemy = EnemyPool.Instance.GetEnemy();

        if (enemy == null)
        {
            Debug.LogError("Не удалось получить врага из пула.");
            return;
        }

        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];
        Transform targetEndPoint = endPoints[spawnIndex];

        enemy.transform.position = spawnPoint.position;
        enemy.transform.rotation = spawnPoint.rotation;
        enemy.Initialize(targetEndPoint);
        enemy.gameObject.SetActive(true);

        Debug.Log($"Враг заспавнен в точке {spawnIndex} и движется к конечной точке.");
    }

    private int CountActiveEnemies()
    {
        return FindObjectsOfType<Enemy>().Length;
    }
}
















/*using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 3f;
    [SerializeField] private int _maxEnemies = 20;
    [SerializeField] private Transform[] spawnPoints; // Точки спавна
    [SerializeField] private Transform[] endPoints; // Конечные точки

    private float _timer;
    private bool _isInitialized = true; // Изменено для отладки — теперь спавн активен с начала

    private void Update()
    {
        if (_isInitialized)
        {
            _timer += Time.deltaTime;

            if (_timer >= _spawnInterval && CountActiveEnemies() < _maxEnemies)
            {
                Debug.Log("Спавн врага...");
                SpawnEnemy();
                _timer = 0f;
            }
        }
    }

    private void SpawnEnemy()
    {
        Enemy enemy = EnemyPool.Instance.GetEnemy();

        if (enemy == null)
        {
            Debug.LogError("Не удалось получить врага из пула.");
            return;
        }

        // Генерируем индекс для спавна и конечной точки
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        // Получаем соответствующую точку спавна и конечную точку
        Transform spawnPoint = spawnPoints[spawnIndex];
        Transform targetEndPoint = endPoints[spawnIndex];

        // Устанавливаем позицию врага в точке спавна
        enemy.transform.position = spawnPoint.position;
        enemy.transform.rotation = spawnPoint.rotation;

        // Инициализируем врага конечной точкой
        enemy.Initialize(targetEndPoint);

        // Активируем врага
        enemy.gameObject.SetActive(true);
        Debug.Log($"Враг заспавнен в точке {spawnIndex} и движется к конечной точке.");
    }

    private int CountActiveEnemies()
    {
        return FindObjectsOfType<Enemy>().Length;
    }
}
*/