using UnityEngine;

public class Sliding : MonoBehaviour
{
    public static bool isSliding = false;
    public bool canSlam = true;

    private CharacterController characterController;
    private PlayerMove playerMove;
    private PlayerInertia inertia;
    private Gravity gravity;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerMove = GetComponent<PlayerMove>();
        inertia = GetComponent<PlayerInertia>();
        gravity = GetComponent<Gravity>();
    }

    private void Update()
    {
        if (Jump.isGrounded)
        {
            canSlam = true;
            gravity.gravity = -19.62f;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && !Jump.isGrounded && canSlam)
        {
            canSlam = false;
            gravity.gravity = gravity.gravity * 5;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && Jump.isGrounded)
        {
            isSliding = true;
            PlayerInertia.memYRotation = inertia.orientation.rotation.eulerAngles.y;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) && isSliding)
            isSliding = false; characterController.height = 2;

        if (isSliding == true)
            Slide();
    }
    void Slide()
    {
        characterController.height = 1;

        if (inertia.ZInputMemory == 0 && Jump.isGrounded && inertia.XInputMemory == 0)
            inertia.ZInputMemory = 1;

        Vector3 movement = (inertia.orientation.right * inertia.XInputMemory + inertia.orientation.forward * inertia.ZInputMemory).normalized;

        characterController.Move(movement * (playerMove.speed + 10) * Time.deltaTime);

    }
}
