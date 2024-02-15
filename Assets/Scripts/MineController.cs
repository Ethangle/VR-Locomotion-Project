using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : MonoBehaviour
{
    [SerializeField] private float activationRadius;
    [SerializeField] private float soundRadiusMultiplier;
    [SerializeField] private Transform activationCircle;
    [SerializeField] private Transform mineSpikes;
    [SerializeField] private GameObject explosionPrefab;

    private Transform playerHead;
    private Transform playerRig;
    private Vector3 playerPoint;
    private float soundDelayTime = 1;
    private AudioSource audioSource;
    private bool soundPlaying;
    private float soundPlayTime;
    // Start is called before the first frame update
    void Start()
    {
        playerHead = GameObject.Find("VR Camera").transform;
        playerRig = playerHead.root;

        Vector3 circleScale = activationCircle.localScale;
        circleScale.x = 2f * activationRadius / transform.localScale.x;
        circleScale.y = 0.01f / transform.localScale.y;
        circleScale.z = 2f * activationRadius / transform.localScale.z;
        activationCircle.localScale = circleScale;

        mineSpikes.rotation = Random.rotation;

        audioSource = GetComponent<AudioSource>();

        soundPlayTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        playerPoint = playerHead.position;
        playerPoint.y = Mathf.Clamp(transform.position.y, playerRig.position.y, playerHead.position.y);

        float playerDistance = (playerPoint - transform.position).magnitude;

        soundDelayTime = Mathf.Max(3f * ((playerDistance / activationRadius) - 1f) / soundRadiusMultiplier, 0.2f);

        if (Time.time >= soundPlayTime + soundDelayTime)
        {
            soundPlayTime = Time.time;
            if (soundPlaying)
            {
                audioSource.volume = 0.5f * Mathf.Clamp(Mathf.Pow(1f - playerDistance / (activationRadius * soundRadiusMultiplier), 2f), 0f, 1f); 
                audioSource.Play();
            }
        }

        if (playerDistance <= soundRadiusMultiplier * activationRadius && !soundPlaying)
        {
            //StartCoroutine(PlaySound());
            soundPlaying = true;
        }

        if (playerDistance > soundRadiusMultiplier * activationRadius && soundPlaying)
        {
            //StopCoroutine(PlaySound());
            soundPlaying = false;
        }

        if (playerDistance <= activationRadius)
        {
            Debug.Log("Mine exploded!!");
            Explode();
        }
    }

    void Explode()
    {
        DataManager.AddMineHit();
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity) as GameObject;
        Destroy(explosion, 2f);
        Destroy(gameObject);
    }

    public void SetActivationRadius(float radius)
    {
        activationRadius = radius;
    }

    IEnumerator PlaySound()
    {
        while (true)
        {
            audioSource.Play();
            yield return new WaitForSeconds(soundDelayTime);
        }
    }
}
