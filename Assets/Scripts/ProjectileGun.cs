using System;
using UnityEngine;

public class ProjectileGun : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shootingInterval = 1.0f;
    [SerializeField] private float projectileSpeed = 25.0f;
    [SerializeField] private float projectileDamage = 50.0f;

    [SerializeField] private float shiftForward = 0;
    [SerializeField] private float shiftRight = 0;
    [SerializeField] private float shiftUp = 0;
    
    [SerializeField] private KeyCode shootingButton = KeyCode.Mouse0;
    [SerializeField] private KeyCode showUIButton = KeyCode.Tab;

    private float _shootingDelay;
    
    [SerializeField] private bool controlledByGamepad = false;
    private GamepadControls _controls;

    private void Awake()
    {
        if (controlledByGamepad)
            _controls = new GamepadControls();
    }

    private void OnEnable()
    {
        if (controlledByGamepad)
            _controls.Gameplay.Enable();
    }
    
    private void OnDisable()
    {
        if (controlledByGamepad)
            _controls.Gameplay.Disable();
    }

    private float _energyPortion = 1.0f;

    public void SetEnergy(float portion)
    {
        _energyPortion = portion;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _shootingDelay = shootingInterval * (1 / _energyPortion);
    }

    private void Shoot()
    {
        if (_energyPortion < 0.001)
            return;
        var projectile = Instantiate(
        projectilePrefab, 
        transform.position 
        + shiftForward * transform.forward 
        + shiftRight * transform.right 
        + shiftUp * transform.up, 
        transform.rotation
        );
        var projContr = GetComponent<ProjectileController>();
        if (projContr == null)
            Debug.Log("missong projectile controller component");
        else
        {
            projContr.SetSpeed(projectileSpeed * _energyPortion);
            projContr.SetDamage(projectileDamage * _energyPortion);
        }

        _shootingDelay = shootingInterval * (1 / _energyPortion);
    }

    // Update is called once per frame
    void Update()
    {
        if (_shootingDelay > 0)
        {
            _shootingDelay -= Time.deltaTime;
            return;
        }

        if ((!controlledByGamepad && Input.GetKey(shootingButton) && !Input.GetKey(showUIButton)) 
            || (controlledByGamepad && _controls.Gameplay.Shoot.IsPressed() && !_controls.Gameplay.ShowEnergyMenu.IsPressed()))
        {
            Shoot();
        }
    }
}
