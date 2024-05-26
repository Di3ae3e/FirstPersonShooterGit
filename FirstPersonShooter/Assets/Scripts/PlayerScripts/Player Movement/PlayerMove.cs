using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    protected CharacterController characterController;
    [SerializeField]
    public float speed = 12f;

    float x;
    float z;

    Vector3 movement;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        
        movement = transform.right.normalized * x + transform.forward.normalized * z;
        if(!Sliding.isSliding && Jump.isGrounded)
            characterController.Move(movement * speed * Time.deltaTime);
    }
}
