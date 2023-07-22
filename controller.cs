using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    
    [SerializeField] private Transform camTransform;
    [SerializeField] private CharacterController charController;

    [SerializeField] private float cameraSensitivity;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveInputDeadZone;

    
    private int leftFingerId, rightFingerId;
    private float halfScreenWidth;

    
    private Vector2 lookInput;
    private float cameraPitch;

    
    private Vector2 moveTouchStartPosition;
    private Vector2 moveInput;

   
    void Start()
    {
       
        rightFingerId = -1;

        halfScreenWidth = Screen.width / 2;

        moveInputDeadZone = Mathf.Pow(Screen.height / moveInputDeadZone, 2);
    }

    
    void Update()
    {
        // Handles input
        GetTouchInput();


        if (rightFingerId != -1)
        {
           
            Debug.Log("Rotating");
            LookAround();
        }

        if (leftFingerId != -1)
        {
            
            Debug.Log("Moving");
            Move();
        }
    }

    void GetTouchInput()
    {
       
        for (int i = 0; i < Input.touchCount; i++)
        {

            Touch t = Input.GetTouch(i);

           
            switch (t.phase)
            {
                case TouchPhase.Began:

                    if (t.position.x < halfScreenWidth && leftFingerId == -1)
                    {
                       
                        leftFingerId = t.fingerId;

                       
                        moveTouchStartPosition = t.position;
                    }
                    else if (t.position.x > halfScreenWidth && rightFingerId == -1)
                    {
                        
                        rightFingerId = t.fingerId;
                    }

                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:

                    if (t.fingerId == leftFingerId)
                    {
                        
                        leftFingerId = -1;
                        Debug.Log("Stopped tracking left finger");
                    }
                    else if (t.fingerId == rightFingerId)
                    {
                       
                        rightFingerId = -1;
                        Debug.Log("Stopped tracking right finger");
                    }

                    break;
                case TouchPhase.Moved:

                   
                    if (t.fingerId == rightFingerId)
                    {
                        lookInput = t.deltaPosition * cameraSensitivity * Time.deltaTime;
                    }
                    else if (t.fingerId == leftFingerId)
                    {

                       
                        moveInput = t.position - moveTouchStartPosition;
                    }

                    break;
                case TouchPhase.Stationary:
   
                    if (t.fingerId == rightFingerId)
                    {
                        lookInput = Vector2.zero;
                    }
                    break;
            }
        }
    }

    void LookAround()
    {

        cameraPitch = Mathf.Clamp(cameraPitch - lookInput.y, -90f, 90f);
        camTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);

      
        transform.Rotate(transform.up, lookInput.x);
    }

    void Move()
    {

      
        if (moveInput.sqrMagnitude <= moveInputDeadZone) return;

        
        Vector2 movementDirection = moveInput.normalized * moveSpeed * Time.deltaTime;
        charController.Move(transform.right * movementDirection.x + transform.forward * movementDirection.y);
    }

}
