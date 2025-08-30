using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveSpawner : MonoBehaviour
{
    [Header("Spawn Prefabs")]
    public GameObject enemyPrefab;
    public GameObject buffPickupPrefab;
    public GameObject itemPickupPrefab;
    public GameObject[] decorativeObjects; // Box, Barrel, Bag models

    [Header("Enemy Spawning")]
    public int minEnemies = 5;
    public int maxEnemies = 10;
    public float enemySpawnHeight = 1f;
    public float minDistanceBetweenEnemies = 8f;

    [Header("Pickup Spawning")]
    public int minBuffPickups = 3;
    public int maxBuffPickups = 6;
    public int minItemPickups = 8;
    public int maxItemPickups = 15;
    public float pickupSpawnHeight = 0.5f;

    [Header("Spawn Area")]
    public Vector3 spawnAreaCenter = Vector3.zero;
    public Vector3 spawnAreaSize = new Vector3(50f, 10f, 50f);
    public LayerMask groundLayer = 1; // Default layer
    public LayerMask obstacleLayer = 1; // What to avoid spawning in

    [Header("Spawn Settings")]
    public int maxSpawnAttempts = 100;
    public float minDistanceFromPlayer = 5f;

    private List<Vector3> spawnedPositions = new List<Vector3>();
    private Transform playerTransform;

    void Start()
    {
        // Find player
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            playerTransform = player.transform;

        StartCoroutine(SpawnAllObjects());
    }

    IEnumerator SpawnAllObjects()
    {
        yield return new WaitForSeconds(0.5f); // Wait for scene to fully load

        spawnedPositions.Clear();

        // Spawn enemies
        SpawnEnemies();
        yield return new WaitForSeconds(0.1f);

        // Spawn buff pickups
        SpawnBuffPickups();
        yield return new WaitForSeconds(0.1f);

        // Spawn item pickups
        SpawnItemPickups();
        yield return new WaitForSeconds(0.1f);

        // Spawn decorative objects
        SpawnDecorativeObjects();

        Debug.Log($"Cave spawning complete! Spawned at {spawnedPositions.Count} locations.");
    }

    void SpawnEnemies()
    {
        if (enemyPrefab == null) return;

        int enemyCount = Random.Range(minEnemies, maxEnemies + 1);
        int spawnedEnemies = 0;

        for (int i = 0; i < maxSpawnAttempts && spawnedEnemies < enemyCount; i++)
        {
            Vector3 spawnPos = GetRandomSpawnPosition(enemySpawnHeight, minDistanceBetweenEnemies);

            if (spawnPos != Vector3.zero)
            {
                GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                enemy.name = $"Enemy_{spawnedEnemies + 1}";

                // Parent to organize hierarchy
                Transform enemyParent = GameObject.Find("EnemySpawns")?.transform;
                if (enemyParent == null)
                {
                    GameObject enemyContainer = new GameObject("EnemySpawns");
                    enemyParent = enemyContainer.transform;
                }
                enemy.transform.SetParent(enemyParent);

                spawnedEnemies++;
                spawnedPositions.Add(spawnPos);
            }
        }

        Debug.Log($"Spawned {spawnedEnemies} enemies in the cave.");
    }

    void SpawnBuffPickups()
    {
        if (buffPickupPrefab == null) return;

        int buffCount = Random.Range(minBuffPickups, maxBuffPickups + 1);
        int spawnedBuffs = 0;

        for (int i = 0; i < maxSpawnAttempts && spawnedBuffs < buffCount; i++)
        {
            Vector3 spawnPos = GetRandomSpawnPosition(pickupSpawnHeight, 3f);

            if (spawnPos != Vector3.zero)
            {
                GameObject buff = Instantiate(buffPickupPrefab, spawnPos, Quaternion.identity);
                buff.name = $"BuffPickup_{spawnedBuffs + 1}";

                // Parent to organize hierarchy
                Transform buffParent = GameObject.Find("BuffSpawns")?.transform;
                if (buffParent == null)
                {
                    GameObject buffContainer = new GameObject("BuffSpawns");
                    buffParent = buffContainer.transform;
                }
                buff.transform.SetParent(buffParent);

                spawnedBuffs++;
                spawnedPositions.Add(spawnPos);
            }
        }

        Debug.Log($"Spawned {spawnedBuffs} buff pickups in the cave.");
    }

    void SpawnItemPickups()
    {
        if (itemPickupPrefab == null) return;

        int itemCount = Random.Range(minItemPickups, maxItemPickups + 1);
        int spawnedItems = 0;

        for (int i = 0; i < maxSpawnAttempts && spawnedItems < itemCount; i++)
        {
            Vector3 spawnPos = GetRandomSpawnPosition(pickupSpawnHeight, 2f);

            if (spawnPos != Vector3.zero)
            {
                GameObject item = Instantiate(itemPickupPrefab, spawnPos, Quaternion.identity);
                item.name = $"ItemPickup_{spawnedItems + 1}";

                // Randomize item type
                ItemPickup itemScript = item.GetComponent<ItemPickup>();
                if (itemScript != null)
                {
                    itemScript.ItemIndex = Random.Range(0, 2); // 0 = mushroom, 1 = oil
                }

                // Parent to organize hierarchy
                Transform itemParent = GameObject.Find("ItemSpawns")?.transform;
                if (itemParent == null)
                {
                    GameObject itemContainer = new GameObject("ItemSpawns");
                    itemParent = itemContainer.transform;
                }
                item.transform.SetParent(itemParent);

                spawnedItems++;
                spawnedPositions.Add(spawnPos);
            }
        }

        Debug.Log($"Spawned {spawnedItems} item pickups in the cave.");
    }

    void SpawnDecorativeObjects()
    {
        if (decorativeObjects == null || decorativeObjects.Length == 0) return;

        int decorativeCount = Random.Range(5, 12);
        int spawnedDecorative = 0;

        for (int i = 0; i < maxSpawnAttempts && spawnedDecorative < decorativeCount; i++)
        {
            Vector3 spawnPos = GetRandomSpawnPosition(0f, 3f);

            if (spawnPos != Vector3.zero)
            {
                GameObject decorative = decorativeObjects[Random.Range(0, decorativeObjects.Length)];
                GameObject spawnedObj = Instantiate(decorative, spawnPos, Quaternion.Euler(0, Random.Range(0, 360), 0));
                spawnedObj.name = $"Decorative_{decorative.name}_{spawnedDecorative + 1}";

                // Parent to organize hierarchy
                Transform decorativeParent = GameObject.Find("DecorativeObjects")?.transform;
                if (decorativeParent == null)
                {
                    GameObject decorativeContainer = new GameObject("DecorativeObjects");
                    decorativeParent = decorativeContainer.transform;
                }
                spawnedObj.transform.SetParent(decorativeParent);

                spawnedDecorative++;
                spawnedPositions.Add(spawnPos);
            }
        }

        Debug.Log($"Spawned {spawnedDecorative} decorative objects in the cave.");
    }

    Vector3 GetRandomSpawnPosition(float heightOffset, float minDistanceFromOthers)
    {
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            // Generate random position within spawn area
            Vector3 randomPos = new Vector3(
                Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                spawnAreaSize.y,
                Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
            ) + spawnAreaCenter;

            // Raycast down to find ground
            RaycastHit hit;
            if (Physics.Raycast(randomPos, Vector3.down, out hit, spawnAreaSize.y * 2, groundLayer))
            {
                Vector3 groundPos = hit.point + Vector3.up * heightOffset;

                // Check if position is valid
                if (IsValidSpawnPosition(groundPos, minDistanceFromOthers))
                {
                    return groundPos;
                }
            }
        }

        return Vector3.zero; // Failed to find valid position
    }

    bool IsValidSpawnPosition(Vector3 position, float minDistance)
    {
        // Check distance from player
        if (playerTransform != null)
        {
            float distanceFromPlayer = Vector3.Distance(position, playerTransform.position);
            if (distanceFromPlayer < minDistanceFromPlayer)
                return false;
        }

        // Check distance from other spawned objects
        foreach (Vector3 spawnedPos in spawnedPositions)
        {
            if (Vector3.Distance(position, spawnedPos) < minDistance)
                return false;
        }

        // Check for obstacles (walls, other objects)
        Collider[] obstacles = Physics.OverlapSphere(position, 1f, obstacleLayer);
        if (obstacles.Length > 0)
            return false;

        return true;
    }

    // Respawn system for dynamic gameplay
    public void RespawnEnemies()
    {
        // Clear existing enemies
        Transform enemyParent = GameObject.Find("EnemySpawns")?.transform;
        if (enemyParent != null)
        {
            for (int i = enemyParent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(enemyParent.GetChild(i).gameObject);
            }
        }

        // Remove enemy positions from spawn list
        spawnedPositions.Clear();

        // Respawn enemies
        SpawnEnemies();
    }

    void OnDrawGizmos()
    {
        // Draw spawn area in editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(spawnAreaCenter, spawnAreaSize);

        // Draw spawned positions
        Gizmos.color = Color.red;
        foreach (Vector3 pos in spawnedPositions)
        {
            Gizmos.DrawWireSphere(pos, 0.5f);
        }
    }
}
