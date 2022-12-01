using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private KeyCode forwardKey = KeyCode.W;
    [SerializeField] private KeyCode backwardKey = KeyCode.S;
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode showUIButton = KeyCode.Tab;

    [SerializeField] private GameObject[] leftThrusters;
    [SerializeField] private GameObject[] rightThrusters;
    [SerializeField] private GameObject[] frontThrusters;
    [SerializeField] private GameObject[] backThrusters;

    [SerializeField] private float acceleration = 10;
    [SerializeField] private float maxSpeed = 10;

    private Rigidbody _rigidbody;

    [SerializeField] private string rotationAxisX = "Mouse X";
    [SerializeField] private string rotationAxisY = "Mouse Y";
    [SerializeField] private float mouseSensitivity = 1.0f;

    private float _rotationX;
    private float _rotationY;

    [SerializeField] private bool controlledByGamepad = false;
    private GamepadControls _controls;
    [SerializeField] private float gamepadSensitivity = 1.0f;

    [SerializeField]
    private Health health;
    [SerializeField]
    private float collisionDamage = 40.0f;


    private void Awake()
    {
        if (controlledByGamepad)
            _controls = new GamepadControls();
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

    private float _energyPortion = 1.0f;

    public void SetEnergy(float portion)
    {
        _energyPortion = portion;
    }

    private void SetThrusters(GameObject[] thrusters, bool value)
    {
        foreach (GameObject thruster in thrusters)
        {
            thruster.SetActive(value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null) Debug.Log("Missing rigidbody component!");

        SetThrusters(frontThrusters, false);
        SetThrusters(backThrusters, false);
        SetThrusters(leftThrusters, false);
        SetThrusters(rightThrusters, false);

        GameUtils.Instance.LockCursor();
    }

    private void GoForward(float accel)
    {
        _rigidbody.AddForce(transform.forward * _energyPortion * accel * Time.deltaTime, ForceMode.Impulse);
        SetThrusters(backThrusters, true);
    }

    private void GoBack(float accel)
    {
        _rigidbody.AddForce(transform.forward * _energyPortion * -accel * Time.deltaTime, ForceMode.Impulse);
        SetThrusters(frontThrusters, true);
    }

    private void GoLeft(float accel)
    {
        _rigidbody.AddForce(transform.right * _energyPortion * -accel * Time.deltaTime, ForceMode.Impulse);
        SetThrusters(rightThrusters, true);
    }

    private void GoRight(float accel)
    {
        _rigidbody.AddForce(transform.right * _energyPortion * accel * Time.deltaTime, ForceMode.Impulse);
        SetThrusters(leftThrusters, true);
    }

    // Update is called once per frame
    void Update()
    {
        SetThrusters(frontThrusters, false);
        SetThrusters(backThrusters, false);
        SetThrusters(leftThrusters, false);
        SetThrusters(rightThrusters, false);

        if (controlledByGamepad)
        {
            Vector2 move = _controls.Gameplay.Move.ReadValue<Vector2>();
            if (move.y > 0)
                GoForward(move.y * acceleration);

            else if (move.y < 0)
                GoBack(move.y * -acceleration);

            if (move.x > 0)
                GoRight(move.x * acceleration);

            else if (move.x < 0)
                GoLeft(move.x * -acceleration);

            //rotace
            if (!_controls.Gameplay.ShowEnergyMenu.IsPressed())
            {
                Vector2 rotate = _controls.Gameplay.Rotate.ReadValue<Vector2>();
                transform.Rotate(new Vector3(gamepadSensitivity * -rotate.y, gamepadSensitivity * rotate.x, 0));
            }
        }
        else
        {
            if (Input.GetKey(forwardKey))
                GoForward(acceleration);

            if (Input.GetKey(backwardKey))
                GoBack(acceleration);

            if (Input.GetKey(rightKey))
                GoRight(acceleration);

            if (Input.GetKey(leftKey))
                GoLeft(acceleration);

            //rotace mysi
            if (!Input.GetKey(showUIButton))
            {
                _rotationX = Input.GetAxis(rotationAxisX);
                _rotationY = Input.GetAxis(rotationAxisY);
                transform.Rotate(new Vector3(mouseSensitivity * -_rotationY, mouseSensitivity * _rotationX, 0));
            }
        }

        //brzdeni
        _rigidbody.AddForce(-_rigidbody.velocity * 2 * Time.deltaTime, ForceMode.Impulse);
        if (_rigidbody.angularVelocity.magnitude > 0.05f)
            _rigidbody.AddTorque(-_rigidbody.angularVelocity.normalized * 100 * Time.deltaTime);
        else
            _rigidbody.angularVelocity = Vector3.zero;

        // clamp speed to maxSpeed
        _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, maxSpeed * _energyPortion);

        //skryt kurzor mysi pomoci Esc.
        if (!controlledByGamepad && Input.GetKeyUp(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.None)
                GameUtils.Instance.LockCursor();
            else
                GameUtils.Instance.UnlockCursor();
        }
    }
}
