using UnityEngine;

[CreateAssetMenu(fileName = "TurretData", menuName = "Turrets/Turret Data", order = 1)]
public class TurretData : ScriptableObject
{
    public GameObject prefab; // ������ ������
    public int price; // ���� ������
    public int damage; // ���� ������
    public float fireRate; // �������� �������� ������
}
