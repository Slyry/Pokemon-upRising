using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileMap : MonoBehaviour 
{
	#region Variables
	[SerializeField] Camera gameCamera;
	[SerializeField] GameManager gameManager;
    [SerializeField] GameObject rocketThiefPrefab;
	[SerializeField] GameObject rocketGruntPrefab;
	[SerializeField] GameObject rocketTamerPrefab;
	[SerializeField] GameObject rocketSquadPrefab;

	public GameObject selectedUnit;
	public TileType[] tileTypes;

	bool movingstate;
	bool actionstate;

	int[,] tiles;
	Node[,] graph;

	int mapSizeX = 17;
	int mapSizeY = 12;
	int unitIndexNumber = 0;
	#endregion

	void Start() 
	{
		GenerateMapData();
		GeneratePathfindingGraph();
		GenerateMapVisual();
	}

	void Update()
	{
		
	}

	//Determines where to place tiles
	void GenerateMapData() {
		// Allocate our map tiles
		tiles = new int[mapSizeX,mapSizeY];

		int x,y;

		// Initialize our map tiles to be grass
		for(x=0; x < mapSizeX; x++) {
			for(y=0; y < mapSizeY; y++) {
				tiles[x,y] = 0;
			}
		}

		#region old map details

		//// Make a big swamp area
		//for(x=3; x <= 5; x++) {
		//	for(y=0; y < 4; y++) {
		//		tiles[x,y] = 1;
		//	}
		//}

		//// Let's make a u-shaped mountain range
		//tiles[4, 4] = 2;
		//tiles[5, 4] = 2;
		//tiles[6, 4] = 2;
		//tiles[7, 4] = 2;
		//tiles[8, 4] = 2;

		//tiles[4, 5] = 2;
		//tiles[4, 6] = 2;
		//tiles[8, 5] = 2;
		//tiles[8, 6] = 2;

		#endregion

		tiles[1, 10] = 2;
		tiles[2, 10] = 2;
		tiles[8, 10] = 2;
		tiles[8, 9] = 2;
		tiles[8, 8] = 2;

		for (x = 0; x <= 6; x++)
		{
			y = 0;
			tiles[x, y] = 2;
		}

		for (x = 9; x <= 16; x++)
		{
			y = 0;
			tiles[x, y] = 2;
		}

		for (x = 10; x <= 15; x++)
		{
			for (y = 7; y < 9; y++)
			{
				tiles[x, y] = 2;
			}
		}

		tiles[10, 6] = 2;
		tiles[10, 5] = 2;
		tiles[10, 4] = 2;
		tiles[11, 4] = 2;
		tiles[15, 6] = 2;
		tiles[15, 5] = 2;
		tiles[14, 4] = 2;
		tiles[15, 4] = 2;
	}

	//Determines cost to enter a space
	public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY) {

		TileType tt = tileTypes[ tiles[targetX,targetY] ];

		if(UnitCanEnterTile(targetX, targetY) == false)
			return Mathf.Infinity;

		float cost = tt.movementCost;

		if( sourceX!=targetX && sourceY!=targetY) {
			// We are moving diagonally!  Fudge the cost for tie-breaking
			// Purely a cosmetic thing!
			cost += 0.001f;
		}
		return cost;
	}

	//Generates Pathing nodes
	void GeneratePathfindingGraph() {
		// Initialize the array
		graph = new Node[mapSizeX,mapSizeY];

		// Initialize a Node for each spot in the array
		for(int x=0; x < mapSizeX; x++) {
			for(int y=0; y < mapSizeY; y++) {
				graph[x,y] = new Node();
				graph[x,y].x = x;
				graph[x,y].y = y;
			}
		}

		// Now that all the nodes exist, calculate their neighbours
		for(int x=0; x < mapSizeX; x++) {
			for(int y=0; y < mapSizeY; y++) {

				// This is the 8-way connection version (allows diagonal movement)
				// Try left
				if(x > 0) {
					graph[x,y].neighbours.Add( graph[x-1, y] );
					if(y > 0)
						graph[x,y].neighbours.Add( graph[x-1, y-1] );
					if(y < mapSizeY-1)
						graph[x,y].neighbours.Add( graph[x-1, y+1] );
				}

				// Try Right
				if(x < mapSizeX-1) {
					graph[x,y].neighbours.Add( graph[x+1, y] );
					if(y > 0)
						graph[x,y].neighbours.Add( graph[x+1, y-1] );
					if(y < mapSizeY-1)
						graph[x,y].neighbours.Add( graph[x+1, y+1] );
				}

				// Try straight up and down
				if(y > 0)
					graph[x,y].neighbours.Add( graph[x, y-1] );
				if(y < mapSizeY-1)
					graph[x,y].neighbours.Add( graph[x, y+1] );
			}
		}
	}

	//Generates tile visual
	void GenerateMapVisual() {
		for(int x=0; x < mapSizeX; x++) {
			for(int y=0; y < mapSizeY; y++) {
				TileType tt = tileTypes[ tiles[x,y] ];
				GameObject go = (GameObject)Instantiate( tt.tileVisualPrefab, new Vector3(x, y, 0), Quaternion.identity );

				ClickableTile ct = go.GetComponent<ClickableTile>();
				ct.tileX = x;
				ct.tileY = y;
				ct.map = this;
			}
		}
	}

	//????
	public Vector3 TileCoordToWorldCoord(int x, int y) 
	{
		return new Vector3(x, y, 0);
	}

	//Checks if the unit can enter
	public bool UnitCanEnterTile(int x, int y) {

		// We could test the unit's walk/hover/fly type against various
		// terrain flags here to see if they are allowed to enter the tile.
		return tileTypes[ tiles[x,y] ].isWalkable;
	}

	//Generates pathing to target
	public void GeneratePathTo(int x, int y) {
		// Clear out our unit's old path.
		selectedUnit.GetComponent<Unit>().currentPath = null;

		if( UnitCanEnterTile(x,y) == false ) {
			// We probably clicked on a mountain or something, so just quit out.
			return;
		}

		Dictionary<Node, float> dist = new Dictionary<Node, float>();
		Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

		// Setup the "Q" -- the list of nodes we haven't checked yet.
		List<Node> unvisited = new List<Node>();

		Node source = graph[
			selectedUnit.GetComponent<Unit>().tileX, 
			selectedUnit.GetComponent<Unit>().tileY
		];

		Node target = graph[
			x, 
			y
		];

		dist[source] = 0;
		prev[source] = null;

		// Initialize everything to have INFINITY distance, since
		// we don't know any better right now. Also, it's possible
		// that some nodes CAN'T be reached from the source,
		// which would make INFINITY a reasonable value
		foreach(Node v in graph) {
			if(v != source) {
				dist[v] = Mathf.Infinity;
				prev[v] = null;
			}

			unvisited.Add(v);
		}

		while(unvisited.Count > 0) {
			// "u" is going to be the unvisited node with the smallest distance.
			Node u = null;

			foreach(Node possibleU in unvisited) {
				if(u == null || dist[possibleU] < dist[u]) {
					u = possibleU;
				}
			}

			if(u == target) {
				break;	// Exit the while loop!
			}

			unvisited.Remove(u);

			foreach(Node v in u.neighbours) {
				//float alt = dist[u] + u.DistanceTo(v);
				float alt = dist[u] + CostToEnterTile(u.x, u.y, v.x, v.y);
				if( alt < dist[v] ) {
					dist[v] = alt;
					prev[v] = u;
				}
			}
		}

		// If we get there, the either we found the shortest route
		// to our target, or there is no route at ALL to our target.

		if(prev[target] == null) {
			// No route between our target and the source
			return;
		}

		List<Node> currentPath = new List<Node>();

		Node curr = target;

		// Step through the "prev" chain and add it to our path
		while(curr != null) {
			currentPath.Add(curr);
			curr = prev[curr];
		}

		// Right now, currentPath describes a route from out target to our source
		// So we need to invert it!

		currentPath.Reverse();

		selectedUnit.GetComponent<Unit>().currentPath = currentPath;
	}

	//Changes the currently selected unit
	public void SelectUnit()
	{
		selectedUnit = gameManager.unitList [unitIndexNumber];
	}

	//Moves the unit
	public void MoveNextTile() 
	{
		Unit unitScript = selectedUnit.GetComponent<Unit> ();

		float remainingMovement = unitScript.moveSpeed;

		while (remainingMovement > 0) 
		{
			if (unitScript.currentPath == null)
				return;

			// Get cost from current tile to next tile
			remainingMovement -= CostToEnterTile (unitScript.currentPath [0].x, unitScript.currentPath [0].y, unitScript.currentPath [1].x, unitScript.currentPath [1].y);

			// Move us to the next tile in the sequence
			unitScript.tileX = unitScript.currentPath [1].x;
			unitScript.tileY = unitScript.currentPath [1].y;

			selectedUnit.transform.position = TileCoordToWorldCoord (unitScript.tileX, unitScript.tileY);	// Update our unity world position

			// Remove the old "current" tile
			unitScript.currentPath.RemoveAt (0);

			if (unitScript.currentPath.Count == 1)
			{
				// We only have one tile left in the path, and that tile MUST be our ultimate
				// destination -- and we are standing on it!
				// So let's just clear our pathfinding info.
				unitScript.currentPath = null;

				unitIndexNumber++;

				if (unitIndexNumber > gameManager.unitList.Count - 1)
				{
					unitIndexNumber = 0;
				}

				SelectUnit ();
			}
		}
	}

    public void SpawnRocketThiefUnits()
    {
        Instantiate(rocketThiefPrefab, new Vector3(4, 1, 0), Quaternion.identity);
    }

	public void SpawnRocketGruntUnits()
	{
		Instantiate(rocketGruntPrefab, new Vector3(6, 1, 0), Quaternion.identity);
	}

	public void SpawnRocketTamerUnits()
	{
		Instantiate(rocketTamerPrefab, new Vector3(8, 1, 0), Quaternion.identity);
	}

	public void SpawnRocketSquadUnits()
	{
		Instantiate(rocketSquadPrefab, new Vector3(10, 1, 0), Quaternion.identity);
	}

}