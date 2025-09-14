using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public Vector2Int posIndex;
    public Board board;

    private Vector2 firstTouchPosition; // Store the first touch position for drag detection
    private Vector2 lastTouchPosition; // Store the last touch position for drag detection

    private bool mousePressed;
    private float swipeAngle; // Angle of the swipe
    private Gem otherGem; // Reference to the other gem being swapped

    private float swapSpeed = 10f; // Speed of the swap animation

    public enum GemType { Red, Blue, Green, Yellow, Purple }
    public GemType gemType; // Type of the gem

    public bool isMatched = false; // Flag to indicate if the gem is part of a match

    private Vector2Int previousPos; // Store the previous position for animation purposes
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Vector2.Distance(transform.position, posIndex) > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, posIndex, swapSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(posIndex.x, posIndex.y, 0f);
            board.allGems[posIndex.x, posIndex.y] = this; // Update the gem's position in the board's array
        }
        if (mousePressed && Input.GetMouseButtonUp(0))
        {
            lastTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the last touch position
            mousePressed = false; // Reset the mouse pressed flag

            CalculateAngle(); // Calculate the swipe angle
        }

    }

    public void SetupGem(Vector2Int pos, Board theBoard)
    {
        posIndex = pos;
        board = theBoard; // Set the board reference
    }

    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the initial touch position
        //Debug.Log(firstTouchPosition);
        mousePressed = true; // Set the mouse pressed flag to true
    }

    private void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(lastTouchPosition.y - firstTouchPosition.y, lastTouchPosition.x - firstTouchPosition.x) * Mathf.Rad2Deg;
        //Debug.Log($"Swipe Angle: {swipeAngle} degrees"); // Log the swipe angle for debugging

        if (Vector3.Distance(firstTouchPosition, lastTouchPosition) > 0.5f)
        {
            MovePieces();
        }
    }

    private void MovePieces()
    {
        previousPos = posIndex; // Store the previous position before moving

        if (swipeAngle > -45 && swipeAngle <= 45 && posIndex.x < board.width - 1) // Right swipe
        {
            //Debug.Log("Right Swipe");
            // Move right
            otherGem = board.allGems[posIndex.x + 1, posIndex.y];
            //Debug.Log(otherGem.posIndex.x);
            posIndex.x += 1;
            otherGem.posIndex.x -= 1;
            //Debug.Log(otherGem.posIndex.x);
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && posIndex.y < board.height - 1) // Up swipe
        {
            //Debug.Log("Up Swipe");
            // Move up
            otherGem = board.allGems[posIndex.x, posIndex.y + 1];
            posIndex.y += 1;
            otherGem.posIndex.y -= 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && posIndex.x > 0) // Left swipe
        {
            //Debug.Log("Left Swipe");
            // Move left
            otherGem = board.allGems[posIndex.x - 1, posIndex.y];
            posIndex.x -= 1;
            otherGem.posIndex.x += 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && posIndex.y > 0) // Down swipe
        {
            //Debug.Log("Down Swipe");
            // Move down
            otherGem = board.allGems[posIndex.x, posIndex.y - 1];
            posIndex.y -= 1;
            otherGem.posIndex.y += 1;
        }
        else
        {
            Debug.Log("Invalid Move");
            // Invalid move, do nothing or provide feedback
        }

        board.allGems[posIndex.x, posIndex.y] = this; // Update the gem's position in the board's array
        board.allGems[otherGem.posIndex.x, otherGem.posIndex.y] = otherGem; // Update the other gem's position in the board's array

        StartCoroutine(CheckMoveCo()); // Start the coroutine to check the move
    }

    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(0.5f); // Wait for the swap animation to complete
        board.matchFinder.FindAllMatch(); // Check for matches on the board
        if (otherGem != null)
        {
            if (!isMatched && !otherGem.isMatched)
            {
                //Debug.Log("No Match Found, Reverting Move");
                otherGem.posIndex = posIndex; // Revert the other gem's position
                posIndex = previousPos; // Revert this gem's position

                board.allGems[posIndex.x, posIndex.y] = this; // Update the gem's position in the board's array
                board.allGems[otherGem.posIndex.x, otherGem.posIndex.y] = otherGem; // Update the other gem's position in the board's array
            }
            else
            {
                board.DestroyAllMatches(); // Destroy all matched gems
            }
        }
    }
}
