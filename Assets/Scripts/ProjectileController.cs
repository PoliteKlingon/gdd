using UnityEngine;

public class ProjectileController : MonoBehaviour
{
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
            Debug.Log("Projectile Tag: " + _playerTag);
            Debug.Log("Hit tag: " + collision.gameObject.tag);
            if (collision.gameObject.tag == _playerTag)
                return;
            if (collision.gameObject.layer == LayerMask.NameToLayer("Shields"))
            {
                collision.gameObject.GetComponent<Shield>().DealDamage(_damage);
                Debug.Log("Dealt dmg");
                Destroy(this.gameObject);
            }
    }
}
