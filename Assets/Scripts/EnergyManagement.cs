using UnityEngine;
using UnityEngine.UI;

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


    private float _enginesInitEnergy = 2.0f / 10;
    private float _weaponsInitEnergy = 2.0f / 10;
    private float _frontShieldsInitEnergy = 1.0f / 10;
    private float _backShieldsInitEnergy = 1.0f / 10;
    private float _leftShieldsInitEnergy = 1.0f / 10;
    private float _rightShieldsInitEnergy = 1.0f / 10;
    private float _topShieldsInitEnergy = 1.0f / 10;
    private float _bottomShieldsInitEnergy = 1.0f / 10;

    private float _enginesEnergyPart;
    private float _weaponsEnergyPart;
    private float _frontShieldsEnergyPart;
    private float _backShieldsEnergyPart;
    private float _leftShieldsEnergyPart;
    private float _rightShieldsEnergyPart;
    private float _topShieldsEnergyPart;
    private float _bottomShieldsEnergyPart;

    private int _maxHeight = 45;
    private int _width = 1;

    [SerializeField] public Image enginesBar;
    [SerializeField] public Image weaponsBar;
    [SerializeField] public Image topBar;
    [SerializeField] public Image bottomBar;
    [SerializeField] public Image leftBar;
    [SerializeField] public Image rightBar;
    [SerializeField] public Image frontBar;
    [SerializeField] public Image backBar;

    private void Start()
    {

        _enginesEnergyPart = _enginesInitEnergy;
        _weaponsEnergyPart = _weaponsInitEnergy;
        _frontShieldsEnergyPart = _frontShieldsInitEnergy;
        _backShieldsEnergyPart = _backShieldsInitEnergy;
        _leftShieldsEnergyPart = _leftShieldsInitEnergy;
        _rightShieldsEnergyPart = _rightShieldsInitEnergy;
        _topShieldsEnergyPart = _topShieldsInitEnergy;
        _bottomShieldsEnergyPart = _bottomShieldsInitEnergy;


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

        enginesBar.rectTransform.localScale = new Vector3(_width, _enginesEnergyPart / (_enginesInitEnergy * 2), 1);
        weaponsBar.rectTransform.localScale = new Vector3(_width, _weaponsEnergyPart / (_weaponsInitEnergy * 2), 1);
        frontBar.rectTransform.localScale = new Vector3(_width, _frontShieldsEnergyPart / (_frontShieldsInitEnergy * 2), 1);
        backBar.rectTransform.localScale = new Vector3(_width, _backShieldsEnergyPart / (_backShieldsInitEnergy * 2), 1);
        leftBar.rectTransform.localScale = new Vector3(_width, _leftShieldsEnergyPart / (_leftShieldsInitEnergy * 2), 1);
        rightBar.rectTransform.localScale = new Vector3(_width,_rightShieldsEnergyPart / (_rightShieldsInitEnergy * 2), 1);
        topBar.rectTransform.localScale = new Vector3(_width, _topShieldsEnergyPart / (_topShieldsInitEnergy * 2), 1);
        bottomBar.rectTransform.localScale = new Vector3(_width,_bottomShieldsEnergyPart / (_bottomShieldsInitEnergy * 2), 1);
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
