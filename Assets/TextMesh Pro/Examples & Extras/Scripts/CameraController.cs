using UnityEngine;
using System.Collections;

namespace TMPro.Examples
{
    public class CameraController : MonoBehaviour
    {
        public Transform target; // The object to rotate around
        public float rotationSpeed = 2.0f; // The speed of rotation
        public float zoomSpeed = 2.0f; // The speed of zooming in/out

        private Vector3 lastMousePosition; // The position of the mouse on the last frame
        private float distanceFromTarget = 300.0f; // The initial distance from the target object
        private float minDistance = 110.0f; // The minimum distance from the target object
        private float maxDistance = 400.0f; // The maximum distance from the target object
        private float minCameraSize = 30.0f; // The minimum camera size
        private float maxCameraSize = 170.0f; // The maximum camera size

        void LateUpdate()
        {
            // Check if the user is pressing the mouse button
            if (Input.GetMouseButton(0))
            {
                // Get the current mouse position
                Vector3 currentMousePosition = Input.mousePosition;

                // Calculate the difference between the current and last mouse positions
                Vector3 deltaMousePosition = currentMousePosition - lastMousePosition;

                // Rotate the camera around the target object based on the mouse movement
                transform.RotateAround(target.position, Vector3.up, deltaMousePosition.x * rotationSpeed);
                transform.RotateAround(target.position, transform.right, -deltaMousePosition.y * rotationSpeed);

                // Update the last mouse position
                lastMousePosition = currentMousePosition;
            }
            else
            {
                // Update the last mouse position
                lastMousePosition = Input.mousePosition;
            }

            // Check if the user is scrolling the mouse wheel
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (Mathf.Abs(scroll) > 0.0f)
            {
                // Adjust the camera size based on the mouse scroll
                Camera.main.orthographicSize -= scroll * zoomSpeed;
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minCameraSize, maxCameraSize);

                // Adjust the distanceFromTarget based on the camera size to maintain the same apparent distance from the target
                float normalizedSize = (Camera.main.orthographicSize - minCameraSize) / (maxCameraSize - minCameraSize);
                distanceFromTarget = Mathf.Lerp(minDistance, maxDistance, normalizedSize);
            }

            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                distanceFromTarget -= (currentMagnitude - prevMagnitude) * 0.1f;
                distanceFromTarget = Mathf.Clamp(distanceFromTarget, minDistance, maxDistance);

                // Adjust the camera size based on distanceFromTarget to maintain the same apparent distance from the target
                float normalizedDistance = (distanceFromTarget - minDistance) / (maxDistance - minDistance);
                Camera.main.orthographicSize = Mathf.Lerp(minCameraSize, maxCameraSize, normalizedDistance);
            }

            // Update the camera position
            Vector3 direction = new Vector3(0, 0, -distanceFromTarget);
            Quaternion rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            transform.position = target.position + (rotation * direction);
        }
    }
}
