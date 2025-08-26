using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    [System.Serializable]
    public class DropItem
    {
        public GameObject itemPrefab;  // The item to drop
        [Range(0f, 1f)] public float dropChance; // Chance (0 = never, 1 = always)
    }

    public List<DropItem> dropTable = new List<DropItem>();

    public void TryDropItem()
    {
        foreach (DropItem drop in dropTable)
        {
            float roll = Random.value; // Random between 0 and 1
            if (roll <= drop.dropChance)
            {
                Instantiate(drop.itemPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
