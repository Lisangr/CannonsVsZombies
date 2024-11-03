using UnityEngine;
using System.Collections.Generic;

//[RequireComponent(typeof(CapsuleCollider))]
public class Turret : MonoBehaviour
{
    public Transform spawnPoint;
    public TurretData turretData;
    public Transform shootingPoint;
    public float attackAngle = 15f;
    public int projectileTypeIndex;

    private List<Enemy> enemiesInRange = new List<Enemy>();
    private float fireCooldown = 0f;

    private void OnEnable()
    {
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            Debug.Log($"������ ������������ � �������: {transform.position}");
        }
        else
        {
            Debug.LogWarning("spawnPoint �� ���������� ��� ������!");
        }

        //CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        //capsuleCollider.isTrigger = true;
        //capsuleCollider.radius = 15.0f;
    }

    private void Start()
    {
        projectileTypeIndex = Random.Range(0, 3);
    }

    private void Update()
    {
        enemiesInRange.RemoveAll(enemy => enemy == null);

        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f)
        {
            Enemy targetEnemy = GetEnemyInFront();
            if (targetEnemy != null)
            {
                Shoot(targetEnemy);
                fireCooldown = 1f / turretData.fireRate;
            }
        }
    }

    private Enemy GetEnemyInFront()
    {
        enemiesInRange.RemoveAll(enemy => enemy == null);

        foreach (var enemy in enemiesInRange)
        {
            if (enemy == null) continue;

            Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
            float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

            if (angleToEnemy <= attackAngle / 2)
            {
                return enemy;
            }
        }
        return null;
    }

    private void Shoot(Enemy enemy)
    {
        projectileTypeIndex = Random.Range(0, 3);
        Projectile projectile = ProjectilePool.Instance.GetProjectile(projectileTypeIndex);
        if (projectile != null && shootingPoint != null)
        {
            projectile.transform.position = shootingPoint.position;
            projectile.Initialize(enemy.transform, turretData.damage, projectileTypeIndex);
            Debug.Log("������ ������� �� �����.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemiesInRange.Add(enemy);
            enemy.OnEnemyDestroy += HandleEnemyDestroyed;
            enemy.OnReachedTarget += HandleEnemyReachedTarget; // ������������� �� ������� ���������� ����
            Debug.Log("���� ����� � ���� ����� ������.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemiesInRange.Remove(enemy);
            enemy.OnEnemyDestroy -= HandleEnemyDestroyed;
            enemy.OnReachedTarget -= HandleEnemyReachedTarget; // ������������ �� ������� ���������� ����
            Debug.Log("���� ����� �� ���� ����� ������.");
        }
    }

    private void HandleEnemyDestroyed(Enemy enemy)
    {
        enemiesInRange.Remove(enemy);
    }

    private void HandleEnemyReachedTarget(Enemy enemy)
    {
        if (enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Remove(enemy); // ������� �����, ���� �� ������ ����
            Debug.Log("���� ������ �������� ����� � ������ �� ������ ����� ������.");
        }
    }

    public void SetSpawnPoint(Transform point)
    {
        spawnPoint = point;
        if (gameObject.activeSelf)
        {
            transform.position = spawnPoint.position;
        }
    }

    private void OnDrawGizmos()
    {
        if (spawnPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(spawnPoint.position, 0.2f);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.2f);

        Gizmos.color = Color.blue;
        Vector3 leftLimit = Quaternion.Euler(0, -attackAngle / 2, 0) * transform.forward;
        Vector3 rightLimit = Quaternion.Euler(0, attackAngle / 2, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, leftLimit * 5f);
        Gizmos.DrawRay(transform.position, rightLimit * 5f);
    }
}
















/*using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CapsuleCollider))]
public class Turret : MonoBehaviour
{
    public Transform spawnPoint;
    public TurretData turretData;
    public Transform shootingPoint;
    public float attackAngle = 15f;
    public int projectileTypeIndex;

    private List<Enemy> enemiesInRange = new List<Enemy>();
    private float fireCooldown = 0f;

    private void OnEnable()
    {
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            Debug.Log($"������ ������������ � �������: {transform.position}");
        }
        else
        {
            Debug.LogWarning("spawnPoint �� ���������� ��� ������!");
        }

        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.isTrigger = true;
        capsuleCollider.radius = 15.0f;
    }

    private void Start()
    {
        projectileTypeIndex = Random.Range(0, 3);
    }

    private void Update()
    {
        // ������� ������ ������ �� ���������� ��������
        enemiesInRange.RemoveAll(enemy => enemy == null);

        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f)
        {
            Enemy targetEnemy = GetEnemyInFront();
            if (targetEnemy != null)
            {
                Shoot(targetEnemy);
                fireCooldown = 1f / turretData.fireRate;
            }
        }
    }

    private Enemy GetEnemyInFront()
    {
        enemiesInRange.RemoveAll(enemy => enemy == null);

        foreach (var enemy in enemiesInRange)
        {
            if (enemy == null) continue;

            Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
            float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

            if (angleToEnemy <= attackAngle / 2)
            {
                return enemy;
            }
        }
        return null;
    }

    private void Shoot(Enemy enemy)
    {
        projectileTypeIndex = Random.Range(0, 3);
        Projectile projectile = ProjectilePool.Instance.GetProjectile(projectileTypeIndex);
        if (projectile != null && shootingPoint != null)
        {
            projectile.transform.position = shootingPoint.position;
            projectile.Initialize(enemy.transform, turretData.damage, projectileTypeIndex);
            Debug.Log("������ ������� �� �����.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemiesInRange.Add(enemy);
            enemy.OnEnemyDestroy += HandleEnemyDestroyed; // �������� �� ������� �����������
            Debug.Log("���� ����� � ���� ����� ������.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemiesInRange.Remove(enemy);
            enemy.OnEnemyDestroy -= HandleEnemyDestroyed; // ������� �� ������� �����������
            Debug.Log("���� ����� �� ���� ����� ������.");
        }
    }

    private void HandleEnemyDestroyed(Enemy enemy)
    {
        enemiesInRange.Remove(enemy);
    }

    public void SetSpawnPoint(Transform point)
    {
        spawnPoint = point;
        if (gameObject.activeSelf)
        {
            transform.position = spawnPoint.position;
        }
    }

    private void OnDrawGizmos()
    {
        if (spawnPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(spawnPoint.position, 0.2f);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.2f);

        Gizmos.color = Color.blue;
        Vector3 leftLimit = Quaternion.Euler(0, -attackAngle / 2, 0) * transform.forward;
        Vector3 rightLimit = Quaternion.Euler(0, attackAngle / 2, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, leftLimit * 5f);
        Gizmos.DrawRay(transform.position, rightLimit * 5f);
    }
}
*/
