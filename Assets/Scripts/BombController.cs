using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {
	
	public GameObject explosion;

	// Use this for initialization
	void Start () {
		StartCoroutine ("explosionCoroutine");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator explosionCoroutine() {
		yield return new WaitForSeconds (1.5f);
		GameObject.Instantiate (explosion, transform.position, Quaternion.identity);
		GameObject.Destroy (this.gameObject);
	}

	void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
		}
	}
}
