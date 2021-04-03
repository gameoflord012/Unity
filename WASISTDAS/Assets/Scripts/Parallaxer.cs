using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxer : MonoBehaviour
{ 
    class PoolObject
    {
        public Transform transform;
        public PoolObject(Transform t) { transform = t; inUse = false; }        
        public bool inUse;
        public void Use() { inUse = true; }
        public void Dispose() { inUse = false; }
    }

    [System.Serializable]
    public struct Range
    {
        public float min;
        public float max;
    }
    public Range ySpawnRange;

    public GameObject prefab;
    public int poolSize;
    public float shiftSpeed;
    public float spawnRate;

    public Vector3 defaultSpawnPos;
    public bool spawnImmediate;
    public Vector3 immediateSpawnPos;
    public Vector2 targetAspectRatio;

    float spawnTimer;
    float targetAspect;
    PoolObject[] poolObjects;
    GameManager gameManager;

    private void Awake()
    {
        Configure();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;        
    }

    private void OnEnable()
    {
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
        GameManager.OnGameStart += OnGameStart;
        
    }

    private void OnDisable()
    {
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void OnGameOverConfirmed()
    {

    }

    void OnGameStart()
    {
        for(int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].Dispose();
            poolObjects[i].transform.position = Vector3.one * 9999;
        }
        if (spawnImmediate)
            SpawnImmediate();
    }

    private void Update()
    {
        if (gameManager.GameOver) return;
        Shift(); 
        spawnTimer += Time.deltaTime;
        if(spawnTimer > spawnRate)
        {
            Spawn();
            spawnTimer = 0;
        }
    } 

    void Configure()
    {
        targetAspect = targetAspectRatio.x / targetAspectRatio.y;
        poolObjects = new PoolObject[poolSize];
        for(int i = 0; i < poolObjects.Length; i++)
        {
            GameObject gameObject = Instantiate(prefab) as GameObject;
            Transform t = gameObject.transform;
            t.SetParent(transform);
            t.position = Vector3.one * 9999;
            poolObjects[i] = new PoolObject(t);
        }
    }

    void Spawn()
    {
        Transform t = GetPoolObject();
        if (t == null) return; // pool size is too small
        Vector3 pos = Vector3.zero;
        pos.x = defaultSpawnPos.x;
        pos.y = Random.Range(ySpawnRange.min, ySpawnRange.max);
        pos.z = 0;
        t.position = pos;
    }

    void SpawnImmediate()
    {
        Transform t = GetPoolObject();
        if (t == null) return; // pool size is too small
        Vector3 pos = Vector3.zero;
        pos.x = immediateSpawnPos.x;
        pos.y = Random.Range(ySpawnRange.min, ySpawnRange.max);
        pos.z = 0;
        t.position = pos;
    }

    void Shift()
    {
        for(int i = 0; i < poolObjects.Length; i++)
        {
            if (poolObjects[i].inUse)
            {
                poolObjects[i].transform.position += Vector3.left * shiftSpeed * Time.deltaTime;
                CheckDisposeObject(poolObjects[i]);
            }
        }
    }

    void CheckDisposeObject(PoolObject poolObject)
    {
        if(poolObject.transform.position.x < -defaultSpawnPos.x)
        {
            poolObject.Dispose();
            poolObject.transform.position = Vector3.one * 9999;
        }
    }

    Transform GetPoolObject()
    {
        for(int i = 0; i < poolObjects.Length; i++)
        {
            if (!poolObjects[i].inUse)
            {
                poolObjects[i].Use();
                return poolObjects[i].transform;
            }
        }
        return null;
    }
}
