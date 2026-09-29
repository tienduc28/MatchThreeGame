using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchFinder : MonoBehaviour
{
    private Board board;
    public List<Gem> currentMatches = new List<Gem>();
    private void Awake()
    {
        board = FindAnyObjectByType<Board>();
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
                        //Debug.Log($"Horizontal match found at ({x}, {y}) with type {currentGem.gemType}");
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
                        //Debug.Log($"Vertical match found at ({x}, {y}) with type {currentGem.gemType}");
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
            currentMatches = currentMatches.Distinct().ToList();
        }

        CheckForBombs();
    }

    public void CheckForBombs()
    {
        for (int i = 0; i < currentMatches.Count; i++)
        {
            Gem gem = currentMatches[i];

            int x = gem.posIndex.x;
            int y = gem.posIndex.y;

            if (gem.posIndex.x > 0)
            {
                if (board.allGems[x - 1, y] != null)
                {
                    if (board.allGems[x - 1, y].gemType == Gem.GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x - 1, y), board.allGems[x - 1, y]);
                    }
                }
            }

            if (gem.posIndex.x < board.width - 1)
            {
                if (board.allGems[x + 1, y] != null)
                {
                    if (board.allGems[x + 1, y].gemType == Gem.GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x + 1, y), board.allGems[x + 1, y]);
                    }
                }
            }

            if (gem.posIndex.y > 0)
            {
                if (board.allGems[x, y - 1] != null)
                {
                    if (board.allGems[x, y - 1].gemType == Gem.GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x, y - 1), board.allGems[x, y - 1]);
                    }
                }
            }

            if (gem.posIndex.y < board.height - 1)
            {
                if (board.allGems[x, y + 1] != null)
                {
                    if (board.allGems[x, y + 1].gemType == Gem.GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x, y + 1), board.allGems[x, y + 1]);
                    }
                }
            }
        }
    }

    public void MarkBombArea(Vector2Int bombPos, Gem theBomb)
    {
        for (int x = bombPos.x - theBomb.blastSize; x <= bombPos.x + theBomb.blastSize; x++)
        {
            for (int y = bombPos.y - theBomb.blastSize; y <= bombPos.y + theBomb.blastSize; y++)
            {
                if (x >= 0 && x < board.width && y >= 0 && y < board.height)
                {
                    if (board.allGems[x, y] != null)
                    {
                        board.allGems[x, y].isMatched = true;
                        currentMatches.Add(board.allGems[x, y]);
                    }
                }
            }
        }

        currentMatches = currentMatches.Distinct().ToList();
    }
}
