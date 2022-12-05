using System;
using UnityEngine;
using Random = UnityEngine.Random;

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

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;

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

    public void setPower(float damage, float speed, float interval)
    {
        this.projectileDamage += damage;
        this.projectileSpeed += speed;
        this.shootingInterval += interval;

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
        var projContr = projectile.GetComponent<ProjectileController>();
        if (projContr == null)
            Debug.Log("missing projectile controller component");
        else
        {
            projContr.SetDamage(projectileDamage * _energyPortion);
            // Debug.Log("Gameobject tag: " + gameObject.tag);
            projContr.SetTag(gameObject.tag);
        }

        var projRigidbody = projectile.GetComponent<Rigidbody>();
        if (projRigidbody == null)
            Debug.Log("projectile rigidbody is missing");
        else
        {
            var myRigidbody = GetComponent<Rigidbody>();
            if (myRigidbody == null) 
                Debug.Log("missing rigidbody component!");
            else
                projRigidbody.AddForce(myRigidbody.velocity + transform.forward * projectileSpeed * _energyPortion, ForceMode.VelocityChange);
        }

        if (source != null && clip != null)
        {
            source.pitch = Random.Range(0.65f, 1.25f);
            source.volume = Random.Range(0.40f, 0.60f);
            source.PlayOneShot(clip);
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
