using System;
using System.Collections.Generic;
using UnityEngine;

public enum EDirection 
{ 
    Null, 
    Right,
    Left, 
    Forward, 
    Backward 
}

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private float distanceToDetect = 100f;
    [SerializeField] private float moveSpeed = 10f;
    private EDirection directionValue = EDirection.Null;

    private Vector3 swipeStart, swipeEnd, direction;

    private bool isMouseDown = false;
    private bool isMoving = false;

    [SerializeField] private float raycastLength = 0.75f;
    private float brickCount = 0;

    private Stack<Transform> playerBricks = new Stack<Transform>();

    public LayerMask layerBrick, layerWall, layerBridge, layerFinish;

    public Transform brickPrefab, playerBrick, playerAvatar, spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        OnInit();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                swipeStart = Input.mousePosition;
                isMouseDown = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                swipeEnd = Input.mousePosition;
                directionValue = GetSwipe(swipeStart, swipeEnd);
                direction = GetDirection(directionValue);
                isMoving = true;
            }
        }
        else
        {
            PlayerMove();
            CheckColliderBrick();
            CheckColliderWall();
            CheckColliderBridge();
            CheckColliderFinish();
        }
    }

    private void PlayerMove()
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void OnInit()
    {
        //something something
    }

    private void SpawnPlayer(Vector3 spawnPoint)
    {
            
    }

    private EDirection GetSwipe(Vector3 swipeStart, Vector3 swipeEnd)
    {
        EDirection direction = EDirection.Null;

        float deltaX = swipeEnd.x - swipeStart.x;
        float deltaY = swipeEnd.y - swipeStart.y;

        if (Vector2.Distance(swipeStart, swipeEnd) <= distanceToDetect)
            direction = EDirection.Null;
        else
        {
            if (Mathf.Abs(deltaY) > Mathf.Abs(deltaX))
            {
                if (deltaY > 0)
                {
                    Debug.Log("Swipe Up!");
                    direction = EDirection.Forward;
                }
                else
                {
                    Debug.Log("Swipe Down!");
                    direction = EDirection.Backward;
                }
            }
            else if (Mathf.Abs(deltaY) < Mathf.Abs(deltaX))
            {
                if (deltaX > 0)
                {
                    Debug.Log("Swipe Right!");
                    direction = EDirection.Right;
                }
                else
                {
                    Debug.Log("Swipe Left!");
                    direction = EDirection.Left;
                }
            }
        }

        return direction;
    }

    private Vector3 GetDirection(EDirection directionValue)
    {
        switch (directionValue)
        {
            case EDirection.Left:
                return Vector3.left;
            case EDirection.Right:
                return Vector3.right;
            case EDirection.Forward:
                return Vector3.forward;
            case EDirection.Backward:
                return Vector3.back;
            default:
                return Vector3.zero;
        }
    }

    private void PlayerStopMove()
    {
        direction = Vector3.zero;
        isMoving = false;
    }

    private void PlayerAddBrick()
    {
        Transform newPlayerBrick = Instantiate(brickPrefab, playerBrick);
        newPlayerBrick.localPosition = brickCount * 0.25f * Vector3.up;
        playerBricks.Push(newPlayerBrick);

        playerAvatar.localPosition += Vector3.up * 0.25f;
    }

    private void PlayerRemoveBrick()
    {
        Destroy(playerBricks.Pop().gameObject);
        playerAvatar.localPosition -= Vector3.up * 0.25f;
    }
        
    private void CheckColliderBrick()
    {
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, raycastLength, layerBrick))
        {
            Destroy(hitInfo.collider.gameObject);
            brickCount++;
            PlayerAddBrick();
            Debug.Log("Hit Brick");
        }
    }

    private void CheckColliderWall()
    {
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, raycastLength, layerWall))
        {
            PlayerStopMove();
            Debug.Log("Hit Wall");
        }
    }

    private void CheckColliderBridge()
    {
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, raycastLength, layerBridge))
        {
            Debug.Log("Hit Bridge!");
            if (brickCount > 0)
            {
                BridgeController bridgeController = hitInfo.collider.gameObject.GetComponent<BridgeController>();
                if (bridgeController == null) return;
                if (!bridgeController.IsBuilt)
                {
                    bridgeController.SetBuilt();
                    brickCount--;
                    PlayerRemoveBrick();
                }
            }
            else
            {
                PlayerStopMove();
            }
        }
    }

    private void CheckColliderFinish()
    {
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, raycastLength, layerFinish))
        {
            Debug.Log("Win!");
            PlayerStopMove();
            GameManager.playerLevel++;
            GameManager.SavePlayerData();
            GameManager.winState = true;
        }
    }
}
