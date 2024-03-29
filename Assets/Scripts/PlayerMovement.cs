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

    Ray upHit, frontHit, upDiagHit, downDiagHit, backHit;
    bool canMove, canClimbUp, canClimbDown, canGrab, canLedge;

    Vector3Int adjustedPlayerPos;
    Vector3 direction = Vector3.forward;

    private void Awake()
    {
        
    }

    private void OnBasicMovement(InputValue value)
    {
        Vector2 temp = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        MovementRaycast();
    }

    void MovementRaycast()
    {
        CheckFront(out RaycastHit frontCast);
        frontCast.distance = 1;
        if (frontCast.collider != null)
        {
            upHit.origin = frontEmitter.transform.position;
            upHit.direction = Vector3.up;
            coll.Raycast(upHit, out RaycastHit upCast, 1);
            Debug.Log("Hit " + frontHit.GetPoint(1));
            //if (frontCast.collider.tag == "Block")
        }
        else
        {
            Debug.Log("Not Hit " + frontHit.GetPoint(1));
        }
    }

    RaycastHit CheckFront(out RaycastHit frontCast)
    {
        frontHit.origin = frontEmitter.transform.position;
        frontHit.direction = Vector3.forward;
        coll.Raycast(frontHit, out frontCast, 1);

        return frontCast;
    }
}
