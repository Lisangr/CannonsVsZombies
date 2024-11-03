using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 3f;
    [SerializeField] private float _minSpawnInterval = 0.5f; // ����������� �������� ������, ����� �� ������� �������� ����������
    [SerializeField] private int _maxEnemies = 20;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] endPoints;

    private float _timer;
    private float _intervalReductionTimer = 0f; // ������ ��� ���������� ��������� ������
    private const float IntervalReduction = 0.2f;
    private const float IntervalReductionTime = 10f;
    private bool _isInitialized = true;

    private void Update()
    {
        if (_isInitialized)
        {
            _timer += Time.deltaTime;
            _intervalReductionTimer += Time.deltaTime;

            // ��������� �������� ������ ������ 10 ������
            if (_intervalReductionTimer >= IntervalReductionTime)
            {
                _intervalReductionTimer = 0f;
                _spawnInterval = Mathf.Max(_minSpawnInterval, _spawnInterval - IntervalReduction);
                Debug.Log($"�������� ������ ��������. ����� ��������: {_spawnInterval} ���.");
            }

            // ������� ������, ���� ������ ����� � ���������� ������ ������ �������������
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
            Debug.LogError("�� ������� �������� ����� �� ����.");
            return;
        }

        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];
        Transform targetEndPoint = endPoints[spawnIndex];

        enemy.transform.position = spawnPoint.position;
        enemy.transform.rotation = spawnPoint.rotation;
        enemy.Initialize(targetEndPoint);
        enemy.gameObject.SetActive(true);

        Debug.Log($"���� ��������� � ����� {spawnIndex} � �������� � �������� �����.");
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
    [SerializeField] private Transform[] spawnPoints; // ����� ������
    [SerializeField] private Transform[] endPoints; // �������� �����

    private float _timer;
    private bool _isInitialized = true; // �������� ��� ������� � ������ ����� ������� � ������

    private void Update()
    {
        if (_isInitialized)
        {
            _timer += Time.deltaTime;

            if (_timer >= _spawnInterval && CountActiveEnemies() < _maxEnemies)
            {
                Debug.Log("����� �����...");
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
            Debug.LogError("�� ������� �������� ����� �� ����.");
            return;
        }

        // ���������� ������ ��� ������ � �������� �����
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        // �������� ��������������� ����� ������ � �������� �����
        Transform spawnPoint = spawnPoints[spawnIndex];
        Transform targetEndPoint = endPoints[spawnIndex];

        // ������������� ������� ����� � ����� ������
        enemy.transform.position = spawnPoint.position;
        enemy.transform.rotation = spawnPoint.rotation;

        // �������������� ����� �������� ������
        enemy.Initialize(targetEndPoint);

        // ���������� �����
        enemy.gameObject.SetActive(true);
        Debug.Log($"���� ��������� � ����� {spawnIndex} � �������� � �������� �����.");
    }

    private int CountActiveEnemies()
    {
        return FindObjectsOfType<Enemy>().Length;
    }
}
*/