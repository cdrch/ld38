using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour
{
    public float opennessFactor = 0.0f;
    public float changeSpeed = 1f; // Seconds to open or close
    public float scaleFactor = 10f; // This is the maximum amount the eyes can be open

	void Start ()
	{
        StartCoroutine(Blink());
	}

	void Update ()
	{
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, scaleFactor * opennessFactor);
	}

    IEnumerator Close()
    {
        while (opennessFactor != 0f)
        {
            opennessFactor -= (1 / changeSpeed) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
            if (opennessFactor < 0f)
                opennessFactor = 0f;
        }
    }

    IEnumerator Open()
    {
        while (opennessFactor != 1f)
        {
            opennessFactor += (1 / changeSpeed) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
            if (opennessFactor > 1f)
                opennessFactor = 1f;
        }
    }

    IEnumerator Blink()
    {
        while (opennessFactor != 0f)
        {
            opennessFactor -= (1 / changeSpeed) * 0.5f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
            if (opennessFactor < 0f)
                opennessFactor = 0f;
        }
        Debug.Log("Now Closed");

        while (opennessFactor != 1f)
        {
            opennessFactor += (1 / changeSpeed) * 0.5f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
            if (opennessFactor > 1f)
                opennessFactor = 1f;
        }
        Debug.Log("Now Open");
    }
}
