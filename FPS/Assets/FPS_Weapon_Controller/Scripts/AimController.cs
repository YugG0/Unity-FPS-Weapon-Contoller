using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField] private Transform aimTransform;
    [SerializeField] private float rateChange = 10f;

    public void GunPosition(Vector3 desiredPosition)
    {
        aimTransform.localPosition = Vector3.Lerp(aimTransform.localPosition, desiredPosition, rateChange * Time.deltaTime);
    }
}
