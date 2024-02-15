using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineGeneration : MonoBehaviour
{
    [SerializeField] private GameObject minePrefab;
    [SerializeField] private float gridSize;
    [SerializeField] private float activationRatio;

    // Start is called before the first frame update
    void Start()
    {
        float cellSize = 40f/gridSize;
        float activationRadius = activationRatio * cellSize / 2;

        for (int x=0; x<gridSize; x++)
        {
            for (int z=0; z<gridSize; z++)
            {
                GameObject mine = Instantiate(minePrefab, gameObject.transform) as GameObject;
                mine.GetComponent<MineController>().SetActivationRadius(activationRadius);
                float newX = (x+0.5f) * cellSize + Random.Range(-1f, 1f) * (1-activationRatio) * (cellSize/2);
                float newZ = 2.5f - (z+0.5f) * cellSize + Random.Range(-1f, 1f) * (1-activationRatio) * (cellSize/2);
                mine.transform.localPosition = new Vector3(newX, 0f, newZ);
            }
        }    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
