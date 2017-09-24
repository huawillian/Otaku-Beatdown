using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Player name input field. Let the user input his name, will appear above the player in the game.
/// </summary>
[RequireComponent(typeof(InputField))]
public class MyPlayerNameInputField : MonoBehaviour
{
	// Store the PlayerPref "Key"
	static string playerNamePrefKey = "PlayerName";

	void Start () {
		string defaultName = "";
		InputField _inputField = this.GetComponent<InputField>();

		if (_inputField!=null)
		{
			// Load the default name "Value" if already stored from previous session
			if (PlayerPrefs.HasKey(playerNamePrefKey))
			{
				defaultName = PlayerPrefs.GetString(playerNamePrefKey);
				_inputField.text = defaultName;
			}
		}

		// Set the NICKNAME for Photon
		PhotonNetwork.playerName =  defaultName;
	}
		
	// Sets the name of the player, and save it in the PlayerPrefs for future sessions.
	public void SetPlayerName(string value)
	{
		// #Important
		PhotonNetwork.playerName = value + " "; // force a trailing space string in case value is an empty string, else playerName would not be updated.
		PlayerPrefs.SetString(playerNamePrefKey,value);
		GetComponent<AudioSource> ().Play ();
	}

}
