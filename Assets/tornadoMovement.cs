using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tornadoMovement : MonoBehaviour
{
    public GameObject tinycloud;
    private int numberOfObjects = 12; // Anzahl der Objekte
    private float radius = 20f; // Radius des Kreises
    private float size = 0.1f;
    private float y = 0;
    private float timeToChangeDirection;
    private float speed;
    public Vector3 movingVector;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        spawnTornado();
        timeToChangeDirection = 1f;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Random.Range(25f,50f) * Time.deltaTime);
        if (timeToChangeDirection < 0f)
        {
            movingVector.x += Random.Range(-0.1f, 0.1f);
            movingVector.z += Random.Range(-0.1f, 0.1f);
            speed = Random.Range(20f, 30f);
            timeToChangeDirection = 10f;
        }
        this.transform.position = this.transform.position + movingVector * Time.deltaTime * speed;
        timeToChangeDirection -= Time.deltaTime;
        checkIfPlayerNearby();
    }

    public void spawnTornado()
    {
        for (int j = 0; j < 40; j++)
        {
            for (int i = 0; i < numberOfObjects; i++)
            {
                // Berechnung des Winkels für das aktuelle Objekt
                float angle = i * Mathf.PI * 2 / numberOfObjects;
                // Berechnung der Position des aktuellen Objekts
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                Vector3 position = new Vector3(x, y, z) + this.transform.position;

                // Erzeuge das Objekt an der berechneten Position
                GameObject cloud = Instantiate(tinycloud, position, Quaternion.identity,transform);
                cloud.transform.localScale = new Vector3(size, size, size);
            }
            radius += 0.5f;
            size += 0.025f;
            y += size*20;
        }
    }

    public void checkIfPlayerNearby()
    {
        Vector3 playerPos = player.transform.position;
        playerPos.y = 0;
        Vector3 tornadoPos = this.transform.position;
        tornadoPos.y = 0;
        if (Vector3.Distance(playerPos, tornadoPos) < 250f)
        {
            Vector3 dir = (tornadoPos - playerPos).normalized;
            player.GetComponent<Player>().movePlayer(dir,15f);
            player.GetComponent<Player>().rotatePlayer(this.transform.position,250f);
        }
    }
}
