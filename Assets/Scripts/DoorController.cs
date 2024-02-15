using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Transform doorPanel;
    [SerializeField] private float doorSpeed;

    private bool hasOpened = false;
    private float doorScale = 1f; 

    private int direction = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (doorPanel.position.y > 20f)
        {
            doorOpen = true;
            direction = 0;
        }
        else if (doorPanel.position.y < 6.5f)
        {
            doorOpen = false;
            direction = 0;
        }
        else
        {
            doorPanel.position += Vector3.up * direction * doorSpeed * Time.deltaTime;
        }
        */

        if ((doorScale < 1f && direction == 1) || (doorScale > 0.01f && direction == -1))
        {
            doorScale = Mathf.Clamp(doorScale + direction * doorSpeed * Time.deltaTime, 0.01f, 1f);
            doorPanel.localScale = new Vector3(doorPanel.localScale.x, doorScale, doorPanel.localScale.z);
        }
        
    }

    public void OpenDoor()
    {
        direction = -1;
        hasOpened = true;
    }

    public void CloseDoor()
    {
        direction = 1;
    }

    public bool HasOpened()
    {
        return hasOpened;
    }
}
