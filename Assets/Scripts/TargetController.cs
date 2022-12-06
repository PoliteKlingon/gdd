using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    [SerializeField] private GameObject ship;
    
    [SerializeField] private float defaultCastShift = 3;
    [SerializeField] private float defaultShift = 50;

    //script for moving the target along the Z axis
    //cast a ray from ship forward
    //if it hits something, move the target to the point of hit
    //otherwise set it to default value
    private void Update()
    {
        RaycastHit hit;
        //TODO: remove unwanted layers fom the raycast target
        if (Physics.Raycast(ship.transform.position + ship.transform.forward * defaultCastShift, ship.transform.forward, out hit, Mathf.Infinity))
        {
            transform.position = hit.point;
        }
        else
        {
            transform.position = ship.transform.position + ship.transform.forward * defaultShift;
        }
    }
}
