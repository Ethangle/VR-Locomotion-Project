using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] Transform targetCenter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float HitTarget(Vector3 hitPoint)
    {
        float score = 100f * (1f - (hitPoint - targetCenter.position).magnitude * (2f / (transform.localScale.z * transform.parent.localScale.z)));
        //Debug.Log("You scored " + score);
        return score;
    }
}
