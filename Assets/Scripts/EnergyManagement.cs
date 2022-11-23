using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManagement : MonoBehaviour
{
    private ShipController _shipController;
    private ProjectileGun _gunController;
    private GameObject _shieldsController;

    /* TODO: nacrt toho, jak by to potom mohlo vypadat.
     * celkova energie (s poskozenim bude postupne klesat) a jeji procentualni
     * distribuce mezi systemama na lodi.
     * Pri kazdem updatu energie (at uz ubytek kvuli poskozeni nebo uprava ze strany hrace)
     * se prepocita, kam jde kolik energie a adekvatne se to upravi.
     * Total energy se da doplnit power-upama 
     */
    
    private float _totalEnergy = 1.0f;

    private float _enginesEnergyPart       = 2/10;
    private float _weaponsEnergyPart       = 2/10;
    private float _frontShieldsEnergyPart  = 1/10;
    private float _backShieldsEnergyPart   = 1/10;
    private float _leftShieldsEnergyPart   = 1/10;
    private float _rightShieldsEnergyPart  = 1/10;
    private float _topShieldsEnergyPart    = 1/10;
    private float _bottomShieldsEnergyPart = 1/10;

    private void Start()
    {
        _shipController = GetComponent<ShipController>();
        if (_shipController == null)
            Debug.Log("missing shipController component!");
        _gunController = GetComponent<ProjectileGun>();
        if (_gunController == null)
            Debug.Log("missing projectileGun component!");
        //TODO: inicializace stitu
    }

    private void setEnergy()
    {
        _gunController.SetEnergy(_weaponsEnergyPart * _totalEnergy);
        _shipController.SetEnergy(_enginesEnergyPart * _totalEnergy);
        //TODO: shields
    }

    public void EnergyToEngines()
    {
        //TODO vypocet a uprava
    }
    
    public void EnergyToWeapons()
    {
        //TODO vypocet a uprava
    }
    
    public void EnergyToShields(GameUtils.ShieldType type)
    {
        //TODO vypocet a uprava
    }
}
