using UnityEngine;

public class FloraEraser : MonoBehaviour
{
    public FloraEraserMode Mode;
    public LayerMask LayerMaskToDestroy;

    private bool Started;

    void Start()
    {
        Started = true;
        if (Mode == FloraEraserMode.RunOnceOnStart ||
            Mode == FloraEraserMode.RunOnFixedUpdate)
        {
            DestroyOverlappingFlora();
        }
    }

    void FixedUpdate()
    {
        if (Mode == FloraEraserMode.RunOnFixedUpdate)
        {
            DestroyOverlappingFlora();
        }
    }

    public void DestroyOverlappingFlora()
    {
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, LayerMaskToDestroy);
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

public enum FloraEraserMode
{
    RunManual,
    RunOnceOnStart,
    RunOnFixedUpdate
}