using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;

    [SerializeField] private List<Enemy> enemyPrefabs;
    [SerializeField] private int poolSize = 100; // Ёто значение будет измен€тьс€ в зависимости от точек
    [SerializeField] private Transform enemiesParent;

    private List<Enemy> enemyPool = new List<Enemy>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Enemy enemy = Instantiate(GetRandomEnemyPrefab(), enemiesParent);
            enemy.gameObject.SetActive(false);
            enemyPool.Add(enemy);
        }
    }

    private Enemy GetRandomEnemyPrefab()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Count);
        return enemyPrefabs[randomIndex];
    }

    public Enemy GetEnemy()
    {
        if (enemyPool.Count > 0)
        {
            Enemy enemy = enemyPool[0];
            enemyPool.RemoveAt(0);
            return enemy;
        }
        else
        {
            Debug.LogWarning("ѕул врагов исчерпан, врагов больше не создаем!");
            return null; // ¬озвращаем null, если пул пуст
        }
    }

    public void ReturnEnemy(Enemy enemy)
    {
        if (enemyPool.Count < poolSize)
        {
            enemy.gameObject.SetActive(false);
            enemyPool.Add(enemy);
        }
        else
        {
            Destroy(enemy.gameObject); // ≈сли пул полон, уничтожаем врага
        }
    }

    // ћетод дл€ установки размера пула на основе числа точек
    public void SetPoolSizeBasedOnPoints(int numberOfPoints)
    {
        poolSize = Mathf.CeilToInt(numberOfPoints * 0.4f); // 10% от количества точек
        Debug.Log($"Ќовый размер пула врагов: {poolSize}");

        // ≈сли текущий пул меньше нового размера, то добавл€ем врагов
        while (enemyPool.Count < poolSize)
        {
            Enemy enemy = Instantiate(GetRandomEnemyPrefab(), enemiesParent);
            enemy.gameObject.SetActive(false);
            enemyPool.Add(enemy);
        }

        // ≈сли в пуле больше врагов, чем нужно, удал€ем лишних
        while (enemyPool.Count > poolSize)
        {
            Enemy enemyToRemove = enemyPool[enemyPool.Count - 1];
            Destroy(enemyToRemove.gameObject);
            enemyPool.RemoveAt(enemyPool.Count - 1);
        }
    }
}
