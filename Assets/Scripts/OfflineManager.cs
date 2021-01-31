using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject computerPrefab;
    [SerializeField] private GameObject bomberPrefabs;
    [SerializeField] private Transform[] spawnPosition;

    [SerializeField] private List<ICharacterStateAble> characters;

    private List<int> usedIndex = new List<int>();
    [SerializeField] private int maxPlayer = 4;
    [SerializeField] private int currentPlayer;

    public static OfflineManager Manager { get; private set; }

    private void Awake()
    {
        Manager = this;
    }

    public void SpawnCharacter()
    {
        Debug.Assert(maxPlayer <= spawnPosition.Length);

        // Create Player
        var character = Instantiate(playerPrefab, spawnPosition[RandomIndexPos()].position, Quaternion.identity);
        character.GetComponent<CharacterMovement>().ChangeAnimator(GameManager.Manager.playerSkins[0]);

        // Create Computer
        for (int i = 1; i < maxPlayer; i++)
        {
            character = Instantiate(computerPrefab, spawnPosition[RandomIndexPos()].position, Quaternion.identity);
            character.GetComponent<CharacterMovement>().ChangeAnimator(GameManager.Manager.playerSkins[i]);
        }

        currentPlayer = maxPlayer;

        // Create Bomber
        Instantiate(bomberPrefabs, spawnPosition[RandomIndexPos()].position, Quaternion.identity);
    }

    public void Start()
    {
        // Start Game
        SpawnCharacter();
    }

    public void NextMatch(bool isBomber = false)
    {
        // Check if is bomber die
        if(!isBomber)
            currentPlayer--;

        if(currentPlayer <= 1)
        {
            Debug.Log("Game Wins");
            // Do Somethings in here
            GameManager.Manager.GameOver();
            return;
        }

        // Create Computer
        int idx = Random.Range(0, spawnPosition.Length);
        Instantiate(bomberPrefabs, spawnPosition[idx].position, Quaternion.identity);
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        // Do Somethings in here
        GameManager.Manager.GameOver();
    }

    public int RandomIndexPos()
    {
        int idx = Random.Range(0, spawnPosition.Length);

        while(usedIndex.Contains(idx))
            idx = Random.Range(0, spawnPosition.Length);

        usedIndex.Add(idx);

        return idx;
    }
}
