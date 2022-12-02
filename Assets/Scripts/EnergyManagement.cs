using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField] public const float REMAINING_ENERGY = 0.98f;
    [SerializeField] public const float GIVEN_ENERGY = 0.02f;
    private const float BASIC_FILL_LEVEL = 0.5f;


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

    [SerializeField] public TextMeshProUGUI engineText;
    [SerializeField] public TextMeshProUGUI weaponText;
    [SerializeField] public TextMeshProUGUI topText;
    [SerializeField] public TextMeshProUGUI bottomText;
    [SerializeField] public TextMeshProUGUI leftText;
    [SerializeField] public TextMeshProUGUI rightText;
    [SerializeField] public TextMeshProUGUI frontText;
    [SerializeField] public TextMeshProUGUI backText;

    [SerializeField] public TextMeshProUGUI controlText;

    [SerializeField] public Bar energyBar;

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

    public float GetTotalEnergy()
    {
        return totalEnergy;
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
        energyBar.SetFillLevel(totalEnergy * BASIC_FILL_LEVEL);

        engineText.text = _enginesEnergyPart.ToString();
        weaponText.text = _weaponsEnergyPart.ToString();
        topText.text = _topShieldsEnergyPart.ToString();
        bottomText.text = _bottomShieldsEnergyPart.ToString();
        leftText.text = _leftShieldsEnergyPart.ToString();
        rightText.text = _rightShieldsEnergyPart.ToString();
        frontText.text = _frontShieldsEnergyPart.ToString();
        backText.text = _backShieldsEnergyPart.ToString();

        controlText.text = (_enginesEnergyPart + _weaponsEnergyPart + _topShieldsEnergyPart + _bottomShieldsEnergyPart + _leftShieldsEnergyPart + _rightShieldsEnergyPart + _frontShieldsEnergyPart + _backShieldsEnergyPart).ToString();
    }

    private void ClipToZero()
    {
        if (_frontShieldsEnergyPart < 0.0001)
            _frontShieldsEnergyPart = 0.0f;
        if (_backShieldsEnergyPart < 0.0001)
            _backShieldsEnergyPart = 0.0f;
        if (_topShieldsEnergyPart < 0.0001)
            _topShieldsEnergyPart = 0.0f;
        if (_bottomShieldsEnergyPart < 0.0001)
            _bottomShieldsEnergyPart = 0.0f;
        if (_leftShieldsEnergyPart < 0.0001)
            _leftShieldsEnergyPart = 0.0f;
        if (_rightShieldsEnergyPart < 0.0001)
            _rightShieldsEnergyPart = 0.0f;
        if (_weaponsEnergyPart < 0.0001)
            _weaponsEnergyPart = 0.0f;
        if (_enginesEnergyPart < 0.0001)
            _enginesEnergyPart = 0.0f;
    } 

    public void EnergyToEngines()
    {
        float toAdd = 0;
        toAdd += _weaponsEnergyPart * GIVEN_ENERGY;

        toAdd += _frontShieldsEnergyPart * GIVEN_ENERGY;

        toAdd += _backShieldsEnergyPart * GIVEN_ENERGY;

        toAdd += _leftShieldsEnergyPart * GIVEN_ENERGY;

        toAdd += _rightShieldsEnergyPart * GIVEN_ENERGY;

        toAdd += _topShieldsEnergyPart * GIVEN_ENERGY;

        toAdd += _bottomShieldsEnergyPart * GIVEN_ENERGY;
        
        if (toAdd + _enginesEnergyPart > _enginesInitEnergy * 2)
        {
            return;
        }

        _weaponsEnergyPart *= REMAINING_ENERGY;
        _frontShieldsEnergyPart *= REMAINING_ENERGY;
        _backShieldsEnergyPart *= REMAINING_ENERGY;
        _leftShieldsEnergyPart *= REMAINING_ENERGY;
        _rightShieldsEnergyPart *= REMAINING_ENERGY;
        _topShieldsEnergyPart *= REMAINING_ENERGY;
        _bottomShieldsEnergyPart *= REMAINING_ENERGY;
        _enginesEnergyPart += toAdd;
        
        ClipToZero();
        SetEnergy();
    }
    
    public void EnergyToWeapons()
    {
        float toAdd = 0;
        toAdd += _enginesEnergyPart * GIVEN_ENERGY;
        
        toAdd += _frontShieldsEnergyPart * GIVEN_ENERGY;
        
        toAdd += _backShieldsEnergyPart * GIVEN_ENERGY;
       
        toAdd += _leftShieldsEnergyPart * GIVEN_ENERGY;
        
        toAdd += _rightShieldsEnergyPart * GIVEN_ENERGY;
        
        toAdd += _topShieldsEnergyPart * GIVEN_ENERGY;
        
        toAdd += _bottomShieldsEnergyPart * GIVEN_ENERGY;

        if (toAdd + _weaponsEnergyPart > _weaponsInitEnergy * 2)
        {
            return;
        }



        _enginesEnergyPart *= REMAINING_ENERGY;
        _frontShieldsEnergyPart *= REMAINING_ENERGY;
        _backShieldsEnergyPart *= REMAINING_ENERGY;
        _leftShieldsEnergyPart *= REMAINING_ENERGY;
        _rightShieldsEnergyPart *= REMAINING_ENERGY;
        _topShieldsEnergyPart *= REMAINING_ENERGY;
        _bottomShieldsEnergyPart *= REMAINING_ENERGY;

        _weaponsEnergyPart += toAdd;
        
        ClipToZero();
        SetEnergy();
    }
    
    public void EnergyToShields(GameUtils.ShieldType type, float remainingEnergy = REMAINING_ENERGY)
    {
        float givenEnergy = 1 - remainingEnergy;
        float toAdd = 0;
        toAdd += _weaponsEnergyPart * givenEnergy;
        
        toAdd += _enginesEnergyPart * givenEnergy;

        
        toAdd += _frontShieldsEnergyPart * givenEnergy;

        toAdd += _backShieldsEnergyPart * givenEnergy;

        toAdd += _leftShieldsEnergyPart * givenEnergy;

        toAdd += _rightShieldsEnergyPart * givenEnergy;

        toAdd += _topShieldsEnergyPart * givenEnergy;

        toAdd += _bottomShieldsEnergyPart * givenEnergy;

        float energyPart = 0.0f;
        float initEnergy = 0.0f;
        switch (type)
        {
            case GameUtils.ShieldType.Front:
                energyPart = _frontShieldsEnergyPart;
                initEnergy = _frontShieldsInitEnergy;
                break;
            case GameUtils.ShieldType.Back:
                energyPart = _backShieldsEnergyPart;
                initEnergy = _backShieldsInitEnergy;
                break;
            case GameUtils.ShieldType.Top:
                energyPart = _topShieldsEnergyPart;
                initEnergy = _topShieldsInitEnergy;
                break;
            case GameUtils.ShieldType.Bottom:
                energyPart = _bottomShieldsEnergyPart;
                initEnergy = _bottomShieldsInitEnergy;
                break;
            case GameUtils.ShieldType.Left:
                energyPart = _leftShieldsEnergyPart;
                initEnergy = _leftShieldsInitEnergy;
                break;
            case GameUtils.ShieldType.Right:
                energyPart = _rightShieldsEnergyPart;
                initEnergy = _rightShieldsInitEnergy;
                break;
        }

        if (toAdd + energyPart > initEnergy * 2)
        {
            return;
        }


        _weaponsEnergyPart *= remainingEnergy;
        _enginesEnergyPart *= remainingEnergy;
        _frontShieldsEnergyPart *= remainingEnergy;
        _backShieldsEnergyPart *= remainingEnergy;
        _leftShieldsEnergyPart *= remainingEnergy;
        _rightShieldsEnergyPart *= remainingEnergy;
        _topShieldsEnergyPart *= remainingEnergy;
        _bottomShieldsEnergyPart *= remainingEnergy;

        switch (type)
        {
            case GameUtils.ShieldType.Back:
                _backShieldsEnergyPart /= remainingEnergy;
                toAdd -= _backShieldsEnergyPart * givenEnergy;
                _backShieldsEnergyPart += toAdd;
                break;
            case GameUtils.ShieldType.Front:
                _frontShieldsEnergyPart /= remainingEnergy;
                toAdd -= _frontShieldsEnergyPart * givenEnergy;
                _frontShieldsEnergyPart += toAdd;
                break;
            case GameUtils.ShieldType.Top:
                _topShieldsEnergyPart /= remainingEnergy;
                toAdd -= _topShieldsEnergyPart * givenEnergy;
                _topShieldsEnergyPart += toAdd;
                break;
            case GameUtils.ShieldType.Bottom:
                _bottomShieldsEnergyPart /= remainingEnergy;
                toAdd -= _bottomShieldsEnergyPart * givenEnergy;
                _bottomShieldsEnergyPart += toAdd;
                break;
            case GameUtils.ShieldType.Left:
                _leftShieldsEnergyPart /= remainingEnergy;
                toAdd -= _leftShieldsEnergyPart * givenEnergy;
                _leftShieldsEnergyPart += toAdd;
                break;
            case GameUtils.ShieldType.Right:
                _rightShieldsEnergyPart /= remainingEnergy;
                toAdd -= _rightShieldsEnergyPart * givenEnergy;
                _rightShieldsEnergyPart += toAdd;
                break;
            default:
                Debug.Log("unknown shield type!");
                break;
        }
        
        ClipToZero();
        SetEnergy();
    }

    public void DealShieldDamage(GameUtils.ShieldType type, float remainingEnergy)
    {
        totalEnergy /= remainingEnergy;
        EnergyToShields(type, remainingEnergy);
    }
}
