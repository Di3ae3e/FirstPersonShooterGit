using UnityEngine;

public class PlayerInertia : MonoBehaviour
{
    public Transform orientation;
    [SerializeField] private float airControl = 15;
    [SerializeField] public float XInputMemory;
    [SerializeField] public float ZInputMemory;
    [SerializeField] public static float memYRotation;

    private CharacterController characterController;
    private PlayerMove playerMove;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerMove = GetComponent<PlayerMove>();
    }
    private void Update()
    {
        //moving player towards inertia direction
        if(!Jump.isGrounded)
        {
            Vector3 movement = orientation.right.normalized * XInputMemory + orientation.forward.normalized * ZInputMemory;

            if (XInputMemory < -1) XInputMemory = -1;
            if (XInputMemory > 1) XInputMemory = 1;
            if (ZInputMemory < -1) ZInputMemory = -1;
            if (ZInputMemory > 1) ZInputMemory = 1;

            XInputMemory += Input.GetAxis("Horizontal") / airControl;
            ZInputMemory += Input.GetAxis("Vertical") / airControl;
            characterController.Move(movement * playerMove.speed * Time.deltaTime);
        }
        //getting input
        if (Jump.isGrounded && !Sliding.isSliding) 
        {
            XInputMemory = Input.GetAxisRaw("Horizontal");
            ZInputMemory = Input.GetAxisRaw("Vertical");
        }
        //rotating to seted direction
        if (!Jump.isGrounded || Sliding.isSliding)
        {
            orientation.rotation = Quaternion.Euler(0, memYRotation, 0);
        }
        else // nullifying rotation
            orientation.eulerAngles = transform.eulerAngles;
    }
}
