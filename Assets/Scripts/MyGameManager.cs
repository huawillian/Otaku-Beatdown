using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;

public class MyGameManager : Photon.PunBehaviour {

	// Keep only one instance of Game Manager
	static public MyGameManager Instance;

	[Tooltip("The prefab to use for representing the player")]
	public GameObject playerPrefab;

	public GameObject playersText;
	public GameObject kunaiUI;
	public GameObject shurikenUI;
	public GameObject bombUI;
	public GameObject ultimateUI;

	void Start() {
		// Set instnace of Game Manager
		Instance = this;

		if (playerPrefab == null) 
		{
			Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
		} 
		else
		{
			// Instantiate the player if the player manager does not have a local player instnace
			if (TaichiPlayerManager.LocalPlayerInstance==null)
			{
				Debug.Log("We are Instantiating LocalPlayer");
				// we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
				// Instantiate the player prefab given the name, position, rotation, (bytegroup?)
				PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f,0.5f,0f), Quaternion.identity, 0);
			}
			else
			{
				Debug.Log("Ignoring scene load");
			}
		}
	}

	void LoadArena()
	{
		// Load Arena only if this client instnace is master
		if (!PhotonNetwork.isMasterClient) 
		{
			Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
			return;
		}

		// If we are client master then load the room
		// Because automaticallySyncScene is on this is load the room for all connected clients
		Debug.Log("PhotonNetwork : Loading Level : " + PhotonNetwork.room.PlayerCount);
		PhotonNetwork.LoadLevel("Room for "+PhotonNetwork.room.playerCount); // <--- Load Level here
	}

	// When local player leaves room
	// Load the Main Menu scene
	public override void OnLeftRoom()
	{
		SceneManager.LoadScene(0);
	}

	// When another (not local) player joins the room
	// If is master client then load a resized arena
	public override void OnPhotonPlayerConnected(PhotonPlayer other)
	{
		Debug.Log("OnPhotonPlayerConnected() " + other.NickName); // not seen if you're the player connecting

		// This may not be necessary if we dont want to reload the scene everytime someone joins
		if (PhotonNetwork.isMasterClient) 
		{
			Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected
			//LoadArena();
		}
	}

	// When another (not local) player leaves the room
	// If is master client then load a resized arena
	public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
	{
		Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName); // seen when other disconnects

		// This may not be necessary if we dont want to reload the scene everytime someone leaves
		if (PhotonNetwork.isMasterClient) 
		{
			Debug.Log("OnPhotonPlayerDisonnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected
			//LoadArena();
		}
	}
		
	// Called with the Leave Game Button
	public void LeaveRoom()
	{
		Debug.Log ("Trying to leave room");
		// Photon will disconnect the client
		// calling OnPhotonPlayerDisconnected on all other clients
		PhotonNetwork.LeaveRoom();
	}


	void Update() {

		string players = "";

		foreach(PhotonPlayer pl in PhotonNetwork.playerList)
		{
			players += pl.NickName + "\n";
		}

		playersText.GetComponent<Text> ().text = players;
		kunaiUI.GetComponent<Text> ().text = "x " + TaichiPlayerManager.LocalPlayerInstance.GetComponent<TaichiPlayerManager> ().kunaiNum;
		shurikenUI.GetComponent<Text> ().text = "x " + TaichiPlayerManager.LocalPlayerInstance.GetComponent<TaichiPlayerManager> ().shurikenNum;
		bombUI.GetComponent<Text> ().text = "x " + TaichiPlayerManager.LocalPlayerInstance.GetComponent<TaichiPlayerManager> ().bombNum;
		ultimateUI.GetComponent<Text> ().text = "x " + TaichiPlayerManager.LocalPlayerInstance.GetComponent<TaichiPlayerManager> ().ultimateNum;

	}
}
