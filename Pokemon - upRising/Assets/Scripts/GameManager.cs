using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
	public List<GameObject> unitList = new List<GameObject>();
	public List<GameObject> enemyList = new List<GameObject> ();

	// Use this for initialization
	void Start () 
	{
		/*foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Unit")) 
		{
			unitList.Add (unit);
		}
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) 
		{
			enemyList.Add (enemy);
		}*/
	}
	
	// Update is called once per frame
	void Update () 
	{
        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Unit"))
        {
            unitList.Add(unit);
        }
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemyList.Add(enemy);
        }
    }
}
