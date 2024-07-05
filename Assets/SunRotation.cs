using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public GameObject target;

    void Update()
    {
        // Spin the object around the target at 20 degrees/second.
        transform.RotateAround(target.transform.position, Vector3.forward, 20 * Time.deltaTime);

    }
}
