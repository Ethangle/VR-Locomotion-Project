using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandPresence : MonoBehaviour
{
    [SerializeField] bool showController = false;
    [SerializeField] bool holdingGun = false;
    [SerializeField] InputDeviceCharacteristics controllerCharacteristics;
    [SerializeField] List<GameObject> controllerPrefabs;
    [SerializeField] GameObject handModelPrefab;
    [SerializeField] GameObject gunPrefab;

    [SerializeField] GameObject teleportRayPrefab;
    [SerializeField] GameObject teleportReticlePrefab;
    [SerializeField] GameObject teleportReticleArrowPrefab;

    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private GameObject spawnedGunModel;
    private GameObject spawnedTeleportRay;
    private GameObject spawnedTeleportReticle;
    private GameObject spawnedTeleportReticleArrow;
    private Animator handAnimator;

    private bool secondaryButtonReleased = true;
    private bool triggerReleased = true;
    private float handMirror = 1f;

    private bool teleportActive = false;
    [SerializeField] bool canTeleport;
    private float teleportAngle;
    private GameObject VRRig;
    private PlayerMovement playerMovement;
    private GameObject VRCamera;

    private bool canRotateCamera;
    private float rotationSpeed;
    private float walkSpeed;

    // Start is called before the first frame update
    void Start()
    {
        TryInitialize();
        VRRig = GameObject.Find("VR Rig");
        playerMovement = VRRig.GetComponent<PlayerMovement>();
        VRCamera = GameObject.Find("VR Camera");
    }

    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        if ((controllerCharacteristics & InputDeviceCharacteristics.Right) == InputDeviceCharacteristics.Right)
        {
            handMirror = 1f;
        }
        else if ((controllerCharacteristics & InputDeviceCharacteristics.Left) == InputDeviceCharacteristics.Left)
        {
            handMirror = -1f;
        }

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
            }
            else 
            {
                Debug.LogError("Did not find corresponding controller model");
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }

            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();

            spawnedGunModel = Instantiate(gunPrefab, transform);
            spawnedGunModel.transform.localScale = 0.15f * Vector3.one;
            spawnedGunModel.transform.Translate(handMirror * 0.02f, 0f, -0.018f);
            spawnedGunModel.transform.Rotate(15f, 0f, 0f);

            spawnedTeleportRay = Instantiate(teleportRayPrefab, transform.parent.parent.parent.parent);

            spawnedTeleportReticle = Instantiate(teleportReticlePrefab);
            spawnedTeleportRay.GetComponent<XRInteractorLineVisual>().reticle = spawnedTeleportReticle;

            spawnedTeleportReticleArrow = Instantiate(teleportReticleArrowPrefab);
        }
    }

    void UpdateHandAnimation()
    {
        if (holdingGun)
        {
            handAnimator.SetFloat("Trigger", 0.4f);
        }
        else if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0f);
        }

        if (holdingGun)
        {
            handAnimator.SetFloat("Grip", 0.4f);
        }
        else if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            if (showController)
            {
                spawnedHandModel.SetActive(false);
                spawnedController.SetActive(true);
            }
            else
            {
                spawnedHandModel.SetActive(true);
                spawnedController.SetActive(false);
                UpdateHandAnimation();
            }

            if (holdingGun)
            {
                spawnedGunModel.SetActive(true);
            }
            else
            {
                spawnedGunModel.SetActive(false);
            }

            if (canTeleport && targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 joystickValue) && joystickValue.magnitude >= 0.1f)
            {
                switch(playerMovement.movementMethod)
                {
                    /*
                    case PlayerMovement.MovementMethod.joystick:
                        switch(handMirror)
                        {
                            case 1:
                                if (Mathf.Abs(joystickValue.x) >= 0.5f)
                                {
                                    if (canRotateCamera)
                                    {
                                        rotationSpeed = 45f;
                                        VRRig.transform.Rotate(0f, rotationSpeed * joystickValue.x / Mathf.Abs(joystickValue.x), 0f);
                                        canRotateCamera = false;
                                    }
                                }
                                else
                                {
                                    canRotateCamera = true;
                                }
                                break;

                            case -1:
                                if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool joystickClick) && joystickClick)
                                {
                                    walkSpeed = 2.3f;
                                }
                                else
                                {
                                    walkSpeed = 1.3f;
                                }
                                VRRig.transform.position += walkSpeed * Time.deltaTime * joystickValue.Rotate(VRCamera.transform.forward.ToVector2XZ().RotationAngleDeg() - 90f).ToVector3XZ(); 
                                break;

                            default:
                                break;
                        }
                        break;
                    */
                    case PlayerMovement.MovementMethod.teleportation:
                        //Debug.Log("Moved joystick");
                        teleportActive = true;
                        spawnedTeleportRay.SetActive(true);
                        spawnedTeleportReticle.SetActive(true);
                        //spawnedTeleportReticleArrow.SetActive(true);

                        teleportAngle = -joystickValue.RotationAngleDeg() + transform.rotation.eulerAngles.y;
                        //Debug.Log(teleportAngle);
                        //spawnedTeleportReticleArrow.transform.position = spawnedTeleportReticle.transform.position;
                        //spawnedTeleportReticleArrow.transform.rotation = Quaternion.Euler(0f, teleportAngle, 0f);
                        break;

                    default:

                        break;
                }
            }
            else
            {
                teleportActive = false;
                spawnedTeleportRay.SetActive(false);
                spawnedTeleportReticle.SetActive(false);
                spawnedTeleportReticleArrow.SetActive(false);
            }
            //Debug.Log(joystickValue.magnitude);

            if (targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue) && secondaryButtonValue)
            {
                if (secondaryButtonReleased)
                {
                    secondaryButtonReleased = false;
                    holdingGun = !holdingGun;
                }
            }
            else
            {
                secondaryButtonReleased = true;
            }

            if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.5f)
            {
                if (triggerReleased)
                {
                    triggerReleased = false;
                    if (holdingGun && !teleportActive)
                        spawnedGunModel.GetComponent<GunHandler>().Fire();

                    if (teleportActive && false)
                    {
                        float rigRotateAngle = teleportAngle - VRCamera.transform.forward.ToVector2XZ().RotationAngleDeg();
                        Vector3 rigOffset = (VRRig.transform.position - VRCamera.transform.position).ToVector2XZ().Rotate(rigRotateAngle).ToVector3XZ();
                        VRRig.transform.position += rigOffset;
                        VRRig.transform.Rotate(0f, -rigRotateAngle, 0f);

                    }

                }
            }
            else
            {
                triggerReleased = true;
            }
            
            
        }

        
    }
}
