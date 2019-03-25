using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using DG.Tweening;

public class Zone : NetworkBehaviour {

    public float Damage { set {
            _damage = value;
        }
    }

    [SerializeField] private Vector3[] _scales;

    [SyncVar] private int idx = 0;

    private float _damage = 1f;
    private List<Destructable> _destructables = new List<Destructable>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && _destructables.Contains(other.GetComponent<Destructable>()))
        {
            other.GetComponent<PlayerController>().DisablePostProcess();
            _destructables.Remove(other.GetComponent<Destructable>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().ActivePostProcess();
            _destructables.Add(other.GetComponent<Destructable>());
        }
    }

    public void Start()
    {
        StartCoroutine(InflictDamage());
    }

    IEnumerator InflictDamage()
    {
        while (true) {
            yield return new WaitForSeconds(0.5f);
            foreach (var destructable in _destructables)
                if (destructable.gameObject.activeSelf)
                    destructable.CmdTakeDamage(_damage);
        }
    }

    public void StartScaleZone()
    {
        StartCoroutine(ScaleZone());
    }

    IEnumerator ScaleZone()
    {
        while (idx < _scales.Length)
        {
            yield return new WaitForSeconds(90f);
            transform.DOScale(_scales[idx], 90);
            _damage *= 2;
            yield return new WaitForSeconds(90f);
            idx++;
        }
    }
}