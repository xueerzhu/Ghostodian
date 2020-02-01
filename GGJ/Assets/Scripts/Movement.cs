using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Vector3 moveDirection;
    private bool  leftInput;
    private bool  rightInput;
    private bool  upInput;
    private bool  downInput;

    public float walkWait = 1f;
    public bool canWalk = true;

    public Vector3 gridPosition;

    void Start()
    {
        gridPosition = transform.position;
        moveDirection = new Vector3(1, 0, 0);
    }

    void Update()
    {
        leftInput = Input.GetButtonDown("Left");
        rightInput = Input.GetButtonDown("Right");
        upInput = Input.GetButtonDown("Up");
        downInput = Input.GetButtonDown("Down");

        if (leftInput)
        {
            moveDirection = new Vector3(-1, 0, 0);
        }
        else if (rightInput)
        {
            moveDirection = new Vector3(1, 0, 0);
        }
        else if (upInput)
        {
            moveDirection = new Vector3(0, 1, 0);
        }
        else if (downInput)
        {
            moveDirection = new Vector3(0, -1, 0);
        }


        Move(moveDirection);

        
    }

    IEnumerator WalkWait()
    {
        yield return new WaitForSeconds(walkWait);
        canWalk = true;
    }
    void Move(Vector3 direction)
    {
        if (canWalk)
        {
            canWalk = false;
            if (ShouldWrap())
            {
                gridPosition = RoundComponenttoInt(WrapTo());
            }
            else 
            {
                gridPosition += RoundComponenttoInt(moveDirection);
            }
            transform.position = gridPosition;  // TODO: smoothing
            StartCoroutine("WalkWait");
        }

        
    }

    bool ShouldWrap()
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position, moveDirection, Color.red);

        if (Physics.Raycast(transform.position, moveDirection, out hit, 1f))
        {
            hit.transform.gameObject.GetComponent<Wall>().isHit = true;
            return true;
        }

        return false;
    }

    Vector3 WrapTo()
    {
        RaycastHit hit;
        Vector3 wraptToPosition = new Vector3();
        if (Physics.Raycast(transform.position, -moveDirection, out hit, 20f))
        {
            hit.transform.gameObject.GetComponent<Wall>().isHit = true;
            wraptToPosition = (hit.point + moveDirection * 0.5f);
        }
        else
        {
            Debug.LogWarning("should wrap, but no wall detected");
        }
        //Debug.Log("wraptToPosition: " + wraptToPosition);
        return wraptToPosition;
        
    }

    Vector3 RoundComponenttoInt(Vector3 v3)
    {
        return new Vector3(Mathf.RoundToInt(v3.x), Mathf.RoundToInt(v3.y),Mathf.RoundToInt(v3.z));
    }

    
}
