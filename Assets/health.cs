using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class health : MonoBehaviour {

    IDeathInstructions deathInstructions;
    IDamageInstructions damageInstructions;

    public int maxHealth;
    public int currentHealth { get; private set; }
    [HideInInspector] public bool isDead;

    public Slider healthSlider;
    public bool disableSliderOnPlay;

    public damageCausers lastDamageCauser { get; private set; }
    public enum damageCausers
    {
        nothing,
        knife,
        shotgun,
        molotov,
    }

    void Start () {
        deathInstructions = GetComponent<IDeathInstructions>();
        damageInstructions = GetComponent<IDamageInstructions>();

        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
            healthSlider.gameObject.SetActive(!disableSliderOnPlay);
        }    
	}

    public void incrementHealthBy(int change)
    {
        if (healthSlider != null && !healthSlider.gameObject.activeSelf) healthSlider.gameObject.SetActive(true);
        if (isDead) return;
        currentHealth += change;
        if (currentHealth <= 0) killObject(currentHealth - change);
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        if (healthSlider != null) healthSlider.value = currentHealth;

        //If the change was negative, then there was damage
        if (change < 0 && damageInstructions != null) damageInstructions.executeDamageInstructions(change);

    }

    public void killObject(int healthBeforeDeath)
    {
        isDead = true;
        if (healthSlider != null) healthSlider.enabled = false;
        if (deathInstructions != null) deathInstructions.executeDeath(healthBeforeDeath);
        else Destroy(gameObject);
    }

    public void updateDamageCauser(damageCausers newCauser)
    {
        if (newCauser == lastDamageCauser) return;
        lastDamageCauser = newCauser;
    }

}
