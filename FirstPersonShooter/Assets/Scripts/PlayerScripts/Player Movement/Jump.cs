using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField]
    private float jumpHeight = 2f;
    public static bool isGrounded = true;

    private Gravity gravity;

    [Header("ground check settings")]
    public Transform groundCheckTransform;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayerMask;

    private void Start()
    {
        gravity = GetComponent<Gravity>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheckTransform.position, groundCheckRadius, groundLayerMask);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            PlayerInertia.memYRotation = GetComponent<PlayerInertia>().orientation.rotation.eulerAngles.y;
            gravity.velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity.gravity);
        }
    }
}
