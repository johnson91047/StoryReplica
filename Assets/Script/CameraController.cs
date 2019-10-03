﻿using UnityEngine;

public class CameraController : MonoBehaviour
{
    public StoryMaster StoryMaster;
    public float CameraInitialZValue = -10f;
    public float SpeedMultiplier = 3f;
    public float CameraMovementDeadZone = 0.05f;
    public float ChangeStoryDeadZone = 100f;
    public float LerpEaseAmount = 15f;
    public Vector3 TargetPosition;
    public Vector2 AspectRatio = new Vector2(9f,16f);

    private Vector3 _initMouseWorldPosition;
    private float _initMouseXValue;
    private Camera _camera; 

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        SetAspectRatio();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        MoveCamera();
    }


    public void SetDestination(Vector3 destination)
    {
        destination.z = CameraInitialZValue;

        TargetPosition = destination;
    }

    private void MoveCamera()
    {
        if (Vector2.Distance(transform.position, TargetPosition) > CameraMovementDeadZone)
        {
            transform.position = Vector3.Lerp(transform.position, TargetPosition, LerpEaseAmount * Time.deltaTime);
        }
    }
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit(0);
        }

        if (Input.GetMouseButtonDown(0))
        {
            _initMouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            _initMouseXValue = Input.mousePosition.x;
        }

        if (Input.GetMouseButton(0))
        {
            StoryMaster.SetCurrentPagePause(true);

            Vector3 dragOffset = _initMouseWorldPosition - _camera.ScreenToWorldPoint(Input.mousePosition);

            dragOffset.y = 0;
            dragOffset.z = 0;

            Vector3 newPos = transform.position + dragOffset * SpeedMultiplier;
            newPos.z = CameraInitialZValue;

            TargetPosition = newPos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            StoryMaster.SetCurrentPagePause(false);

            Vector3 currentMouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            float currentMouseXvalue = Input.mousePosition.x;
            float offset = currentMouseXvalue - _initMouseXValue;

            if (_initMouseWorldPosition.Equals(currentMouseWorldPosition))
            {
                DoClick(Input.mousePosition);
                return;
            }

            if (offset > 100f)
            {
                StoryMaster.ChangeToPreviousStory();
            }
            else if(offset < -100f)
            {
                StoryMaster.ChangeToNextStory();
            }
            else
            {
                StoryMaster.StayAtCurrentStory();
            }
        }
    }

    private void DoClick(Vector3 mousePosition)
    {
        Vector2 viewPortPos = _camera.ScreenToViewportPoint(mousePosition);
        if (viewPortPos.x > 1 || viewPortPos.x < 0 || viewPortPos.y > 1 || viewPortPos.y < 0)
        {
            return;
        }

        if (mousePosition.x > Screen.width / 2f)
        {
            if (mousePosition.x - (Screen.width / 2f) >= ChangeStoryDeadZone)
            {
                StoryMaster.ChangeToNextPage();
            }
        }
        else
        {
            if ((Screen.width / 2f) - mousePosition.x >= ChangeStoryDeadZone)
            {
                StoryMaster.ChangeToPreviousPage();
            }
        }
    }

    private void SetAspectRatio()
    {
        // set the desired aspect ratio (the values in this example are
        // hard-coded for 16:9, but you could make them into public
        // variables instead so you can set them at design time)
        float targetaspect = AspectRatio.x / AspectRatio.y;

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // obtain camera component so we can modify its viewport
        Camera camera = GetComponent<Camera>();

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }

}
