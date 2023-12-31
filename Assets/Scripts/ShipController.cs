using TMPro;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private KeyCode forwardKey = KeyCode.W;
    [SerializeField] private KeyCode backwardKey = KeyCode.S;
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode showUIButton = KeyCode.Tab;
    [SerializeField] private KeyCode handbrakeKey = KeyCode.Space;

    [SerializeField] private ParticleSystem[] leftThrusters;
    [SerializeField] private ParticleSystem[] rightThrusters;
    [SerializeField] private ParticleSystem[] frontThrusters;
    [SerializeField] private ParticleSystem[] backThrusters;

    [SerializeField] private GameObject visual;
    [SerializeField] private GameObject visualEmission;
    
    [SerializeField] private GameObject visualInvisible;
    [SerializeField] private GameObject visualEmissionInvisible;

    [SerializeField] private float acceleration = 10;
    [SerializeField] private float maxSpeed = 10;
    [SerializeField] private float braking = 2.0f;
    [SerializeField] private float handbrake = 0.75f;

    private Rigidbody _rigidbody;

    [SerializeField] private string rotationAxisX = "Mouse X";
    [SerializeField] private string rotationAxisY = "Mouse Y";
    [SerializeField] private float mouseSensitivity = 1.0f;

    private float _rotationX;
    private float _rotationY;

    [SerializeField] private bool controlledByGamepad = false;
    private GamepadControls _controls;
    [SerializeField] private float gamepadSensitivity = 1.0f;

    [SerializeField] private Canvas warningCanvas;
    [SerializeField] private TMP_Text timeLeft;
    [SerializeField] private float deathTimeout = 10.0f;
    private float _deathTimeout;
    
    private bool _invisible = false;
    
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

    private void SetThrusters(ParticleSystem[] thrusters, bool value)
    {
        foreach (ParticleSystem thruster in thrusters)
        {
            //thruster.SetActive(value);
            if (value)
            {
                thruster.Play();
            }
            else
            {
                thruster.Stop();
            }
            if (_invisible)
                //thruster.SetActive(false);
                thruster.Stop();
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
    
        setVisible();
        
        _deathTimeout = deathTimeout;
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

    private void HandBrake()
    {
        _rigidbody.velocity *= handbrake;
    }
    
    public float getAcceleration()
    {
        return this.acceleration;
    }

    public float getMaxSpeed()
    {
        return this.maxSpeed;
    }

    public void setAcceleration(float acceleration)
    {
        this.acceleration += acceleration;
    }

    public void setMaxSpeed(float maxSpeed)
    {
        this.maxSpeed += maxSpeed;
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
            
            if (_controls.Gameplay.Handbrake.IsPressed())
                HandBrake();

            //rotace
            if (!_controls.Gameplay.ShowEnergyMenu.IsPressed())
            {
                Vector2 rotate = _controls.Gameplay.Rotate.ReadValue<Vector2>();
                //transform.Rotate(new Vector3(gamepadSensitivity * -rotate.y, gamepadSensitivity * rotate.x, 0));
                //_rigidbody.AddRelativeTorque(25 * new Vector3(gamepadSensitivity * -rotate.y, gamepadSensitivity * rotate.x, 0));
                _rigidbody.angularVelocity = gamepadSensitivity * -rotate.y * transform.right 
                                             + gamepadSensitivity * rotate.x * transform.up;
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
            
            if (Input.GetKey(handbrakeKey))
                HandBrake();

            //rotace mysi
            if (!Input.GetKey(showUIButton))
            {
                _rotationX = Input.GetAxis(rotationAxisX);
                _rotationY = Input.GetAxis(rotationAxisY);
                //_rigidbody.AddRelativeTorque(25 * new Vector3(mouseSensitivity * -_rotationY, mouseSensitivity * _rotationX, 0));
                _rigidbody.angularVelocity = mouseSensitivity * -_rotationY * transform.right + mouseSensitivity * _rotationX * transform.up;
                //transform.Rotate(new Vector3(mouseSensitivity * -_rotationY, mouseSensitivity * _rotationX, 0));
            }
        }

        //brzdeni
        _rigidbody.AddForce(-_rigidbody.velocity * braking * Time.deltaTime, ForceMode.Impulse);
        /*if (_rigidbody.angularVelocity.magnitude > 0.05f)
            _rigidbody.AddTorque(-_rigidbody.angularVelocity.normalized * 500 * Time.deltaTime);
        else
            _rigidbody.angularVelocity = Vector3.zero;*/

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

        if (_deathTimeout < 0)
        {
            if (gameObject.CompareTag("Player1"))
            {
                FindObjectOfType<DeadScreenCanvas>().ShowWinner("Player 2");
            } 
            else if (gameObject.CompareTag("Player2"))
            {
                FindObjectOfType<DeadScreenCanvas>().ShowWinner("Player 1");
            }

            var eff = Instantiate(GetComponent<Health>().explosion, transform.position, transform.rotation);
            Destroy(eff, 3.0f);
            Destroy(gameObject);
        }

        if (Mathf.Abs(transform.position.x) > EnvironmentProps.Instance.GetX()
            || Mathf.Abs(transform.position.y) > EnvironmentProps.Instance.GetY()
            || Mathf.Abs(transform.position.z) > EnvironmentProps.Instance.GetZ())
        {
            warningCanvas.enabled = true;
            timeLeft.text = ((int)_deathTimeout).ToString();
            _deathTimeout -= Time.deltaTime;
        }
        else
        {
            warningCanvas.enabled = false;
            _deathTimeout = deathTimeout;
        }
    }

    public void setInvisible()
    {
        visual.GetComponent<MeshRenderer>().enabled = false;
        visualInvisible.GetComponent<MeshRenderer>().enabled = true;
        visualEmission.GetComponent<MeshRenderer>().enabled = false;
        visualEmissionInvisible.GetComponent<MeshRenderer>().enabled = true;
        _invisible = true;
    }

    public void setVisible()
    {
        visual.GetComponent<MeshRenderer>().enabled = true;
        visualInvisible.GetComponent<MeshRenderer>().enabled = false;
        visualEmission.GetComponent<MeshRenderer>().enabled = true;
        visualEmissionInvisible.GetComponent<MeshRenderer>().enabled = false;
        _invisible = false;
    }
}
