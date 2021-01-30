using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PUNController : MonoBehaviour
{

    [SerializeField] private string versionName = "0.1";
    [SerializeField] private byte maxPlayerinRoom = 4;

    [SerializeField] private GameObject connectPanel;
    [SerializeField] private GameObject startButton;
    [SerializeField] private InputField usernameInput;
    [SerializeField] private Text pingText;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(versionName);
    }

    void Start()
    {
        connectPanel.SetActive(true);
    }

    private void Update()
    {
        pingText.text = "Ping : " + PhotonNetwork.GetPing();
    }

    private void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");
    }

    public void ChangeUserName()
    {
        if (usernameInput.text.Length >= 3 && PhotonNetwork.connectedAndReady)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
    }

    public void StartGame()
    {
        PhotonNetwork.playerName = usernameInput.text;
        usernameInput.interactable = false;
        startButton.SetActive(false);
        JoinGame("Test");
    }

    public void CreateGame(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName, new RoomOptions() { maxPlayers = maxPlayerinRoom }, null);
    }

    public void JoinGame(string roomName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.maxPlayers = maxPlayerinRoom;
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        usernameInput.interactable = true;
        startButton.SetActive(true);
    }

    public void OnJoinedRoom()
    {
        SpawnPlayer();
        connectPanel.SetActive(false);
    }

    [SerializeField] private GameObject playerPrefabs;
    [SerializeField] private Transform[] spwanPos;
    

    public void SpawnPlayer()
    {
        int id = PhotonNetwork.room.PlayerCount - 1;
        var player = PhotonNetwork.Instantiate(playerPrefabs.name, 
            spwanPos[id].position, Quaternion.identity, 0);

        if (PhotonNetwork.room.MasterClientId == PhotonNetwork.player.ID)
            player.GetComponent<ICharacterStateAble>().ChangeStateToBomber();
        
        player.GetComponent<OnlinePlayer>().ChangeAnimator(id);
    }
}
