using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width = 10; // Width of the board
    public int height = 10; // Height of the board

    public GameObject tileBgPrefab; // Prefab for the tile background

    public Gem[] gem; // Array to hold gem prefabs, if needed
    public Gem[,] allGems; // 2D array to hold all gems on the board

    public MatchFinder matchFinder;
    private void Awake()
    {
        matchFinder = FindObjectOfType<MatchFinder>();
    }
    // Start is called before the first frame update
    void Start()
    {
        allGems = new Gem[width, height]; // Initialize the 2D array to hold gems

        Setup();
        //cameraPosition.transform.position = new Vector2((float)(width - 1) / 2, (float)(height - 1) / 2); // Center the camera on the board
    }

    // Update is called once per frame
    private void Update()
    {
        matchFinder.FindAllMatch();
    }   
    private void Setup()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Instantiate the tile background prefab at the specified position
                GameObject tileBg = Instantiate(tileBgPrefab, new Vector2(x, y), Quaternion.identity);
                tileBg.transform.SetParent(transform); // Set the parent to the board
                tileBg.name = $"Tile {x}, {y}"; // Name the tile for easier identification

                int gemIndex = Random.Range(0, gem.Length); // Randomly select a gem prefab from the array

                int iterations = 0; // Safety counter to prevent infinite loops
                while (MatchesAt(new Vector2Int(x, y), gem[gemIndex]) && iterations < 10)
                {
                    gemIndex = Random.Range(0, gem.Length); // Re-select if it creates a match
                    iterations++;
                    if (iterations > 0)
                    {
                        Debug.Log(iterations);
                    }
                }
                SpawnGem(new Vector2Int(x, y), gem[gemIndex]);
            }
        }
    }

    private void SpawnGem(Vector2Int pos, Gem gemPrefab)
    {
        Gem gem = Instantiate(gemPrefab, new Vector3(pos.x, pos.y, 0f), Quaternion.identity);
        gem.transform.SetParent(transform); // Set the parent to the board
        gem.name = $"Gem {pos.x}, {pos.y}"; // Name the gem for easier identification

        allGems[pos.x, pos.y] = gem; // Store the gem in the 2D array
        gem.SetupGem(pos, this); // Initialize the gem with its position and board reference
    }

    bool MatchesAt(Vector2Int posToCheck, Gem gemToCheck)
    {
        if (posToCheck.x > 1)
        {
            if (allGems[posToCheck.x - 1, posToCheck.y].gemType == gemToCheck.gemType &&
                allGems[posToCheck.x - 2, posToCheck.y].gemType == gemToCheck.gemType)
            {
                return true;
            }
        }

        if (posToCheck.y > 1)
        {
            if (allGems[posToCheck.x, posToCheck.y - 1].gemType == gemToCheck.gemType &&
                allGems[posToCheck.x, posToCheck.y - 2].gemType == gemToCheck.gemType)
            {
                return true;
            }
        }
        return false;
    }

    private void DestroyMatchedGemAt(Vector2Int pos)
    {
        if (allGems[pos.x, pos.y] != null)
        {
            if (allGems[pos.x, pos.y].isMatched)
            {
                Destroy(allGems[pos.x, pos.y].gameObject);
                allGems[pos.x, pos.y] = null; // Clear the reference in the array
            }
        }
    }

    public void DestroyAllMatches()
    {
        for (int i = 0; i < matchFinder.currentMatches.Count; i++)
        {
            if (matchFinder.currentMatches[i] != null)
            {
                DestroyMatchedGemAt(matchFinder.currentMatches[i].posIndex);
            }                
        }
    }
}
