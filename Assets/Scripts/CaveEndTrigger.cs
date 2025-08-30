using UnityEngine;

public class CaveEndTrigger : MonoBehaviour
{
    void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the cave exit!");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.TriggerGameEnd();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
