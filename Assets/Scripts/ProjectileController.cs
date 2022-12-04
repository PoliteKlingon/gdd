using System.Security.Cryptography;
using UnityEngine;
using Input = UnityEngine.Windows.Input;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitMeteor;
    [SerializeField] private ParticleSystem hitShields;
    [SerializeField] private ParticleSystem hitShip;
    
    private float _damage = 50.0f;
    private string _playerTag;
    
    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public void SetTag(string tag) 
    {
        _playerTag = tag;
    }

    private void OnCollisionEnter(Collision collision)
    {
            //Debug.Log("Projectile Tag: " + _playerTag);
            //Debug.Log("Hit tag: " + collision.gameObject.tag);
            if (collision.gameObject.tag == _playerTag)
                return;
            if (collision.gameObject.layer == LayerMask.NameToLayer("Shields"))
            {
                if (hitShields != null)
                {
                    var hitEffect = Instantiate(hitShields, collision.GetContact(0).point, transform.rotation);
                    hitEffect.Play();
                    Destroy(hitEffect, 1.0f);
                }
                collision.gameObject.GetComponent<Shield>().DealDamage(_damage);
                //Debug.Log("Dealt dmg");
                Destroy(this.gameObject);
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Meteor"))
            {
                if (hitMeteor != null)
                {
                    var hitEffect = Instantiate(hitMeteor, transform.position, transform.rotation);
                    hitEffect.Play();
                    Destroy(hitEffect, 1.0f);
                }
                collision.gameObject.GetComponent<Health>().DealDamage(_damage);
                //Debug.Log("Dealt dmg to meteor");
                Destroy(this.gameObject);
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Ship"))
            {
                if (hitShip != null)
                {
                    var hitEffect = Instantiate(hitShip, transform.position, transform.rotation);
                    hitEffect.Play();
                    Destroy(hitEffect, 1.0f);
                }
            }

            var playerCamera = collision.gameObject.GetComponent<ShakeCamera>();
            if (playerCamera != null)
                playerCamera.Shake();
    }
}
