using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 spawnPos;
    public GameObject Cloud1;
    public GameObject Cloud2;
    private float timer;
    private float spawnInstantly;
    void Start()
    {
        for(int i = 0; i < 150; i++)
        {
            spawnPos = new Vector3(Random.Range(-500, 1500f), 750f, Random.Range(-200, 1700f));
            if (Random.Range(0, 2) == 0)
            {
                Instantiate(Cloud1, spawnPos, Quaternion.identity);
            }
            else
            {
                Instantiate(Cloud2, spawnPos, Quaternion.identity);
            }
            timer = Random.Range(0.5f, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            spawnPos = new Vector3(-500f, 750f, Random.Range(-200, 1700f));
            if (Random.Range(0, 2) == 0)
            {
                Instantiate(Cloud1, spawnPos, Quaternion.identity);
            }
            else
            {
                Instantiate(Cloud2, spawnPos, Quaternion.identity);
            }
            timer = Random.Range(0.5f, 1);
        }
        if (spawnInstantly > 0)
        {
            spawnInstantly -= Time.deltaTime;
            spawnPos = new Vector3(Random.Range(-500f, 1000f), 750f, Random.Range(0f, 1500f));
            if (Random.Range(0, 2) == 0)
            {
                Instantiate(Cloud1, spawnPos, Quaternion.identity);
            }
            else
            {
                Instantiate(Cloud2, spawnPos, Quaternion.identity);
            }
        }
    }


    public void setSpawnInstantly()
    {
        spawnInstantly = 1f;
    }
}
