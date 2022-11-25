using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldsController : MonoBehaviour
{
    [SerializeField]
    private Shield _frontShields;
    [SerializeField]
    private Shield _backShields;
    [SerializeField]
    private Shield _leftShields;
    [SerializeField]
    private Shield _rightShields;
    [SerializeField]
    private Shield _topShields;
    [SerializeField]
    private Shield _bottomShields;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEnergy(float amount, GameUtils.ShieldType shieldType)
    {
        switch (shieldType)
        {
            case GameUtils.ShieldType.Front:
                _frontShields.SetEnergy(amount);
                break;
            case GameUtils.ShieldType.Back:
                _backShields.SetEnergy(amount);
                break;
            case GameUtils.ShieldType.Top:
                _topShields.SetEnergy(amount);
                break;
            case GameUtils.ShieldType.Bottom:
                _bottomShields.SetEnergy(amount);
                break;
            case GameUtils.ShieldType.Left:
                _leftShields.SetEnergy(amount);
                break;
            case GameUtils.ShieldType.Right:
                _rightShields.SetEnergy(amount);
                break;
            default:
                Debug.Log("Nonexisting shield part!");
                break;
        }
    }
}
