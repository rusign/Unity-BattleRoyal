using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : InventoryItem {

    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private Weapon _prefab;

    public Weapon Prefab {
        get {
            return _prefab;
        }
    }

    public WeaponType WeaponType{ 
        get {return _weaponType; }
    }

    [SerializeField]private GameObject _weaponPrefab;
    public GameObject WeaponPrefab
    {
        get { return _weaponPrefab; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.GetComponent<Inventory>().AddItem(this);
        }
    }
}
