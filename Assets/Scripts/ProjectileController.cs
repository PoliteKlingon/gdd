using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float _speed = 25.0f;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    void Update()
    {
        if (_rigidbody.velocity.magnitude < 0.1)
            _rigidbody.AddForce(transform.forward * _speed, ForceMode.VelocityChange);
    }
}
