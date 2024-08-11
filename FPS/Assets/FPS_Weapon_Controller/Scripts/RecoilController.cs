using UnityEngine;

public class RecoilController : MonoBehaviour
{
    [SerializeField] private Transform recoilTransform;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float snappinessRot, snappinessPos;
    [SerializeField] private float returnSpeedRot, returnSpeedPos;

    private Vector3 currentRotation, targetRotation, targetPosition;

    private void Update()
    {
        ReturnRecoil();
    }

    private void ReturnRecoil()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeedRot * Time.deltaTime);
        targetPosition = Vector3.Lerp(targetPosition, Vector3.zero, returnSpeedPos * Time.deltaTime);

        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappinessRot * Time.fixedDeltaTime);
        recoilTransform.localPosition = Vector3.Slerp(recoilTransform.localPosition, targetPosition, snappinessPos * Time.deltaTime);
        cameraTransform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void Recoil(Vector3 recoilRotation, Vector3 recoilPosition, bool isAim, float reducedRecoilAiming)
    {
        if (isAim)
        {
            targetRotation += new Vector3(recoilRotation.x / reducedRecoilAiming, Random.Range(-recoilRotation.y, recoilRotation.y) / reducedRecoilAiming, Random.Range(-recoilRotation.z, recoilRotation.z) / reducedRecoilAiming);
            targetPosition += new Vector3(Random.Range(-recoilPosition.x, recoilPosition.x) / reducedRecoilAiming, recoilPosition.y / reducedRecoilAiming, recoilPosition.z / reducedRecoilAiming);
        }
        else
        {
            targetRotation += new Vector3(recoilRotation.x, Random.Range(-recoilRotation.y, recoilRotation.y), Random.Range(-recoilRotation.z, recoilRotation.z));
            targetPosition += new Vector3(Random.Range(-recoilPosition.x, recoilPosition.x), recoilPosition.y, recoilPosition.z);
        }
    }
}
