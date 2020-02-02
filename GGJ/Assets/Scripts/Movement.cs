using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Vector3 moveDirection = new Vector3(1, 0, 0);
    private bool  leftInput;
    private bool  rightInput;
    private bool  upInput;
    private bool  downInput;

    public float walkWait = 1f;
    public bool canWalk = true;
    public bool shouldStop = false;

    public Vector3 gridPosition;


    public Vector3 originPosition;
    private Vector3 originMoveDirection;
    public bool shouldTravelwithBox =  false;
    public Box boxBeforeMe;

    void Start()
    {
        originPosition = transform.position; 
        originMoveDirection = moveDirection;
        gridPosition = transform.position;
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
            else if (shouldStop)
            {
                // stop the player
            }
            else
            {
                gridPosition += RoundComponenttoInt(moveDirection);

                if (shouldTravelwithBox)
                {
                    boxBeforeMe.transform.position += RoundComponenttoInt(moveDirection);
                }
            }
            transform.position = gridPosition;  // TODO: smoothing
            StartCoroutine("WalkWait");
        }

        
    }

    // raycast next hit object
    bool ShouldWrap()
    {
        shouldStop = false;
        shouldTravelwithBox = false;
        RaycastHit hit;

        Debug.DrawRay(transform.position, moveDirection, Color.red);

        if (Physics.Raycast(transform.position, moveDirection, out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<Wall>() != null)
            {
                hit.transform.gameObject.GetComponent<Wall>().isHit = true;
                return true;
            }
            else if (hit.transform.gameObject.GetComponent<Box>() != null) 
            {
                boxBeforeMe = hit.transform.gameObject.GetComponent<Box>();
                if (ShouldStopBeforeBox(boxBeforeMe))
                {
                    shouldStop = true;
                }
                else
                {
                    shouldTravelwithBox = true;
                }

            }   
        }
        return false;
    }

    // if hit a box, check should stop result
    bool ShouldStopBeforeBox(Box hitBox)
    {
        int boxHitInfo = hitBox.BoxHitInfo(moveDirection);
        if (boxHitInfo >= 0)  // 0 is wall, 1 is box
        {
            return true;
        }
        return false;
    }

    // raycast wrap position
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

    public void ResetPlayer()
    {
        transform.position = originPosition;
        gridPosition = originPosition;
        moveDirection = originMoveDirection;
        canWalk = true;
        shouldStop = false;
    }

    
}
