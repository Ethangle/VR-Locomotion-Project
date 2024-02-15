using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    [SerializeField] private float activationDistance;
    [SerializeField] private GameObject targetMain;
    [SerializeField] private GameObject targetMainGreen;

    private Transform playerHead;
    private Transform playerRig;
    private bool isHit = false;
    private Vector3 playerPoint;
    // Start is called before the first frame update
    void Start()
    {
        playerHead = GameObject.Find("VR Camera").transform;
        playerRig = playerHead.root;
        targetMainGreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isHit)
            return;

        playerPoint = playerHead.position;
        playerPoint.y = playerRig.position.y;

        if ((playerPoint - transform.position).magnitude <= activationDistance)
        {
            targetMain.SetActive(true);
        }
        else
        {
            targetMain.SetActive(false);
        }
    }

    public void SetHit(bool playSound)
    {
        isHit = true;
        targetMain.SetActive(false);
        targetMainGreen.SetActive(true);
        if (playSound)
            GetComponent<AudioSource>().Play();
    }

    public bool IsHit()
    {
        return targetMainGreen.activeSelf;
    }
}
