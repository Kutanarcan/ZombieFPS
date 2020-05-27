using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    //[SerializeField]
    //float amount = 0.1f;
    //[SerializeField]
    //float maxAmount = 0.3f;
    //[SerializeField]
    //float smoothness = 6f;

    //Vector3 initPos;

    //private void Awake()
    //{
    //    initPos = transform.localPosition;
    //}

    //void Update()
    //{
    //    float moveX = -Input.GetAxis("Mouse X") * amount;
    //    float moveY = -Input.GetAxis("Mouse Y") * amount;

    //    moveX = Mathf.Clamp(moveX, -maxAmount, maxAmount);
    //    moveY = Mathf.Clamp(moveY, -maxAmount, maxAmount);

    //    Vector3 finalPosToMove = new Vector3(moveX, moveY, 0);
    //    transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosToMove + initPos, Time.deltaTime * smoothness);
    //}


    [SerializeField]
    float movementRotationBuildUpSpeed;

    PlayerMovement playerMovement;
    float movementRotationX = 0f;
    float movementRotationZ = 0f;

    const float ROTATION_AMOUNT = 3F;
    const float FIX_ROTATION_CLOSE_ZERO = 0.1F;
    const float ORIGINAL_POS = 0F;

    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    void LateUpdate()
    {
        if (!playerMovement.IsRunning)
            return;

        AsyncMovementWithPlayer();
    }

    void AsyncMovementWithPlayer()
    {
        if (playerMovement.MoveXAxis != 0)
        {
            movementRotationZ = Mathf.Lerp(movementRotationZ, -ROTATION_AMOUNT * playerMovement.MoveXAxis, movementRotationBuildUpSpeed * Time.deltaTime);
        }
        else if (playerMovement.MoveZAxis != 0)
        {
            movementRotationX = Mathf.Lerp(movementRotationX, ROTATION_AMOUNT * playerMovement.MoveZAxis, movementRotationBuildUpSpeed * Time.deltaTime);
        }

        if (playerMovement.MoveZAxis == 0 && playerMovement.MoveXAxis == 0)
        {
            movementRotationZ = Mathf.Lerp(movementRotationZ, ORIGINAL_POS, movementRotationBuildUpSpeed * Time.deltaTime);
            movementRotationX = Mathf.Lerp(movementRotationX, ORIGINAL_POS, movementRotationBuildUpSpeed * Time.deltaTime);

            FixRotationOnZero();
        }

        transform.localRotation = Quaternion.Euler(movementRotationX, ORIGINAL_POS, movementRotationZ);

        void FixRotationOnZero()
        {
            if (Mathf.Abs(movementRotationX) + Mathf.Abs(movementRotationZ) < FIX_ROTATION_CLOSE_ZERO)
            {
                movementRotationZ = ORIGINAL_POS;
                movementRotationX = ORIGINAL_POS;
            }
        }
    }
}
