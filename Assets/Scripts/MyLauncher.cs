using UnityEngine;

public class MyLauncher : Photon.PunBehaviour
{
	public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;

	[Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
	public byte MaxPlayersPerRoom = 20;
	[Tooltip("The Ui Panel to let the user enter name, connect and play")]
	public GameObject controlPanel;
	[Tooltip("The UI Label to inform the user that the connection is in progress")]
	public GameObject progressLabel;
	public GameObject controlsPanel;

	private string _gameVersion = "1";
	private bool isConnecting;

	void Awake()
	{
		// #Critical
		// we don't join the lobby. There is no need to join a lobby to get the list of rooms.
		PhotonNetwork.autoJoinLobby = false;
		// #Critical
		// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
		PhotonNetwork.automaticallySyncScene = true;
		// #NotImportant
		// Force LogLevel
		PhotonNetwork.logLevel = Loglevel;
	}
		
	void Start()
	{
		// Set Initial UI
		progressLabel.SetActive(false);
		controlPanel.SetActive(true);
		controlsPanel.SetActive (false);
	}

	public void ShowControls() {
		progressLabel.SetActive(false);
		controlPanel.SetActive(false);
		controlsPanel.SetActive (true);
	}

	public void HideControls() {
		progressLabel.SetActive(false);
		controlPanel.SetActive(true);
		controlsPanel.SetActive (false);
	}
		
	/// <summary>
	/// Start the connection process. 
	/// - If already connected, we attempt joining a random room
	/// - if not yet connected, Connect this application instance to Photon Cloud Network
	/// </summary>
	/// Connect() is called by the Play Button
	public void Connect()
	{
		// keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
		isConnecting = true;

		// Set Connecting UI
		progressLabel.SetActive(true);
		controlPanel.SetActive(false);

		// we check if we are connected or not, we join if we are , else we initiate the connection to the server.
		if (PhotonNetwork.connected)
		{
			// #Critical we need at this point to attempt joining a Random Room.
			// If it fails, we'll get notified in OnPhotonRandomJoinFailed() and we'll create one.
			PhotonNetwork.JoinRandomRoom();
		}
		else
		{
			// #Critical, we must first and foremost connect to Photon Online Server.
			PhotonNetwork.ConnectUsingSettings(_gameVersion);
		}
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("DemoAnimator/Launcher: OnConnectedToMaster() was called by PUN");
		// Check if Play was clicked
		// Sometimes when you quit a game scene you load Launcher but you are still connected to Photon
		if (isConnecting)
		{
			// #Critical: The first we try to do is to join a potential existing room.
			// If there is, good, else, we'll be called back with OnPhotonRandomJoinFailed()
			PhotonNetwork.JoinRandomRoom();
		}
	}
		
	public override void OnPhotonRandomJoinFailed (object[] codeAndMsg)
	{
		// #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
		PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("DemoAnimator/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
		// #Critical: We only load if we are the first player,
		// else we rely on  PhotonNetwork.automaticallySyncScene to sync our instance scene.
		// Other players will join this first player's scene
		if (PhotonNetwork.room.PlayerCount == 1)
		{
			Debug.Log("We load the 'Room for 1' ");
			PhotonNetwork.LoadLevel("Room for 1"); // <------ change this when done with scene
		}
	}

	public override void OnDisconnectedFromPhoton()
	{
		Debug.LogWarning("DemoAnimator/Launcher: OnDisconnectedFromPhoton() was called by PUN");   
		// Set UI to default
		progressLabel.SetActive(false);
		controlPanel.SetActive(true);
	}

}
