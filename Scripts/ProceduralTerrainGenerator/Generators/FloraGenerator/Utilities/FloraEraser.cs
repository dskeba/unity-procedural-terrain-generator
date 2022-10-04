using UnityEngine;

public class FloraEraser : MonoBehaviour
{
    bool Started;
    public LayerMask LayerMask;

    void Start()
    {
        Started = true;
    }

    void FixedUpdate()
    {
        CheckCollisions();
    }

    void CheckCollisions()
    {
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, LayerMask);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            Object.Destroy(hitColliders[i].gameObject);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (Started)
        {
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
    }
}