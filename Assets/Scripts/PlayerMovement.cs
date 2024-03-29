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
    [Tooltip("Raycast Emitters")]
    GameObject frontEmitter, backEmitter;

    [Header("Variables")]
    [SerializeField]
    [Tooltip("Player Speed")]
    float speed;

    [Tooltip("Colliders checking if there is a block inside")]
    bool UColl, FColl, FUColl, FDColl, BColl, BDColl, BDDColl;
    bool isMoving, canMove, canLedge, canGoDown, canGoUp, canGrab, grabbing, ledged;

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
        isMoving = true;
    }

    private void FixedUpdate()
    {
        if (isMoving)
            Move();
    }

    private void Move()
    {
        canMove = !FColl && FDColl;
        canGoUp = !UColl && !FUColl && FColl;
        canGoDown = !FColl && !FDColl;
        canLedge = !BColl && !BDColl && !BDDColl;
        canGrab = FColl && !BColl && BDColl;
        bool facIsDir = (direction.z == facing.z && direction.x == facing.x);

        if (facIsDir)
        {
            if (canMove) MoveForward();
            if (canGoUp) MoveUp();
            if (canGoDown) MoveDown();
            if (canLedge) MoveLedge();
            if (canGrab && grabbing) MoveGrab();
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

    }

    private void MoveToSideLedge()
    {

    }

    private void MoveForward()
    {

    }
    private void MoveUp()
    {

    }
    private void MoveDown()
    {

    }
    private void MoveLedge()
    {

    }
    private void MoveGrab()
    {

    }
}
