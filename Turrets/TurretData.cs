using UnityEngine;

[CreateAssetMenu(fileName = "TurretData", menuName = "Turrets/Turret Data", order = 1)]
public class TurretData : ScriptableObject
{
    public GameObject prefab; // Префаб турели
    public int price; // Цена турели
    public int damage; // Урон турели
    public float fireRate; // Скорость стрельбы турели
}
