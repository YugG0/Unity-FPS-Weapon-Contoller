using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon
{
    [Header("Data")]
    public Gun gun;

    [Header("Models")]
    public GameObject gunModel;
    public ParticleSystem muzzleFlash;

    [Header("Ammo")]
    public int currentAmmo;
    public int additionalAmmo;

    [Header("Gun")]
    public bool reloading;
}

public class WeaponSwitchingController : MonoBehaviour
{
    [SerializeField] private List<Weapon> weapons;

    private void Awake()
    {
        InitializingWeapons();
    }

    private void InitializingWeapons()
    {
        foreach (Weapon weapon in weapons) 
        {
            weapon.currentAmmo = weapon.gun.magazineSize;
            weapon.additionalAmmo = weapon.gun.maximumAdditionalAmmo;
        }
    }

    public Weapon GetGun(int gunId)
    {
        foreach (Weapon weapon in weapons) {
            weapon.gunModel.SetActive(false);
        }

        weapons[gunId].gunModel.SetActive(true);

        return weapons[gunId];
    }

    public int GetNumberWeapons()
    {
        return weapons.Count;
    }
}
