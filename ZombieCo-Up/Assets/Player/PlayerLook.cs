using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    float mouseSensitivity = 250f;
    [SerializeField]
    Transform playerBody;

    public float RecoilAmount { get; set; } = 0f;
    public float RecoilRecoverAmount { get; set; } = 0f;
    public float MouseSensitivity { get => mouseSensitivity; set => mouseSensitivity = value; }

    float xRotation = 0f;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //transform.localRotation = Quaternion.Slerp(Quaternion.Euler(xRotation, 0f, 0f), Quaternion.Euler(xRotation + -RecoilAmount, 0f, 0f), 5f * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(xRotation + -RecoilAmount, 0f, 0f);
        //transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        if (RecoilAmount > 0f)
        {
            RecoilAmount -= Time.deltaTime * RecoilRecoverAmount;
        }
        else
            RecoilAmount = 0f;

        playerBody.Rotate(Vector3.up * mouseX);
    }

    //void Update()
    //{
    //    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
    //    float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

    //    xRotation -= mouseY;

    //    xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    //    transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

    //    playerBody.Rotate(Vector3.up * mouseX);
    //}
}
