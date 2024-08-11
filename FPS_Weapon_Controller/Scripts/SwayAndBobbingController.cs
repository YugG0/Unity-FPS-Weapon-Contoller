using UnityEngine;

public class SwayAndBobbingController : MonoBehaviour
{
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private Transform swayAndBobTransform;

    [Header("Sway Settings")]
    [SerializeField] private float swayAmount = 0.2f;
    [SerializeField] private float maxSwayAmount = 0.2f;
    [SerializeField] private float swaySmoothness = 4.0f;

    [Header("Bobbing Settings")]
    [SerializeField] private float bobbingSpeed = 0.18f;
    [SerializeField] private float bobbingVerticalAmount = 0.2f;
    [SerializeField] private float bobbingHorizontalAmount = 0.05f;
    [SerializeField] private float bobbingTiltAmount = 2.0f;

    [Header("Aim Settings")]
    [SerializeField] private float shareAimingOn = 5;
    [SerializeField] private bool isAim = false;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float bobbingTimer = 0.0f;
    private Vector3 currentSwayPosition;
    private Vector3 targetPosition;

    private float previousHorizontalInput = 0.0f;
    private float previousVerticalInput = 0.0f;

    private void Start()
    {
        initialPosition = swayAndBobTransform.localPosition;
        initialRotation = swayAndBobTransform.localRotation;
        currentSwayPosition = initialPosition;
        targetPosition = initialPosition;
    }

    private void Update()
    {
        isAim = weaponController.GetIsAim();

        HandleSway();
        HandleBobbing();
        ApplySwayAndBobbing();
    }

    private void HandleSway()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float currentSwayAmount = swayAmount;
        float currentMaxSwayAmount = maxSwayAmount;
        float currentSwaySmoothness = swaySmoothness;

        if (isAim)
        {
            currentSwayAmount /= shareAimingOn;
            currentMaxSwayAmount /= shareAimingOn;
            currentSwaySmoothness *= shareAimingOn;
        }

        float swayX = Mathf.Clamp(-mouseX * currentSwayAmount, -currentMaxSwayAmount, currentMaxSwayAmount);
        float swayY = Mathf.Clamp(-mouseY * currentSwayAmount, -currentMaxSwayAmount, currentMaxSwayAmount);

        Vector3 swayTargetPosition = new Vector3(swayX, swayY, 0);
        currentSwayPosition = Vector3.Lerp(currentSwayPosition, swayTargetPosition, Time.deltaTime * currentSwaySmoothness);
    }

    private void HandleBobbing()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float currentBobbingSpeed = bobbingSpeed;
        float currentBobbingVerticalAmount = bobbingVerticalAmount;
        float currentBobbingHorizontalAmount = bobbingHorizontalAmount;
        float currentBobbingTiltAmount = bobbingTiltAmount;

        if (isAim)
        {
            currentBobbingVerticalAmount /= shareAimingOn;
            currentBobbingHorizontalAmount /= shareAimingOn;
            currentBobbingTiltAmount /= shareAimingOn;
        }

        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            Bobbing(swaySmoothness, currentBobbingSpeed, currentBobbingVerticalAmount, currentBobbingHorizontalAmount, currentBobbingTiltAmount, 1f);
        }
        else
        {
            Bobbing(swaySmoothness, currentBobbingSpeed, currentBobbingVerticalAmount, currentBobbingHorizontalAmount, currentBobbingTiltAmount, 5f);
        }

        currentSwayPosition = Vector3.Lerp(currentSwayPosition, targetPosition, Time.deltaTime * swaySmoothness);
    }

    private void Bobbing(float swaySmoothness, float currentBobbingSpeed, float currentBobbingAmount, float currentBobbingHorizontalAmount, float currentBobbingTiltAmount, float moderator)
    {
        bobbingTimer += Time.deltaTime * currentBobbingSpeed;

        float bobbingOffsetY = Mathf.Sin(bobbingTimer) * (currentBobbingAmount / moderator);
        float bobbingOffsetX = Mathf.Cos(bobbingTimer) * (currentBobbingHorizontalAmount / moderator);

        targetPosition = new Vector3(initialPosition.x + bobbingOffsetX, initialPosition.y + bobbingOffsetY, initialPosition.z);

        float tiltAngle = Mathf.Sin(bobbingTimer) * (currentBobbingTiltAmount / moderator);
        swayAndBobTransform.localRotation = Quaternion.Lerp(swayAndBobTransform.localRotation, Quaternion.Euler(new Vector3(tiltAngle, 0, 0)), Time.deltaTime * swaySmoothness);
    }

    private void ApplySwayAndBobbing()
    {
        swayAndBobTransform.localPosition = currentSwayPosition;
    }
}
