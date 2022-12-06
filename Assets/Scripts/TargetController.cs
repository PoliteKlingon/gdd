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
        int mask = (1 << 14) | (1 << 15) | (1 << 13); //mask layer that should not be hit by raycast
        if (Physics.Raycast(ship.transform.position + ship.transform.forward * defaultCastShift, ship.transform.forward, out hit, Mathf.Infinity, ~mask))
            transform.position = hit.point;
        else
            transform.position = ship.transform.position + ship.transform.forward * defaultShift;
    }
}
