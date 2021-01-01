/* 
    ------------------- Code Monkey -------------------
    
    Thank you for downloading the Code Monkey Utilities
    I hope you find them useful in your projects
    If you have any questions use the contact form
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using UnityEngine;

/*
 * Script to handle Camera Movement and Zoom
 * Place on Camera GameObject
 * */
public class CameraFollow : MonoBehaviour
{
    private Camera myCamera;
    private Func<Vector3> GetCameraFollowPositionFunc;
    private Func<float> GetCameraZoomFunc;
    private Vector3 cameraPosition;
    private float orthoSize = 7.5f;

    public void Setup(Func<Vector3> GetCameraFollowPositionFunc, Func<float> GetCameraZoomFunc, bool teleportToFollowPosition, bool instantZoom)
    {
        this.GetCameraFollowPositionFunc = GetCameraFollowPositionFunc;
        this.GetCameraZoomFunc = GetCameraZoomFunc;

        if (teleportToFollowPosition)
        {
            Vector3 cameraFollowPosition = GetCameraFollowPositionFunc();
            cameraFollowPosition.z = transform.position.z;
            transform.position = cameraFollowPosition;
        }

        if (instantZoom)
        {
            myCamera.orthographicSize = GetCameraZoomFunc();
        }
    }

    private void Awake()
    {
        myCamera = transform.GetComponent<Camera>();
        cameraPosition = Vector3.zero;
        Setup(() => cameraPosition, () => orthoSize, true, true);
    }

    public void SetCameraFollowPosition(Vector3 cameraFollowPosition)
    {
        SetGetCameraFollowPositionFunc(() => cameraFollowPosition);
    }

    public void SetGetCameraFollowPositionFunc(Func<Vector3> GetCameraFollowPositionFunc)
    {
        this.GetCameraFollowPositionFunc = GetCameraFollowPositionFunc;
    }

    public void SetCameraZoom(float cameraZoom)
    {
        SetGetCameraZoomFunc(() => cameraZoom);
    }

    public void SetGetCameraZoomFunc(Func<float> GetCameraZoomFunc)
    {
        this.GetCameraZoomFunc = GetCameraZoomFunc;
    }


    private void Update()
    {
        float cameraSpeed = 100f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            cameraPosition += new Vector3(-1, 0) * cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            cameraPosition += new Vector3(+1, 0) * cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            cameraPosition += new Vector3(0, +1) * cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            cameraPosition += new Vector3(0, -1) * cameraSpeed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            orthoSize -= 10f;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            orthoSize += 10f;
        }
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        if (GetCameraFollowPositionFunc == null) return;
        Vector3 cameraFollowPosition = GetCameraFollowPositionFunc();
        cameraFollowPosition.z = transform.position.z;

        Vector3 cameraMoveDir = (cameraFollowPosition - transform.position).normalized;
        float distance = Vector3.Distance(cameraFollowPosition, transform.position);
        float cameraMoveSpeed = 3f;

        if (distance > 0)
        {
            Vector3 newCameraPosition = transform.position + cameraMoveDir * distance * cameraMoveSpeed * Time.deltaTime;

            float distanceAfterMoving = Vector3.Distance(newCameraPosition, cameraFollowPosition);

            if (distanceAfterMoving > distance)
            {
                // Overshot the target
                newCameraPosition = cameraFollowPosition;
            }

            transform.position = newCameraPosition;
        }
    }

    private void HandleZoom()
    {
        if (GetCameraZoomFunc == null) return;
        float cameraZoom = GetCameraZoomFunc();

        float cameraZoomDifference = cameraZoom - myCamera.orthographicSize;
        float cameraZoomSpeed = 1f;

        myCamera.orthographicSize += cameraZoomDifference * cameraZoomSpeed * Time.deltaTime;

        if (cameraZoomDifference > 0)
        {
            if (myCamera.orthographicSize > cameraZoom)
            {
                myCamera.orthographicSize = cameraZoom;
            }
        }
        else
        {
            if (myCamera.orthographicSize < cameraZoom)
            {
                myCamera.orthographicSize = cameraZoom;
            }
        }
    }
}