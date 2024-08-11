using UnityEngine;

public class DeleteAfterTime : MonoBehaviour
{
    [SerializeField] private float time;

    private void Start()
    {
        Invoke(nameof(Delete), time);
    }

    private void Delete()
    {
        Destroy(gameObject);
    }
}
