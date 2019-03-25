using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour {

    [SerializeField] private float speed;
    [SerializeField] private float timeToLive;
    [SerializeField] private float damage;

    private TimeBehaviour timeBehaviour;

    private void Start()
    {
        timeBehaviour = GetComponent<TimeBehaviour>();
         Destroy(gameObject, timeToLive);
    }

    void Update () {
        transform.Translate(Vector3.right * speed * Time.deltaTime * timeBehaviour.LocalTimeScale);
	}

    protected virtual void OnTriggerEnter(Collider other)
    {
        var destructable = other.transform.GetComponent<Destructable>();
        if ((!destructable && other.tag == "ForceField") || (other.tag == "Zone")) {
            return;
        }
        if (!destructable) {
            Destroy(gameObject);
            return;
        }
        destructable.CmdTakeDamage(damage);
        Destroy(gameObject);
    }
}
