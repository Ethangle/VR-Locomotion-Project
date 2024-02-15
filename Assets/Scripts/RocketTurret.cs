using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTurret : MonoBehaviour
{
    [SerializeField] private Transform turretStand;
    [SerializeField] private Transform turretBarrel;
    [SerializeField] private Transform turretRocket;
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private float turretYawSpeed;
    [SerializeField] private float turretPitchSpeed;
    //[SerializeField] private float turretRotationSpeed;
    [SerializeField] private float delayTime;
    [SerializeField] private float activationRange;
    [SerializeField] private bool canTrackPlayer;

    private bool hasSeenPlayer = false;
    private Transform playerHead;
    private Vector3 playerVector;
    private bool canMove = false;
    private RoomController roomController;
    // Start is called before the first frame update
    void Start()
    {
        playerHead = GameObject.Find("VR Camera").transform;
        StartCoroutine(DelayMove(Random.Range(0f, 1f)));

        roomController = transform.root.gameObject.GetComponent<RoomController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove || roomController.IsFinished())
            return;

        playerVector = playerHead.position - turretBarrel.position;// - Vector3.up * 0.5f;
        if (playerVector.magnitude > activationRange)
            return;

        if (!canTrackPlayer)
        {
            if (hasSeenPlayer)
            {
                FireRocket();
            }
            else
            {
                StartCoroutine(DelayMove(2f));
                hasSeenPlayer = true;
            }
            return;
        }

        if (!CanSeePlayer())
            return;

        
        

        //turretBarrel.forward = Vector3.RotateTowards(turretBarrel.forward, playerVector, turretRotationSpeed * Time.deltaTime, 0f);
        //turretBarrel.localRotation = Quaternion.Euler(Vector3.Scale(turretBarrel.localRotation.eulerAngles, new Vector3(1f, 0f, 1f)) + Vector3.up * 90f);
        
        
        //Vector3 rotationDirection = Quaternion.FromToRotation(turretBarrel.forward, playerVector).eulerAngles;

        float yawRotation = Vector2.SignedAngle(playerVector.ToVector2XZ(), turretBarrel.forward.ToVector2XZ());
        //float pitchRotation = Vector3.SignedAngle(turretBarrel.forward.ZeroY,playerVector.ZeroY);

        //float yawRotation = Vector3.SignedAngle(turretBarrel.forward, playerVector, Vector3.right);
        float pitchRotation = Vector3.SignedAngle(turretBarrel.forward, playerVector, turretBarrel.right);
        //Debug.Log(yawRotation.ToString("F1") + ", " + pitchRotation.ToString("F2"));

        if (Mathf.Abs(yawRotation) > turretYawSpeed * Time.deltaTime * 0.5f)
        {
            turretBarrel.Rotate(Vector3.up * Mathf.Sign(yawRotation) * turretYawSpeed * Time.deltaTime, Space.World);
        }
        else if (Mathf.Abs(yawRotation) > 0.5f)
        {
            turretBarrel.Rotate(Vector3.up * yawRotation, Space.World);
        }
        else if (Mathf.Abs(pitchRotation) > turretPitchSpeed * Time.deltaTime * 0.5f)
        {
            turretBarrel.Rotate(Vector3.right * Mathf.Sign(pitchRotation) * turretPitchSpeed * Time.deltaTime, Space.Self);
        }
        else if (Mathf.Abs(pitchRotation) > 0.5f)
        {
            turretBarrel.Rotate(Vector3.right * pitchRotation, Space.Self);
        }
        else
        {
            FireRocket();
        }
        
        //turretBarrel.Rotate(Vector3.Scale(rotationDirection, new Vector3(turretRotationSpeed, turretRotationSpeed, turretRotationSpeed) * Time.deltaTime));
    }

    void FireRocket()
    {
        turretRocket.gameObject.SetActive(false);
        GameObject rocket = Instantiate(rocketPrefab, turretRocket.position, turretRocket.rotation) as GameObject;
        Destroy(rocket, 100f);
        StartCoroutine(DelayMove(delayTime));
    }

    IEnumerator DelayMove(float delay)
    {
        canMove = false;
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f) * delay);
        canMove = true;
        turretRocket.gameObject.SetActive(true);
    }

    bool CanSeePlayer()
    {
        LayerMask mask = ~LayerMask.GetMask("Ignore Raycast");
        Vector3 rayVector = playerHead.position - turretRocket.position;
        Debug.DrawRay(turretRocket.position, rayVector, Color.white);
        if (Physics.Raycast(turretRocket.position, rayVector, out RaycastHit hit, rayVector.magnitude, mask))
        {
            return false;
        }
        return true;
    }
}
