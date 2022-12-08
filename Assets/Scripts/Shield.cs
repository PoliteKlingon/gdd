using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private GameUtils.ShieldType type;
    [SerializeField] private float damageDivider = 10;
    private const float ENERGY_DAMAGE_DIVIDER = 100f;
    // [SerializeField] private float rocketDamage = 20;
    [SerializeField] private Health shipHealth;

    private static readonly float METEOR_COLLISION_DMG = 75;
    private static readonly float SHIP_COLLISION_DMG = 50;
    private float _energy;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEnergy(float amount)
    {
        _energy = amount;
    }

    public void DealDamage(float dmgAmount)
    {
        float energyDamage = dmgAmount / ENERGY_DAMAGE_DIVIDER;
        float totalEnergy = GetComponentInParent<EnergyManagement>().GetTotalEnergy();
        energyDamage = (energyDamage > _energy) ? _energy : energyDamage;
        float remainingEnergyConstant = totalEnergy / (totalEnergy - energyDamage);
        GetComponentInParent<EnergyManagement>().DealShieldDamage(type, remainingEnergyConstant);
        Debug.Log("Energy dmg constant: " + remainingEnergyConstant);
        if (_energy <= 0)
        {   
            float healthDamage = (dmgAmount - energyDamage * ENERGY_DAMAGE_DIVIDER) / damageDivider;
            Debug.Log("Deal HP dmg: " + healthDamage);
            shipHealth.DealDamage(healthDamage);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == this.tag)
                return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Shields"))
        {
            collision.gameObject.GetComponent<Shield>().DealDamage(SHIP_COLLISION_DMG);
            Debug.Log("Ship collided!");
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Meteor"))
        {
            DealDamage(METEOR_COLLISION_DMG);
            Debug.Log("Meteor collided!");
        }
    }
}
