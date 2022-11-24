using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{

    [SerializeField] public Canvas canvas;

    [SerializeField] private KeyCode activateKey = KeyCode.Tab;
    [SerializeField] private bool unlockCursorOnActivation = true;
    
    // Start is called before the first frame update
    void Start()
    {
        canvas = gameObject.GetComponent<Canvas>();
        canvas.enabled = false; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(activateKey))
        {
            canvas.enabled = true;
            if (unlockCursorOnActivation) 
                GameUtils.Instance.UnlockCursor();
        }
        if (Input.GetKeyUp(activateKey))
        {
            canvas.enabled = false;
            if (unlockCursorOnActivation) 
                GameUtils.Instance.LockCursor();
        }
    }
}
