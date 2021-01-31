using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PUNController : MonoBehaviour
{

    [SerializeField] private string versionName = "0.1";
    [SerializeField] private byte maxPlayerInRoom = 4;

    [SerializeField] private GameObject connectPanel;
    [SerializeField] private GameObject startButton;
    [SerializeField] private InputField usernameInput;
    [SerializeField] private Text pingText;

    public static PUNController Controller { get; private set; }

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(versionName);
        Controller = this;
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
        PhotonNetwork.CreateRoom(roomName, new RoomOptions() { maxPlayers = maxPlayerInRoom }, null);
    }

    public void JoinGame(string roomName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.maxPlayers = maxPlayerInRoom;
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
    [SerializeField] private GameObject bomberPrefabs;
    [SerializeField] private int countPlayer;
    [SerializeField] private Button playButton;
    [SerializeField] private int idPlayer;
    [SerializeField] private GameObject player;

    public void SpawnPlayer()
    {
        idPlayer = PhotonNetwork.room.PlayerCount - 1;
        player = PhotonNetwork.Instantiate(playerPrefabs.name, 
            spwanPos[idPlayer].position, Quaternion.identity, 0);
        
        player.GetComponent<OnlinePlayer>().ChangeAnimator(idPlayer);

        if (PhotonNetwork.isMasterClient)
            playButton.gameObject.SetActive(true);
    }

    public void StartMatch()
    {
        if (PhotonNetwork.room.PlayerCount < 2)
            return;

        Debug.Log("Start");
        playButton.gameObject.SetActive(false);

        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.room.IsVisible = false;

        countPlayer = PhotonNetwork.room.PlayerCount + 1;
        playButton.gameObject.SetActive(false);

        GetComponent<PhotonView>().RPC("SetPositionPlayer", PhotonTargets.AllBuffered, null);

        NextMatch();
    }

    [PunRPC]
    public void SetPositionPlayer()
    {
        player.transform.position = spwanPos[idPlayer].position;
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.GetComponent<CharacterMovement>().DeathFunction = NextMatch;
        }
    }

    public void NextMatch()
    {
        countPlayer--;
        if (countPlayer == 1)
        {
            Debug.Log("You Win");
            PhotonNetwork.room.IsOpen = true;
            PhotonNetwork.room.IsVisible = true;

        }
        else
        {
            PhotonNetwork.InstantiateSceneObject(
                bomberPrefabs.name, RandomPosition(), Quaternion.identity, 0, null);
        }
    }

    public void GameOver()
    {
        // do something here
    }

    public Vector3 RandomPosition()
    {
        int idx = Random.Range(0, spwanPos.Length);
        return spwanPos[idx].position;
    }
}
