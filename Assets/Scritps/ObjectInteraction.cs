using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectInteraction : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float scaleSpeed = 0.5f;
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 2.0f;

    private bool isRotating = false;
    private bool isScaling = false;
    private Vector2 previousTouchPosition;
    private float initialDistance = 0f;
    private Vector3 initialScale;

    private void Start()
    {
        initialScale = transform.localScale;
    }

    private void Update()
    {
        // Avstå från interaktion om vi är över UI-element
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // Hantera touch-input
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isRotating = true;
                    previousTouchPosition = touch.position;
                    break;

                case TouchPhase.Moved:
                    if (isRotating)
                    {
                        Vector2 touchDelta = touch.position - previousTouchPosition;
                        transform.Rotate(Vector3.up, -touchDelta.x * rotationSpeed * Time.deltaTime);
                        previousTouchPosition = touch.position;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isRotating = false;
                    break;
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touch0.position, touch1.position);
                isScaling = true;
            }
            else if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                if (isScaling)
                {
                    float currentDistance = Vector2.Distance(touch0.position, touch1.position);
                    float scaleFactor = currentDistance / initialDistance;

                    Vector3 newScale = initialScale * scaleFactor;

                    // Begränsa skalan inom minimalt och maximalt värde
                    newScale = new Vector3(
                        Mathf.Clamp(newScale.x, minScale, maxScale),
                        Mathf.Clamp(newScale.y, minScale, maxScale),
                        Mathf.Clamp(newScale.z, minScale, maxScale)
                    );

                    transform.localScale = newScale;
                }
            }
            else if ((touch0.phase == TouchPhase.Ended || touch0.phase == TouchPhase.Canceled) ||
                     (touch1.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Canceled))
            {
                isScaling = false;
                initialScale = transform.localScale;
            }
        }
    }
}