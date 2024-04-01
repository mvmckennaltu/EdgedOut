using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    bool UColl,DColl, FColl, FUColl, FDColl, FDDColl, BColl, BDColl, BDDColl,ULColl,URColl, LColl, RColl,FULColl,FURColl;
    bool isMoving, canMove, canLedge, canGoDown, canGoUp, canGrab, grabbing; 
        bool ledged = false;
    Block block;
    private List<Block> interactingBlocks = new List<Block>();

    Vector3 direction = Vector3.forward, facing = Vector3.forward;
    private void Awake()
    {
        BlockCheckers.CheckedBlock += BlockContacted;
        DColl = true;
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
            case TypeOfChecker.UpLeft:
                ULColl = value;
                break;
            case TypeOfChecker.UpRight:
                URColl = value;
                break;
            case TypeOfChecker.Left:
                LColl = value;
                break;
            case TypeOfChecker.Right:
                RColl = value;
                break;
            case TypeOfChecker.FrontUpLeft:
                FULColl = value;
                break;
            case TypeOfChecker.FrontUpRight:
                FURColl = value;
                break;
            case TypeOfChecker.Down:
                DColl = value;
                break;
        }
    }

    private void OnBasicMovement(InputValue value)
    {
        if(!Pause.isPaused)
        {
            Vector2 temp = value.Get<Vector2>();
            direction = new(temp.x, 0, temp.y);
            isMoving = direction != Vector3.zero;
        }
        
    }
   private void OnGrab()
    {
        if (FColl && !grabbing && !ledged)
        {
            grabbing = true;
            Debug.Log("Grabbed!");

        }
        
    }
    private void OnDrop()
    {
        if (ledged == true)
        {
            ledged = false;
        }
    }

    private void FixedUpdate()
    {
        if (isMoving && !grabbing)
            Move();

        if (!DColl && !ledged)
        {
            playerTransform.position = Vector3.MoveTowards(playerTransform.position, playerTransform.position - new Vector3(0, 2, 0), Time.deltaTime);
        }

        if (grabbing)
            MoveGrab();
    }

    private void Move()
    {
        
        bool facIsDir = (direction.z == facing.z && direction.x == facing.x);
        
        if (facIsDir)
        {
            canMove = !FColl && FDColl && !ledged;
            canGoUp = !UColl && !FUColl && FColl && !ledged;
            canGoDown = !FColl && !FDColl && FDDColl && !ledged;
            canLedge = !FColl && !FDColl && !FDDColl && !ledged;
            canGrab = FColl && !ledged;
            if (canMove) MoveForward();
            if (canGoUp) MoveUp();
            if (canGoDown) MoveDown();
            if (canLedge && direction != Vector3.zero) MoveLedge();
            if (canGrab && grabbing) MoveGrab();
        }
        else
        {
            if (ledged) MoveOnLedge();
            else TurnPlayer();
        }

        isMoving = false;
    }

    private void TurnPlayer()
    {
        Debug.Log("Turning!");
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
            playerTransform.rotation = Quaternion.LookRotation(facing);
        }
        

        // Rotate the player to face the new direction
        
    }

    private void MoveOnLedge() //handles movement when the player is on a ledge
    {
        Debug.Log("Moving on a ledge!");
        if (direction.x != 0)
        {
            Vector3 targetPosition = playerTransform.position;
            if (facing == Vector3.forward || facing == Vector3.back)
            {
                targetPosition.x += direction.x;
            }
            else
            {
                targetPosition.z += direction.z;
            }
            

            // Check if there is a block adjacent to the left or right
            if ((direction.x < 0 && (LColl && !ULColl)) || (direction.x > 0 && (RColl || !URColl)))
            {
                StartCoroutine(SmoothMove(targetPosition));
            }
            else
            {
                // Shuffle to the other side of the block
                targetPosition.z += facing.z;

                // Check if there is a block on the other side
                if (!FULColl && !FURColl)
                {
                    StartCoroutine(SmoothMove(targetPosition));
                }
            }
        }
        else if (direction.z < 0 && !UColl && !FUColl)
        {
            // Move back up from the ledge
            Debug.Log("Moving up from a ledge!");
            Vector3 targetPosition = playerTransform.position;
            targetPosition.y += .5f;
            StartCoroutine(SmoothMove(targetPosition));
            ledged = false;
            
        }
    }

    private void MoveForward()
    {
        Debug.Log("Moving forward!");
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
        Debug.Log("Going up!");
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
        Debug.Log("Going down!");
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
        Debug.Log("Ledging!");
        Vector3 targetPosition = playerTransform.position + facing;
        targetPosition.y = Mathf.Round(playerTransform.position.y) - 0.5f;

        if (targetPosition != playerTransform.position)
        {
            StartCoroutine(SmoothMove(targetPosition));
            ledged = true;
            facing = -facing;
            playerTransform.rotation = Quaternion.LookRotation(facing);
        }
    }
    private void MoveGrab()
    {
        if (interactingBlocks.Count > 0)
        {
            Block facingBlock = GetFacingBlock();

            if (facingBlock != null)
            {
                if (direction == -facing)
                {
                    // Pull the block
                    Vector3 targetPosition = playerTransform.position - facing;
                    facingBlock.BlockGrabMove(-facing);
                    StartCoroutine(SmoothMove(targetPosition));

                    // Check if there is no ground beneath the player after moving back
                    if (!DColl)
                    {
                        // Automatically ledge onto the ledge of the block
                        MoveLedge();
                        grabbing = false;
                    }
                }
                else if (direction == facing)
                {
                    // Push the block
                    facingBlock.BlockGrabMove(facing);
                    grabbing = false;
                }
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
            if (Time.timeScale > 0f)
            {
                playerTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
            }
            
            yield return null;
        }

        playerTransform.position = targetPosition;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Goal"))
            {
            if(LevelProgression.currentLevel == 1)
            {
                LevelProgression.level1Complete = true;
            }
            if(LevelProgression.currentLevel == 2)
            {
                LevelProgression.level2Complete = true;
            }
            if(LevelProgression.currentLevel == 3 )
            {
                LevelProgression.level3Complete = true;
            }
            //SceneManager.LoadScene("MainMenu");
        }
        if(other.CompareTag("Death"))
        {
            SceneManager.LoadScene("GameOver");
        }
        if (other.CompareTag("Face"))
        {
            Block block = other.GetComponentInParent<Block>();
            if (block != null && !interactingBlocks.Contains(block))
            {
                interactingBlocks.Add(block);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Face"))
        {
            Block block = other.GetComponentInParent<Block>();
            if (block != null && interactingBlocks.Contains(block))
            {
                interactingBlocks.Remove(block);
            }
        }
    }
    private Block GetFacingBlock()
    {
        foreach (Block block in interactingBlocks)
        {
            Vector3 directionToBlock = block.transform.position - playerTransform.position;
            float angle = Vector3.Angle(directionToBlock, facing);

            if (angle <= 45f) // Adjust the angle threshold as needed
            {
                return block;
            }
        }

        return null;
    }
}
