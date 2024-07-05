using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createTerrain : MonoBehaviour
{
    //Trees
    public GameObject tree1;
    public GameObject tree2;
    public GameObject tree3;
    public GameObject tree4;
    public GameObject tree5;

    //Grass
    public GameObject grass1;
    public GameObject grass2;

    //Details
    public GameObject detail1;
    public GameObject detail2;
    public GameObject detail3;
    public GameObject detail4;
    public GameObject detail5;

    // Start is called before the first frame update
    void Start()
    {
        generateTrees();
        generateGrass();
        generateDetails();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void generateTrees()
    {
        GameObject selectedTree = null;
        for (int i = 0; i < 10000; i++)
        {
            int random = Random.Range(0, 5);
            switch (random)
            {
                case 0: selectedTree = tree1; break;
                case 1: selectedTree = tree2; break;
                case 2: selectedTree = tree3; break;
                case 3: selectedTree = tree4; break;
                case 4: selectedTree = tree5; break;
            }
            Instantiate(selectedTree, new Vector3(Random.Range(0, 1000f), 0, Random.Range(0, 1000f)), Quaternion.identity);
        }
    }

    public void generateGrass()
    {
        GameObject selectedGrass = null;
        for (int i = 0; i < 5000; i++)
        {
            int random = Random.Range(0, 2);
            switch (random)
            {
                case 0: selectedGrass = grass1; break;
                case 1: selectedGrass = grass2; break;
            }
            Instantiate(selectedGrass, new Vector3(Random.Range(0, 1000f), 0, Random.Range(0, 1000f)), Quaternion.identity);
        }
    }

    public void generateDetails()
    {
        GameObject selectedDetail = null;
        for (int i = 0; i < 1000; i++)
        {
            int random = Random.Range(0, 5);
            switch (random)
            {
                case 0: selectedDetail = detail1; break;
                case 1: selectedDetail = detail2; break;
                case 2: selectedDetail = detail3; break;
                case 3: selectedDetail = detail4; break;
                case 4: selectedDetail = detail5; break;
            }
            Instantiate(selectedDetail, new Vector3(Random.Range(0, 1000f), 0, Random.Range(0, 1000f)), Quaternion.identity);
        }
    }

}
