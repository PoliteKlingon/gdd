using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    [SerializeField]
    private RectTransform healthBarForeground;

    // Sets the new percentage of the health-bar fill
    public void SetFillLevel(float newFilLevel)
    {
        healthBarForeground.localScale = new Vector3(Mathf.Clamp(newFilLevel / 1.0f,
        0.0f, 1.0f), 1.0f, 1.0f);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
