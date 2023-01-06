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

    [SerializeField] public GameObject explosion;

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip explosionClip;

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
                if (explosion != null)
                {
                    var exp = Instantiate(explosion, transform.position, transform.rotation);
                    Destroy(exp, 3.0f);
                }
                Debug.Log(gameObject + "died");
                if (gameObject.layer == LayerMask.NameToLayer("Meteor"))
                {
                    PowerUpsManager.Instance.spawnPowerUpAt(this.gameObject.transform.position);
                    Destroy(gameObject);
                }

                if (gameObject.CompareTag("Player1"))
                {
                    FindObjectOfType<DeadScreenCanvas>().ShowWinner("Player 2");
                } 
                else if (gameObject.CompareTag("Player2"))
                {
                    FindObjectOfType<DeadScreenCanvas>().ShowWinner("Player 1");
                }

                if (explosionClip != null)
                {
                    AudioSource expSource = GameObject.Find("ExpAudioSource").GetComponent<AudioSource>();
                    expSource.pitch = Random.Range(0.65f, 1.25f);
                    expSource.volume = Random.Range(0.70f, 0.90f);
                    expSource.PlayOneShot(explosionClip);
                }
            }
            else
            {
                if (source != null && hitClip != null)
                {
                    source.pitch = Random.Range(0.65f, 1.25f);
                    source.volume = Random.Range(0.70f, 0.90f);
                    source.PlayOneShot(hitClip);
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
    public void Heal(float healthPoints)
    {
        DealDamage(-healthPoints);
    }
}
