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
        Vector3 direction = (player.transform.position + Vector3.up * 2f) - this.transform.position;
        float dist = direction.magnitude;
        direction.Normalize();
        Ray ray = new Ray(this.transform.position, direction);
        RaycastHit hit;
        //Debug.DrawLine(this.transform.position, direction * dist + this.transform.position, Color.red);
        bool hitAnything = Physics.Linecast(this.transform.position, (player.transform.position + Vector3.up * 2f), out hit);

        if (hitAnything && hit.collider && hit.collider.CompareTag("Player"))
        {
            return true;
        }
        return false;
    }
}
