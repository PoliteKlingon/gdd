using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{

    [SerializeField] public Canvas canvas;

    [SerializeField] private KeyCode activateKey = KeyCode.Tab;
    [SerializeField] private bool unlockCursorOnActivation = true;
    
    [SerializeField] private bool controlledByGamepad = false;
    private GamepadControls _controls;

    private void Awake()
    {
        if (controlledByGamepad)
            _controls = new GamepadControls();
    }

    private void OnEnable()
    {
        _controls.Gameplay.Enable();
    }
    
    private void OnDisable()
    {
        _controls.Gameplay.Disable();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        canvas = gameObject.GetComponent<Canvas>();
        canvas.enabled = false; 
    }

    // Update is called once per frame
    void Update()
    {
        if ((!controlledByGamepad && Input.GetKey(activateKey)) 
            || (controlledByGamepad && _controls.Gameplay.ShowEnergyMenu.IsPressed()))
        {
            canvas.enabled = true;
            if (unlockCursorOnActivation) 
                GameUtils.Instance.UnlockCursor();
        }
        if ((!controlledByGamepad && Input.GetKeyUp(activateKey)) 
            || (controlledByGamepad && _controls.Gameplay.ShowEnergyMenu.WasReleasedThisFrame()))
        {
            canvas.enabled = false;
            if (unlockCursorOnActivation) 
                GameUtils.Instance.LockCursor();
        }
    }
}
