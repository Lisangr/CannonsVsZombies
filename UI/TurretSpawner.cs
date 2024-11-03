using UnityEngine;
using UnityEngine.UI;

public class TurretSpawner : MonoBehaviour
{
    [SerializeField] private TurretData turretData;
    [SerializeField] private Transform spawnPoint; // ����� ��������� ������
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
            Debug.LogError("������ ��� ������ �� ��������� � ����������!");
        }

        if (spawnPoint == null)
        {
            Debug.LogError("����� ������ �� ���������!");
        }
    }

    private void AttemptToSpawnTurret()
    {
        if (ResourceManager.Instance.SpendResources(turretData.price))
        {
            // ������� ������ � ���������� ���������
            GameObject turretInstance = Instantiate(turretData.prefab);
            turretInstance.SetActive(false); // ��������� ����� ����� ��������

            Turret turretComponent = turretInstance.GetComponent<Turret>();
            if (turretComponent != null && spawnPoint != null)
            {
                turretComponent.SetSpawnPoint(spawnPoint); // ������������� ����� ������
                Debug.Log($"����� ������ ����������� �� {spawnPoint.position}"); // �������� ���������
            }
            else
            {
                Debug.LogWarning("spawnPoint �� ���������� ��� ������ ��� Turret ��������� �� ������.");
            }

            turretInstance.SetActive(true); // ���������� ������ ����� ��������� ����� ������
        }
        else
        {
            Debug.Log("������������ �������� ��� ������ ������.");
        }
    }
}















/*using UnityEngine;
using UnityEngine.UI;

public class TurretSpawner : MonoBehaviour
{
    [SerializeField] private TurretData turretData;
    [SerializeField] private Transform spawnPoint; // ����� ��������� ������
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
            Debug.LogError("������ ��� ������ �� ��������� � ����������!");
        }

        if (spawnPoint == null)
        {
            Debug.LogError("����� ������ �� ���������!");
        }
    }

    private void AttemptToSpawnTurret()
    {
        if (ResourceManager.Instance.SpendResources(turretData.price))
        {
            // ������� ������ � ���������� ���������
            GameObject turretInstance = Instantiate(turretData.prefab);
            turretInstance.SetActive(false); // ��������� ����� ����� ��������

            Turret turretComponent = turretInstance.GetComponent<Turret>();
            if (turretComponent != null && spawnPoint != null)
            {
                turretComponent.SetSpawnPoint(spawnPoint); // ������������� ����� ������
                Debug.Log($"����� ������ ����������� �� {spawnPoint.position}"); // �������� ���������
            }
            else
            {
                Debug.LogWarning("spawnPoint �� ���������� ��� ������ ��� Turret ��������� �� ������.");
            }

            turretInstance.SetActive(true); // ���������� ������ ����� ��������� ����� ������
        }
        else
        {
            Debug.Log("������������ �������� ��� ������ ������.");
        }
    }
}
*/