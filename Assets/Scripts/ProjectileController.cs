using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float _damage = 50.0f;
    
    public void SetDamage(float damage)
    {
        _damage = damage;
    }
}
