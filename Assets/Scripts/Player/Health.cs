using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Health : Destructable
{
    public Image healthBar;

    private void Start()
    {
        if (isLocalPlayer)
            healthBar = GameObject.FindWithTag("HealthBar").GetComponent<Image>();
    }

    public override void Die()
    {
        base.Die();
        gameObject.SetActive(false);
    }

    public override void OnDamageTaken(float damage)
    {
        base.OnDamageTaken(damage);

        if (healthBar)
            healthBar.fillAmount = (hitPoints - damage) / hitPoints;
    }
}
