using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerMovement : MonoBehaviour
{
    public enum MovementMethod{joystick, teleportation, custom, humanJoystick, none};
    public MovementMethod movementMethod = MovementMethod.custom;
    //private float constantSpeed = 1f;
    private Vector3 constantDirection;
    [SerializeField] private float constantSpeedDistance;
    [SerializeField] private float speedMultiplier;
    private bool isConstantSpeed = false;
    private Vector3 playerLocal2DPos;
    private Vector3 playerPrevPos;
    private float playerLocalSpeed = 1f;
    [SerializeField] private Transform playerHead;

    [SerializeField] private Transform boundaryIndicator;

    private CharacterController character;
    // Start is called before the first frame update
    void Start()
    {
        DataManager.movementMethod = movementMethod;
        character = GetComponent<CharacterController>();
        switch (movementMethod)
        {
            case MovementMethod.joystick:
                GetComponent<ContinuousMovement>().enabled = true;
                GetComponent<DeviceBasedSnapTurnProvider>().enabled = true;

                boundaryIndicator.gameObject.SetActive(false);
                break;

            case MovementMethod.teleportation:
                GetComponent<ContinuousMovement>().enabled = false;
                GetComponent<DeviceBasedSnapTurnProvider>().enabled = false;

                boundaryIndicator.gameObject.SetActive(false);
                break;

            case MovementMethod.custom:
                GetComponent<ContinuousMovement>().enabled = true;
                GetComponent<DeviceBasedSnapTurnProvider>().enabled = false;

                boundaryIndicator.gameObject.SetActive(true);
                boundaryIndicator.localScale = constantSpeedDistance*(new Vector3(0.2f, 0f, 0.2f));
                break;

            case MovementMethod.humanJoystick:
                GetComponent<ContinuousMovement>().enabled = true;
                GetComponent<DeviceBasedSnapTurnProvider>().enabled = false;

                boundaryIndicator.gameObject.SetActive(true);
                boundaryIndicator.localScale = 0.2f * (new Vector3(0.2f, 0f, 0.2f));
                isConstantSpeed = true;
                break;

            default:
                Debug.Log("No movement method selected");
                break;
        }
        //playerLocal2DPos = new Vector3(playerHead.localPosition.x, 0f, playerHead.localPosition.z);
        playerLocal2DPos = playerHead.localPosition.ZeroY();
        playerPrevPos = playerLocal2DPos;
    }

    // Update is called once per frame
    void Update()
    {
        switch(movementMethod)
        {
            case MovementMethod.joystick:
                
                break;

            case MovementMethod.teleportation:

                break;

            case MovementMethod.custom:
                //playerLocal2DPos = new Vector3(playerHead.localPosition.x, 0f, playerHead.localPosition.z);
                playerLocal2DPos = playerHead.localPosition.ZeroY();
                

                if (playerLocal2DPos.magnitude >= constantSpeedDistance) {
                    isConstantSpeed = true;
                    //transform.position += playerLocalSpeed * Time.deltaTime * playerLocal2DPos.normalized;
                } else {
                    isConstantSpeed = false;
                    playerLocalSpeed = (playerLocal2DPos - playerPrevPos).magnitude / Time.deltaTime;
                }

                playerPrevPos = playerLocal2DPos;

                break;

            case MovementMethod.humanJoystick:
                playerLocal2DPos = playerHead.localPosition.ZeroY();
                playerLocalSpeed = playerLocal2DPos.magnitude;
                break;
            case MovementMethod.none:
                break;
            default:
                Debug.LogError("No movement method selected.");
                break;
        }
    }

    void FixedUpdate()
    {
        if (isConstantSpeed)
        {
            character.Move(speedMultiplier * playerLocalSpeed * Time.fixedDeltaTime * playerLocal2DPos.normalized);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit col)
    {
        if (col.gameObject.tag == "Obstacle")
        {
            Debug.Log("Controller collision with obstacle");
            DataManager.AddObstacleHit();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Obstacle")
        {
            Debug.Log("Collision with obstacle");
        }
    }
}
