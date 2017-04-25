using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public Transform target;
    private Text text;

    void Start()
    {
        SetTarget(target);
    }

    void Update ()
	{
		if (target != null)
        {
            Vector2 targetPosition = SmartCameraSystem.GetCurrentCamera().WorldToScreenPoint(target.position);
            transform.position = targetPosition;
        }
	}

    public void SetTarget(Transform t)
    {
        target = t;
        text.text = target.name;
    }
}
