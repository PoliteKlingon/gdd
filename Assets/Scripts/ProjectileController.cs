using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float _speed = 25.0f;

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    void Update()
    {
        transform.localPosition += transform.forward * _speed * Time.deltaTime;
    }
}
