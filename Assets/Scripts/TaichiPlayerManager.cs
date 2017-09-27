using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class TaichiPlayerManager : Photon.PunBehaviour, IPunObservable
{
	[Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
	public static GameObject LocalPlayerInstance;
	[Tooltip("The current Health of our player")]
	public float Health = 1f;
	// [Tooltip("The Beams GameObject to control")]
	// public GameObject Beams;
	[Tooltip("The Player's UI GameObject Prefab")]
	public GameObject PlayerUiPrefab;
	// True, when the user is firing
	// Used to render the laser beams
	// private bool IsFiring;
	public GameObject ownPunch;
	public GameObject ownKick;
	public GameObject specialPunch;
	public GameObject leftPunch;
	public GameObject specialKick;

	private Animator animator;

	public bool hasBeenKicked = false;
	public bool hasBeenPunched = false;
	public bool isDead = false;

	private AudioSource audioSrc;
	public AudioClip punchSound;
	public AudioClip kickSound;
	public AudioClip health;
	public AudioClip throwSound;
	public AudioClip narutoSound;
	public AudioClip sasukeSound;
	public AudioClip madaraSound;
	public AudioClip hashiramaSound;
	public AudioClip ultimateSound;
	public float knockUpForce = 1000f;
	public float knockAwayForce = 1000f;
	public GameObject kunaiPrefab;
	public GameObject shurikenPrefab;
	public GameObject bombPrefab;

	private PlayerMovementMouse mvScript;

	public int kunaiNum = 3;
	public int shurikenNum = 1;
	public int bombNum = 1;
	public int ultimateNum = 1;

	public GameObject flamethrowerPrefab;
	public bool hitByFlame = false;

	void Awake()
	{
		audioSrc = GetComponent<AudioSource> ();
		animator = GetComponent<Animator>();
		mvScript = GetComponent<PlayerMovementMouse> ();
		if (!animator)
		{
			Debug.LogError("AttackAnimatorController is Missing Animator Component",this);
		}
		if (photonView.isMine) {
			TaichiPlayerManager.LocalPlayerInstance = this.gameObject;
		} else {
			// GetComponent<MyThirdPersonCharacter> ().enabled = false;
			// GetComponent<MyUserControls> ().enabled = false;
		}

		DontDestroyOnLoad(this.gameObject);
	}

	void Start()
	{
		if (PlayerUiPrefab!=null)
		{
			GameObject _uiGo =  Instantiate(PlayerUiPrefab) as GameObject;
			_uiGo.SendMessage ("SetTarget", this, SendMessageOptions.RequireReceiver);
		} else {
			Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.",this);
		}

	 	if (photonView.isMine)
	 	{
			Camera.main.gameObject.GetComponent<UnityStandardAssets.Utility.SmoothFollow> ().target = this.transform;
		}

		UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
		{
			if(this != null) {
				this.CalledOnLevelWasLoaded(scene.buildIndex);
			}
		};
	}

	void CalledOnLevelWasLoaded(int level)
	{
		// Create UI and follow this player
		GameObject _uiGo = Instantiate(this.PlayerUiPrefab) as GameObject;
		_uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
	}

	void Update()
	{
		if (photonView.isMine) {
			if (Input.GetKeyDown (KeyCode.Q)) {
				if (kunaiNum > 0) {
					photonView.RPC ("throwKunai", PhotonTargets.All, transform.position, transform.forward);
					kunaiNum--;
				}
			}

			if (Input.GetKeyDown (KeyCode.E)) {
				if (shurikenNum > 0) {
					photonView.RPC ("throwShuriken", PhotonTargets.All, transform.position, transform.forward);
					shurikenNum--;
				}
			}

			if (Input.GetKeyDown (KeyCode.F)) {
				if (bombNum > 0) {
					photonView.RPC ("throwBomb", PhotonTargets.All, transform.position, transform.forward);
					bombNum--;
				}
			}

			if (Input.GetKeyDown (KeyCode.R)) {
				if (ultimateNum > 0) {
					photonView.RPC ("spawnFlamethrower", PhotonTargets.All, transform.position, transform.forward);
					ultimateNum--;
				}
			}

			if (Input.GetKeyDown (KeyCode.Alpha1) && animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle")) {
				photonView.RPC ("narutoShout", PhotonTargets.All);
			}

			if (Input.GetKeyDown (KeyCode.Alpha2) && animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle")) {
				photonView.RPC ("sasukeShout", PhotonTargets.All);
			}

			if (Input.GetKeyDown (KeyCode.Alpha3) && animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle")) {
				photonView.RPC ("madaraShout", PhotonTargets.All);
			}

			if (Input.GetKeyDown (KeyCode.Alpha4) && animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle")) {
				photonView.RPC ("hashiramaShout", PhotonTargets.All);
			}

		}
	}

	[PunRPC]
	void narutoShout() {
		audioSrc.PlayOneShot (narutoSound);
		animator.Play ("Shout");
	}

	[PunRPC]
	void sasukeShout() {
		audioSrc.PlayOneShot (sasukeSound);
		animator.Play ("Shout");
	}

	[PunRPC]
	void madaraShout() {
		audioSrc.PlayOneShot (madaraSound);
		animator.Play ("Shout");
	}

	[PunRPC]
	void hashiramaShout() {
		audioSrc.PlayOneShot (hashiramaSound);
		animator.Play ("Shout");
	}

	[PunRPC]
	void throwKunai(Vector3 start, Vector3 dir) {
		animator.Play ("Throw");
		Instantiate (kunaiPrefab, start + dir + new Vector3 (0, 1, 0), transform.rotation * Quaternion.Euler(0, Random.Range(-12f, 12f),0));
		Instantiate (kunaiPrefab, start + dir + new Vector3 (0, 0.5f, 0), transform.rotation * Quaternion.Euler(0, Random.Range(-12f, 12f),0));
		Instantiate (kunaiPrefab, start + dir + new Vector3 (0, 1.5f, 0), transform.rotation * Quaternion.Euler(0, Random.Range(-12f, 12f),0));


		audioSrc.PlayOneShot (throwSound);
	}

	[PunRPC]
	void throwShuriken(Vector3 start, Vector3 dir) {
		animator.Play ("Throw");
		Instantiate (shurikenPrefab, start + dir * 2.2f + new Vector3(0, 1, 0), transform.rotation);
		audioSrc.PlayOneShot (throwSound);
	}

	[PunRPC]
	void throwBomb(Vector3 start, Vector3 dir) {
		animator.Play ("Throw");
		Instantiate (bombPrefab, start + dir + new Vector3(0, 0, 0), transform.rotation);
		audioSrc.PlayOneShot (throwSound);
	}

	[PunRPC]
	void spawnFlamethrower(Vector3 start, Vector3 dir) {
		animator.Play ("Shout");
		Instantiate (flamethrowerPrefab, start + dir + new Vector3(0, 1, 0), transform.rotation);
		audioSrc.PlayOneShot (ultimateSound);
	}
		
	IEnumerator deathRoutine() {
		isDead = true;

		GetComponent<Rigidbody> ().isKinematic = true;

		animator.StopPlayback ();
		animator.Play ("Death");

		yield return new WaitForSeconds (1f);

		if (photonView.isMine) {
			MyGameManager.Instance.LeaveRoom();
		}
	}

	void OnTriggerStay(Collider other) 
	{
		if (!photonView.isMine)
		{
			return;
		}

		if (other.name.Contains ("FlameThrowerEffect")) {
			if (hitByFlame)
				return;
			
			Health -= 0.015f;
			hitByFlame = true;
			StartCoroutine ("FlameTriggerReset");


			if (Health <= 0f && photonView.isMine && !isDead) {
				photonView.RPC ("death", PhotonTargets.All);
			} else if (!isDead){
				photonView.RPC ("down", PhotonTargets.All);
				GetComponent<Rigidbody>().velocity = Vector3.zero;
				mvScript.AddForce (new Vector3(0, knockUpForce * 0.75f, 0));
			}
		}
	}

	IEnumerator FlameTriggerReset() {
		yield return new WaitForSeconds (0.05f);
		hitByFlame = false;
	}

	void OnTriggerEnter(Collider other) 
	{
		if (!photonView.isMine)
		{
			return;
		}
			
		if (other.name.Contains ("Punch") && !GameObject.ReferenceEquals(other.gameObject, ownPunch) && !GameObject.ReferenceEquals(other.gameObject, leftPunch)) {
			if (hasBeenPunched)
				return;

			hasBeenPunched = true;
			StartCoroutine ("punchreset");

			Health -= 0.15f;

			if (Health <= 0f && photonView.isMine && !isDead) {
				photonView.RPC ("death", PhotonTargets.All);
			} else if (!isDead){
				photonView.RPC ("damage", PhotonTargets.All);
			}
		}

		if (other.name.Contains ("Special") && !GameObject.ReferenceEquals(other.gameObject, specialPunch) && !GameObject.ReferenceEquals(other.gameObject, specialKick)) {
			if (hasBeenPunched)
				return;

			hasBeenPunched = true;
			StartCoroutine ("punchreset");

			Health -= 0.35f;

			if (Health <= 0f && photonView.isMine && !isDead) {
				photonView.RPC ("death", PhotonTargets.All);
			} else if (!isDead) {
				photonView.RPC ("down", PhotonTargets.All);
				Vector3 dir = transform.forward * -1.0f * knockAwayForce;
				mvScript.AddForce (new Vector3(dir.x, knockUpForce, dir.y));
			}
		}

		if (other.name.Contains ("Kick") && !GameObject.ReferenceEquals(other.gameObject, ownKick) && !GameObject.ReferenceEquals(other.gameObject, specialKick)) {
			if (hasBeenKicked)
				return;

			hasBeenKicked = true;
			StartCoroutine ("kickreset");
			Health -= 0.1f;

			if (Health <= 0f && photonView.isMine && !isDead) {
				photonView.RPC ("death", PhotonTargets.All);
			} else if (!isDead) {
				photonView.RPC ("down", PhotonTargets.All);
				Vector3 dir = transform.forward * -1.0f * knockAwayForce;
				mvScript.AddForce (new Vector3(dir.x, knockUpForce, dir.y));
			}
		}

		if (other.name.Contains ("Health")) {
			audioSrc.PlayOneShot (health);
			Health = 1.0f;
		}

		if (other.name.Contains ("KunaPowerUp")) {
			audioSrc.PlayOneShot (health);
			kunaiNum += 2;
		}

		if (other.name.Contains ("ShurikePowerUp")) {
			audioSrc.PlayOneShot (health);
			shurikenNum += 1;
		}

		if (other.name.Contains ("BombPowerUp")) {
			audioSrc.PlayOneShot (health);
			bombNum += 1;
		}

		if (other.name.Contains ("UltimatePowerUp")) {
			audioSrc.PlayOneShot (health);
			ultimateNum += 1;
		}

		if (other.name.Contains ("Kunai")) {
			Health -= 0.03f;

			if (Health <= 0f && photonView.isMine && !isDead) {
				photonView.RPC ("death", PhotonTargets.All);
			} else if (!isDead){
				photonView.RPC ("damage", PhotonTargets.All);;
			}
		}

		if (other.name.Contains ("Shuriken")) {
			Health -= 0.05f;

			if (Health <= 0f && photonView.isMine && !isDead) {
				photonView.RPC ("death", PhotonTargets.All);
			} else if (!isDead){
				photonView.RPC ("down", PhotonTargets.All);
				Vector3 dir = transform.forward * -1.0f * knockAwayForce * 0.3f;
				mvScript.AddForce (new Vector3(dir.x, knockUpForce * 0.75f, dir.y));
			}
		}

		if (other.name.Contains ("Explosion")) {
			Health -= 0.2f;

			if (Health <= 0f && photonView.isMine && !isDead) {
				photonView.RPC ("death", PhotonTargets.All);
			} else if (!isDead){
				photonView.RPC ("down", PhotonTargets.All);
				mvScript.AddForce (new Vector3(0, knockUpForce * 2.0f, 0));
			}
		}
	}
		
	void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (Health);
		} else {
			float tempHealth = (float)stream.ReceiveNext ();

			if (tempHealth < this.Health) {
				photonView.RPC ("playPunchSound", PhotonTargets.All);
			}
			this.Health = tempHealth;
		}
	}

	[PunRPC]
	void playPunchSound() {
		if (Random.Range (0, 2) == 0) {
			audioSrc.PlayOneShot (punchSound);
		} else {
			audioSrc.PlayOneShot (kickSound);
		}
	}

	[PunRPC]
	void death() {
		StartCoroutine ("deathRoutine");
	}

	[PunRPC]
	void down() {
		animator.Play ("Down");
	}

	[PunRPC]
	void damage() {
		animator.Play ("Damage");
	}

	IEnumerator kickreset() {
		yield return new WaitForSeconds (1.0f);
		hasBeenKicked = false;
	}

	IEnumerator punchreset() {
		yield return new WaitForSeconds (0.05f);
		hasBeenPunched = false;
	}
}