using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    public float frequence; // times per second
    private float tstamp;
    private float starttime;
	// Use this for initialization
	void Start () {
        tstamp = 0;
        starttime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateFrequece(Time.time);
            Debug.Log(frequence);
            starttime = Time.time;
        }
        else
        {
            if (frequence > 0)
            {
                frequence -= 0.1f;
            }
        }
        Vector3 v1 = transform.position;
        Vector3 v2 = transform.position + new Vector3(frequence * frequence, 0, 0);
        Vector3 velocity = frequence * transform.forward;
        transform.position = Vector3.SmoothDamp(v1,v2,ref velocity,1);
        //transform.position += new Vector3(frequence / 2, 0, 0);

    }

    void UpdateFrequece(float t)
    {
        frequence = 1 / (t - tstamp);
        tstamp = t;
    }
}
