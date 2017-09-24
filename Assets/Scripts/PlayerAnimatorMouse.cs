using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorMouse : Photon.PunBehaviour {
	private Animator anim;
	private Rigidbody rb;

	public GameObject ownPunch;
	public GameObject ownKick;
	public GameObject specialPunch;
	public GameObject leftPunch;
	public GameObject specialKick;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo (0);

		if (info.IsName ("Base Layer.Punch1")) {
			leftPunch.SetActive (true);
			specialPunch.SetActive (false);
			ownPunch.SetActive (false);
			ownKick.SetActive (false);
			specialKick.SetActive (false);
		} else if (info.IsName ("Base Layer.Punch2")) {
			ownPunch.SetActive (true);
			ownKick.SetActive (false);
			leftPunch.SetActive (false);
			specialPunch.SetActive (false);
			specialKick.SetActive (false);
		} else if (info.IsName ("Base Layer.Punch3")) {
			ownPunch.SetActive (false);
			ownKick.SetActive (false);
			leftPunch.SetActive (false);
			specialPunch.SetActive (true);
			specialKick.SetActive (false);
		} else if (info.IsName ("Base Layer.Kick2")) {
			ownPunch.SetActive (false);
			ownKick.SetActive (true);
			leftPunch.SetActive (false);
			specialPunch.SetActive (false);
			specialKick.SetActive (false);
		} else if (info.IsName ("Base Layer.Kick1")) {
			ownPunch.SetActive (false);
			ownKick.SetActive (false);
			leftPunch.SetActive (false);
			specialPunch.SetActive (false);
			specialKick.SetActive (true);
		} else {
			ownPunch.SetActive (false);
			ownKick.SetActive (false);
			leftPunch.SetActive (false);
			specialPunch.SetActive (false);
			specialKick.SetActive (false);
		}

		if (!photonView.isMine)
			return;

		float xVel = rb.velocity.x;
		float zVel = rb.velocity.z;
		float horSpeed = xVel * xVel + zVel * zVel;
		anim.SetFloat ("Speed", horSpeed);

		if (info.IsName("Base Layer.Airborne"))
		{
			if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) 
			{
				photonView.RPC ("kickSpecial", PhotonTargets.All);
			}
		}

		if (info.IsName("Base Layer.Idle") || info.IsName("Base Layer.Run"))
		{
			if (Input.GetButtonDown("Fire2") && !(info.IsName("Base Layer.Damage"))) 
			{
				photonView.RPC ("kick", PhotonTargets.All);
			}                  
		}

		if (info.IsName("Base Layer.Idle") || info.IsName("Base Layer.Run"))
		{
			if (Input.GetButtonDown("Fire1") && !(info.IsName("Base Layer.Damage")) ) 
			{
				photonView.RPC ("punch", PhotonTargets.All);
			}                  
		}

		if (info.IsName("Base Layer.Punch1"))
		{
			if (Input.GetButtonDown("Fire1") && !(info.IsName("Base Layer.Damage"))) 
			{
				anim.SetTrigger("Punch2");
			}                  
		}

		if (info.IsName("Base Layer.Punch2"))
		{
			if (Input.GetButtonDown("Fire1") && !(info.IsName("Base Layer.Damage"))) 
			{
				anim.SetTrigger("Punch3");
			}                  
		}
			
		if (info.IsName ("Base Layer.Kick1") ||
		   info.IsName ("Base Layer.Kick2") ||
		   info.IsName ("Base Layer.Punch1") ||
		   info.IsName ("Base Layer.Punch2") ||
			info.IsName ("Base Layer.Punch3")) {
			GetComponent<PlayerMovementMouse> ().lockCtrl = true;
		} else {
			GetComponent<PlayerMovementMouse> ().lockCtrl = false;
		}

		if (info.IsName ("Base Layer.Kick2") ||
		    info.IsName ("Base Layer.Punch1") ||
		    info.IsName ("Base Layer.Punch2") ||
			info.IsName ("Base Layer.Punch3")) {
			rb.velocity = new Vector3 (rb.velocity.x * 0.92f, rb.velocity.y, rb.velocity.z * 0.92f);
		}

		if(info.IsName ("Base Layer.Punch3")) {
			anim.ResetTrigger ("Punch1");
		}
	}

	[PunRPC]
	void punch() {
		anim.Play ("Punch1");
	}

	[PunRPC]
	void kick() {
		anim.Play ("Kick2");
	}

	[PunRPC]
	void kickSpecial() {
		anim.Play ("Kick1");
	}

}
