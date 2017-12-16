using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {

	public GameObject Penguin;
	private float offset;


	// Use this for initialization
	void Start () {
		offset = transform.position.x - Penguin.transform.position.x;
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = new Vector3((Penguin.transform.position.x + offset), transform.position.y, transform.position.z);

	}
}
