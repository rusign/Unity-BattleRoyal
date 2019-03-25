using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : InventoryItem
{
    [SerializeField] private GameObject _weaponPrefab;
    public GameObject WeaponPrefab
    {
        get { return _weaponPrefab; }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("grenade touche");
        if (other.tag == "Player")
        {
            other.transform.GetComponent<Inventory>().AddItem(this);
        }
    }
}