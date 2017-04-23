using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour {

    [SerializeField]
    public GameObject startPoint;
    [SerializeField]
    public GameObject waypoint1;
    [SerializeField]
    public GameObject waypoint2;
    [SerializeField]
    public GameObject faderObject;
    [SerializeField]
    public GameObject textManager;

    SceneManager scm;
    Color test;

    // Use this for initialization
    void Start ()
    {
        test = faderObject.GetComponent<SpriteRenderer>().color;
        StartCoroutine(Fadein());
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    void FixedUpdate()
    {
        Spielberg(waypoint1);
    }

    void Spielberg(GameObject newWayPoint)
    {
        transform.position = Vector3.Lerp(transform.position, newWayPoint.transform.position, 0.01f);

        if (textManager.GetComponent<TextManager>().textOver == true)
        {
            transform.position = Vector3.Lerp(transform.position, waypoint2.transform.position, 0.01f);
            StartCoroutine(Fadeout());
        }

        if (this.transform == waypoint2.transform)
        {
            SceneManager.LoadScene("");  
        }
    }

    IEnumerator Fadein()
    {
        for (float f = 1f; f >= 0; f -= 0.05f)
        {
            test.a -= 0.1f;
            faderObject.GetComponent<SpriteRenderer>().color = test;
            yield return null;
        }
    }

    IEnumerator Fadeout()
    {
        for (float f = 1f; f >= 0; f -= 0.05f)
        {
            test.a += 0.1f;
            faderObject.GetComponent<SpriteRenderer>().color = test;
            yield return null;
        }
    }

}
