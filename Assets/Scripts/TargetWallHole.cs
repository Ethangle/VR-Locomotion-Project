using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetWallHole : MonoBehaviour
{
    [SerializeField] private bool randomise;
    [SerializeField] private bool animate;
    [SerializeField] private float speed;
    [SerializeField] private float height;
    [SerializeField] private float depth;

    [SerializeField] private Transform topWall;
    [SerializeField] private Transform bottomWall;
    [SerializeField] private Transform hole;
    [SerializeField] private Transform holeSideWalls;

    private int animateDirection = 1;
    private float heightRange = 0.85f;
    // Start is called before the first frame update
    void Start()
    {
        if (randomise)
        {
            height = Random.Range(-heightRange, heightRange);
            depth = Random.Range(1f, 3f);
        }
        UpdatePosition();
    }

    // Update is called once per frame
    void Update()
    {
        if ((height < heightRange && animateDirection == 1) || (height > -heightRange && animateDirection == -1))
        {
            height = Mathf.Clamp(height + speed * animateDirection * Time.deltaTime, -heightRange, heightRange);
            UpdatePosition();
        }
        else
        {
            animateDirection *= -1;
        }

    }

    void UpdatePosition()
    {
        topWall.localPosition = new Vector3(0f, 2.5f + height/2f, 0f);
        topWall.localScale = new Vector3(topWall.localScale.x, 1f - height, topWall.localScale.z);
        bottomWall.localPosition = new Vector3(0f, 0.5f + height/2f, 0f);
        bottomWall.localScale = new Vector3(bottomWall.localScale.x, 1f + height, bottomWall.localScale.z);

        hole.localPosition = new Vector3(0f, 1.5f + height, depth);
        holeSideWalls.localScale = new Vector3(1f, 1f, depth-0.075f);
    }
}
