using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    [SerializeField] private float rocketMaxSpeed;
    [SerializeField] private GameObject rocketExplosionPrefab;
    private float rocketSpeed = 0f;
    private float rocketAcceleration = 0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LaunchSequence());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        transform.Translate(transform.forward * rocketSpeed * Time.fixedDeltaTime, Space.World);
        if (rocketAcceleration > 0f && rocketSpeed < rocketMaxSpeed)
        {
            rocketSpeed += rocketAcceleration * Time.fixedDeltaTime;
            //rocketAcceleration *= 0.9f * Time.fixedDeltaTime;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
            DataManager.AddRocketHit();
        GameObject explosion = Instantiate(rocketExplosionPrefab, transform.position, Quaternion.identity) as GameObject;
        Destroy(explosion, 2.1f);
        Destroy(gameObject);
    }

    IEnumerator LaunchSequence()
    {
        rocketAcceleration = 0f;
        yield return new WaitForSeconds(1.5f);
        rocketAcceleration = 10f;
    }
}
