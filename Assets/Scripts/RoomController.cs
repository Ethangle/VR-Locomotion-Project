using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public int roomID;

    [SerializeField] private GameObject exitDoor;
    [SerializeField] private Transform waypointPos;
    [SerializeField] private GameObject[] targets;
    [SerializeField] private RandomObjectPlacement[] objectPlacements;
    
    // Start is called before the first frame update
    void Start()
    {
        PlaceObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetDoor()
    {
        return exitDoor;
    }

    public Vector3 GetWaypointPos()
    {
        return waypointPos.position;
    }

    private void PlaceObjects()
    {
        foreach (RandomObjectPlacement placement in objectPlacements)
        {
            Transform tempPos;
            for (int i = 0; i < placement.objects.Length; i++)
            {
                int rnd = Random.Range(i, placement.locations.Length);
                tempPos = placement.locations[rnd];
                placement.locations[rnd] = placement.locations[i];
                placement.locations[i] = tempPos;

                placement.objects[i].position = placement.locations[i].position;
                if (placement.useRotation)
                    placement.objects[i].rotation = placement.locations[i].rotation;

                if (placement.useScale)
                    placement.objects[i].localScale = placement.locations[i].localScale;
            }
        }
    }

    public bool IsComplete()
    {
        foreach (GameObject target in targets)
        {
            if (!target.GetComponent<TargetController>().IsHit())
                return false;
        }
        return true;
    }

    public void SetComplete()
    {
        foreach(GameObject target in targets)
        {
            target.GetComponent<TargetController>().SetHit(false);
        }
    }

    public bool IsFinished()
    {
        return exitDoor.GetComponent<DoorController>().HasOpened();
    }
}

[System.Serializable] public class RandomObjectPlacement
{
    public string objectType;
    public bool useScale = false;
    public bool useRotation = true;
    public Transform[] objects;
    public Transform[] locations; 
}
