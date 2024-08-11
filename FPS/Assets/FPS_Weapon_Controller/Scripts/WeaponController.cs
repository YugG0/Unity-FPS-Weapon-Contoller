using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Gun")]
    [SerializeField] private Weapon weapon;

    [Header("Shoot Controller")]
    [SerializeField] private Transform shootStart;
    [SerializeField] private AudioSource weaponAudioSource;
    float timeSinceLastShot;

    [Header("Aim")]
    [SerializeField] private AimController aimController;

    [SerializeField] private bool aimInverse;
    private bool isAim;

    [Header("Recoil")]
    [SerializeField] private RecoilController recoilController;

    [Header("Shoot Impact")]
    [SerializeField] private GameObject impactPrefab;

    [Header("Weapon Switching")]
    [SerializeField] private WeaponSwitchingController weaponSwitchingController;
    [SerializeField] private int selectedWeapon;
    private int numberWeapons;

    [Header("HUD")]
    [SerializeField] private HudController hudController;

    private void Start()
    {
        numberWeapons = weaponSwitchingController.GetNumberWeapons();

        SetGun(selectedWeapon);

        ResetTimeSinceLastShot();
    }

    private void Update()
    {
        WeaponSwitching();
        ShootingAndAiming();
    }

    private void WeaponSwitching()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= numberWeapons - 1)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon++;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
            {
                selectedWeapon = numberWeapons - 1;
            }
            else
            {
                selectedWeapon--;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedWeapon = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedWeapon = 3;
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            SetGun(selectedWeapon);
        }
    }

    private void ShootingAndAiming()
    {
        if (weapon.gun.isAuto)
        {
            if (Input.GetMouseButton(0)) { Shoot(); }
        }
        else
        {
            if (Input.GetMouseButtonDown(0)) { Shoot(); }
        }

        timeSinceLastShot += Time.deltaTime;

        if (Input.GetMouseButton(1))
        {
            isAim = true;
            if (aimInverse) { isAim = false; }
        }
        else
        {
            isAim = false;
            if (aimInverse) { isAim = true; }
        }

        if (isAim)
        {
            aimController.GunPosition(weapon.gun.aimPosition);
        }
        else
        {
            aimController.GunPosition(weapon.gun.defautPosition);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReloading();
        }
    }

    private void Shoot()
    {
        if (CanShoot())
        {
            timeSinceLastShot = 0;

            weapon.currentAmmo--;
            if (weapon.currentAmmo <= 0) { StartReloading(); }
            hudController.SetAmmoText(weapon.currentAmmo, weapon.additionalAmmo);

            RaycastHit hit;

            for (int i = 0; i < weapon.gun.bulletsPerTap; i++)
            {
                Vector3 recoil = new Vector3(Random.Range(-weapon.gun.raycastRecoil, weapon.gun.raycastRecoil), Random.Range(-weapon.gun.raycastRecoil, weapon.gun.raycastRecoil), 0);
                if (isAim)
                {
                    recoil /= weapon.gun.reducedRaycastRecoilAiming;
                }

                if (Physics.Raycast(shootStart.position, shootStart.TransformDirection(Vector3.forward + recoil), out hit, weapon.gun.maxDistance))
                {
                    if (hit.transform.gameObject.tag == "Enemy")
                    {
                        hit.transform.GetComponent<Health>().Damage(weapon.gun.damage);
                    }

                    Instantiate(impactPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }

            weaponAudioSource.PlayOneShot(weapon.gun.shootClip);
            weapon.muzzleFlash.Play();
            recoilController.Recoil(weapon.gun.recoilRotation, weapon.gun.recoilPosition, isAim, weapon.gun.reducedRecoilAiming);
        }
    }

    private bool CanShoot() 
    { 
        return !weapon.reloading && weapon.currentAmmo > 0 && timeSinceLastShot > 1f / (weapon.gun.fireRate / 60f);
    }

    public bool GetIsAim()
    {
        return isAim;
    }

    private void ResetTimeSinceLastShot() 
    { 
        timeSinceLastShot = 1f / (weapon.gun.fireRate / 60f); 
    }

    private void StartReloading()
    {
        if (!weapon.reloading && weapon.currentAmmo < weapon.gun.magazineSize && weapon.additionalAmmo > 0)
        {
            StartCoroutine(Reloading());
        }
    }

    private IEnumerator Reloading()
    {
        weapon.reloading = true;

        weapon.additionalAmmo += weapon.currentAmmo;
        weapon.currentAmmo = 0;

        weaponAudioSource.PlayOneShot(weapon.gun.reloadClip);
        hudController.SetAmmoText(weapon.currentAmmo, weapon.additionalAmmo);

        weaponAudioSource.PlayOneShot(weapon.gun.reloadClip);

        yield return new WaitForSeconds(weapon.gun.reloadTime);

        weapon.reloading = false;

        if (weapon.additionalAmmo > weapon.gun.magazineSize)
        {
            weapon.currentAmmo = weapon.gun.magazineSize;
            weapon.additionalAmmo -= weapon.gun.magazineSize;
        }
        else
        {
            weapon.currentAmmo = weapon.additionalAmmo;
            weapon.additionalAmmo = 0;
        }

        hudController.SetAmmoText(weapon.currentAmmo, weapon.additionalAmmo);
    }

    private void SetGun(int gunId)
    {
        if (!weapon.reloading)
        {
            weapon = weaponSwitchingController.GetGun(gunId);

            ResetTimeSinceLastShot();
            hudController.SetAmmoText(weapon.currentAmmo, weapon.additionalAmmo);
            hudController.SetGunName(weapon.gun.name);
        }
    }
}
