using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Obstacle hit");
        if (col.gameObject.tag == "Player")
        {
            DataManager.AddObstacleHit();
            Debug.Log("Player hit obstacle");
        }
    }
}
