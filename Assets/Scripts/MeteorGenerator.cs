using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorGenerator : MonoBehaviour
{
    [SerializeField] private GameObject meteorPrefab;

    [SerializeField] private int meteorNum = 0;

    [SerializeField] private float minScale;
    [SerializeField] private float maxScale;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < meteorNum; i++)
        {
            var meteor = Instantiate(
                meteorPrefab,
                new Vector3(
                    Random.Range(-EnvironmentProps.Instance.GetX(), EnvironmentProps.Instance.GetX()),
                    Random.Range(-EnvironmentProps.Instance.GetX(), EnvironmentProps.Instance.GetX()),
                    Random.Range(-EnvironmentProps.Instance.GetX(), EnvironmentProps.Instance.GetX())
                ),
                new Quaternion(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360))
            );
            var scale = Random.Range(minScale, maxScale);
            meteor.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
