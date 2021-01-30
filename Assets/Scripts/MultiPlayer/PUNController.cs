using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PUNController : MonoBehaviour
{
    [SerializeField] private string versionName = "0.1";
    [SerializeField] private GameObject connectPanel;

    [SerializeField] private InputField usernameInput;

    [SerializeField] private GameObject startButton;

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
        if (usernameInput.text.Length >= 3)
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
        connectPanel.SetActive(false);
        PhotonNetwork.playerName = usernameInput.text;
        JoinGame("Test");
    }

    public void CreateGame(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName, new RoomOptions() { maxPlayers = 5 }, null);
    }

    public void JoinGame(string roomName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.maxPlayers = 5;
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }


    public void OnJoinedRoom()
    {
        SpawnPlayer();
        connectPanel.SetActive(false);
    }

    public GameObject playerPrefabs;
    public void SpawnPlayer()
    {
        float randomValueX = Random.Range(-7.5f, 7.5f);
        float randomValueY = Random.Range(-3.5f, 3.5f);

        var player = PhotonNetwork.Instantiate(playerPrefabs.name, 
            new Vector2(randomValueX, randomValueY), Quaternion.identity, 0);
        if (PhotonNetwork.room.MasterClientId == PhotonNetwork.player.ID)
            player.GetComponent<ICharacterStateAble>().ChangeStateToBomber();
    }
}
