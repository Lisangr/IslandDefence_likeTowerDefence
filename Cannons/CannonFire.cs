using UnityEngine;

public class CannonFire : MonoBehaviour
{
    public GameObject[] projectilePrefabs; // Массив префабов снарядов
    public Transform firePoint; // Точка спавна снаряда
    public float fireRadius = 20f; // Радиус поиска врагов
    public float fireDelay = 2f; // Задержка между выстрелами
    public LayerMask enemyLayer; // Слой врагов для поиска
    public float rotationSpeed = 5f; // Скорость поворота пушки
    public CannonData cannonData; // Данные пушки, содержащие урон

    private void Start()
    {
        // Начинаем периодическое выполнение функции FireProjectile
        InvokeRepeating(nameof(FireProjectile), 0f, fireDelay);
    }

    void Update()
    {
        RotateTowardsNearestEnemy();
    }

    private void RotateTowardsNearestEnemy()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(firePoint.position, fireRadius, enemyLayer);

        if (enemiesInRange.Length > 0)
        {
            Transform closestEnemy = null;
            float minDistance = Mathf.Infinity;

            foreach (Collider enemy in enemiesInRange)
            {
                if (enemy.gameObject == gameObject) continue; // Пропускаем, если это объект пушки
                float distance = Vector3.Distance(firePoint.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }

            // Поворачиваем пушку к ближайшему врагу только по оси Y
            if (closestEnemy != null)
            {
                Vector3 directionToEnemy = (closestEnemy.position - transform.position).normalized;

                // Устанавливаем компонент Y направления и обнуляем остальные
                Vector3 flatDirection = new Vector3(directionToEnemy.x, 0, directionToEnemy.z);

                Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }

    void FireProjectile()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(firePoint.position, fireRadius, enemyLayer);

        if (enemiesInRange.Length > 0)
        {
            Transform closestEnemy = null;
            float minDistance = Mathf.Infinity;

            foreach (Collider enemy in enemiesInRange)
            {
                if (enemy.gameObject == gameObject) continue;
                float distance = Vector3.Distance(firePoint.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }

            // Спавним снаряд и направляем его к ближайшему врагу
            if (closestEnemy != null)
            {
                GameObject selectedProjectilePrefab = projectilePrefabs[Random.Range(0, projectilePrefabs.Length)];
                GameObject projectileInstance = Instantiate(selectedProjectilePrefab, firePoint.position, Quaternion.identity);

                ProjectileMover projectileMover = projectileInstance.GetComponent<ProjectileMover>();
                if (projectileMover != null)
                {
                    // Установка направления движения снаряда к врагу
                    projectileInstance.transform.forward = (closestEnemy.position - firePoint.position).normalized;

                    // Установка цели для снаряда
                    projectileMover.SetTarget(closestEnemy);

                    // Установка урона для снаряда
                    projectileMover.damage = cannonData.damage;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(firePoint.position, fireRadius);
    }
}
