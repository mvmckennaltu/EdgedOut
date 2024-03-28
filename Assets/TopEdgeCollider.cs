using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopEdgeCollider : MonoBehaviour
{
    [SerializeField]
    GameObject parentBlock;

    bool isOnEdge, isGrounded;

    public bool OnEdge => isOnEdge;
    public bool Grounded => isGrounded;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Block") && parentBlock != other.gameObject)
            if (parentBlock.transform.position.x == other.gameObject.transform.position.x || parentBlock.transform.position.z == other.gameObject.transform.position.z)
            {
                isOnEdge = true;
                isGrounded = other.GetComponent<Block>().grounded;
            }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Block") && parentBlock != other.gameObject)
        {
            isOnEdge = false;
            isGrounded = false;
        }
    }
}
