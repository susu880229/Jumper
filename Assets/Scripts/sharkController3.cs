using UnityEngine;
using System.Collections;

public class sharkController3 : MonoBehaviour {

	Transform player;
	public float rotationSpeed=10f;
	Rigidbody2D rb2d;
	public float force = 7f;
	//public bool inWater;
	public float timer = 20f;
	public float rest = 0f;
	private Vector3 direction;
	public float awaySpeed = 3f;
	public float currentDis = 200f;
	public float ChaseSpeed = 3f;
	Animator anim;
	bool right = false;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Penguin").transform;

		rb2d = GetComponent<Rigidbody2D>();
		//inWater = true;
		anim = GetComponent<Animator>();

	
	}

	// Update is called once per frame
	// if use Time.deltatime, it will be excecute after certain amout of time
	void Update () {
		
		Debug.Log ("shark positon" + right);
		currentDis = Vector2.Distance (player.position, transform.position);
		if (currentDis <= 4) 
		{
			anim.enabled = true;
			if (transform.InverseTransformPoint (player.position).x > 0) 
			{
				anim.SetBool ("right", true);
			} 
			else if (transform.InverseTransformPoint (player.position).x < 0) 
			{
				anim.SetBool ("right", false);
			}
		} 
		else if (currentDis > 4) 
		{
			anim.enabled = false;
		}
		//Debug.Log("shark's position:" + transform.position.y);

		//start to aim and timing when approaching the player within 10f
		if (timer > 0) 
		{
			//rotate towards player
			facePlayer();
			if(currentDis <= 10f)
				timer -= Time.deltaTime;
				
		}
		//shark on the left
		if (transform.InverseTransformPoint(player.position).x > 0) 
		{
			right = true;
			anim.SetBool ("right", true);
			if (transform.localScale.x < 0) 
			{
				transform.localScale = new Vector3 (-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}

		} 
		//shark on the right
		else if (transform.InverseTransformPoint(player.position).x < 0) 
		{
			right = false;
			anim.SetBool ("right", false);
			if (transform.localScale.x > 0) 
			{
				transform.localScale = new Vector3 (-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}

		}

	
	}

	public void facePlayer()
	{
		direction = player.position - transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpeed);


	}
	void FixedUpdate()
	{
		//start chase the player
		if (timer > 0) 
		{
			//rb2d.AddForce (transform.right * force);
			if (transform.position.y <= -1f) 
			{
				rb2d.AddForce (Vector2.up * force);
			}
			//using moveTowards is better than only addforce to the shark
			transform.position = Vector2.MoveTowards(transform.position, player.transform.position, ChaseSpeed * Time.deltaTime);
		} 
		//start run away and refill the timer to prepare chase again later
		else if (timer <= 0 && rest <= 20f) 
		{
			//transform.Translate(-direction.normalized * awaySpeed * Time.deltaTime);
			rb2d.AddForce(Vector2.right * force);

			rest += Time.deltaTime;
			if (rest > 20f) 
			{
				rest = 0f;
				timer = 20f;
			}

		}
		/*
		if (transform.position.y >= 2) 
		{
			rb2d.velocity = Vector2.zero;
		}
		*/
		
	}




}
