using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public AudioSource boxPushAudio;
    public AudioSource boxBumpAudio;

    public GameObject ghostModel;
    public Vector3 moveDirection = new Vector3(1, 0, 0);
    private bool  leftInput;
    private bool  rightInput;
    private bool  upInput;
    private bool  downInput;

    public float walkWait = 1f;

    private bool canWalk = false;
    private bool shouldStop = false;

    private Vector3 gridPosition;


    private Vector3 originPosition;
    private Vector3 originMoveDirection;

    [SerializeField]
    private bool shouldTravelwithBox =  false;
    public Box boxBeforeMe;
    [SerializeField]
    private bool shouldStopBeforeWrapBox = false;

    void Start()
    {
        originPosition = transform.position; 
        originMoveDirection = moveDirection;
        gridPosition = transform.position;
        StartCoroutine("WalkWait");
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

        updateGhostFacingDirection();


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
            StartCoroutine("WalkWait");
            canWalk = false;
            if (ShouldWrap())
            {
                gridPosition = RoundComponenttoInt(WrapTo());
                
                if (shouldTravelwithBox)
                {
                    boxBeforeMe.transform.position += RoundComponenttoInt(moveDirection);
                    boxPushAudio.Play();
                }
            }
            else if (shouldStop || shouldStopBeforeWrapBox)
            {
                boxBumpAudio.Play();
            }
            else
            {
                gridPosition += RoundComponenttoInt(moveDirection);

                if (shouldTravelwithBox)
                {
                    boxBeforeMe.transform.position += RoundComponenttoInt(moveDirection);
                    boxPushAudio.Play();
                }
            }
            transform.position = gridPosition;  // TODO: smoothing
            
        }

        
    }

    // raycast next hit object
    bool ShouldWrap()
    {
        shouldStop = false;
        shouldTravelwithBox = false;
        shouldStopBeforeWrapBox = false;
        RaycastHit hit;

        Debug.DrawRay(transform.position, moveDirection, Color.red);

        if (Physics.Raycast(transform.position, moveDirection, out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<Wall>() != null)
            {
                
                Vector3 wrapToLocation = WrapTo();

                // is there a box at my wrapping location?
                RaycastHit isBoxThere;
                int layerMaskBox = 1 << 9;  // bit shift layer 9: box
                if (Physics.Raycast(wrapToLocation - moveDirection * 0.5f, moveDirection, out isBoxThere, 0.5f, layerMaskBox))
                {
                    boxBeforeMe = isBoxThere.transform.gameObject.GetComponent<Box>();
                    shouldStopBeforeWrapBox = ShouldStopBeforeBox(isBoxThere.transform.gameObject.GetComponent<Box>());
                    if (!shouldStopBeforeWrapBox)
                    {
                        shouldTravelwithBox = true;
                        hit.transform.gameObject.GetComponent<Wall>().isHit = true;
                        return true;
                    }
                
                }
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
        int layerMaskWall = 1 << 8;  // bit shift layer 8: wall
        if (Physics.Raycast(transform.position, -moveDirection, out hit, 20f, layerMaskWall))  // the opposite wall is hit
        {
            

            if (hit.transform.gameObject.GetComponent<Wall>() != null)
            {
                hit.transform.gameObject.GetComponent<Wall>().isHit = true;
                
                wraptToPosition = (hit.point + moveDirection * 0.5f);
            }
            
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
        StopAllCoroutines();
        transform.position = originPosition;
        gridPosition = originPosition;
        moveDirection = originMoveDirection;
        canWalk = true;
        shouldStop = false;
        
    }

    void updateGhostFacingDirection()
    {

        transform.LookAt(transform.position + moveDirection, -Vector3.forward);

        // Vector3 localForward = transform.right;
        // Quaternion targetRotation = Quaternion.FromToRotation(localForward, moveDirection) * transform.rotation;
        // transform.rotation = targetRotation;
    }

    
}
