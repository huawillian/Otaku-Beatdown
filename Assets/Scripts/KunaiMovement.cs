using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody> ().velocity = transform.forward * 10;
	}
}
