using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class BlockCheckers : MonoBehaviour
{
    public static event Action<TypeOfChecker, bool> CheckedBlock;

    [SerializeField] TypeOfChecker checkerType;
    private bool isOnBlock = false;

    private void FixedUpdate()
    {
        // Check if the checker is on a block
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.lossyScale / 2f, transform.rotation, LayerMask.GetMask("Block"));
        bool newIsOnBlock = colliders.Length > 0;

        // If the collision state has changed, invoke the event
        if (newIsOnBlock != isOnBlock)
        {
            isOnBlock = newIsOnBlock;
            CheckedBlock?.Invoke(checkerType, isOnBlock);
        }
    }
}

public enum TypeOfChecker
{
    Up,
    Down,
    Front,
    FrontUpDiag,
    FrontDownDiag,
    FrontDownDownDiag,
    Back,
    BackDownDiag,
    BackDownDownDiag,
    UpLeft,
    UpRight,
    Left,
    Right,
    FrontUpLeft,
    FrontUpRight
}