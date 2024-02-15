using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreIndicator : MonoBehaviour
{
    [SerializeField] private float scoreRiseHeight;
    [SerializeField] private float scoreRiseTime;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float relativeScale;
    private float scoreRiseSpeed;
    private float scoreAlphaSpeed;
    private Transform cam;
    //private float alpha = 1f;
    private Color textColour;
    private Vector3 camVector;
    private float scoreHeight;
    private float scale;
    private float offset;
    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("VR Camera").transform;
        scoreRiseSpeed = scoreRiseHeight / scoreRiseTime;
        scoreAlphaSpeed = 1f / scoreRiseTime;
    }

    // Update is called once per frame
    void Update()
    {
        camVector = transform.position - cam.position;
        transform.forward = camVector.normalized;

        scale = Mathf.Max(camVector.magnitude / 4f, 1f) * relativeScale;
        transform.localScale = Vector3.one * scale;

        scoreText.color = textColour;

        textColour.a -= scoreAlphaSpeed * Time.deltaTime;

        //transform.position += Vector3.up * scoreRiseSpeed * Time.deltaTime;

        offset += scoreRiseSpeed * Time.deltaTime;

        transform.position = startPosition + Vector3.up * offset * scale;

        if (textColour.a < 0)
            Destroy(gameObject);
    }

    public void SetScoreText(int score, Vector3 start)
    {
        scoreText.text = "+" + score.ToString();

        textColour.g = 2f * (score - 1) / 9f;
        textColour.r = 2f - textColour.g;
        textColour.b = 0f;
        textColour.a = 1f;

        transform.position = start;
        startPosition = start;
    }
}
