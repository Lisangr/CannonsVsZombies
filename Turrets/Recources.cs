using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    public int playerResources = 100; // Количество начальных ресурсов

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Метод для уменьшения ресурсов
    public bool SpendResources(int amount)
    {
        if (playerResources >= amount)
        {
            playerResources -= amount;
            return true;
        }
        return false;
    }
}
