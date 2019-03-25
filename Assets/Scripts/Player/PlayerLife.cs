using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerLife : NetworkBehaviour {

    [SyncVar] private int life;

	public void TakeDamage(int nb)
    {
        life -= nb;

        if (life <= 0) {
            //DEAD
        }
    }
}
