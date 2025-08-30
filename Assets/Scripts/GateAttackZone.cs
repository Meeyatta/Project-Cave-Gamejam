using UnityEngine;

public class GateAttackZone : MonoBehaviour
{
    private PersistentNightGate parentGate;

    void Start()
    {
        parentGate = GetComponentInParent<PersistentNightGate>();

        if (parentGate == null)
        {
            Debug.LogError("GateAttackZone must be child of object with PersistentNightGate script!");
        }

        // Make sure this collider is set as trigger
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (parentGate != null && (other.CompareTag("Player") || other.name.Contains("AttackTrigger")))
        {
            parentGate.OnPlayerEnterAttackRange(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (parentGate != null && (other.CompareTag("Player") || other.name.Contains("AttackTrigger")))
        {
            parentGate.OnPlayerExitAttackRange(other);
        }
    }
}
