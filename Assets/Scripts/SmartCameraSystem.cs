using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SmartCameraSystem : MonoBehaviour
{
    // Singleton
    public static SmartCameraSystem instance;

    private Renderer targetRenderer;
    private List<Camera> cameras;
    private List<Camera> currentCamerasWithViewOfTarget;
    public Camera startingCamera;
    private Camera prevCamera;
    private Camera currentCamera;

    private PlayerController player;

    // If the target/player is closer to a camera than this, it won't be considered for being an active camera
    public float minimumAllowedDistanceToCamera = 1f;

	void Start ()
	{
        // Singleton structure
        if (instance == null)
        {
            instance = this;

            targetRenderer = GameObject.FindGameObjectWithTag("Target").GetComponent<Renderer>();

            // Get list of Cameras in scene
            GameObject[] cameraObjects = GameObject.FindGameObjectsWithTag("Camera");
            cameras = new List<Camera>();
            foreach (GameObject c in cameraObjects)
            {
                cameras.Add(c.GetComponent<Camera>());
                c.SetActive(false);
            }

            prevCamera = startingCamera;
            prevCamera.gameObject.SetActive(true);
            currentCamera = prevCamera;

            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            player.OnCameraChange(currentCamera);
        }
        else
        {
            Destroy(this);
        }        
	}

	void Update ()
	{
        CheckForIdealCamera(); // Consider making this a coroutine?
	}

    // Pick a camera with which to render the scene, keeping the target in clear view as much as possible
    private void CheckForIdealCamera()
    {
        currentCamerasWithViewOfTarget = new List<Camera>();
        // Get a list of all cameras that could see the player in theory
        foreach (Camera cam in cameras)
        {/*
            if (IsVisibleFrom(targetRenderer, cam))
            {
                Debug.Log("1");
                currentCamerasWithViewOfTarget.Add(cam);
            }*/
            Vector3 viewPos = cam.WorldToViewportPoint(targetRenderer.transform.position);
            //Debug.Log(cam + ": " + viewPos.x + " " + viewPos.y);
            if (!(viewPos.x < 0f || viewPos.x > 1f || viewPos.y < 0f || viewPos.y > 1f))
            {
                Ray ray = cam.ViewportPointToRay(viewPos);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                if (hit.collider && (hit.collider.tag == "Player" || hit.collider.tag == "Target"))
                {
                    //Debug.Log("1");
                    currentCamerasWithViewOfTarget.Add(cam);
                }
                
            }
        }

        // If there is only one camera, use that
        if (currentCamerasWithViewOfTarget.Count == 1)
        {
            //Debug.Log("Just The One");
            currentCamera = currentCamerasWithViewOfTarget.ElementAt(0);
        }
        else
        {
            // Go through all cameras still under consideration and pick the one closest to the target, ignoring those which are too close
            float shortestDistance = float.MaxValue;
            foreach (Camera cam in currentCamerasWithViewOfTarget)
            {
                float distance = Vector3.Distance(cam.transform.position, targetRenderer.transform.position);
                if (distance < minimumAllowedDistanceToCamera)
                {
                    currentCamerasWithViewOfTarget.Remove(cam);
                }
                else if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    currentCamera = cam;
                }
                //Debug.Log(cam + ": " + distance);
            }
        }

        // If the selected camera was not already the active camera, make it the new active camera, so long as the new camera is at least 5 units closer or the old camera has lost sight of the player
        if (currentCamera != prevCamera && (Vector3.Distance(currentCamera.transform.position, targetRenderer.transform.position) + 5f < Vector3.Distance(prevCamera.transform.position, targetRenderer.transform.position) || !currentCamerasWithViewOfTarget.Contains(prevCamera)))
        {
            //Debug.Log("2");
            prevCamera.gameObject.SetActive(false);
            currentCamera.gameObject.SetActive(true);
            prevCamera = currentCamera;

            //player.OnCameraChange(currentCamera);
        }
    }

    public bool IsVisibleFrom(Renderer r, Camera cam)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        return GeometryUtility.TestPlanesAABB(planes, r.bounds);
    }

    public static Camera GetCurrentCamera()
    {
        return instance.prevCamera;
    }
}