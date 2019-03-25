using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour {

    [SerializeField] private PlayerController playerController;
    [SerializeField] private MoveController moveController;
    [SerializeField] private TransformSync transformSync;
    [SerializeField] private PlayerShoot playerShoot;
    [SerializeField] private Destructable destructable;
    [SerializeField] private Inventory inventory;

	void Start () {
        if (isLocalPlayer)
        {
            playerController.enabled = true;
            moveController.enabled = true;
            transformSync.enabled = true;
            playerShoot.enabled = true;
            destructable.enabled = true;
            inventory.enabled = true;
        }
	}
}
