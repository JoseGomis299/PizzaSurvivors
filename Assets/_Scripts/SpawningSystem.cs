using System;
using System.Collections;
using ProjectUtils.Helpers;
using ProjectUtils.ObjectPooling;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawningSystem : MonoBehaviour
{
    public static event Action OnEnemySpawned;
    public float RoundTimer { get; private set; }
    public float SpawnRate { get; private set; }
    public int MaxSpawnCount {get; private set;}
    public int CurrentRound { get; private set; }
    public int CurrentSpawnCount => entitiesParent.childCount;
    
    [SerializeField] private Rect spawnArea;
    [SerializeField] private Transform entitiesParent;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject spawnEffectPrefab;
    [SerializeField, Tooltip("MaxSpawnCount = initialMaxSpawnCount ")] private AnimationCurve spawnCurve;
    [SerializeField] private int initialMaxSpawnCount = 10;
    [SerializeField] private float initialSpawnRate = 1;
    [SerializeField] private float roundDuration = 60f;
    [SerializeField] private float spawnDelay = 0.5f;
    
    private float _timer;
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
        
        float difficulty = spawnCurve.Evaluate(CurrentRound);
        MaxSpawnCount = initialMaxSpawnCount + (int) (initialMaxSpawnCount * difficulty);
        
        if(difficulty < 1) difficulty = 1;        
        SpawnRate = initialSpawnRate / difficulty;

        
        _isSpawning = true;
    }
    
    private void Update()
    {
        if(!_isSpawning) return;
        
        _timer += Time.deltaTime;
        RoundTimer -= Time.deltaTime;
        
        if (_timer >= SpawnRate)
        {
            _timer = 0;
            SpawnEnemy();
        }
        
        if (RoundTimer <= 0)
        {
            entitiesParent.DisableChildren();
            _isSpawning = false;
            GoNextRound();
        }
    }
    
    private void SpawnEnemy()
    {
        if (CurrentSpawnCount + _pendingSpawnCount < MaxSpawnCount)
        {
            _pendingSpawnCount++;
            StartCoroutine(SpawnEnemyWithDelay());
        }
    }
    
    private IEnumerator SpawnEnemyWithDelay()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(spawnArea.xMin, spawnArea.xMax), Random.Range(spawnArea.yMin, spawnArea.yMax), 0);
        while (Physics2D.OverlapCircle(spawnPosition, 0.1f) != null)
        {
            spawnPosition = new Vector3(Random.Range(spawnArea.xMin, spawnArea.xMax), Random.Range(spawnArea.yMin, spawnArea.yMax), 0);
            yield return null;
        }
        GameObject effect = ObjectPool.Instance.InstantiateFromPool(spawnEffectPrefab, spawnPosition, Quaternion.identity);

        yield return new WaitForSeconds(spawnDelay/5f * 3);
        var spriteRenderers = effect.GetComponentsInChildren<SpriteRenderer>();
        
        foreach (var spriteRederer in spriteRenderers)
            spriteRederer.DoBlink(spawnDelay/5f*2, 3, new Color(0,0,0,0));
        
        yield return new WaitForSeconds(spawnDelay/5f * 2);
        effect.SetActive(false);
        
        if(!_isSpawning) yield break;
        
        GameObject enemy = ObjectPool.Instance.InstantiateFromPool(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPosition, Quaternion.identity);
        enemy.transform.parent = entitiesParent;
        enemy.GetComponent<Enemy>().Initialize();
        
        _pendingSpawnCount--;
        OnEnemySpawned?.Invoke();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(spawnArea.center, spawnArea.size);
    }
#endif
}