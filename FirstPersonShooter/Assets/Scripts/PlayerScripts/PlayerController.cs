using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float jumpHeight = 2f;
    [SerializeField]
    private float speed = 12f;
    [SerializeField]
    private float gravity = -9.81f;
    [SerializeField]
    private float slamForce = 5;
    //[SerializeField]
    //private float hookForce = 1;

    float x;
    float z;

    private CharacterController characterController;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private Vector3 velocity;// хз что это
    public bool canRun = true;
    private bool isGrounded = true;
    private bool canSlam = false;
    private bool isSliding = false;
    public GameObject cam;
    //костыль ебаный
    public int counter = 0;
    int slideCounter = 0;
    public Transform orientation;
    [SerializeField] float XInputMemory;
    [SerializeField] float ZInputMemory;
    [SerializeField] float memYRotation;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //Player input
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        if (isGrounded)
            gravity = -19.62f; 
        else orientation.eulerAngles = transform.eulerAngles; 

        if (isGrounded && !isSliding)
            canRun = true;
        if(!isGrounded || isSliding)
            orientation.rotation = Quaternion.Euler(0, memYRotation, 0);
        else
            orientation.eulerAngles = transform.eulerAngles;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            counter = 0;
            XInputMemory = x; ZInputMemory = z;
            orientation.eulerAngles = transform.eulerAngles;
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            canSlam = true;
            canRun = false;
            memYRotation = orientation.rotation.eulerAngles.y;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (Input.GetKeyDown(KeyCode.LeftShift) && canSlam && !isGrounded)
            Slam();
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
            Sliding(); 
        else if (canRun)
        {
            Run();
            characterController.height = 2;
            slideCounter = 0;
        }
    }

    void Run()
    {
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f; 

        cam.transform.localRotation = Quaternion.Euler(Look.xRotation, 0, -x);

        Vector3 movement = Vector3.zero;
        if (isGrounded)
            movement = transform.right * x + transform.forward * z;
        else
        {
            movement = orientation.right * XInputMemory + orientation.forward * ZInputMemory;
            Vector3 xz = transform.right * x / 3 + transform.forward * z / 3;
            movement += xz;
        }
        characterController.Move(movement * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }


    void Slam()//шлепнутьс€ об землю
    {
        gravity *= slamForce;
        canSlam = false;
    }

    void Sliding()
    {
        canRun = false;
        if (slideCounter == 0)
        {
            XInputMemory = x; ZInputMemory = z;
            orientation.eulerAngles = transform.eulerAngles;
            memYRotation = orientation.rotation.eulerAngles.y;
            slideCounter++;
        }
        orientation.rotation = Quaternion.Euler(0, memYRotation, 0);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (cam.GetComponent<Camera>().fieldOfView < 70)
            cam.GetComponent<Camera>().fieldOfView += Mathf.Abs(z) * 0.5f;
        //уменьшаем до нормы
        if (z == 0 && cam.GetComponent<Camera>().fieldOfView > 60)
            cam.GetComponent<Camera>().fieldOfView -= 0.5f;

        cam.transform.localRotation = Quaternion.Euler(Look.xRotation, 0, -x);
        if (ZInputMemory == 0 && XInputMemory == 0)
            ZInputMemory = 1;
        Vector3 movement = orientation.right * XInputMemory + orientation.forward * ZInputMemory;

        characterController.Move(movement * (speed + 10) * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);

        characterController.height = 1;
    }
}
