using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    [SerializeField] private GameObject ship;
    
    //script for moving the target along the Z axis
    //cast a ray from ship forward
    //if it hits something, move the target to the point of hit
    //otherwise set it to default value
}
