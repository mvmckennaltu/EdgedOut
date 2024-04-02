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
    public bool isMoveable;

    public bool grounded;

    private void FixedUpdate()
    {
        isOnEdge = CheckEdgeStatus();
        grounded = CheckGroundedStatus();

        if (isOnEdge && grounded)
            BlockBehavior();
        else
            BlockFall();
        if(isGrabbed && grounded)
        {

        }
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
    public void BlockGrabMove(Vector3 direction)
    {
        Debug.Log("BlockGrabMove called. Direction: " + direction);
        Vector3 targetPosition = rb.position + direction;

        // Check if there are any blocks in the pushing direction
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, 1f);
        List<Block> blocksToMove = new List<Block>();

        foreach (RaycastHit hit in hits)
        {
            Block block = hit.collider.GetComponent<Block>();
            if (block != null && block.isMoveable)
            {
                blocksToMove.Add(block);
            }
            else
            {
                // If there is an immovable block, stop pushing
                return;
            }
        }

        // Move the current block
        StartCoroutine(SmoothBlockMove(targetPosition));

        // Move any adjacent moveable blocks
        foreach (Block block in blocksToMove)
        {
            block.BlockGrabMove(direction);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            grounded = true;
        }
    }
    private IEnumerator SmoothBlockMove(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        float duration = 0.2f;

        Vector3 startPosition = rb.position;

        while (elapsedTime < duration)
        {
            rb.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.position = targetPosition;
    }
}

public enum TypeOfBlock
{
    Moveable,
    Immovable
}