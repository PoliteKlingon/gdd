using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PowerUps
{
    SPEED,
    HEALTH,
    DAMAGE,
    INVISIBILITY,
    ENERGY
}
public class PowerUpsManager : MonoBehaviour
{
   
    public static PowerUpsManager Instance { get; private set; }

    [SerializeField] public float accShift = 200.0f;
    [SerializeField] public float speedShift = 200.0f;
    [SerializeField] public float shootIntervalShift = -0.3f;
    [SerializeField] public float shootSpeedShift = 200.0f;
    [SerializeField] public float damageShift = 200.0f;
    [SerializeField] List<GameObject> powerUps;
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;

    [SerializeField] float PowerUpDelay = 20;
    [SerializeField] int PowerUpCount = 3; // x instances of each powerup (number of powerups * x = total powerups in game)
    [SerializeField] public List<PowerUpDuration> activePowers;
    // Start is called before the first frame update

    public void Awake()
    {
        activePowers = new List<PowerUpDuration>();
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");
        if (Instance == null)
        {
            // 'this' is the first instance created => save it.
            Instance = this;

            // We want to keep the UI always present
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            // Destroy 'this' object as there exist another instance
            Destroy(this.gameObject);
        }


    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<int> indexes = new List<int>();
        for(int i = 0; i < activePowers.Count; i++)
        {
            if (activePowers[i].recalculateDelay())
            {
                activePowers[i].endPowerUp();
                indexes.Add(i);
            }
        }
        indexes.Reverse();
        foreach (int powerUp in indexes)
        {
            activePowers.RemoveAt(powerUp);
        }
    }

    public void assignPower(GameObject player, PowerUps powerUp)
    {
        activePowers.Add(new PowerUpDuration(this.PowerUpDelay, powerUp, player));
    }

}
