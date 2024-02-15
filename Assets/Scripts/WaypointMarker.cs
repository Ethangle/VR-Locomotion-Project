using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaypointMarker : MonoBehaviour
{
    [SerializeField] private Transform waypointIcon;
    [SerializeField] private RawImage waypointImg;
    [SerializeField] private TextMeshProUGUI waypointText;
    [SerializeField] private Transform waypointBeam;
    [SerializeField] private Transform cam;
    [SerializeField] private float markerHeight;
    //[SerializeField] private float distanceFromCamera;
    [SerializeField] private float viewAngleMax;
    [SerializeField] private float viewAngleMin;
    [SerializeField] private float transparencyMax;
    [SerializeField] private float transparencyMin;
    [SerializeField] private float activationRange;
    [SerializeField] private float relativeScale;
    private Vector3[] objectivePoints;
    private DoorController[] objectiveDoors;
    
    //private float currentAlpha = 1f;
    //private Vector3 startingScale;
    private Vector3 currentObjective;
    private int currentObjectiveIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        waypointBeam.localScale = new Vector3(waypointBeam.localScale.x, markerHeight/2f, waypointBeam.localScale.z);
        waypointBeam.localPosition = new Vector3(0f, markerHeight/2f, 0f);
        waypointIcon.localPosition = new Vector3(0f, markerHeight, 0f);
        //NextObjective();  
        //startingScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (objectiveDoors == null)
        {
            return;
        }
        if (currentObjective == Vector3.zero)
        {
            currentObjective = objectivePoints[0];
            transform.position = currentObjective;
        }
        UpdateVisuals();

        if ((transform.position - cam.position).ToVector2XZ().magnitude <= activationRange && Mathf.Abs(transform.position.y + 1f - cam.position.y) <= 1.5f)
        {
            if (objectiveDoors[currentObjectiveIndex].transform.root.GetComponent<RoomController>().IsComplete())
                NextObjective();
        }
    }

    void NextObjective()
    {
        if (currentObjectiveIndex >= 0)
        {
            //Debug.Log(currentObjectiveIndex);
            DataManager.ObjectiveReached();
            if (currentObjectiveIndex < objectiveDoors.Length)
            {
                objectiveDoors[currentObjectiveIndex].OpenDoor();
                if (currentObjectiveIndex != 0)
                {
                    objectiveDoors[currentObjectiveIndex - 1].CloseDoor();
                }
            }
            
        }
        if (currentObjectiveIndex == objectivePoints.Length - 1)
            Destroy(gameObject);
        currentObjectiveIndex = (currentObjectiveIndex + 1) % objectivePoints.Length;
        currentObjective = objectivePoints[currentObjectiveIndex];
        transform.position = currentObjective;
    }

    void UpdateVisuals()
    {
        Vector3 objectiveVector = currentObjective - cam.position;
        objectiveVector.y += markerHeight; 
        waypointIcon.forward = objectiveVector.normalized;
        //transform.position = cam.position + Mathf.Min(distanceFromCamera, objectiveVector.magnitude) * transform.forward;

        waypointIcon.localScale = Mathf.Max(objectiveVector.magnitude / 4f, 1f) * Vector3.one * relativeScale;// * startingScale;

        waypointText.text = ((int)objectiveVector.magnitude).ToString() + "m";

        float viewAngle = Vector3.Angle(cam.forward, waypointIcon.forward);

        /*if (viewAngle < transparencyViewAngle && currentAlpha != transparencyMin)
            SetAlpha(transparencyMin);
        else if (viewAngle > transparencyViewAngle && currentAlpha != transparencyMax)
            SetAlpha(transparencyMax);
        */
        SetAlpha(transparencyMin + (1 - Mathf.Cos((90f / (viewAngleMax-viewAngleMin))*(Mathf.Clamp(viewAngle, viewAngleMin, viewAngleMax) - viewAngleMin)*Mathf.Deg2Rad)) * (transparencyMax - transparencyMin));
    }

    void SetAlpha(float alpha)
    {
        //Debug.Log("Set alpha to " + alpha.ToString("F2"));
        //currentAlpha = alpha;
        /*Color newColour = waypointText.color;
        newColour.a = alpha;
        waypointText.color = newColour;

        newColour = waypointImg.color;
        newColour.a = alpha;
        waypointImg.color = newColour;
        */
        waypointText.color = new Color(waypointText.color.r, waypointText.color.g, waypointText.color.b, alpha);
        waypointImg.color = new Color(waypointImg.color.r, waypointImg.color.g, waypointImg.color.b, alpha);
    }


    public void SetObjectives(Vector3[] objs, DoorController[] doors)
    {
        objectivePoints = objs;
        objectiveDoors = doors;
    }
}
