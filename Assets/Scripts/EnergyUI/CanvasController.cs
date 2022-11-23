using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{

    [SerializeField] public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas = gameObject.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("tab"))
        {
            canvas.enabled = true;
        }
        if (Input.GetKeyUp("tab"))
        {
            canvas.enabled = false;
        }
    }
}
