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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
        //transform.position = new Vector3(pos.x, pos.y, 0f); // Set the position of the gem
        //gameObject.name = $"Gem {pos.x}, {pos.y}"; // Name the gem for easier identification
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
        if (swipeAngle > -45 && swipeAngle <= 45 && posIndex.x < board.width - 1) // Right swipe
        {
            Debug.Log("Right Swipe");
            // Move right
            otherGem = board.allGems[posIndex.x + 1, posIndex.y];
            posIndex.x += 1;
            otherGem.posIndex.x -= 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && posIndex.y < board.height - 1) // Up swipe
        {
            Debug.Log("Up Swipe");
            // Move up
           otherGem = board.allGems[posIndex.x, posIndex.y + 1];
            posIndex.y += 1;
            otherGem.posIndex.y -= 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && posIndex.x > 0) // Left swipe
        {
            Debug.Log("Left Swipe");
            // Move left
            otherGem = board.allGems[posIndex.x - 1, posIndex.y];
            posIndex.x -= 1;
            otherGem.posIndex.x += 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && posIndex.y > 0) // Down swipe
        {
            Debug.Log("Down Swipe");
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
    }
}
