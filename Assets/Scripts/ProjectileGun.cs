using System;
using UnityEngine;

public class ProjectileGun : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shootingInterval = 1.0f;
    [SerializeField] private float projectileSpeed = 25.0f;

    [SerializeField] private float shiftForward = 0;
    [SerializeField] private float shiftRight = 0;
    [SerializeField] private float shiftUp = 0;
    
    [SerializeField] private KeyCode shootingButton = KeyCode.Mouse0;

    private float _shootingDelay;
    // Start is called before the first frame update
    void Start()
    {
        _shootingDelay = shootingInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (_shootingDelay > 0)
        {
            _shootingDelay -= Time.deltaTime;
            return;
        }

        if (Input.GetKey(shootingButton))
        {
            var projectile = Instantiate(
                projectilePrefab, 
                transform.position + shiftForward * transform.forward + shiftRight * transform.right + shiftUp * transform.up, 
                transform.rotation
                );
            projectile.GetComponent<ProjectileController>().SetSpeed(projectileSpeed);
            _shootingDelay = shootingInterval;            
        }
    }
}
