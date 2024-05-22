using UnityEngine;
public class Look : MonoBehaviour
{
    public float mouseSensivity = 100f; 
    [SerializeField]
    private Transform playerBody;
    [HideInInspector]
    static public float xRotation = 0;

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
