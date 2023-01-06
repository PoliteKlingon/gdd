using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{


        // Check, if we do not have any instance yet.    
        
    [SerializeField] private PowerUps powerUp;
    // Start is called before the first frame update


    void Start()
    {


    }
    private void OnCollisionEnter(Collision collision)
    {
        PowerUpsManager.Instance.assignPower(collision.gameObject, powerUp);

        Destroy(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
