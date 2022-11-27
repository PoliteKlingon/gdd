using System;
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

    private Button _activeButton = null;

    private void Awake()
    {
        if (controlledByGamepad)
            _controls = new GamepadControls();
        if (player == null) 
            Debug.Log("connected player is missing");
        else
        {
            if (!controlledByGamepad)
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

        if (controlledByGamepad)
        {
            if (_activeButton != null && _controls.Gameplay.ConfirmEnergyTransfer.WasReleasedThisFrame())
            {
                if (_activeButton == enginesButton)
                    player.EnergyToEngines();
                else if (_activeButton == weaponsButton)
                    player.EnergyToWeapons();
                else if (_activeButton == frontShieldsButton)
                    player.EnergyToShields(GameUtils.ShieldType.Front);
                else if (_activeButton == backShieldsButton)
                    player.EnergyToShields(GameUtils.ShieldType.Back);
                else if (_activeButton == topShieldsButton)
                    player.EnergyToShields(GameUtils.ShieldType.Top);
                else if (_activeButton == bottomShieldsButton)
                    player.EnergyToShields(GameUtils.ShieldType.Bottom);
                else if (_activeButton == leftShieldsButton)
                    player.EnergyToShields(GameUtils.ShieldType.Left);
                else if (_activeButton == rightShieldsButton)
                    player.EnergyToShields(GameUtils.ShieldType.Right);
            }

            if (_controls.Gameplay.Rotate.IsInProgress())
            {
                Vector2 rotation = _controls.Gameplay.Rotate.ReadValue<Vector2>();
                if (rotation.magnitude > 0.1f)
                {
                    var angle = Mathf.Atan2(rotation.y, rotation.x);
                    angle += 22.5f;
                    if (angle < 0)
                        angle += 360;
                    angle /= 45;
                    switch (angle)
                    {
                        case 0:
                        case 8:
                            _activeButton = weaponsButton; //TODO: nharadit podle layoutu
                            break;
                        case 1:
                            _activeButton = frontShieldsButton;
                            break;
                        case 2:
                            _activeButton = rightShieldsButton;
                            break;
                        case 3:
                            _activeButton = bottomShieldsButton;
                            break;
                        case 4:
                            _activeButton = enginesButton;
                            break;
                        case 5:
                            _activeButton = backShieldsButton;
                            break;
                        case 6:
                            _activeButton = leftShieldsButton;
                            break;
                        case 7:
                            _activeButton = topShieldsButton;
                            break;
                        default:
                            Debug.Log("angle is wrong!");
                            break;
                    }
                }
                else _activeButton = null;
                //TODO: nejake zvyrazneni tlacitka, ktere je active?
            }
        }
    }
}
