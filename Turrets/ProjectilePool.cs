using UnityEngine;
using System.Collections.Generic;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance;
    public GameObject[] projectilePrefabs; // ������ �������� ��������
    public int poolSize = 10;

    [SerializeField] private Transform bulletParent; // ������������ ������ ��� ��������

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
                GameObject projectileObject = Instantiate(projectilePrefabs[i], bulletParent); // ��������� �������� ��� ��������
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
            projectile.transform.SetParent(bulletParent); // ��������� �������� ��� ���������
            return projectile;
        }
        else if (projectilePrefabs.Length > prefabIndex)
        {
            GameObject projectileObject = Instantiate(projectilePrefabs[prefabIndex], bulletParent);
            return projectileObject.GetComponent<Projectile>();
        }

        Debug.LogWarning("�������� ������ ������� �������!");
        return null;
    }

    public void ReturnProjectile(Projectile projectile, int prefabIndex)
    {
        if (!projectilePools.ContainsKey(prefabIndex))
        {
            Debug.LogWarning("������ ������� ������� �� ���������� � ����.");
            return;
        }

        projectile.gameObject.SetActive(false);
        projectile.transform.SetParent(bulletParent); // ��������� �������� ��� �������� � ���
        projectilePools[prefabIndex].Enqueue(projectile);
    }
}
