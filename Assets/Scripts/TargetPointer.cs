using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetPointer : MonoBehaviour
{
    // Supposed to be attached on targeting arrow (or different image)
    [SerializeField] private GameObject target;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Canvas canvas;

    public enum PlayerScreen { left, right }
    [SerializeField] private PlayerScreen screen;
    private static readonly Vector3 viewportCenter = new Vector3(0.5f, 0.5f, 0);
    private Vector3 screenBottomLeft;
    private Vector3 screenTopRight;
    private Vector3 screenCenter;

    // Start is called before the first frame update
    void Start()
    {
        if (screen == PlayerScreen.left)
        {
            screenBottomLeft = new Vector3(0, 0, 0);
            screenTopRight = new Vector3(Screen.width / 2, Screen.height, 0);
        }
        else if (screen == PlayerScreen.right)
        {
            screenBottomLeft = new Vector3(Screen.width / 2, 0, 0);
            screenTopRight = new Vector3(Screen.width, Screen.height, 0);
        }
        else
        {
            Debug.LogError("NO PLAYER SCREEN ATTACHED TO SCRIPT!");
            return;
        }
        screenCenter = (screenBottomLeft + screenTopRight) / 2;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = GetScreenPosition();
        Color color = gameObject.GetComponent<Image>().color;
        if (IsTargetVisible(screenPos))
        {
            // We don't want to see pointer when target is on screen
            //gameObject.GetComponent<CanvasRenderer>().cull = true;
            gameObject.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0);
        }
        else
        {
            // Target is offscreen -> we want to see pointer
            //gameObject.GetComponent<CanvasRenderer>().cull = true;
            gameObject.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 255);

            // Screen position needs to be flipped, when target is behind us
            if (screenPos.z < 0)
            {
                screenPos = WorldToScreenSpace();
                screenPos *= -1;
            }
            else
            {
                screenPos = WorldToScreenSpace();
            }
            

            Vector3[] canvasPoints = new Vector3[4];
            canvas.GetComponent<RectTransform>().GetLocalCorners(canvasPoints);

            float xMin = canvasPoints[0].x * 0.8f;
            float xMax = canvasPoints[2].x * 0.8f;
            float yMin = canvasPoints[0].y * 0.8f;
            float yMax = canvasPoints[2].y * 0.8f;

            //POSITION
            if (screenPos.x <= xMin) screenPos.x = xMin;
            if (screenPos.x >= xMax) screenPos.x = xMax;
            if (screenPos.y <= yMin) screenPos.y = yMin;
            if (screenPos.y >= yMax) screenPos.y = yMax;

            screenPos.z = 0f;
            gameObject.transform.localPosition = screenPos;

            // ROTATION - not good for this case - rotating even in Z (3D)
            // Vector3 vectorToTarget = target.transform.position - gameObject.transform.position;
            // float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            // angle -= 90;
            // Quaternion newRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            // gameObject.transform.rotation = newRotation;

            float angle = Mathf.Atan2(screenPos.y, screenPos.x);
            angle -= 90 * Mathf.Deg2Rad;
            transform.localRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);

            // OLD - BUT FUNCTIONAL ON FIXED RES; ANGLE FUNCTIONAL IN 2D -> WANTED
            // // make 00 the center of player screen instead of bottom left
            // screenPos -= screenCenter;

            // // find angle from center of viewport(player screen) to mouse position
            // float angle = Mathf.Atan2(screenPos.y, screenPos.x);
            // angle -= 90 * Mathf.Deg2Rad;

            // float cos = Mathf.Cos(angle);
            // float sin = -Mathf.Sin(angle);

            // screenPos = screenCenter + new Vector3(sin * 150, cos * 150, 0);

            // // y = ax + b
            // float a = cos / sin;

            // Vector3 screenBounds = new Vector3(Screen.width / 4, Screen.height / 2, 0) * 1.8f;

            // if (cos > 0)
            // {
            //     screenPos = new Vector3(screenBounds.y / a, screenBounds.y, 0);
            // }
            // else
            // {
            //     screenPos = new Vector3(-screenBounds.y / a, -screenBounds.y, 0);
            // }

            // if (screenPos.x > screenBounds.x)
            // {
            //     screenPos = new Vector3(screenBounds.x, screenBounds.x * a, 0);
            // }
            // else if (screenPos.x < -screenBounds.x)
            // {
            //     screenPos = new Vector3(-screenBounds.x, -screenBounds.x * a, 0);
            // }

            // //screenPos += screenCenter;

            // transform.localPosition = screenPos;
            // transform.localRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);

            // ALTERNATIVE TO OLD (NOT TRIED) 
            // RectTransform rt = gameObject.GetComponent<RectTransform>();
            // rt.localPosition = screenPos;

            // float x = screenpos.x;
            // float y = screenpos.y;
            // float offset = 10;

            // if (screenpos.z < 0)
            // {
            //     screenpos = -screenpos;
            // }

            // if (screenpos.x > Screen.width)
            // {
            //     x = Screen.width - offset;
            // }
            // if (screenpos.x < 0)
            // {
            //     x = offset;
            // }

            // if (screenpos.y > Screen.height)
            // {
            //     y = Screen.height - offset;
            // }
            // if (screenpos.y < 0)
            // {
            //     y = offset;
            // }

            // OffScreenSprite.rectTransform.position = new Vector3(x, y, 0);

        }
    }

    private Vector3 GetViewportPosition()
    {
        Vector3 viewportPosition = playerCamera.WorldToViewportPoint(target.transform.position);
        return viewportPosition;
    }

    private Vector3 GetScreenPosition()
    {
        Vector3 screenPosition = playerCamera.WorldToScreenPoint(target.transform.position);
        return screenPosition;
    }

    private bool IsTargetVisibleView(Vector3 viewportPosition)
    {
        bool isTargetVisible = viewportPosition.z > 0 &&
                               viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
                               viewportPosition.y >= 0 && viewportPosition.y <= 1;
        return isTargetVisible;
    }

    private bool IsTargetVisible(Vector3 screenPosition)
    {
        bool isTargetVisible = screenPosition.z > 0 &&
                               screenPosition.x > screenBottomLeft.x && screenPosition.x < screenTopRight.x &&
                               screenPosition.y > screenBottomLeft.y && screenPosition.y < screenTopRight.y;
        return isTargetVisible;
    }

    private Vector3 WorldToScreenSpace()
    {
        Vector3 screenPoint = playerCamera.WorldToScreenPoint(target.transform.position);
        screenPoint.z = 0;

        Vector2 screenPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, playerCamera, out screenPos))
        {
            return screenPos;
        }

        return screenPoint;
    }
}
