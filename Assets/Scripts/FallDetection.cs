using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDetection : MonoBehaviour
{
    [SerializeField] private Transform resetPos;
    [SerializeField] private Transform skipPos;
    [SerializeField] private int maxFallCount;

    private int fallCount;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            fallCount ++;
            if (fallCount >= maxFallCount)
            {
                transform.root.GetComponent<RoomController>().SetComplete();
                col.transform.root.GetComponent<CharacterController>().Move(skipPos.position - col.transform.root.position);
            }
            else
            {
                col.transform.root.GetComponent<CharacterController>().Move(resetPos.position - col.transform.root.position);
            }
            DataManager.AddObstacleHit();
        }
    }
}
