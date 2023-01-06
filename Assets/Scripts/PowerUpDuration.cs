using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDuration
{
    private float duration;
    private PowerUps powerUp;
    private GameObject player;


    public PowerUpDuration(float duration, PowerUps powerup, GameObject player)
    {
        this.duration = duration;
        this.powerUp = powerup;
        this.player = player;
        this.startPowerUp();
    }

    public bool recalculateDuration()
    {
        this.duration -= Time.deltaTime;
        return this.duration < 0;
    }

    public void endPowerUp()
    {
        ShipController shipController = player.GetComponentInParent<ShipController>();
        ProjectileGun gunScript = player.GetComponentInParent<ProjectileGun>();
        switch (powerUp)
        {
            case PowerUps.SPEED:
                shipController.setAcceleration(-PowerUpsManager.Instance.accShift);
                shipController.setMaxSpeed(-PowerUpsManager.Instance.speedShift);
                break;
            case PowerUps.HEALTH:
                break;
            case PowerUps.DAMAGE:
                gunScript.setPower(-PowerUpsManager.Instance.damageShift, -PowerUpsManager.Instance.shootSpeedShift, -PowerUpsManager.Instance.shootIntervalShift);
                break;
            case PowerUps.INVISIBILITY:
                shipController.setVisible();
                break;
            case PowerUps.ENERGY:
                break;
        }
    }

    public void startPowerUp()
    {
        ShipController shipController = player.GetComponentInParent<ShipController>();
        ProjectileGun gunScript = player.GetComponentInParent<ProjectileGun>();
        EnergyManagement energyScript = player.GetComponentInParent<EnergyManagement>();
        Health healthScript = player.GetComponentInParent<Health>();

        Debug.Log(shipController == null);
        switch (powerUp)
        {
            case PowerUps.SPEED:
                shipController.setAcceleration(PowerUpsManager.Instance.accShift);
                shipController.setMaxSpeed(PowerUpsManager.Instance.speedShift);
                break;
            case PowerUps.HEALTH:
                healthScript.Heal(PowerUpsManager.Instance.healthPlus);
                break;
            case PowerUps.DAMAGE:
                gunScript.setPower(PowerUpsManager.Instance.damageShift, PowerUpsManager.Instance.shootSpeedShift, PowerUpsManager.Instance.shootIntervalShift);
                break;
            case PowerUps.INVISIBILITY:
                shipController.setInvisible();
                break;
            case PowerUps.ENERGY:
                energyScript.AddEnergy(PowerUpsManager.Instance.energyPlus);
                break;
        }
    }

    public bool recalculateDelay() // true if powerup should end
    {
        this.duration -= Time.deltaTime;
        return this.duration < 0;
    }
}
