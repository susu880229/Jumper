using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour {
	
	public bool inWater = false;
	public float maxHspeedWater = 10f;
	public float maxVspeedWater = 10f;
	public float HforceWater = 10f;
	public float VforceWater = 5f;
	public float maxSpeedLand = 5f;
	public float forceLand = 2f;
	public Slider oxygenBar;
	GameObject oxygenBarColor;
	Rigidbody2D rb2d;
	//SpriteRenderer penguinSprite;
	float Distance;
	//GameObject Ocean;
	//private float OceanWidth;
	public float OxygenTimer;
	byte OxygenTrans;
	public Canvas finishUI;
	public Text disText_play;
	public Text disText_finish;
	GameObject shark;
	float shark_dis;
	bool freeze = true;
	bool facingRight = true;
	//public Sprite front;
	Animator anim;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		//penguinSprite = GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator> ();
		Distance = 0f;
		//Ocean = GameObject.FindWithTag ("Ocean");
		//OceanWidth = Ocean.GetComponent<SpriteRenderer>().bounds.size.x - 37f;
		disText_play.text = "0 m ";
		disText_finish.text = "0 m";
		oxygenBar.maxValue = 10;
		OxygenTimer = 10f;
		finishUI.gameObject.SetActive (false);
		oxygenBarColor = GameObject.FindWithTag ("fill");
		OxygenTrans = 255;
		shark = GameObject.FindWithTag ("shark");
		shark_dis = 200f;

	}
	
	// Update is called once per frame
	void Update () {
		
		//Debug.Log ("penguin's position" + transform.position.y);
		//Debug.Log("facingright" + facingRight);
		shark_dis = shark.GetComponent<sharkController3>().currentDis;
	    Distance += rb2d.velocity.x * Time.deltaTime;
		if (Distance < 0f) 
		{
			Distance = 0f;
		}


		disText_play.text = Distance.ToString("f0") + " " + "m";
		disText_finish.text = Distance.ToString ("f0") + " " +  "m";

		//track the oxygen status and display on the oxygen bubble
		if (inWater == true && OxygenTimer > 0) 
		{
			OxygenTimer -= Time.deltaTime;

		}
		//osygen increase when in air
		else if (inWater == false && OxygenTimer < 10) 
		{
			OxygenTimer += Time.deltaTime;

		}
		oxygenBar.value = (int)OxygenTimer;


		//change the tranparency of oxygenbar color to show the percentage of oxygen
		OxygenTrans = (byte)((OxygenTimer / 10) * 255);
		oxygenBarColor.GetComponent<Image> ().color = new Color32(255, 255, 255, OxygenTrans);
	
		//finish the game
		if (OxygenTimer <= 0 || shark_dis <= 2) 
		{
			if (freeze) 
			{
				Time.timeScale = 0;
			}

			finishUI.gameObject.SetActive (true);

		}


		//flip the penguin 
		if (Input.GetKey (KeyCode.RightArrow)) 
		{
			if (!facingRight) 
			{
				//anim.SetTrigger ("right");
				anim.SetBool("right_swim", true);
				facingRight = true;
			}

		}
			
		if (Input.GetKey (KeyCode.LeftArrow)) 
		{	
			if (facingRight) {
				//anim.SetTrigger ("left");
				anim.SetBool("right_swim", false);
				facingRight = false;
			}
		}


	}

	//when penguin jump into air and onto iceberg situation
	void FixedUpdate()
	{
		if (inWater == false) 
		{
			if (Input.GetKey (KeyCode.RightArrow)) 
			{
				moveRight (maxSpeedLand, forceLand);
			}

			//move left
			if (Input.GetKey (KeyCode.LeftArrow))
			{	

				moveLeft (maxSpeedLand, forceLand);

			}


		}


	}


	//enter into water
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Ocean")) 
		{
			inWater = true;
			//Physics2D.gravity = Vector2.zero;
		}
		else if (other.gameObject.CompareTag("rightBorder"))
		{
			reload();
		}


	}

	//stay in water
	void OnTriggerStay2D(Collider2D other) 
	{
		//even in the water to detect collider and inwater is true
		if (other.gameObject.CompareTag ("Ocean")) 
		{
			inWater = true;
			//Physics2D.gravity = Vector2.zero;
		}

		//Physics2D.gravity = Vector2.zero;
		if (Input.GetKey (KeyCode.UpArrow)) 
		{
			if (Mathf.Abs(rb2d.velocity.y) < maxVspeedWater)
				rb2d.AddForce (Vector2.up * HforceWater);

			else if (Mathf.Abs (rb2d.velocity.y) > maxVspeedWater)
				rb2d.velocity = new Vector2 ( rb2d.velocity.x, Mathf.Sign(rb2d.velocity.y) * maxVspeedWater);



		}

		if (Input.GetKey (KeyCode.DownArrow)) 
		{
			if (Mathf.Abs(rb2d.velocity.y) < maxVspeedWater)
				rb2d.AddForce (Vector2.down * HforceWater);

			else if (Mathf.Abs (rb2d.velocity.y) > maxVspeedWater)
				rb2d.velocity = new Vector2 ( rb2d.velocity.x, Mathf.Sign(rb2d.velocity.y) * maxVspeedWater);


		}

		//move right
		if (Input.GetKey (KeyCode.RightArrow)) 
		{
			
			moveRight (maxHspeedWater, HforceWater);
		}

		//move left
		if (Input.GetKey (KeyCode.LeftArrow))
		{	
			
			moveLeft (maxHspeedWater, HforceWater);
		}

	}

	//exit water 
	void OnTriggerExit2D(Collider2D other)
	{

		inWater = false;

	}



	void moveRight(float maxSpeed, float force)
	{
		if (Mathf.Abs(rb2d.velocity.x) < maxSpeed)
			rb2d.AddForce (Vector2.right * force);

		else if (Mathf.Abs (rb2d.velocity.x) > maxSpeed)
			rb2d.velocity = new Vector2 ( Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

	}

	void moveLeft(float maxSpeed, float force)
	{
		if (Mathf.Abs(rb2d.velocity.x) < maxSpeed)
			rb2d.AddForce (Vector2.left * force);

		else if (Mathf.Abs (rb2d.velocity.x) > maxSpeed)
			rb2d.velocity = new Vector2 (Mathf.Sign (rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);



	}



	public void reload()
	{
		//Time.timeScale = 1.0f;
		//SceneManager.LoadScene ("Antarctic");
		freeze = false;
		Time.timeScale = 1.0f;
		Application.LoadLevel(0);
	}

	public void quit()
	{
		Application.Quit();

	}


}
