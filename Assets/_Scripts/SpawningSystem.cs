using System;
using System.Collections;
using ProjectUtils.Helpers;
using ProjectUtils.ObjectPooling;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawningSystem : MonoBehaviour
{
    public static event Action OnEnemySpawned;
    public static event Action<int> OnRoundStart;
    public static event Action<int> OnRoundEnd;
    public static event Action<float> OnTimerChanged;
    public float RoundTimer { get; private set; }
    public float SpawnRate { get; private set; }
    public int MaxSpawnCount {get; private set;}
    public int CurrentRound { get; private set; }
    public int CurrentSpawnCount => entitiesParent.childCount + _pendingSpawnCount;
    
    [SerializeField] private Rect spawnArea;
    [SerializeField] private Transform entitiesParent;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject spawnEffectPrefab;
    [SerializeField, Tooltip("MaxSpawnCount = initialMaxSpawnCount ")] private AnimationCurve spawnCurve;
    [SerializeField] private int initialMaxSpawnCount = 10;
    [SerializeField] private float initialSpawnRate = 2;
    [SerializeField] private float roundDuration = 60f;
    [SerializeField] private float spawnDelay = 0.5f;
    
    private float _timer;
    private float _spawnTime;
    private int _pendingSpawnCount;
    private bool _isSpawning;

    private void Start()
    {
        _timer = 0;
        CurrentRound = 0;
        
        GoNextRound();
    }
    
    public void GoNextRound()
    {
        CurrentRound++;
        RoundTimer = roundDuration;
        OnRoundStart?.Invoke(CurrentRound);
        
        float difficulty = spawnCurve.Evaluate(CurrentRound);
        MaxSpawnCount = initialMaxSpawnCount + (int) (initialMaxSpawnCount * difficulty);
        SpawnRate = initialSpawnRate + (int) (initialSpawnRate * difficulty);
        
        _spawnTime = 1 / SpawnRate;
        _timer = _spawnTime - 0.1f;
        _isSpawning = true;
    }
    
    private void Update()
    {
        if (RoundTimer <= 0 && Input.GetKeyDown(KeyCode.N))
            GoNextRound();
        
        if(!_isSpawning) return;
        
        _timer += Time.deltaTime;
        RoundTimer -= Time.deltaTime;
        
        if (_timer >= _spawnTime)
        {
            _timer = 0;
            for(int i = 0; i < GetSpawnCount(); i++)
                SpawnEnemy();
        }
        
        if (RoundTimer <= 0)
        {
            entitiesParent.DisableChildren();
            _isSpawning = false;
            OnRoundEnd?.Invoke(CurrentRound);
        }
        
        OnTimerChanged?.Invoke(RoundTimer);
    }
    
    private void SpawnEnemy()
    {
        if (CurrentSpawnCount < MaxSpawnCount)
        {
            _pendingSpawnCount++;
            StartCoroutine(SpawnEnemyWithDelay());
        }
    }
    
    private IEnumerator SpawnEnemyWithDelay()
    {
        // Find a valid spawn position
        Vector3 spawnPosition = new Vector3(Random.Range(spawnArea.xMin, spawnArea.xMax), Random.Range(spawnArea.yMin, spawnArea.yMax), 0);
        while (Physics2D.OverlapCircle(spawnPosition, 0.1f) != null)
        {
            spawnPosition = new Vector3(Random.Range(spawnArea.xMin, spawnArea.xMax), Random.Range(spawnArea.yMin, spawnArea.yMax), 0);
            yield return null;
        }
        
        // Spawn effect and play animation
        GameObject effect = ObjectPool.Instance.InstantiateFromPool(spawnEffectPrefab, spawnPosition, Quaternion.identity);
        yield return new WaitForSeconds(spawnDelay/5f * 3);
        var spriteRenderers = effect.GetComponentsInChildren<SpriteRenderer>();
        
        foreach (var spriteRederer in spriteRenderers)
            spriteRederer.DoBlink(spawnDelay/5f*2, 3, new Color(0,0,0,0));
        
        yield return new WaitForSeconds(spawnDelay/5f * 2);
        effect.SetActive(false);
        
        if(!_isSpawning) yield break;
        
        // Spawn enemy
        GameObject enemy = ObjectPool.Instance.InstantiateFromPool(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPosition, Quaternion.identity);
        enemy.transform.parent = entitiesParent;
        enemy.GetComponent<EnemyBase>().Initialize(CurrentRound);
        
        _pendingSpawnCount--;
        OnEnemySpawned?.Invoke();
    }
    
    private int GetSpawnCount()
    {
        float probability = Random.value;
        if (probability < 0.5f) return 1;
        if (probability < 0.7f) return 2;
        if (probability < 0.8f) return 3;
        if (probability < 0.9f) return 4;
        return 5;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(spawnArea.center, spawnArea.size);
    }
#endif
}