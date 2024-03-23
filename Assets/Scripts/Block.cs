using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Rigidbody rb;
    private bool isOnEdge = false;
    private Transform edgeCenter;
    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag ("Top Edge"))
        {
            Edge();
            edgeCenter = GetComponentInChildren<Transform>();

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Top Edge"))
        {
            
        }
        isOnEdge = false;
    }
    void Edge()
    {
        isOnEdge = true;
        rb.useGravity = false;
        rb.isKinematic = true;
        
    }
}
