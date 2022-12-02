using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 200.0f;

    [SerializeField]
    private float maxBasicHealth = 100.0f;
    [SerializeField]
    private Bar healthBar;

    private float _currentHealth;
    private float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = value;
            if (healthBar != null)
                healthBar.SetFillLevel(_currentHealth / maxHealth);
            if (_currentHealth <= 0)
            {
                Debug.Log(gameObject + "died");
                if (gameObject.layer == LayerMask.NameToLayer("Meteor"))
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = maxBasicHealth;
    }

    public void DealDamage(float damage)
    {
        CurrentHealth -= damage;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
