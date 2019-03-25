using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Destructable : NetworkBehaviour {

    [SerializeField] protected float hitPoints;

    public event System.Action OnDeath;
    public event System.Action OnDamageReceived;

    [SyncVar(hook = "OnDamageHook")]
    [SerializeField] protected float damageTaken;

	public float HitPointsRemaining
    {
        get
        {
            return hitPoints - damageTaken;
        }
    }

    public bool IsAlive
    {
        get
        {
            return HitPointsRemaining > 0;
        }
    }

    public virtual void Die()
    {
        if (!IsAlive)
            return;
        if (OnDeath != null)
            OnDeath();
    }

    [Command]
    public virtual void CmdTakeDamage(float amount)
    {
        if (!isServer)
            return;
        damageTaken += amount;

        if (OnDamageReceived != null)
            OnDamageReceived();

        if (HitPointsRemaining <= 0)
            Die();
    }

    public void Reset()
    {
        damageTaken = 0;
    }

    void OnDamageHook(float damage)
    {
        OnDamageTaken(damage);
    }

    public virtual void OnDamageTaken(float damage)
    {
    }
}
