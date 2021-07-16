using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour{

	public int tileX;
	public int tileZ;
	public TileMap map;

	int unitSpeed = 4;

	public List<Node> currentPath = null;

	void Update() {
		if(currentPath != null) {

			int currNode = 0;

			while( currNode < currentPath.Count-1 ) {

				Vector3 start = map.TileCoordToWorldCoord( currentPath[currNode].x, currentPath[currNode].z ) + 
					new Vector3(0, 1f, 0) ;
				Vector3 end   = map.TileCoordToWorldCoord( currentPath[currNode+1].x, currentPath[currNode+1].z )  + 
					new Vector3(0, 1f, 0) ;

				Debug.DrawLine(start, end, Color.red);

				currNode++;
			}

		}
	}

	public void MoveNextTile() {
		float remainingMovement = unitSpeed;

		while (remainingMovement > 0){
			if (currentPath == null)
				return;

			remainingMovement -= map.CostToEnterTile(currentPath[0].x, currentPath[0].z, currentPath[1].x, currentPath[1].z);

			// Remove the old current/first node from the path/ 
			currentPath.RemoveAt(0);

			// Now grab the new first node and move us to that position.
			tileX = currentPath[0].x;
			tileZ = currentPath[0].z;
			transform.position = map.TileCoordToWorldCoord(tileX, tileZ);

			if (currentPath.Count == 1){
				// We only have one tile left in the path, and that tile MUST be our ultimate destination
				// Now lets clear our pathfinding info.
				currentPath = null;
			}
			
		}

	}

}
