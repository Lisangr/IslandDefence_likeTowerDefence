using UnityEngine.Splines;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int _currentHealth;
    [SerializeField] private EnemyData _enemyData;
    private SplineContainer _splineContainer;
    private SplineAnimate _splineAnimate;
    private int _maxHealth;


    public delegate void DeathAction(int exp);
    public static event DeathAction OnEnemyDeath;
    public delegate void DAction();
    public static event DAction OnEnemyDestroy;
    public static event DAction OnEnemyFinished;

    void Start()
    {
        _maxHealth = _enemyData.health;
        _currentHealth = _maxHealth;

        InitializeSpline();
    }

    private void OnEnable()
    {
        _currentHealth = _maxHealth;
        InitializeSpline();      
        
        if (_splineAnimate != null)
        {
            _splineAnimate.Restart(true);
        }
    }

    private void InitializeSpline()
    {
        if (_splineAnimate == null)
        {
            SplineContainer[] splineContainers = FindObjectsOfType<SplineContainer>();
            if (splineContainers.Length > 0)
            {
                _splineContainer = splineContainers[Random.Range(0, splineContainers.Length)];
                _splineAnimate = gameObject.AddComponent<SplineAnimate>();
                _splineAnimate.Container = _splineContainer;
                _splineAnimate.AnimationMethod = SplineAnimate.Method.Speed;
                _splineAnimate.MaxSpeed = 5f;
                _splineAnimate.Completed += OnSplineAnimationCompleted;
            }
        }
    }

    private void OnDisable()
    {
        if (_splineAnimate != null)
        {
            _splineAnimate.Pause();
        }
    }

    private void OnSplineAnimationCompleted()
    {
        ReturnToPool();
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Max(_currentHealth, 0);
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        int gold = _enemyData.gold;
        OnEnemyDeath?.Invoke(gold);
        OnEnemyDestroy?.Invoke();
        if (_splineAnimate != null)
        {
            _splineAnimate.Completed -= OnSplineAnimationCompleted;
        }
        ReturnToPool();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            OnEnemyFinished?.Invoke();
            Debug.Log("ÂÎÇÂÐÀÙÀÅÌ ÄÎÁÅÆÀÂØÅÃÎ ÂÐÀÃÀ Â ÏÓË");
            ReturnToPool();
        }
    }
    private void ReturnToPool()
    {
        EnemyPool.Instance.ReturnEnemy(this);
    }  
}
