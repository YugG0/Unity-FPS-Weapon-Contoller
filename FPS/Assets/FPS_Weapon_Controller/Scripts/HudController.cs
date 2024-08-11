using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    [SerializeField] private Text ammoText;
    [SerializeField] private Text gunNameText;

    public void SetAmmoText(int currentAmmo, int additionalAmmo)
    {
        ammoText.text = currentAmmo.ToString() + "|" + additionalAmmo.ToString();
    }

    public void SetGunName(string gunName)
    {
        gunNameText.text = gunName;
    }
}
