using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
public class GrenadeShoot : NetworkBehaviour {

    public GameObject Player;

    [SerializeField] private float speed;
    [SerializeField] private float timeToLive;
    [SerializeField] private GameObject exploPrefab;


    private void Start()
    {
        StartCoroutine(DestroyTimer());
        GetComponent<Rigidbody>().AddForce(new Vector3(transform.forward.x,transform.forward.y + 1.3f, transform.forward.z)  * speed);
    }

    IEnumerator DestroyTimer(){
        yield return new WaitForSeconds(timeToLive);
        CmdEndTimer();
    }

    [Command]
    void CmdEndTimer()
    {
        var tmp = Instantiate(exploPrefab, transform.position, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(tmp);
        NetworkServer.Destroy(gameObject);
    }

}
