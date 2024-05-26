using UnityEngine;

public class ClipPrevent : MonoBehaviour
{
    public GameObject clipProjector;
    public float checkDistance;
    public Vector3 newDirection;

    float lerpPos;
    RaycastHit hit;

    private void Update()
    {
        if(Physics.Raycast(clipProjector.transform.position, clipProjector.transform.forward,out hit, checkDistance))
        {
            //get a percentage from 0 to max distance
            lerpPos = 1 - (hit.distance /  checkDistance);
        }
        else
        {
            //if we aren't hitting anything, set to 0
            lerpPos = 0;
        }

        Mathf.Clamp01(lerpPos);

        transform.localRotation = Quaternion.Lerp(Quaternion.Euler(Vector3.zero),Quaternion.Euler(newDirection),lerpPos);
    }
}
