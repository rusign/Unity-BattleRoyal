using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponReloader))]
public class Shooter : MonoBehaviour
{

    [SerializeField] private float rateOfFire;
    [SerializeField] protected GameObject projectile;

    [SerializeField] protected Transform muzzle;
    [SerializeField] private int weaponType;

    private WeaponReloader weaponReloader;

    public bool isLocalPlayer = false;

    public GameObject Prefab {
        get {
            return projectile;
        }
    }

    public Transform Muzzle {
        get
        {
            return muzzle;
        }
    }

    private float nextFireAllowed;
    protected bool canFire;
    protected Transform spine;

    private void Awake()
    {
        spine = transform.parent;
        weaponReloader = GetComponent<WeaponReloader>();

    }

    public void Equip()
    {
        spine = transform.parent;
        weaponReloader = GetComponent<WeaponReloader>();
        transform.GetComponentInParent<Animator>().SetInteger("WeaponType_int", weaponType);
    }

    public virtual bool Fire()
    {
        canFire = false;

        if (Time.time < nextFireAllowed)
            return false;
        if (weaponReloader.IsReloading)
            return false;
        if (weaponReloader.RoundsRemainingClip <= 0)
        {
            weaponReloader.Reload();
            return false;
        }
        weaponReloader.TakeFromClip(1);
        nextFireAllowed = Time.time + rateOfFire;
        canFire = true;
        return true;
    }

    public virtual void Update()
    {
        if (isLocalPlayer)
            PositionSpine();
    }

    void PositionSpine()
    {
        Transform mainCamT = Camera.main.transform;
        Vector3 mainCamPos = mainCamT.position;
        Vector3 dir = mainCamT.forward;
        Ray ray = new Ray(mainCamPos, dir);

        transform.parent.LookAt(ray.GetPoint(500));

        //Vector3 eulerAngleOffset = new Vector3(3.67f, 44.95f, 7.45f);
        //spine.Rotate(eulerAngleOffset);
    }

    public void Reload()
    {
        weaponReloader.Reload();
    }
}
