using UnityEngine;
using System.Collections.Generic;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance;
    public GameObject[] projectilePrefabs; // Массив префабов снарядов
    public int poolSize = 10;

    [SerializeField] private Transform bulletParent; // Родительский объект для снарядов

    private Dictionary<int, Queue<Projectile>> projectilePools = new Dictionary<int, Queue<Projectile>>();

    private void Awake()
    {
        Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < projectilePrefabs.Length; i++)
        {
            Queue<Projectile> projectileQueue = new Queue<Projectile>();

            for (int j = 0; j < poolSize; j++)
            {
                GameObject projectileObject = Instantiate(projectilePrefabs[i], bulletParent); // Назначаем родителя при создании
                Projectile projectile = projectileObject.GetComponent<Projectile>();
                projectileObject.SetActive(false);
                projectileQueue.Enqueue(projectile);
            }

            projectilePools.Add(i, projectileQueue);
        }
    }

    public Projectile GetProjectile(int prefabIndex)
    {
        if (projectilePools.ContainsKey(prefabIndex) && projectilePools[prefabIndex].Count > 0)
        {
            Projectile projectile = projectilePools[prefabIndex].Dequeue();
            projectile.gameObject.SetActive(true);
            projectile.transform.SetParent(bulletParent); // Назначаем родителя при активации
            return projectile;
        }
        else if (projectilePrefabs.Length > prefabIndex)
        {
            GameObject projectileObject = Instantiate(projectilePrefabs[prefabIndex], bulletParent);
            return projectileObject.GetComponent<Projectile>();
        }

        Debug.LogWarning("Неверный индекс префаба снаряда!");
        return null;
    }

    public void ReturnProjectile(Projectile projectile, int prefabIndex)
    {
        if (!projectilePools.ContainsKey(prefabIndex))
        {
            Debug.LogWarning("Индекс префаба снаряда не существует в пуле.");
            return;
        }

        projectile.gameObject.SetActive(false);
        projectile.transform.SetParent(bulletParent); // Назначаем родителя при возврате в пул
        projectilePools[prefabIndex].Enqueue(projectile);
    }
}
