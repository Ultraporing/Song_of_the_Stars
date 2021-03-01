using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{

    public float maxZoom = 400f;
    public float minZoom = 120f;
    public float zoomStepSize = 40f;
    public float defaultZoom = 200f;
    public float mouseScrollSpeed = 2.0f;
    public float mousePanSpeed = 12.0f;
    public float keyboardScrollSpeed = 2.0f;
    public bool edgeScrolling = true;

    private float mousePosX;
    private float mousePosY;
    private float lastMousePosX;
    private float lastMousePosY;
    private float mousePosDeltaX;
    private float mousePosDeltaY;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        ///////////////////////
        // Keyboard Scrolling

        float translationX = Input.GetAxis("Horizontal") * keyboardScrollSpeed;
        float translationY = Input.GetAxis("Vertical") * keyboardScrollSpeed;
        float fastTranslationX = 2 * translationX;
        float fastTranslationY = 2 * translationY;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(fastTranslationX, fastTranslationY, 0);
        }
        else
        {
            transform.Translate(translationX,translationY, 0);
        }

        ///////////////////////
        // Mouse Scrolling

        mousePosX = Input.mousePosition.x;
        mousePosY = Input.mousePosition.y;


        int scrollDistance = 5;

        ///////////////////////
        // Paning

        if (Input.GetKey(KeyCode.Mouse2))
        {
            mousePosDeltaX = lastMousePosX - mousePosX;
            mousePosDeltaY = lastMousePosY - mousePosY;

            if (mousePosDeltaX > 4f)
            {
                transform.Translate(mousePanSpeed, 0, 0);
            }
            else if (mousePosDeltaX < -4f)
            {
                transform.Translate(-mousePanSpeed, 0, 0);
            }

            if (mousePosDeltaY > 4f)
            {
                transform.Translate(0, mousePanSpeed, 0);
            }
            else if (mousePosDeltaY < -4f)
            {
                transform.Translate(0, -mousePanSpeed, 0);
            }
        }
        else if (edgeScrolling)
        {

            // Horizontal camera movement
            // horizontal, left
            if (mousePosX < scrollDistance)
            {
                transform.Translate(-mouseScrollSpeed, 0, 0);
            }
            // horizontal, right
            else if (mousePosX >= Screen.width - scrollDistance)
            {
                transform.Translate(mouseScrollSpeed, 0, 0);
            }

            // Vertical camera movement
            // vertical, down
            if (mousePosY < scrollDistance)
            {
                transform.Translate(0, -mouseScrollSpeed, 0);
            }
            // vertical, up
            else if (mousePosY >= Screen.height - scrollDistance)
            {
                transform.Translate(0, mouseScrollSpeed, 0);
            }
        }


        ///////////////////////
        // Zooming

        // zoom in
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && Camera.main.orthographicSize > minZoom)
        {
            Camera.main.orthographicSize -= 1;
        }
        // zoom out
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && Camera.main.orthographicSize < maxZoom)
        {
            Camera.main.orthographicSize += 1;
        }
        // default zoom
        if (Input.GetKeyDown(KeyCode.C))
        {
            Camera.main.orthographicSize = defaultZoom;
        }

        lastMousePosX = mousePosX;
        lastMousePosY = mousePosY;
    }
}
