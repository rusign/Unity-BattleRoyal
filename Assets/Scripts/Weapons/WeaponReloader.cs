using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReloader : MonoBehaviour {

    [SerializeField] private float reloadTime;
    [SerializeField] private int clipSize;
    [SerializeField] private WeaponType _weaponType;
    private Inventory _inventory;

    private bool isReloading;
    public int ammo;

    public int RoundsRemainingClip {
        get {
            return ammo;
        }
    }

    public bool IsReloading {
        get {
            return isReloading;
        }
    }

    public void Start()
    {
        _inventory = GetComponentInParent<Inventory>();
        ammo = clipSize;
    }

    public void Reload()
    {
        if (isReloading)
            return;
        isReloading = true;
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(reloadTime);
        ExecuteReload();
    }

    void ExecuteReload()
    {
        isReloading = false;
        ammo = _inventory.GetAmmo(_weaponType, clipSize - ammo);
    }

    public void TakeFromClip(int amount)
    {
        ammo -= amount;
    }
}
