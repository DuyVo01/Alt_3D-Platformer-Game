using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    StateMachine stateMachine;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask whatIsGround;

    //Components
    public InputHandler inputHandler;
    public Rigidbody playerRB;
    public Transform cameraTransform;
    public CapsuleCollider playerCapsuleCollider;
    public Animator playerAnimator;
    public Material mat;
    public PlayerStatus playerStatus;

    //Ground relative and Rotation variables
    public float movementSpeed;
    public Vector3 movementDirection;
    public Vector3 slopeMovementDirection;
    public float rotationSpeed;

    //Dash variables
    public float dashSpeed;
    public int amountOfDashLeft;

    //Jump Variables
    public float jumpHeight;
    //public float holdingJumpHeight;
    public float fallJumpMultiplier;
    public float lowJumpMultiplier;
    public int amountOfJumpLeft;

    //Checking Variables
    public Vector3 currentVelocity;
    public bool isGrounded;
    public bool isOnPlatform;
    public bool isOnSlope;
    public bool animationFinished;
    public float groundCheckRadius;
    public RaycastHit slopeHit;

    //Floating
    public RaycastHit rayFloatHit;
    public float floatingDistance;
    public float characterOffsetY;

    //Child States variables
    public IdleState idleState;
    public MoveState moveState;
    public JumpState jumpState;
    public InAirState inAirState;
    public DashState dashState;
    public LandState landState;

    // Start is called before the first frame update
    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();

        playerRB = GetComponent<Rigidbody>();

        cameraTransform = Camera.main.transform;

        playerCapsuleCollider = GetComponent<CapsuleCollider>();

        playerAnimator = GetComponent<Animator>();

    }
    void Start()
    {
        stateMachine = new StateMachine();

        idleState = new IdleState(this, stateMachine, "Idle");

        moveState = new MoveState(this, stateMachine, "Moving");

        jumpState = new JumpState(this, stateMachine, "InAir");

        inAirState = new InAirState(this, stateMachine, "InAir");

        dashState = new DashState(this, stateMachine, "Dashing");

        landState = new LandState(this, stateMachine, "Landing");

        stateMachine.Initialize(idleState);

    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();

        OnSlope();

        MovementDirRelatedToCameraDir();

        SlopMoveDirection();

        currentVelocity = playerRB.velocity;

        stateMachine.LogicalUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicalUpdate();
    }

    public void MovementDirRelatedToCameraDir()
    {
        //Get the Vector axis values for direction from inputs and normalize them
        movementDirection = new Vector3(inputHandler.rawMovementInput.x, 0f, inputHandler.rawMovementInput.y);
        movementDirection.Normalize();

        //Modify the directions from the input to be relative to the camera heading direction
        movementDirection = movementDirection.x * cameraTransform.right.normalized + movementDirection.z * cameraTransform.forward.normalized;
        movementDirection.y = 0f;
    }

    public void SlopMoveDirection()
    {
        // get the slope direction by casting a vector project onto the ground based on
        // character heading direction
        slopeMovementDirection = Vector3.ProjectOnPlane(transform.forward, slopeHit.normal).normalized;
        Debug.DrawRay(slopeHit.point, slopeMovementDirection, Color.black);
    }

    public IEnumerator PlayerRotationT(Vector2 movementInput)
    {
        //Get the Y axis values of the based on the diretion inputs and camera Y rotation
        float targetAngle = Mathf.Atan2(movementInput.x, movementInput.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

        //Make the character to rotate to desire direction within a duration
        float timePassed = 0f;
        while(timePassed < 1f)
        {
            timePassed += Time.deltaTime * rotationSpeed;

            playerRB.MoveRotation(Quaternion.Lerp(transform.rotation, targetRotation, timePassed));

            yield return new WaitForEndOfFrame();
        }
    }

    public void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    public void OnSlope()
    {
        if (Physics.Raycast(transform.TransformPoint(playerCapsuleCollider.center), Vector3.down, out slopeHit, playerCapsuleCollider.height + 0.5f))
        {
            Debug.DrawRay(transform.TransformPoint(playerCapsuleCollider.center), slopeHit.normal, Color.red);
            if(slopeHit.normal != Vector3.up)
            {
                isOnSlope = true;
            }
            else
            {
                isOnSlope = false;
            }
        }
        else
        {
            isOnSlope = false;
        } 
    }

    public void FloatingCollider()
    {
        float rideHeight = floatingDistance;

        Ray downRay = new Ray(playerCapsuleCollider.bounds.center, Vector3.down);

        bool rayDidHit = Physics.Raycast(downRay, out rayFloatHit, rideHeight, whatIsGround);

        if (rayDidHit)
        {
            

            Debug.DrawRay(playerCapsuleCollider.bounds.center, downRay.direction, Color.yellow);

            float distanceToLift = playerCapsuleCollider.center.y * transform.localScale.y - rayFloatHit.distance;

            float amountToLift = distanceToLift * 50 - playerRB.velocity.y;

            Vector3 springForce = new Vector3(0f, amountToLift, 0f);
             
            playerRB.AddForce(springForce, ForceMode.VelocityChange);

            FootHolding();

        }  
    }

    public void FootHolding()
    {
        if (rayFloatHit.collider.CompareTag("MovingPlatform"))
        {
            Vector3 platformVel = rayFloatHit.transform.GetComponent<EnterPlatform>().velocity;

            platformVel.y = 0f;

            Vector3 velDif = platformVel - playerRB.velocity;

            playerRB.AddForce(new Vector3(velDif.x, 0, velDif.z) * 1.5f + movementDirection * movementSpeed * Time.deltaTime, ForceMode.VelocityChange) ;
        }
    }


    public void AnimationFinished()
    {
        animationFinished = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    
}
