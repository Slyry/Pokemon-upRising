﻿using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour 
{

	public int tileX;
	public int tileY;
	public TileMap map;

	public List<Node> currentPath = null;

	public int moveSpeed = 2;
	public bool isSelected = false;
	public bool isAlive = true;

	void Update() 
	{
		if (map.selectedUnit.gameObject.name == this.gameObject.name) {
			isSelected = true;
		} 
		else 
		{
			isSelected = false;
		}

		if(currentPath != null) 
		{
			int currNode = 0;

			while( currNode < currentPath.Count-1 ) 
			{

				Vector3 start = map.TileCoordToWorldCoord( currentPath[currNode].x, currentPath[currNode].y ) + 
					new Vector3(0, 0, -1f) ;
				Vector3 end   = map.TileCoordToWorldCoord( currentPath[currNode+1].x, currentPath[currNode+1].y )  + 
					new Vector3(0, 0, -1f) ;

				Debug.DrawLine(start, end, Color.red);

				currNode++;
			}

		}
	}

	/*public void MoveNextTile() 
	{
		if(isSelected == true)
		{
			float remainingMovement = moveSpeed;

			while (remainingMovement > 0) {
				if (currentPath == null)
					return;

				// Get cost from current tile to next tile
				remainingMovement -= map.CostToEnterTile (currentPath [0].x, currentPath [0].y, currentPath [1].x, currentPath [1].y);

				// Move us to the next tile in the sequence
				tileX = currentPath [1].x;
				tileY = currentPath [1].y;

				transform.position = map.TileCoordToWorldCoord (tileX, tileY);	// Update our unity world position

				// Remove the old "current" tile
				currentPath.RemoveAt (0);

				if (currentPath.Count == 1) {
					// We only have one tile left in the path, and that tile MUST be our ultimate
					// destination -- and we are standing on it!
					// So let's just clear our pathfinding info.
					currentPath = null;
				}
			}
		}

	}*/

	void TakeDamage()
	{
		/*if (isAlive == false) 
		{
			GameManager gameManagerScript;
			GameObject gameManager = GameObject.Find ("Game Manager");
			gameManagerScript = gameManager.GetComponent<GameManager> ();

			gameManagerScript.unitList.Remove (gameManagerScript.unitList.);
		}*/
	}
}