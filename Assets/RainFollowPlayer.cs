using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainFollowPlayer : MonoBehaviour
{
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Player.transform.position.x;
        float z = Player.transform.position.z;
        float y = Player.transform.position.y+50f;
        Vector3 newPos = new Vector3(x, y, z);
        this.transform.position = newPos;
    }
}
