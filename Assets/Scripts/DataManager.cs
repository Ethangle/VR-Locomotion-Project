using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static int playerID;
    public static PlayerMovement.MovementMethod movementMethod;

    private static int[] roomOrder;
    private static int shotNumber = 0;
    private static List<TargetShot> targetShots;
    private static List<RoomEvent> rocketHits;
    private static List<RoomEvent> mineHits;
    private static List<RoomEvent> obstacleHits;
    private static List<ObjectiveCompleted> objectivesCompleted;
    private static float objectiveStartTime = 0f;
    private static float objectiveTotalTime = 0f;
    private static int currentObjective = 0;
    private static string log;
    // Start is called before the first frame update
    void Start()
    {
        targetShots = new List<TargetShot>();
        rocketHits = new List<RoomEvent>();
        mineHits = new List<RoomEvent>();
        obstacleHits = new List<RoomEvent>();
        objectivesCompleted = new List<ObjectiveCompleted>();
        StartObjectivesTimer();
        //OutputData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetRoomOrder(GameObject[] rooms)
    {
        roomOrder = new int[rooms.Length-1];
        for (int i=0; i<rooms.Length-1; i++)
        {
            roomOrder[i] = rooms[i].GetComponent<RoomController>().roomID;
        }
    }

    public static void AddShot()
    {
        shotNumber ++;
        //Debug.LogFormat("{0} shots fired", shotNumber);
    }

    public static void AddTargetShot(bool onTar, float acc, int tarID)
    {
        targetShots.Add(new TargetShot(onTar, acc, tarID));
        //Debug.LogFormat("{0} target shots fired", targetShots.Count);
        Debug.Log("Shot on target: " + onTar.ToString() + ",  Shot accuracy: " + acc.ToString("F2") + "%,  Shot target ID: " + tarID.ToString());
    }

    public static void AddRocketHit()
    {
        rocketHits.Add(new RoomEvent());
        Debug.Log("Hit by rocket");
    }

    public static void AddMineHit()
    {
        mineHits.Add(new RoomEvent());
        Debug.Log("Hit by mine");
    }

    public static void AddObstacleHit()
    {
        obstacleHits.Add(new RoomEvent());
        Debug.Log("Hit obstacle");
    }

    public static void ObjectiveReached()
    {
        //float objTime = Time.time - objectiveStartTime;
        objectivesCompleted.Add(new ObjectiveCompleted(roomOrder[currentObjective], objectiveStartTime));
        //objectiveTotalTime += objTime;
        Debug.Log("Objective " + currentObjective.ToString() + " completed in " + (Time.time - objectiveStartTime).ToString("F2") + " seconds");
        if (currentObjective == 4)
            OutputData();
        objectiveStartTime = Time.time;
        currentObjective++;
    }

    public static void StartObjectivesTimer()
    {
        objectiveStartTime = Time.time;
    }

    private static void OutputData()
    {
        log = "";

        string localPath = "D:/Documents/_Uni/Year 4/Computer Science/Advanced Project/Project Data/";
        localPath = "./Data/";

        DateTime now = DateTime.Now;
        string fileName = "VR_Data_" + now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt";

        log += playerID.ToString("D2") + "#";
        switch (movementMethod)
        {
            case PlayerMovement.MovementMethod.joystick:
                log += "joy#";
                break;
            case PlayerMovement.MovementMethod.teleportation:
                log += "tel#";
                break;
            case PlayerMovement.MovementMethod.custom:
                log += "cus#";
                break;
            case PlayerMovement.MovementMethod.humanJoystick:
                log += "hum#";
                break;
            case PlayerMovement.MovementMethod.none:
                log += "non#";
                break;
            default:
                log += "_#";
                break;
        }

        for (int i=0; i<objectivesCompleted.Count; i++)
        {
            ObjectiveCompleted obj = objectivesCompleted[i];
            log += obj.ID.ToString() + "?" + obj.startTime.ToString() + "?" + obj.endTime.ToString();
            if (i < objectivesCompleted.Count - 1)
                log += "!";
        }

        log += "#";

        for (int i=0; i<targetShots.Count; i++)
        {
            TargetShot shot = targetShots[i];

            log += shot.roomID.ToString("D2") + "?" + shot.targetID.ToString("D2") + "?" + shot.time.ToString() + "?";
            switch (shot.onTarget)
            {
                case true:
                    log += "1?";
                    break;
                case false:
                    log += "0?";
                    break;
            }
            log += shot.accuracy.ToString();

            if (i < targetShots.Count - 1)
                log += "!";
        }

        log += "#";

        for (int i=0; i<rocketHits.Count; i++)
        {
            RoomEvent hit = rocketHits[i];
            log += hit.roomID.ToString("D2") + "?" + hit.time.ToString();

            if (i < rocketHits.Count - 1)
                log += "!";
        }

        log += "#";

        for (int i=0; i<mineHits.Count; i++)
        {
            RoomEvent hit = mineHits[i];
            log += hit.roomID.ToString("D2") + "?" + hit.time.ToString();

            if (i < mineHits.Count - 1)
                log += "!";
        }

        log += "#";

        for (int i=0; i<obstacleHits.Count; i++)
        {
            RoomEvent hit = obstacleHits[i];
            log += hit.roomID.ToString("D2") + "?" + hit.time.ToString();

            if (i < obstacleHits.Count - 1)
                log += "!";
        }

        StreamWriter writer = new StreamWriter(localPath+fileName, true);

        writer.Write(log);
        writer.Close();

        Debug.Log("Data successfully saved to '" + fileName + "'");
    }

    private class TargetShot
    {
        public bool onTarget;
        public float accuracy;
        public int targetID;
        public int roomID;
        public float time;

        public TargetShot(bool onTar, float acc, int tarID)
        {
            onTarget = onTar;
            accuracy = acc;
            targetID = tarID;
            roomID = roomOrder[currentObjective];
            time = Time.time;
        }
    }

    private class RoomEvent
    {
        public float time;
        public int roomID;

        public RoomEvent()
        {
            time = Time.time;
            roomID = roomOrder[currentObjective];
        }
    }

    private class ObjectiveCompleted
    {
        public int ID;
        //float timeTaken;
        public float startTime;
        public float endTime;

        public ObjectiveCompleted(int objID, float objStartTime)
        {
            ID = objID;
            //timeTaken = objTime;
            startTime = objStartTime;
            endTime = Time.time;
        }
    }
}
