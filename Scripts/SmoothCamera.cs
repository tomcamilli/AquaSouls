using UnityEngine;
 
public class SmoothCamera : MonoBehaviour 
{
    public Transform target;

    public float smoothSpeed;
    public Vector3 offset;

    //private Vector3 velocity = Vector3.zero;

    //private void Start()
    //{
    //	offset = camTransform.position - target.position;
    //}

    void LateUpdate()
    {
    	Vector3 desiredPosition = target.position + offset;
    	Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    	/*if (transform.position.x > smoothedPosition.x)
        {
            transform.position = new Vector3(transform.position.x -= 0.01f;
        }
        if (transform.position.y > smoothedPosition.y)
        {
            transform.position.y -= 0.01f;
        }
        if (transform.position.x < smoothedPosition.x)
        {
            transform.position.x += 0.01f;
        }
        if (transform.position.y < smoothedPosition.y)
        {
            transform.position.y += 0.01f;
        } */
        transform.position = smoothedPosition;

    	transform.LookAt(target);
    	//transform.position = Vector2.Lerp(transform.position, desiredPosition, Time.deltaTime);
    }
}