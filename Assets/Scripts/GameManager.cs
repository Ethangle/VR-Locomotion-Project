using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int playerID;
    [SerializeField] private GameObject[] rooms;
    [SerializeField] private WaypointMarker waypointMarker;

    private Vector3[] objectivePoints;
    private DoorController[] objectiveDoors;
    // Start is called before the first frame update
    void Start()
    {
        DataManager.playerID = playerID;
        //ShuffleRooms();
        DataManager.SetRoomOrder(rooms);
        objectivePoints = new Vector3[rooms.Length-1];
        objectiveDoors = new DoorController[rooms.Length-1];

        for (int i = 0; i < rooms.Length; i++)
        {
            if (i > 0)
                rooms[i].transform.position = rooms[i-1].GetComponent<RoomController>().GetDoor().transform.position;
            if (i < rooms.Length - 1)
            {
                objectivePoints[i] = rooms[i].GetComponent<RoomController>().GetWaypointPos();
                objectiveDoors[i] = rooms[i].GetComponent<RoomController>().GetDoor().GetComponent<DoorController>();
            }
        }

        waypointMarker.SetObjectives(objectivePoints, objectiveDoors);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void ShuffleRooms()
    {
        GameObject tempRoom;
        for (int i = 1; i < rooms.Length-2; i++)
        {
            int rnd = Random.Range(i, rooms.Length-1);
            tempRoom = rooms[rnd];
            rooms[rnd] = rooms[i];
            rooms[i] = tempRoom;
        }
    }
}
