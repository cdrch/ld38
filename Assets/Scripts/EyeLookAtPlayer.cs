using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EyeLookAtPlayer : MonoBehaviour {
    private Collider m_collider;

	// Use this for initialization
	void Start () {
        m_collider = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool LookForPlayer(GameObject player)
    {
        Vector3 direction = this.transform.position - player.transform.position;
        direction.Normalize();
        Ray ray = new Ray(this.transform.position, direction);
        RaycastHit hit;

        bool hitAnything = m_collider.Raycast(ray, out hit, 1000f);

        if (hitAnything && hit.collider && hit.collider.CompareTag("Player"))
        {
            return true;
        }
        return false;
    }
}
