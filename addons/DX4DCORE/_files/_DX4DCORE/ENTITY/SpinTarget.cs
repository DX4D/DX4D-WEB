using UnityEngine;

public class SpinTarget : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] float rotationSpeed = 10.0f;
    [SerializeField] float lerpSpeed = 1.0f;
    [SerializeField] bool rotateX = true;
    [SerializeField] bool rotateY = true;
    [SerializeField] bool rotateZ = true;

    private Vector3 speed = new Vector3();
    private Vector3 avgSpeed = new Vector3();
    private bool dragging = false;

    void OnMouseDown()
    {
        dragging = true;
    }

    void Update()
    {
        if (Input.GetMouseButton(1)) {
            speed = Vector3.zero; //Reset Speed Variable

            speed.x =  true ? (-Input.GetAxis("Mouse X")*1.0f) : -1.0f;
            //speed.y += Input.GetMouseButton(0) ? 1.0f : 0.0f; //Input.GetAxis("Mouse Y")
            //speed.z += Input.GetMouseButton(2) ? 1.0f : 0.0f;

            avgSpeed = DX4D.SmoothVector(avgSpeed, speed);
        } else {
            if (dragging)
            {
                speed = avgSpeed;
                dragging = false;
            }

            speed = DX4D.SmoothVector(speed, lerpSpeed);
        }

        speed = DX4D.LockRotation(speed, !rotateX, !rotateY, !rotateZ);
        DX4D.RotateGameObject(target, speed, rotationSpeed);
    }
}