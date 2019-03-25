using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Shooter {

    public override bool Fire()
    {
        base.Fire();

        if (canFire)
        {
            //var bullet = Instantiate(projectile, muzzle.position, muzzle.rotation);
            return true;
        }
        return false;
    }

    public override void Update() {
        base.Update();
        if (GameManager.Instance.InputController.Reload) {
            Reload();
        }
    }
}
