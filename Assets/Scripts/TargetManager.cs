using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        foreach (Transform target in transform)
        {
            target.GetComponent<TargetMove>().targetID = i;
            i ++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
