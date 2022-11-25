using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{

    [SerializeField] public Canvas canvas;

    [SerializeField] private KeyCode activateKey = KeyCode.Tab;
    [SerializeField] private bool unlockCursorOnActivation = true;

    [SerializeField] private Button enginesButton;
    [SerializeField] private Button weaponsButton;
    [SerializeField] private Button frontShieldsButton;
    [SerializeField] private Button backShieldsButton;
    [SerializeField] private Button topShieldsButton;
    [SerializeField] private Button bottomShieldsButton;
    [SerializeField] private Button leftShieldsButton;
    [SerializeField] private Button rightShieldsButton;

    [SerializeField] private EnergyManagement player;
    
    [SerializeField] private bool controlledByGamepad = false;
    private GamepadControls _controls;

    private void Awake()
    {
        if (controlledByGamepad)
            _controls = new GamepadControls();
        if (player == null)
            Debug.Log("connected player is missing");
        else
        {
            enginesButton.onClick.AddListener(() => player.EnergyToEngines());
            weaponsButton.onClick.AddListener(() => player.EnergyToWeapons());
            frontShieldsButton.onClick.AddListener(() => player.EnergyToShields(GameUtils.ShieldType.Front));
            backShieldsButton.onClick.AddListener(() => player.EnergyToShields(GameUtils.ShieldType.Back));
            topShieldsButton.onClick.AddListener(() => player.EnergyToShields(GameUtils.ShieldType.Top));
            bottomShieldsButton.onClick.AddListener(() => player.EnergyToShields(GameUtils.ShieldType.Bottom));
            leftShieldsButton.onClick.AddListener(() => player.EnergyToShields(GameUtils.ShieldType.Left));
            rightShieldsButton.onClick.AddListener(() => player.EnergyToShields(GameUtils.ShieldType.Right));
        }
    }

    private void OnEnable()
    {
        if (controlledByGamepad)
            _controls.Gameplay.Enable();
    }
    
    private void OnDisable()
    {
        if (controlledByGamepad)
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
