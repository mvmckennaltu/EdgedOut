using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class BlockCheckers : MonoBehaviour
{
    public static event Action<TypeOfChecker, bool> CheckedBlock;

    [SerializeField] TypeOfChecker checkerType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Block"))
            CheckedBlock(checkerType, true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Block"))
            CheckedBlock(checkerType, false);
    }
}

public enum TypeOfChecker
{
    Up,
    Front,
    FrontUpDiag,
    FrontDownDiag,
    Back,
    BackDownDiag,
    BackDownDownDiag
}