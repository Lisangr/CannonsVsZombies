using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage;
    public GameObject hitParticlesPrefab;
    public int prefabIndex;
    private Transform target;
    private bool hasHitTarget; // Флаг для предотвращения повторного нанесения урона

    public void Initialize(Transform target, int damage, int prefabIndex)
    {
        this.target = target;
        this.damage = damage;
        this.prefabIndex = prefabIndex;
        hasHitTarget = false; // Сбрасываем флаг при инициализации
    }

    private void Update()
    {
        if (target == null)
        {
            ReturnToPool();
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        if (!hasHitTarget && Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            HitTarget();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasHitTarget && (other.transform == target || other.gameObject.CompareTag("Ground")))
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        if (hasHitTarget) return; // Если уже попали, выходим

        hasHitTarget = true; // Устанавливаем флаг, чтобы больше не наносить урон

        if (target != null)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);

                if (hitParticlesPrefab != null)
                {
                    Instantiate(hitParticlesPrefab, transform.position, Quaternion.identity);
                }
            }
        }

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        target = null;
        hasHitTarget = false; // Сбрасываем флаг при возврате в пул
        ProjectilePool.Instance.ReturnProjectile(this, prefabIndex); // Возвращаем в пул
    }
}





















/*using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage;
    public GameObject hitParticlesPrefab;
    public int prefabIndex;
    private Transform target;

    public void Initialize(Transform target, int damage, int prefabIndex)
    {
        this.target = target;
        this.damage = damage;
        this.prefabIndex = prefabIndex;
    }

    private void Update()
    {
        if (target == null)
        {
            ReturnToPool();
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            HitTarget();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == target || other.gameObject.CompareTag("Ground"))
        {
            HitTarget();
            Destroy(gameObject);
        }
    }
    
    private void HitTarget()
    {
        if (target != null)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);

                if (hitParticlesPrefab != null)
                {
                    Instantiate(hitParticlesPrefab, transform.position, Quaternion.identity);
                }
            }
        }
    }
        private void ReturnToPool()
    {
        target = null;
        ProjectilePool.Instance.ReturnProjectile(this, prefabIndex); // Возвращаем в пул
    }
}*/