using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Vector3 originPosition;

    void Start()
    {
        originPosition = transform.position; 

    }

    void Update()
    {
        
    }

    public void ResetBox()
    {
        transform.position = originPosition;

    }

    // not hit -1; hit wall 0; hit another box 1;
    public int BoxHitInfo(Vector3 moveDirection)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, moveDirection, out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<Wall>() != null)
            {
                return 0;
            }
            else if (hit.transform.gameObject.GetComponent<Box>() != null)
            {
                return 1;
            }

        }
        return -1;


        
    }
        

}
