using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Direction { Left, Right, Forward, Backward, Null }

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float distanceToDetect = 150f;
    private Vector2 swipeStart, swipeEnd;
    private bool isMouseDown = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMouseDown == false && Input.GetMouseButtonDown(0))
        {
            swipeStart = Input.mousePosition;
            isMouseDown = true;
        }

        if (isMouseDown == true && Input.GetMouseButtonUp(0))
        {
            swipeEnd = Input.mousePosition;
            if (isMouseDown)
                GetSwipeDirection(swipeStart, swipeEnd);
            isMouseDown = false;
        }
    }

    private Direction GetSwipeDirection(Vector2 swipeStart, Vector2 swipeEnd)
    {
        Direction direction = Direction.Null;

        float deltaX = swipeEnd.x - swipeStart.x;
        float deltaY = swipeEnd.y - swipeStart.y;

        if (Vector2.Distance(swipeStart, swipeEnd) <= distanceToDetect)
            direction = Direction.Null;
        else
        {
            if (Mathf.Abs(deltaY) > Mathf.Abs(deltaX))
            {
                if (deltaY > 0)
                {
                    Debug.Log("Swipe Up!");
                    direction = Direction.Forward;
                }
                else
                {
                    Debug.Log("Swipe Down!");
                    direction = Direction.Backward;
                }
            }
            else if (Mathf.Abs(deltaY) < Mathf.Abs(deltaX))
            {
                if (deltaX > 0)
                {
                    Debug.Log("Swipe Right!");
                    direction = Direction.Right;
                }
                else
                {
                    Debug.Log("Swipe Left!");
                    direction = Direction.Left;
                }
            }
        }

        return direction;
    }
}
