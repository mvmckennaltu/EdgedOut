using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    [Tooltip("Player inputs")]
    PlayerInput PInput;
    [SerializeField]
    [Tooltip("Player Collider")]
    Collider coll;
    [SerializeField]
    [Tooltip("Player Transform")]
    
    Transform playerTransform;
    [SerializeField]
    [Tooltip("Raycast Emitters")]
    GameObject frontEmitter, backEmitter;

    [Header("Variables")]
    [SerializeField]
    [Tooltip("Player Speed")]
    float speed;

    [Tooltip("Colliders checking if there is a block inside")]
    bool UColl,DColl, FColl, FUColl, FDColl, FDDColl, BColl, BDColl, BDDColl;
    bool isMoving, canMove, canLedge, canGoDown, canGoUp, canGrab, grabbing, ledged;
    Block block;

    Vector3 direction = Vector3.forward, facing = Vector3.forward;
    private void Awake()
    {
        BlockCheckers.CheckedBlock += BlockContacted;
    }
    private void OnDestroy()
    {
        BlockCheckers.CheckedBlock -= BlockContacted;
    }

    private void BlockContacted(TypeOfChecker checker, bool value)
    {
        switch (checker)
        {
            case TypeOfChecker.Up:
                UColl = value;
                break;
            case TypeOfChecker.Front:
                FColl = value;
                break;
            case TypeOfChecker.FrontUpDiag:
                FUColl = value;
                break;
            case TypeOfChecker.FrontDownDiag:
                FDColl = value;
                break;
            case TypeOfChecker.FrontDownDownDiag:
                FDDColl = value;
                break;
            case TypeOfChecker.Back:
                BColl = value;
                break;
            case TypeOfChecker.BackDownDiag:
                BDColl = value;
                break;
            case TypeOfChecker.BackDownDownDiag:
                BDDColl = value;
                break;
        }
    }

    private void OnBasicMovement(InputValue value)
    {
        Vector2 temp = value.Get<Vector2>();
        direction = new(temp.x, 0, temp.y);
        isMoving = direction != Vector3.zero;
    }
   private void OnGrab()
    {
        
    }

    private void FixedUpdate()
    {
        if (isMoving)
            Move();   
    }

    private void Move()
    {
        
        bool facIsDir = (direction.z == facing.z && direction.x == facing.x);
        
        if (facIsDir)
        {
            canMove = !FColl && FDColl;
            canGoUp = !UColl && !FUColl && FColl;
            canGoDown = !FColl && !FDColl && FDDColl;
            canLedge = !FColl && !BColl && !BDColl && !BDDColl;
            canGrab = FColl;
            if (canMove) MoveForward();
            if (canGoUp) MoveUp();
            if (canGoDown) MoveDown();
            //if (canLedge) MoveLedge();
            //if (canGrab && grabbing) MoveGrab();
        }
        else
        {
            if (ledged) MoveToSideLedge();
            else TurnPlayer();
        }

        isMoving = false;
    }

    private void TurnPlayer()
    {
        if (!grabbing)
        {
            if (direction.x > 0)
            {
                facing = Vector3.right;
            }
            else if (direction.x < 0)
            {
                facing = Vector3.left;
            }
            else if (direction.z > 0)
            {
                facing = Vector3.forward;
            }
            else if (direction.z < 0)
            {
                facing = Vector3.back;
            }
        }
        

        // Rotate the player to face the new direction
        playerTransform.rotation = Quaternion.LookRotation(facing);
    }

    private void MoveToSideLedge()
    {
        if (direction.x != 0)
        {
            Vector3 targetPosition = playerTransform.position;
            targetPosition.x += direction.x;
            StartCoroutine(SmoothMove(targetPosition));
            ledged = false;
        }
    }

    private void MoveForward()
    {
        Vector3 targetPosition = new Vector3(
        Mathf.Round(playerTransform.position.x + facing.x),
        playerTransform.position.y,
        playerTransform.position.z + facing.z);
        if (targetPosition != playerTransform.position)
        {
            // Smoothly move the player to the target position
            StartCoroutine(SmoothMove(targetPosition));
        }
    }
    private void MoveUp()
    {
        Vector3 targetPosition = new Vector3(
            Mathf.Round(playerTransform.position.x + facing.x), playerTransform.position.y + 1, playerTransform.position.z + facing.z);
        if (targetPosition != playerTransform.position)
        {
            // Smoothly move the player to the target position
            StartCoroutine(SmoothMove(targetPosition));
        }
    }
    private void MoveDown()
    {
        Vector3 targetPosition = new Vector3(
           Mathf.Round(playerTransform.position.x + facing.x), playerTransform.position.y -1, playerTransform.position.z + facing.z);
        if (targetPosition != playerTransform.position)
        {
            // Smoothly move the player to the target position
            StartCoroutine(SmoothMove(targetPosition));
        }
    }
    private void MoveLedge()
    {
     /*   Vector3 targetPosition = playerTransform.position + facing;
        targetPosition.y = playerTransform.position.y;

        if (targetPosition != playerTransform.position)
        {
            StartCoroutine(SmoothMove(targetPosition));
        }*/
    }
    private void MoveGrab()
    {
        if (!BColl)
        {
            Vector3 targetPosition = playerTransform.position + direction;
            block.BlockGrabMove(direction);

            // Check if there is no ground beneath the player after moving back
            if (!DColl)
            {
                // Automatically ledge onto the ledge of the block
                Vector3 ledgePosition = playerTransform.position;
                ledgePosition.y = block.transform.position.y;
                StartCoroutine(SmoothMove(ledgePosition));
                ledged = true;
            }
        }

    }
    private IEnumerator SmoothMove(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        float duration = 0.2f; // Adjust the duration as needed for the desired smoothness

        Vector3 startPosition = playerTransform.position;

        while (elapsedTime < duration)
        {
            playerTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerTransform.position = targetPosition;
    }
}
