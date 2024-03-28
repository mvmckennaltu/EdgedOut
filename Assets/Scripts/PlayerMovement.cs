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
        Vector2 temp = value.Get<Vector2>();
        direction = new(temp.x, direction.x , temp.y);
    }

    private void FixedUpdate()
    {
        Debug.Log(direction);
        if (direction == Vector3.zero)
        {
            transform.position += direction;
        }
    }
}
