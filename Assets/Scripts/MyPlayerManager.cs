using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Player manager. 
/// Handles fire Input and Beams.
/// </summary>
public class MyPlayerManager : Photon.PunBehaviour, IPunObservable
{
	[Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
	public static GameObject LocalPlayerInstance;
	[Tooltip("The current Health of our player")]
	public float Health = 1f;
	[Tooltip("The Beams GameObject to control")]
	public GameObject Beams;
	[Tooltip("The Player's UI GameObject Prefab")]
	public GameObject PlayerUiPrefab;
	// True, when the user is firing
	// Used to render the laser beams
	private bool IsFiring;

	void Awake()
	{
		// Check if beams prefab attached
		if (Beams == null)
		{
			Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
		}
		else
		{
			Beams.SetActive(false);
		}
		// #Important
		// If the player is the clients then set the Local Player Instnace
		// otherwise ignore bc the player spawned is for other clients
		if (photonView.isMine)
		{
			MyPlayerManager.LocalPlayerInstance = this.gameObject;
		}
		// #Critical
		// we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
		// Keep this player when we reload the scene
		// May not be necessary if we are not loading other scenes
		DontDestroyOnLoad(this.gameObject);
	}
		
	void Start()
	{
		// Use other Camera Work, following script
		ExitGames.Demos.DemoAnimator.CameraWork _cameraWork = this.gameObject.GetComponent<ExitGames.Demos.DemoAnimator.CameraWork>();
		// Check if UI exists, if so, then create and follow this player
		if (PlayerUiPrefab!=null)
		{
			GameObject _uiGo =  Instantiate(PlayerUiPrefab) as GameObject;
			_uiGo.SendMessage ("SetTarget", this, SendMessageOptions.RequireReceiver);
		} else {
			Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.",this);
		}
		// if camera work exists and this player is the local player then make camera follow this player
		if (_cameraWork != null)
		{
			if (photonView.isMine)
			{
				_cameraWork.OnStartFollowing();
			}
		}
		else
		{
			Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.",this);
		}
		// Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
		// May not need to do this if we are not loading a new scene
		UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
		{
			if(this != null) {
				this.CalledOnLevelWasLoaded(scene.buildIndex);
			}
		};
	}

	void CalledOnLevelWasLoaded(int level)
	{
		// check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
		if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
		{
			transform.position = new Vector3(0f, 5f, 0f);
		}

		// Create UI and follow this player
		GameObject _uiGo = Instantiate(this.PlayerUiPrefab) as GameObject;
		_uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
	}
		
	void Update()
	{
		// Process Inputs the player is the local player
		if (photonView.isMine) {
			ProcessInputs ();
		}

		// If player's health is less than 0, then leave the room
		// This check only happens on the local player otherwise this leave room may be called for other players
		// We can make this player die and respawn later
		if (Health <= 0f && photonView.isMine)
		{
			MyGameManager.Instance.LeaveRoom();
		}

		// trigger Beams active state 
		if (Beams != null && IsFiring != Beams.GetActive()) 
		{
			Beams.SetActive(IsFiring);
		}
	}
		
	void OnTriggerEnter(Collider other) 
	{
		if (!photonView.isMine)
		{
			return;
		}
		if (!other.name.Contains("Beam"))
		{
			return;
		}
		Health -= 0.1f;
	}
		
	void OnTriggerStay(Collider other) 
	{
		if (!photonView.isMine) 
		{
			return;
		}
		if (!other.name.Contains("Beam"))
		{
			return;
		}
		Health -= 0.1f*Time.deltaTime; 
	}

	/// <summary>
	/// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
	/// </summary>
	void ProcessInputs()
	{
		if (Input.GetButtonDown("Fire1")) 
		{
			IsFiring = true;
		}

		if (Input.GetButtonUp("Fire1")) 
		{
			IsFiring = false;
		}
	}

	void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			// We own this player: send the others our data
			stream.SendNext (IsFiring);
			stream.SendNext (Health);
		} else {
			// Network player, receive data
			this.IsFiring = (bool) stream.ReceiveNext();
			this.Health = (float)stream.ReceiveNext ();
		}
	}
}