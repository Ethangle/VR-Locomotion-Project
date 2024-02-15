using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarker : MonoBehaviour
{
    [SerializeField] private Transform playerHead;

    private Transform VRRig;
    // Start is called before the first frame update
    void Start()
    {
        VRRig = playerHead.root;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 playerView2D = new Vector2(playerHead.forward.x, playerHead.forward.z);
        //transform.rotation = Quaternion.Euler(0f, Mathf.Atan2(playerHead.forward.x, playerHead.forward.z), 0f);
        //transform.position = playerHead.position + new Vector3(0f, -995f, 0f);

        transform.position = new Vector3(playerHead.position.x, VRRig.position.y, playerHead.position.z);
    }
}
