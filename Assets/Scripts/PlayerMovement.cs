using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    [Tooltip("Player inputs")]
    PlayerInput PInput;

    [Header("Variables")]
    [SerializeField]
    [Tooltip("Player Speed")]
    float speed;

    Vector3Int adjustedPlayerPos;
    Vector3 direction = Vector3.zero;

    private void Awake()
    {
        
    }

    private void OnBasicMovement(InputValue value)
    {
        direction = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        Debug.Log(direction);
        transform.position += direction;
    }
}
