using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBehavior : MonoBehaviour 
{

	public float speed = 500f;
	private RaycastHit hit;
	[SerializeField] private GameObject explosionPrefab;
	[SerializeField] private GameObject scoreIndicatorPrefab;
	private LayerMask mask;

	private bool shotOnTarget = false;
	private float shotAccuracy = 0f;
	private int shotTargetID = -1;
	// Use this for initialization
	void Start() 
	{
		mask = ~LayerMask.GetMask("Ignore Raycast");
		DataManager.AddShot();
	}
	
	// Update is called once per frame
	void Update() 
	{
		if (Physics.Raycast(transform.position, transform.forward, out hit, speed * Time.deltaTime, mask))
		{
			
			if (hit.collider.gameObject.tag == "TargetRange")
			{
				shotTargetID = hit.collider.GetComponent<TargetMove>().targetID;
			}
			else
			{

				if (hit.collider.gameObject.tag == "TargetMain" || hit.collider.gameObject.tag == "TargetCase")
				{
					shotTargetID = hit.collider.transform.parent.GetComponent<TargetMove>().targetID;
					shotOnTarget = true;

					if (hit.collider.gameObject.tag == "TargetMain")
					{
						GameObject target = hit.collider.gameObject;
						shotAccuracy = target.GetComponent<Target>().HitTarget(hit.point);
						target.transform.parent.GetComponent<TargetController>().SetHit(true);
						GameObject scoreIndicator = Instantiate(scoreIndicatorPrefab) as GameObject;
						//scoreIndicator.transform.position = hit.point;
						scoreIndicator.GetComponent<ScoreIndicator>().SetScoreText(Mathf.Min((int)(shotAccuracy / 10f) + 1, 10), hit.point);
					}
				}
				GameObject explosion = Instantiate(explosionPrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal), null) as GameObject;
				//explosion.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
				explosion.transform.SetParent(hit.collider.transform, true);
				//explosion.transform.localScale = Vector3.Scale(explosion.transform.localScale, new Vector3(1f/hit.collider.transform.localScale.x, 1f/hit.collider.transform.localScale.y, 1f/hit.collider.transform.localScale.z));
				/*
				foreach (GameObject particle in explosion.transform)
				{
					particle.GetComponent<ParticleSystem>().Play();
				}
				*/
				Destroy(explosion, 5f);
				Destroy(gameObject);
			}
		}
		transform.position += transform.forward * Time.deltaTime * speed;
	}

	void OnDestroy()
	{
		if (shotTargetID != -1)
			DataManager.AddTargetShot(shotOnTarget, shotAccuracy, shotTargetID);
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "TargetRange")
			shotTargetID = col.GetComponent<TargetMove>().targetID;
	}

	/*void OnTriggerEnter(Collider col)
	{
		if (Physics.Raycast(transform.position, transform.forward, out hit, 10f, mask))
		{
			GameObject explosion = Instantiate(explosionPrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
			foreach (GameObject particle in explosion.transform)
			{
				particle.GetComponent<ParticleSystem>().Play();
			}
			Destroy(explosion, 5f);
		}
		Destroy(gameObject);
	}*/
}
