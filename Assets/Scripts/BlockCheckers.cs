using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class BlockCheckers : MonoBehaviour
{
    public static event Action<TypeOfChecker, bool> CheckedBlock;

    [SerializeField] TypeOfChecker checkerType;
    private bool isTriggered = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Block") && !isTriggered)
        {
            Debug.Log($"Block entered {checkerType} checker");
            CheckedBlock(checkerType, true);
            isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Block"))
        {
            Debug.Log($"Block exited {checkerType} checker");
            CheckedBlock(checkerType, false);
            isTriggered = false;
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
    BackDownDownDiag
}