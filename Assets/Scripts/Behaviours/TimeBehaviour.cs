using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBehaviour : MonoBehaviour {

    private float _localTimeScale = 1f;
    private Rigidbody rigid;

    public float LocalTimeScale {
        get {
            return _localTimeScale;
        }
    }

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void TimeController(float value)
    {
        float multiplier = value / _localTimeScale;
        if (rigid)
        {
            rigid.drag *= multiplier;
            rigid.angularDrag *= multiplier;
            rigid.angularVelocity *= multiplier;
            //rigid.velocity *= multiplier;
        }
        _localTimeScale = value;
    }

    private void FixedUpdate()
    {
        CounterGravity();
    }

    void CounterGravity()
    {
        if (rigid)
            rigid.AddForce(-Physics.gravity + (Physics.gravity * _localTimeScale * _localTimeScale), ForceMode.Acceleration);
    }
}
