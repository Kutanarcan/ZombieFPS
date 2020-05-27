using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float gravity = -9.81f;
    [SerializeField]
    float groundCheckRadius = 0.4f;
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    Transform groundCheckPoint;
    [SerializeField]
    float jumpHeight = 3f;

    [Space]
    [SerializeField]
    float slopeForce;
    [SerializeField]
    float slopeForceRayLength;

    [Space]
    [SerializeField]
    float runSpeed;
    [SerializeField]
    float walkSpeed;
    [SerializeField]
    float crouchSpeed;
    [SerializeField]
    float walkBuildUpSpeed;
    [SerializeField]
    float groundedBuildUpSpeed;
    [SerializeField]
    float crouchBuildUpSpeed;

    [Space]
    [SerializeField]
    AudioClip[] jumpSounds;
    [SerializeField]
    AudioClip[] landSounds;
    [SerializeField]
    AudioClip[] footStepSounds;

    public float MoveXAxis { get; private set; }
    public float MoveZAxis { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool IsSloping { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsRunning { get; private set; }

    float movementSpeed;
    float crouchHeigth = 1f;

    CharacterController characterController;
    Vector3 velocity;
    Vector3 crouchHeightVec => new Vector3(1f, crouchHeigth, 1f);
    Vector3 originalHeightVec;
    PlayerController playerController;
    AudioSource audioSource;

    const float PLAYER_ORIGINAL_HEIGHT_ON_Y = 1f;

    private void Awake()
    {
        originalHeightVec = new Vector3(1f, 1f, 1f);

        movementSpeed = runSpeed;
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        HandleMovementSpeed();
        HandleCrouch();

        IsGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayer);

        if (IsGrounded && velocity.y < 0f)
        {
            if (IsJumping)
            {
                movementSpeed = Mathf.Lerp(movementSpeed, 0f, Time.deltaTime * groundedBuildUpSpeed);
                int landSoundIndex = Random.Range(0, landSounds.Length);
                audioSource.PlayOneShot(landSounds[landSoundIndex]); ;
            }

            velocity.y = -2f;
            IsJumping = false;
            characterController.slopeLimit = 45f;
        }

        MoveXAxis = Input.GetAxisRaw("Horizontal");
        MoveZAxis = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * MoveXAxis + transform.forward * MoveZAxis;

        characterController.Move(move.normalized * movementSpeed * Time.deltaTime);

        if ((MoveXAxis != 0 || MoveZAxis != 0) && IsSloping)
            characterController.Move(Vector3.down * characterController.height / 2f * slopeForce * Time.deltaTime);

        HandleJump();

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }

    void LateUpdate()
    {
        FootStepSound();
    }

    public void HandleJump()
    {
        if (Input.GetKeyDown(playerController.PlayerInput.JumpKey) && !IsJumping)
        {
            int jumpSoundIndex = Random.Range(0, jumpSounds.Length);
            audioSource.PlayOneShot(jumpSounds[jumpSoundIndex]); ;

            characterController.slopeLimit = 90f;
            velocity.y = Mathf.Sqrt(jumpHeight - 2 * gravity);
            IsJumping = true;
        }
    }

    void HandleMovementSpeed()
    {
        if (!Input.GetKey(playerController.PlayerInput.CrouchKey))
        {
            ReturnOriginalScale();

            if (Input.GetKey(playerController.PlayerInput.WalkKey) && !IsJumping)
            {
                IsRunning = false;
                movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime * walkBuildUpSpeed);

            }
            else if (!Input.GetKey(playerController.PlayerInput.WalkKey))
            {
                IsRunning = true;
                movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, Time.deltaTime * walkBuildUpSpeed);
            }
        }
    }

    void HandleCrouch()
    {
        if (!Input.GetKey(playerController.PlayerInput.WalkKey))
        {
            if (Input.GetKey(playerController.PlayerInput.CrouchKey) && !IsJumping)
            {
                IsRunning = false;

                crouchHeigth = Mathf.Lerp(crouchHeigth, PLAYER_ORIGINAL_HEIGHT_ON_Y / 2f, crouchBuildUpSpeed * Time.deltaTime);
                movementSpeed = Mathf.Lerp(movementSpeed, crouchSpeed, Time.deltaTime * walkBuildUpSpeed);

                transform.localScale = crouchHeightVec;
            }
            else
            {
                IsRunning = true;
                ReturnOriginalScale();
            }
        }
    }

    void ReturnOriginalScale()
    {
        crouchHeigth = Mathf.Lerp(crouchHeigth, PLAYER_ORIGINAL_HEIGHT_ON_Y, crouchBuildUpSpeed * Time.deltaTime);

        transform.localScale = crouchHeightVec;
    }

    void FootStepSound()
    {
        if (IsGrounded && !audioSource.isPlaying && IsRunning && (Mathf.Abs(MoveXAxis) > 0 || Mathf.Abs(MoveZAxis) > 0f))
        {
            int footstepIndex = Random.Range(0, footStepSounds.Length);
            audioSource.PlayOneShot(footStepSounds[footstepIndex]);
        }
    }

    void OnSlope()
    {
        if (IsJumping)
        {
            IsSloping = false;
            return;
        }

        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, characterController.height / 2f * slopeForceRayLength))
        {
            if (hitInfo.normal != Vector3.up)
            {
                IsSloping = true;
                return;
            }
        }
        IsSloping = false;
    }
}
