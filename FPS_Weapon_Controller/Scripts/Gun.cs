using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class Gun : ScriptableObject
{
    [Header("Info")]
    public string name;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;
    public float fireRate;
    public int bulletsPerTap;
    public bool isAuto;

    [Header("Ammo")]
    public int magazineSize;
    public int maximumAdditionalAmmo;

    public float reloadTime;

    [Header("Aim")]
    public Vector3 defautPosition;
    public Vector3 aimPosition;

    [Header("Raycast Recoil")]
    public float raycastRecoil;
    public float reducedRaycastRecoilAiming;

    [Header("Weapon And Camera Recoil")]
    public Vector3 recoilRotation;
    public Vector3 recoilPosition;
    public float reducedRecoilAiming;

    [Header("Sounds")]
    public AudioClip shootClip;
    public AudioClip reloadClip;
}
