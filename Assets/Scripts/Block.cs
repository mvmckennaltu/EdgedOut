using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    TypeOfBlock blockType;
    [SerializeField]
    TopEdgeCollider[] topEdge;
    [SerializeField]
    BottomEdgeCollider[] bottomEdge;

    bool isOnEdge, isGrabbed;

    public bool grounded;

    private void FixedUpdate()
    {
        isOnEdge = CheckEdgeStatus();
        grounded = CheckGroundedStatus();

        if (isOnEdge && grounded)
            BlockBehavior();
        else
            BlockFall();
    }

    private bool CheckEdgeStatus()
    {
        foreach (TopEdgeCollider edge in topEdge) 
        {
            if (edge.OnEdge)
                return true;
        }
        foreach (BottomEdgeCollider edge in bottomEdge)
        {
            if (edge.OnEdge)
                return true;
        }
        return false;
    }
    private bool CheckGroundedStatus()
    {
        foreach (TopEdgeCollider edge in topEdge) 
        {
            if (edge.Grounded)
                return true;
        }
        foreach (BottomEdgeCollider edge in bottomEdge)
        {
            if (edge.Grounded)
                return true;
        }
        return false;
    }

    private void BlockBehavior()
    {
        if (rb.position.y > (int)rb.position.y + 0.75f)
            rb.position = new((int)rb.position.x, (int)rb.position.y + 1, (int)rb.position.z);
        else
            rb.position = new((int)rb.position.x, (int)rb.position.y, (int)rb.position.z);
    }
    private void BlockFall()
    {
        rb.position = Vector3.MoveTowards(rb.position, rb.position - new Vector3(0, 1, 0), Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            grounded = true;
        }
    }
}

public enum TypeOfBlock
{
    Moveable,
    Immovable
}