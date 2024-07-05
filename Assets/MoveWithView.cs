using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithView : MonoBehaviour
{
    public Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = mainCamera.transform.forward * 5f;
        transform.LookAt(mainCamera.transform.position);
    }
}
