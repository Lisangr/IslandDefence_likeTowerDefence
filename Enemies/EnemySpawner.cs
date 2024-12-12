using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 5f;
    [SerializeField] private int _maxEnemies = 3;
    private float _timer;
    private bool _isInitialized = true;
    [SerializeField] private Vector3 spawnPosition;

    private void Update()
    {
        if (_isInitialized)
        {
            _timer += Time.deltaTime;
            if (_timer >= _spawnInterval && CountActiveEnemies() < _maxEnemies)
            {
                SpawnEnemy();
                _timer = 0f;
            }
        }
    }

    private void SpawnEnemy()
    {
        Enemy enemy = EnemyPool.Instance.GetEnemy();
        if (enemy == null)
        {
            Debug.LogError("Не удалось получить врага из пула.");
            return;
        }
        enemy.transform.position = spawnPosition;
        enemy.transform.rotation = Quaternion.identity;
        enemy.gameObject.SetActive(true);
    }

    private int CountActiveEnemies()
    {
        return FindObjectsOfType<Enemy>().Length;
    }
}