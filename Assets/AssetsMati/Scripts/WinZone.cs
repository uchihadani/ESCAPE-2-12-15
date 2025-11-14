using UnityEngine;

public class WinZone : MonoBehaviour
{
    [SerializeField] private GameSceneController controller;

    private void Reset()
    {
        // Si agregás el script desde el Inspector, intenta autollenar
        if (controller == null) controller = FindObjectOfType<GameSceneController>();
        var col = GetComponent<Collider>();
        if (col) col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (controller == null)
            controller = FindObjectOfType<GameSceneController>();

        if (controller != null)
            controller.Victory();
        else
            Debug.LogWarning("GameSceneController no asignado en WinZone.");
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0.2f, 0.25f);
        var col = GetComponent<Collider>();
        if (col is BoxCollider b)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(b.center, b.size);
        }
    }
#endif
}
