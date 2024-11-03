using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int _currentHealth;
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private GameObject deathParticlesPrefab;

    private int _maxHealth;
    private float speedIncreaseTimer = 0f;
    private float speedIncreaseInterval = 10f;
    private float speedMultiplier = 0f;

    private Transform _targetEndPoint;
    private Animator _animator;

    public delegate void DeathAction(int exp);
    public static event DeathAction OnEnemyDeath;

    public event System.Action<Enemy> OnEnemyDestroy;
    public event System.Action<Enemy> OnReachedTarget; // ������� ��� ���������� ����

    public void Initialize(Transform targetEndPoint)
    {
        _targetEndPoint = targetEndPoint;
    }

    void Start()
    {
        _maxHealth = _enemyData.health;
        _currentHealth = _maxHealth;
        speedMultiplier = 0f;

        _animator = GetComponent<Animator>();

        Debug.Log("���� ���������������.");
    }

    void Update()
    {
        MoveToEndPoint();

        speedIncreaseTimer += Time.deltaTime;
        if (speedIncreaseTimer >= speedIncreaseInterval)
        {
            speedIncreaseTimer = 0f;
            speedMultiplier += 1f;
            Debug.Log($"�������� ����� ��������� �� 1. ����� ��������: {_enemyData.speed + speedMultiplier}");
        }

        CheckIfReachedEndPoint();
    }

    private void MoveToEndPoint()
    {
        if (_targetEndPoint == null) return;

        Vector3 direction = (_targetEndPoint.position - transform.position).normalized;
        float currentSpeed = _enemyData.speed + speedMultiplier;

        if (_animator != null)
        {
            _animator.SetFloat("speed", currentSpeed);
        }

        if (currentSpeed > 0.1f)
        {
            transform.Translate(direction * currentSpeed * Time.deltaTime, Space.World);
        }
    }

    private void CheckIfReachedEndPoint()
    {
        if (_targetEndPoint == null) return;

        if (Vector3.Distance(transform.position, _targetEndPoint.position) < 0.5f)
        {
            OnReachedTarget?.Invoke(this); // ���������� ������ � ���������� �������� �����
            ReturnToPool();
            Debug.Log("���� ������ �������� ����� � ��������� � ���.");
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Max(_currentHealth, 0);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        int exp = _enemyData.exp;
        OnEnemyDeath?.Invoke(exp);
        OnEnemyDestroy?.Invoke(this);

        if (deathParticlesPrefab != null)
        {
            Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
            Debug.Log("������� ������ ����������.");
        }

        ReturnToPool();
        Debug.Log("���� �����.");
    }

    private void ReturnToPool()
    {
        _currentHealth = _maxHealth;
        EnemyPool.Instance.ReturnEnemy(this);
        Debug.Log("���� ��������� � ���.");
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }
}














/*using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int _currentHealth;
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private GameObject deathParticlesPrefab;

    private int _maxHealth;
    private float speedIncreaseTimer = 0f;
    private float speedIncreaseInterval = 10f;
    private float speedMultiplier = 0f;

    private Transform _targetEndPoint;
    private Animator _animator;

    public delegate void DeathAction(int exp);
    public static event DeathAction OnEnemyDeath;

    // �������� ������� �� �������������, ����� ���������� ���������� ��������� �����
    public event System.Action<Enemy> OnEnemyDestroy;

    public void Initialize(Transform targetEndPoint)
    {
        _targetEndPoint = targetEndPoint;
    }

    void Start()
    {
        _maxHealth = _enemyData.health;
        _currentHealth = _maxHealth;
        speedMultiplier = 0f;

        _animator = GetComponent<Animator>();

        Debug.Log("���� ���������������.");
    }

    void Update()
    {
        MoveToEndPoint();

        speedIncreaseTimer += Time.deltaTime;
        if (speedIncreaseTimer >= speedIncreaseInterval)
        {
            speedIncreaseTimer = 0f;
            speedMultiplier += 1f;
            Debug.Log($"�������� ����� ��������� �� 1. ����� ��������: {_enemyData.speed + speedMultiplier}");
        }

        CheckIfReachedEndPoint();
    }

    private void MoveToEndPoint()
    {
        if (_targetEndPoint == null) return;

        Vector3 direction = (_targetEndPoint.position - transform.position).normalized;
        float currentSpeed = _enemyData.speed + speedMultiplier;

        if (_animator != null)
        {
            _animator.SetFloat("speed", currentSpeed);
        }

        if (currentSpeed > 0.1f)
        {
            transform.Translate(direction * currentSpeed * Time.deltaTime, Space.World);
        }
    }

    private void CheckIfReachedEndPoint()
    {
        if (_targetEndPoint == null) return;

        if (Vector3.Distance(transform.position, _targetEndPoint.position) < 0.5f)
        {
            ReturnToPool();
            Debug.Log("���� ������ �������� ����� � ��������� � ���.");
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Max(_currentHealth, 0);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        int exp = _enemyData.exp;
        OnEnemyDeath?.Invoke(exp);
        OnEnemyDestroy?.Invoke(this); // �������� ���������� ��������� �����

        if (deathParticlesPrefab != null)
        {
            Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
            Debug.Log("������� ������ ����������.");
        }

        ReturnToPool();
        Debug.Log("���� �����.");
    }

    private void ReturnToPool()
    {
        _currentHealth = _maxHealth;
        EnemyPool.Instance.ReturnEnemy(this);
        Debug.Log("���� ��������� � ���.");
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }
}
*/