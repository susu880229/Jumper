using UnityEngine;
using System.Collections;

public class icebergController : MonoBehaviour {
	
	public float targetScale = 0.75f;

	public float shrinkxSpeed = 0.1f;
	public float shrinkySpeed = 0.1f;
	//public float speedLimit = 0.1f;
	bool shrink = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame, so that is the reason to make the ice melt so quick without much impact from shrinkspeed
	void Update () {
		
	
	}

	//only happen within fixed amount of time
	void FixedUpdate()
	{
		if (shrink == true  ) 
		{
			if (transform.localScale.x > targetScale || transform.localScale.y > targetScale) 
			{
				transform.localScale -= new Vector3 (Time.deltaTime * shrinkxSpeed, Time.deltaTime * shrinkySpeed, 1);
			}


		}
	}


	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.gameObject.CompareTag ("Penguin")) 
		{
			//Destroy (gameObject);
			shrink = true;

		}

	}

	void OnTriggerExit2D(Collider2D other)
	{

		shrink = false;

	}
}
