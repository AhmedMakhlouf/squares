using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public class MainMenu : MonoBehaviourPunCallbacks
{
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 3;

    string gameVersion = "1";


    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private Button joinButton;
    [SerializeField] private TextMeshProUGUI joinButtonMessage;

    
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        StartCoroutine("TryConnect");
    }

    public void Connect()
    {
        message.text = string.Empty;
        joinButtonMessage.text = "JOINING...";

        PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions { MaxPlayers = maxPlayersPerRoom }, TypedLobby.Default);
    }

    public override void OnConnectedToMaster()
    {
        joinButtonMessage.text = "PLAY";
        joinButton.interactable = true;

    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        joinButtonMessage.text = "Disconnected";
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        joinButton.interactable = true;
        joinButtonMessage.text = "PLAY";

        this.message.text = "Room Full - Try Again Later";
    }

    public override void OnJoinedRoom()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(1);
    }

    public IEnumerator TryConnect()
    {
        while (true)
        {
            if(PhotonNetwork.IsConnected == false)
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }

            yield return new WaitForSeconds(2);
        }
    }
}
