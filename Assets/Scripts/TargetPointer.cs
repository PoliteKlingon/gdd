using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetPointer : MonoBehaviour
{
    // Supposed to be attached on targeting arrow (or different image)
    [SerializeField] private GameObject target;
    [SerializeField] private Camera playerCamera;

    public enum PlayerScreen {left, right}
    [SerializeField] private PlayerScreen screen;
    private static readonly Vector3 viewportCenter = new Vector3(0.5f, 0.5f, 0);
    private Vector3 screenBottomLeft;
    private Vector3 screenTopRight;
    
    // Start is called before the first frame update
    void Start()
    {
        if (screen == PlayerScreen.left)
        {
            screenBottomLeft = new Vector3(0, 0, 0);
            screenTopRight = new Vector3(Screen.width/2, Screen.height, 0);
        }
        else if (screen == PlayerScreen.right)
        {
            screenBottomLeft = new Vector3(Screen.width/2, 0, 0);
            screenTopRight = new Vector3(Screen.width, Screen.height, 0);
        }
        else
        {
            Debug.LogError("NO PLAYER SCREEN ATTACHED TO SCRIPT!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewportPosition = GetViewportPosition();
        Color color = gameObject.GetComponent<Image>().color; 
        if (IsTargetVisible(viewportPosition))
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

            // Viewport position needs to be flipped, when target is behind us
            if (viewportPosition.z < 0)
            {
                viewportPosition *= -1;
            }

            // make 00 the center of viewport(player screen) instead of bottom left
            viewportPosition -= viewportCenter;

            // find angle from center of viewport(player screen) to mouse position
            float angle = Mathf.Atan2(viewportPosition.y, viewportPosition.x);
            angle -= 90 * Mathf.Deg2Rad;

            float cos = Mathf.Cos(angle);
            float sin = -Mathf.Sin(angle);

            viewportPosition = viewportCenter + new Vector3(sin * 80, cos*80, 0);

            // y = ax + b
            float a = cos / sin;

            Vector3 viewportBounds = viewportCenter * 0.98f;

            if (cos > 0)
            {
                viewportPosition = new Vector3(viewportBounds.y/a, viewportBounds.y, 0);
            }
            else
            {
                viewportPosition = new Vector3(-viewportBounds.y/a, -viewportBounds.y, 0);
            }

            if (viewportPosition.x > viewportBounds.x)
            {
                viewportPosition = new Vector3(viewportBounds.x, viewportBounds.x*a, 0);
            }
            else if (viewportPosition.x < -viewportBounds.x)
            {
                viewportPosition = new Vector3(-viewportBounds.x, -viewportBounds.x*a, 0);
            }

            //viewportPosition += viewportCenter;
            Vector3 screenPosition = playerCamera.ViewportToScreenPoint(viewportPosition);

            //transform.localPosition = screenPosition;
            transform.localRotation = Quaternion.Euler(0, 0, angle*Mathf.Rad2Deg);
            RectTransform rt = gameObject.GetComponent<RectTransform>();
            rt.localPosition = screenPosition;

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

    private bool IsTargetVisible(Vector3 viewportPosition)
    {
        bool isTargetVisible = viewportPosition.z > 0 && viewportPosition.x >= 0 && viewportPosition.x <= 1 && viewportPosition.y >= 0 && viewportPosition.y <= 1;
        return isTargetVisible;
    }
}
