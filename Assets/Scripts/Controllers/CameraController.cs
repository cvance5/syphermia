using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Controller
{
    [SerializeField]
    [Range(0, 1)]
    float LookSensitivity = .1f;

    public override bool HandleHoverOver(GameObject Object)
    {
        return false;
    }
    public override bool HandleSelection(GameObject selectedObject)
    {
        return false;
    }
    public override bool HandleMouseDrag(GameObject selectedObject, Vector3 dragVector)
    {
        Transform CameraTransform = Camera.main.transform;
        Vector3 rotationPoint = BoardManager.BoardCenter;

        if(selectedObject != null)
        {
            rotationPoint = selectedObject.transform.position;
            Quaternion LookAtRotation = Quaternion.LookRotation(rotationPoint - CameraTransform.position);

            if (Quaternion.Angle(LookAtRotation, CameraTransform.rotation) > 10)
            {
                CameraTransform.rotation = Quaternion.Lerp(CameraTransform.rotation, LookAtRotation, Time.deltaTime);
            }
            if (Vector3.Distance(CameraTransform.position, rotationPoint) > VIEW_RANGE)
            {
                Vector3 zoomLocation = Vector3.Lerp(CameraTransform.position, rotationPoint, Time.deltaTime);
                CameraTransform.position = new Vector3(zoomLocation.x, CameraTransform.position.y, zoomLocation.z);
            }
        }

        if((Camera.main.WorldToScreenPoint(rotationPoint) - Input.mousePosition).magnitude > ControllerManager.MOUSE_DEADZONE && dragVector.magnitude > 1)
        {
            CameraTransform.RotateAround(rotationPoint, Vector3.up, dragVector.x * LookSensitivity);
            CameraTransform.RotateAround(rotationPoint, Vector3.right, dragVector.y * LookSensitivity * 2);
        }
        if (CameraTransform.rotation.eulerAngles.x > MAX_PITCH || CameraTransform.rotation.eulerAngles.x < MIN_PITCH)
        {
            CameraTransform.rotation = Quaternion.Euler(MIN_PITCH, CameraTransform.rotation.eulerAngles.y, CameraTransform.rotation.eulerAngles.z);
        }
        if (CameraTransform.position.y < 0)
        {
            CameraTransform.position = new Vector3(CameraTransform.position.x, 0, CameraTransform.position.z);
        }
        CameraTransform.rotation = Quaternion.Euler(CameraTransform.rotation.eulerAngles.x, CameraTransform.rotation.eulerAngles.y, 0);

        return true;
    }
    public override bool HandleKeyPress(string key)
    {
        return false;
    }
    protected override Context[] InitiatlizeValidContexts()
    {
        return new Context[] {Context.Combat};
    }

    const int MIN_PITCH = 22;
    const int MAX_PITCH = 90;
    const int VIEW_RANGE = 7;
    const int FOCUS_DELAY_FRAMES = 4;
}
