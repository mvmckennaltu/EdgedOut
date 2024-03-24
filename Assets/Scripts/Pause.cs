using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    bool isPaused;
    private void OnPause()
    {
        Debug.Log("Paused");
    }
}
