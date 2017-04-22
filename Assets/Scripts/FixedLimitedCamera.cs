using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedLimitedCamera : MonoBehaviour
{
    private Quaternion defaultFacing;
    public float maxLeftLookAngle = 15f;
    public float maxRightLookAngle = 15f;
    public float maxUpLookAngle = 15f;
    public float maxDownLookAngle = 15f;

    private Transform target;
    private Vector3 trueTarget;

    void Start ()
	{
        defaultFacing = transform.rotation;
        target = GameObject.FindGameObjectWithTag("Target").transform;
	}

	void LateUpdate ()
	{
        // First, turn towards the target
        transform.LookAt(target);
        // Second, clamp the rotation of the camera to the set maximum angles
        transform.localEulerAngles = new Vector3(Mathf.Clamp(transform.localEulerAngles.x, defaultFacing.eulerAngles.x - maxDownLookAngle, defaultFacing.eulerAngles.x + maxUpLookAngle), Mathf.Clamp(transform.localEulerAngles.y, defaultFacing.eulerAngles.y - maxLeftLookAngle, defaultFacing.eulerAngles.y + maxRightLookAngle), 0f);
    }
}
