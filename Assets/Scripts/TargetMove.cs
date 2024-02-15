using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMove : MonoBehaviour
{
    public int targetID;
    [SerializeField] private Vector4[] targetPath;
    [SerializeField] private bool loopPoints;

    private Vector3 origin;
    private Vector3 currentPoint;
    private int currentPointIndex = 1;
    private float currentSpeed = 1f;
    private int pathDirection = 1;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.localPosition;
        transform.localPosition = origin + (Vector3)targetPath[0];
        if (targetPath.Length > 1) {
            currentSpeed = targetPath[1][3];
            UpdateValues();
        }
        //currentPoint = origin;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPath.Length < 2)
            return;


        if ((transform.localPosition - currentPoint).magnitude <= currentSpeed * Time.deltaTime)
        {
            transform.localPosition = currentPoint;
            if (!loopPoints && ((pathDirection == 1 && currentPointIndex == targetPath.Length - 1) || (pathDirection == -1 && currentPointIndex == 0)))
                pathDirection *= -1;

            currentPointIndex = (currentPointIndex + pathDirection) % targetPath.Length;
            UpdateValues();
        }
        else
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, currentPoint, currentSpeed * Time.deltaTime);    
        }

        

        
    }

    private void UpdateValues()
    {
        currentPoint = origin + (Vector3)targetPath[currentPointIndex];
        currentSpeed = targetPath[(currentPointIndex + (1-pathDirection)/2) % targetPath.Length][3];
    }
}
