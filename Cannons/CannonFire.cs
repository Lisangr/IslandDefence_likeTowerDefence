using UnityEngine;

public class CannonFire : MonoBehaviour
{
    public GameObject[] projectilePrefabs; // ������ �������� ��������
    public Transform firePoint; // ����� ������ �������
    public float fireRadius = 20f; // ������ ������ ������
    public float fireDelay = 2f; // �������� ����� ����������
    public LayerMask enemyLayer; // ���� ������ ��� ������
    public float rotationSpeed = 5f; // �������� �������� �����
    public CannonData cannonData; // ������ �����, ���������� ����

    private void Start()
    {
        // �������� ������������� ���������� ������� FireProjectile
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
                if (enemy.gameObject == gameObject) continue; // ����������, ���� ��� ������ �����
                float distance = Vector3.Distance(firePoint.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }

            // ������������ ����� � ���������� ����� ������ �� ��� Y
            if (closestEnemy != null)
            {
                Vector3 directionToEnemy = (closestEnemy.position - transform.position).normalized;

                // ������������� ��������� Y ����������� � �������� ���������
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

            // ������� ������ � ���������� ��� � ���������� �����
            if (closestEnemy != null)
            {
                GameObject selectedProjectilePrefab = projectilePrefabs[Random.Range(0, projectilePrefabs.Length)];
                GameObject projectileInstance = Instantiate(selectedProjectilePrefab, firePoint.position, Quaternion.identity);

                ProjectileMover projectileMover = projectileInstance.GetComponent<ProjectileMover>();
                if (projectileMover != null)
                {
                    // ��������� ����������� �������� ������� � �����
                    projectileInstance.transform.forward = (closestEnemy.position - firePoint.position).normalized;

                    // ��������� ���� ��� �������
                    projectileMover.SetTarget(closestEnemy);

                    // ��������� ����� ��� �������
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
