using UnityEngine;

public class EnergyManagement : MonoBehaviour
{
    private ShipController _shipController;
    private ProjectileGun _gunController;
    private ShieldsController _shieldsController;

    /* Nacrt:
     * celkova energie (s poskozenim bude postupne klesat) a jeji procentualni
     * distribuce mezi systemama na lodi.
     * Pri kazdem updatu energie (at uz ubytek kvuli poskozeni nebo uprava ze strany hrace)
     * se prepocita, kam jde kolik energie a adekvatne se to upravi.
     * Total energy se da doplnit power-upama 
     */
    
    [SerializeField] private float totalEnergy = 1.0f; //to be changed? we'll see

    private float _enginesEnergyPart       = 2.0f/10;
    private float _weaponsEnergyPart       = 2.0f/10;
    private float _frontShieldsEnergyPart  = 1.0f/10;
    private float _backShieldsEnergyPart   = 1.0f/10;
    private float _leftShieldsEnergyPart   = 1.0f/10;
    private float _rightShieldsEnergyPart  = 1.0f/10;
    private float _topShieldsEnergyPart    = 1.0f/10;
    private float _bottomShieldsEnergyPart = 1.0f/10;

    private void Start()
    {
        _shipController = GetComponent<ShipController>();
        if (_shipController == null)
            Debug.Log("missing shipController component!");
        _gunController = GetComponent<ProjectileGun>();
        if (_gunController == null)
            Debug.Log("missing projectileGun component!");
        _shieldsController = GetComponent<ShieldsController>();
        if (_shieldsController == null)
            Debug.Log("missing shieldsController component!");
        SetEnergy();
    }

    public void AddEnergy(float amount) //or subtract
    {
        totalEnergy += amount;
        if (totalEnergy < 0.0f)
            totalEnergy = 0.0f;
    }

    private void SetEnergy()
    {
        _gunController.SetEnergy(_weaponsEnergyPart * totalEnergy);
        _shipController.SetEnergy(_enginesEnergyPart * totalEnergy);
        _shieldsController.SetEnergy(_frontShieldsEnergyPart * totalEnergy, GameUtils.ShieldType.Front);
        _shieldsController.SetEnergy(_backShieldsEnergyPart * totalEnergy, GameUtils.ShieldType.Back);
        _shieldsController.SetEnergy(_topShieldsEnergyPart * totalEnergy, GameUtils.ShieldType.Top);
        _shieldsController.SetEnergy(_bottomShieldsEnergyPart * totalEnergy, GameUtils.ShieldType.Bottom);
        _shieldsController.SetEnergy(_leftShieldsEnergyPart * totalEnergy, GameUtils.ShieldType.Left);
        _shieldsController.SetEnergy(_rightShieldsEnergyPart * totalEnergy, GameUtils.ShieldType.Right);
    }

    public void EnergyToEngines()
    {
        float toAdd = 0;
        toAdd += _weaponsEnergyPart * 0.1f;
        _weaponsEnergyPart *= 0.9f;
        toAdd += _frontShieldsEnergyPart * 0.1f;
        _frontShieldsEnergyPart *= 0.9f;
        toAdd += _backShieldsEnergyPart * 0.1f;
        _backShieldsEnergyPart *= 0.9f;
        toAdd += _leftShieldsEnergyPart * 0.1f;
        _leftShieldsEnergyPart *= 0.9f;
        toAdd += _rightShieldsEnergyPart * 0.1f;
        _rightShieldsEnergyPart *= 0.9f;
        toAdd += _topShieldsEnergyPart * 0.1f;
        _topShieldsEnergyPart *= 0.9f;
        toAdd += _bottomShieldsEnergyPart * 0.1f;
        _bottomShieldsEnergyPart *= 0.9f;
        
        _enginesEnergyPart += toAdd;
        SetEnergy();
    }
    
    public void EnergyToWeapons()
    {
        float toAdd = 0;
        toAdd += _enginesEnergyPart * 0.1f;
        _enginesEnergyPart *= 0.9f;
        toAdd += _frontShieldsEnergyPart * 0.1f;
        _frontShieldsEnergyPart *= 0.9f;
        toAdd += _backShieldsEnergyPart * 0.1f;
        _backShieldsEnergyPart *= 0.9f;
        toAdd += _leftShieldsEnergyPart * 0.1f;
        _leftShieldsEnergyPart *= 0.9f;
        toAdd += _rightShieldsEnergyPart * 0.1f;
        _rightShieldsEnergyPart *= 0.9f;
        toAdd += _topShieldsEnergyPart * 0.1f;
        _topShieldsEnergyPart *= 0.9f;
        toAdd += _bottomShieldsEnergyPart * 0.1f;
        _backShieldsEnergyPart *= 0.9f;
        
        _weaponsEnergyPart += toAdd;
        SetEnergy();
    }
    
    public void EnergyToShields(GameUtils.ShieldType type)
    {
        float toAdd = 0;
        toAdd += _weaponsEnergyPart * 0.1f;
        _weaponsEnergyPart *= 0.9f;
        toAdd += _enginesEnergyPart * 0.1f;
        _enginesEnergyPart *= 0.9f;
        
        toAdd += _frontShieldsEnergyPart * 0.1f;
        _frontShieldsEnergyPart *= 0.9f;
        toAdd += _backShieldsEnergyPart * 0.1f;
        _backShieldsEnergyPart *= 0.9f;
        toAdd += _leftShieldsEnergyPart * 0.1f;
        _leftShieldsEnergyPart *= 0.9f;
        toAdd += _rightShieldsEnergyPart * 0.1f;
        _rightShieldsEnergyPart *= 0.9f;
        toAdd += _topShieldsEnergyPart * 0.1f;
        _topShieldsEnergyPart *= 0.9f;
        toAdd += _bottomShieldsEnergyPart * 0.1f;
        _bottomShieldsEnergyPart *= 0.9f;

        switch (type)
        {
            case GameUtils.ShieldType.Back:
                toAdd -= _backShieldsEnergyPart * 0.1f;
                _backShieldsEnergyPart /= 0.9f;
                _backShieldsEnergyPart += toAdd;
                break;
            case GameUtils.ShieldType.Front:
                toAdd -= _frontShieldsEnergyPart * 0.1f;
                _frontShieldsEnergyPart /= 0.9f;
                _frontShieldsEnergyPart += toAdd;
                break;
            case GameUtils.ShieldType.Top:
                toAdd -= _topShieldsEnergyPart * 0.1f;
                _topShieldsEnergyPart /= 0.9f;
                _topShieldsEnergyPart += toAdd;
                break;
            case GameUtils.ShieldType.Bottom:
                toAdd -= _bottomShieldsEnergyPart * 0.1f;
                _bottomShieldsEnergyPart /= 0.9f;
                _bottomShieldsEnergyPart += toAdd;
                break;
            case GameUtils.ShieldType.Left:
                toAdd -= _leftShieldsEnergyPart * 0.1f;
                _leftShieldsEnergyPart /= 0.9f;
                _leftShieldsEnergyPart += toAdd;
                break;
            case GameUtils.ShieldType.Right:
                toAdd -= _rightShieldsEnergyPart * 0.1f;
                _rightShieldsEnergyPart /= 0.9f;
                _rightShieldsEnergyPart += toAdd;
                break;
            default:
                Debug.Log("unknown shield type!");
                break;
        }
        
        SetEnergy();
    }
}
