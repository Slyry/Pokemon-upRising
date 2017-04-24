using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour 
{

	public int tileX;
	public int tileY;

	public List<Node> currentPath = null;

	public int moveSpeed = 2;
	public bool isAlive = true;


	void Start()
	{
		
	}

	void Update() 
	{
		UnitStuff ();
	}

	void UnitStuff()
	{
		GameObject tileMap = GameObject.Find ("Map");
		TileMap map = tileMap.GetComponent<TileMap> ();

		if(currentPath != null) 
		{
			int currNode = 0;

			while( currNode < currentPath.Count-1 ) 
			{

				Vector3 start = map.TileCoordToWorldCoord( currentPath[currNode].x, currentPath[currNode].y ) + 
					new Vector3(0, 0, -1f) ;
				Vector3 end = map.TileCoordToWorldCoord( currentPath[currNode+1].x, currentPath[currNode+1].y )  + 
					new Vector3(0, 0, -1f) ;

				Debug.DrawLine(start, end, Color.red);

				currNode++;
			}

		}
	}
}
