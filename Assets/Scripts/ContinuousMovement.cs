using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    //[SerializeField] XRNode leftInputSource;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float additionalHeight;
    [SerializeField] private XRNode inputSource;
    [SerializeField] private XRNode leftInputSource;
    [SerializeField] private XRNode rightInputSource;
    [SerializeField] private GameObject leftHandObject;
    [SerializeField] private GameObject rightHandObject;

    private float fallingSpeed;
    private XRRig rig;
    private Vector2 inputAxis;
    private CharacterController character;
    private InputDevice leftHandDevice;
    private InputDevice rightHandDevice;
    private bool jumpButtonPressed;
    private float sprintButtonPressed;
    private float speed;
    private bool useJoysticks = false;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
        //leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        //rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (GetComponent<PlayerMovement>().movementMethod == PlayerMovement.MovementMethod.joystick)
        {
            useJoysticks = true;
        }
        
        if (jumpHeight < 0f)
            jumpHeight *= -1f;

    }

    // Update is called once per frame
    void Update()
    {
        //InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        //device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);

        leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (useJoysticks)
        {
            leftHandDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
            leftHandDevice.TryGetFeatureValue(CommonUsages.trigger, out sprintButtonPressed);
        }
        
        rightHandDevice.TryGetFeatureValue(CommonUsages.primaryButton, out jumpButtonPressed);
        
        //Debug.Log(rightPrimaryButtonPressed);
        
    }

    private void FixedUpdate()
    {
        CapsuleFollowHeadset();
        //Quaternion headYaw = Quaternion.Euler(0f, rig.cameraGameObject.transform.eulerAngles.y, 0f);
        Quaternion handYaw = Quaternion.Euler(0f, leftHandObject.transform.eulerAngles.y, 0f);
        Vector3 direction = (handYaw * inputAxis.ToVector3XZ()).normalized;

        if (sprintButtonPressed > 0.5f)
            speed = sprintSpeed;
        else
            speed = walkSpeed;

        if (direction.magnitude < 0.1f)
            direction = Vector3.forward * 0.0001f;
        
        character.Move(direction * Time.fixedDeltaTime * speed);

        if (CheckIfGrounded())
        {
            if (jumpButtonPressed)
                fallingSpeed = Mathf.Sqrt(-2f * gravity * jumpHeight);
            else
                fallingSpeed = 0f;
        }
        else
            fallingSpeed += gravity * Time.fixedDeltaTime;

        character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
    }

    void CapsuleFollowHeadset()
    {
        character.height = rig.cameraInRigSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height / 2f + character.skinWidth, capsuleCenter.z);
    }

    bool CheckIfGrounded()
    {
        //tells us if on ground
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLength = character.center.y + 0.01f;
        return Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
    }
}
