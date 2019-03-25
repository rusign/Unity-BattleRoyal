using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RotationSync : NetworkBehaviour {

    [SyncVar] private Quaternion syncRot;

    [SerializeField] private Transform myTransform;
    [SerializeField] private float lerpRate = 15;

    private void FixedUpdate()
    {
        TransmitRotation();
        LerpRotation();
    }

    void LerpRotation()
    {
        if (!isLocalPlayer)
        {
            myTransform.rotation = Quaternion.Lerp(myTransform.rotation, syncRot, Time.deltaTime * lerpRate);
        }
    }

    [Command]
    void CmdProvideRotationToServer(Quaternion rot)
    {
        syncRot = rot;
    }

    [ClientCallback]
    void TransmitRotation()
    {
        if (isLocalPlayer)
        {
            CmdProvideRotationToServer(myTransform.rotation);
        }
    }
}
