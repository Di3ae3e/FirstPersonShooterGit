using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float gravity = -19.62f;
    public Vector3 velocity;

    private CharacterController characterController;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        if (characterController.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }
}
