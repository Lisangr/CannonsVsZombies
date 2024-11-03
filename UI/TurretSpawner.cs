using UnityEngine;
using UnityEngine.UI;

public class TurretSpawner : MonoBehaviour
{
    [SerializeField] private TurretData turretData;
    [SerializeField] private Transform spawnPoint; // Точка установки турели
    private Button spawnButton;

    private void Start()
    {
        spawnButton = GetComponent<Button>();
        if (spawnButton != null)
        {
            spawnButton.onClick.AddListener(AttemptToSpawnTurret);
        }
        else
        {
            Debug.LogError("Кнопка для спавна не присвоена в инспекторе!");
        }

        if (spawnPoint == null)
        {
            Debug.LogError("Точка спавна не присвоена!");
        }
    }

    private void AttemptToSpawnTurret()
    {
        if (ResourceManager.Instance.SpendResources(turretData.price))
        {
            // Создаем турель в неактивном состоянии
            GameObject turretInstance = Instantiate(turretData.prefab);
            turretInstance.SetActive(false); // Отключаем сразу после создания

            Turret turretComponent = turretInstance.GetComponent<Turret>();
            if (turretComponent != null && spawnPoint != null)
            {
                turretComponent.SetSpawnPoint(spawnPoint); // Устанавливаем точку спавна
                Debug.Log($"Точка спавна установлена на {spawnPoint.position}"); // Проверка установки
            }
            else
            {
                Debug.LogWarning("spawnPoint не установлен для турели или Turret компонент не найден.");
            }

            turretInstance.SetActive(true); // Активируем объект после установки точки спавна
        }
        else
        {
            Debug.Log("Недостаточно ресурсов для спавна турели.");
        }
    }
}















/*using UnityEngine;
using UnityEngine.UI;

public class TurretSpawner : MonoBehaviour
{
    [SerializeField] private TurretData turretData;
    [SerializeField] private Transform spawnPoint; // Точка установки турели
    private Button spawnButton;

    private void Start()
    {
        spawnButton = GetComponent<Button>();
        if (spawnButton != null)
        {
            spawnButton.onClick.AddListener(AttemptToSpawnTurret);
        }
        else
        {
            Debug.LogError("Кнопка для спавна не присвоена в инспекторе!");
        }

        if (spawnPoint == null)
        {
            Debug.LogError("Точка спавна не присвоена!");
        }
    }

    private void AttemptToSpawnTurret()
    {
        if (ResourceManager.Instance.SpendResources(turretData.price))
        {
            // Создаем турель в неактивном состоянии
            GameObject turretInstance = Instantiate(turretData.prefab);
            turretInstance.SetActive(false); // Отключаем сразу после создания

            Turret turretComponent = turretInstance.GetComponent<Turret>();
            if (turretComponent != null && spawnPoint != null)
            {
                turretComponent.SetSpawnPoint(spawnPoint); // Устанавливаем точку спавна
                Debug.Log($"Точка спавна установлена на {spawnPoint.position}"); // Проверка установки
            }
            else
            {
                Debug.LogWarning("spawnPoint не установлен для турели или Turret компонент не найден.");
            }

            turretInstance.SetActive(true); // Активируем объект после установки точки спавна
        }
        else
        {
            Debug.Log("Недостаточно ресурсов для спавна турели.");
        }
    }
}
*/