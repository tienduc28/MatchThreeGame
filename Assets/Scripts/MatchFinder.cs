using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MatchFinder : MonoBehaviour
{
    private Board board;
    public List<Gem> currentMatches = new List<Gem>();
    private void Awake()
    {
        board = FindObjectOfType<Board>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FindAllMatch()
    {
        currentMatches.Clear(); // Clear previous matches
        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y < board.height; y++)
            {
                Gem currentGem = board.allGems[x, y];
                if (currentGem == null) continue;
                // Check horizontal match
                if (x > 0 && x < board.width - 1)
                {
                    Gem leftGem = board.allGems[x - 1, y];
                    Gem rightGem = board.allGems[x + 1, y];
                    if (leftGem != null && rightGem != null &&
                        leftGem.gemType == currentGem.gemType &&
                        rightGem.gemType == currentGem.gemType)
                    {
                        Debug.Log($"Horizontal match found at ({x}, {y}) with type {currentGem.gemType}");
                        currentGem.isMatched = true;
                        leftGem.isMatched = true;
                        rightGem.isMatched = true;

                        currentMatches.Add(currentGem);
                        currentMatches.Add(leftGem);
                        currentMatches.Add(rightGem);
                    }
                }
                // Check vertical match
                if (y > 0 && y < board.height - 1)
                {
                    Gem topGem = board.allGems[x, y + 1];
                    Gem bottomGem = board.allGems[x, y - 1];
                    if (topGem != null && bottomGem != null &&
                        topGem.gemType == currentGem.gemType &&
                        bottomGem.gemType == currentGem.gemType)
                    {
                        Debug.Log($"Vertical match found at ({x}, {y}) with type {currentGem.gemType}");
                        currentGem.isMatched = true;
                        topGem.isMatched = true;
                        bottomGem.isMatched = true;

                        currentMatches.Add(currentGem);
                        currentMatches.Add(topGem);
                        currentMatches.Add(bottomGem);
                    }
                }
            }
        }

        if (currentMatches.Count > 0)
        {
            //foreach (Gem gem in currentMatches)
            //{
            //    Debug.Log($"Matched Gem at ({gem.posIndex.x}, {gem.posIndex.y}) of type {gem.gemType}");
            //}

            currentMatches = currentMatches.Distinct().ToList();
        }
    }
}
