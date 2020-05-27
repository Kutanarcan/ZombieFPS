using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    PlayerController playerController;

    [SerializeField]
    KeyCode jumpKey;
    [SerializeField]
    KeyCode walkKey;
    [SerializeField]
    KeyCode crouchKey;

    public KeyCode WalkKey => walkKey;
    public KeyCode JumpKey => jumpKey;
    public KeyCode CrouchKey => crouchKey;
}
