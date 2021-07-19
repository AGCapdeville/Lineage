using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileMap : MonoBehaviour{

    public GameObject selectedUnit;

    public TileType[] tileTypes;
    public GameConstants GameConstants;
    
    int[,] tiles;
    Node[,] graph;

    // For right now this is assuming that we only have one unit... So this is just a begining.
    // List<Node> currentPath = null; 

    private int mapSizeX = 22;
    private int mapSizeZ = 16;

    void Start() {
        // Set up selected units variables
        selectedUnit.GetComponent<Unit>().tileX = (int)selectedUnit.transform.position.x;
        selectedUnit.GetComponent<Unit>().tileZ = (int)selectedUnit.transform.position.z;
        selectedUnit.GetComponent<Unit>().map = this;

        GenerateMapData();
        GeneratePathFindingGraph();
        GenerateMapVisuals();
    }

    public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetZ) {
        if (UnitCanEnterTitle(targetX, targetZ) == false)
            return Mathf.Infinity;

        TileType tt = tileTypes[tiles[targetX, targetZ]]; 
        return tt.movementCost;
    }

    void GenerateMapData() {
        // Allocate map tiles
        tiles = new int[mapSizeX, mapSizeZ];
        // Initialize map tiles to dirt:
        for (int x = 0; x < mapSizeX; x++){
            for (int z = 0; z < mapSizeZ; z++){
                tiles[x,z] = GameConstants.TILE_DIRT;
            }
        }

        // Initialize u-shaped water:
        tiles[1,1] = GameConstants.TILE_WATER;
        tiles[2,2] = GameConstants.TILE_WATER;
        tiles[3,3] = GameConstants.TILE_WATER;
        tiles[3,4] = GameConstants.TILE_WATER;
        tiles[2,5] = GameConstants.TILE_WATER;
        tiles[1,6] = GameConstants.TILE_WATER;

        tiles[7,7] = GameConstants.TILE_TREE;
    }

    void GeneratePathFindingGraph() {
        // Initialize the array.
        graph = new Node[mapSizeX, mapSizeZ];

        // Initialize a node for each spot in the array.
        for (int x = 0; x < mapSizeX; x++){
            for (int z = 0; z < mapSizeZ; z++){
                graph[x,z] = new Node();
                graph[x,z].x = x;
                graph[x,z].z = z;
            }
        }

        // Now that all the nodes exist, calculate their neighbors.
        for (int x = 0; x < mapSizeX; x++){
            for (int z = 0; z < mapSizeZ; z++){
                // We have a 4-way connected map. 
                // This also works with 6-way hexes and 8-way tiles and n-way variable areas.
                if (x > 0)
                    graph[x,z].neighbours.Add( graph[x-1, z] );
                if (x < mapSizeX-1)
                    graph[x,z].neighbours.Add( graph[x+1, z] );
                if (z > 0)
                    graph[x,z].neighbours.Add( graph[x, z-1] );
                if (z < mapSizeZ-1)
                    graph[x,z].neighbours.Add( graph[x, z+1] );
                    
            }
        }
    }

    void GenerateMapVisuals() {
        for (int x = 0; x < mapSizeX; x++){
            for (int z = 0; z < mapSizeZ; z++){
                TileType td = tileTypes[tiles[x,z]];
                GameObject go = (GameObject)Instantiate(td.tileVisualPrefab, new Vector3(x, 0, z), Quaternion.identity);
            
                ClickableTile ct = go.GetComponent<ClickableTile>();
                ct.tileX = x;
                ct.tileZ = z;
                ct.map = this;
            }
        }
        
    }

    public Vector3 TileCoordToWorldCoord(int x, int z) {
        return new Vector3(x, 0, z);
    }

    public bool UnitCanEnterTitle(int x, int z) {
        return tileTypes[tiles[x,z]].isWalkable;
    }

    public void GeneratePathTo(int x, int z) {
        // Clear out our units old path.
        selectedUnit.GetComponent<Unit>().currentPath = null;

        if (UnitCanEnterTitle(x,z) == false){
            return;
        }
        
        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        // Setup "Q" -- the list of nodes we haven't checked yet.
        List<Node> unvisited = new List<Node>();

        Node source = graph[
            selectedUnit.GetComponent<Unit>().tileX,
            selectedUnit.GetComponent<Unit>().tileZ 
        ];

        Node target = graph[x,z];

        dist[source] = 0;
        prev[source] = null;

        // Initialize everything to have INFINITY distance, since
        // we don't know any better right now. Also, it's possible
        // that some nodes CAN'T be reached from the source,
        // which would make INFINITY a reasonable value.
        foreach (Node v in graph) {
            if (v != source){
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }

            unvisited.Add(v);
        }

        while (unvisited.Count > 0) {
            // "u" is going to be the unvisited node with the smallest distance.
            Node u = null;

            foreach(Node possibleU in unvisited){
                if (u == null || dist[possibleU] < dist[u]){
                    u = possibleU;
                }
            }

            if (u == target){
                break;
            }
            
            unvisited.Remove(u);
        
            foreach (Node v in u.neighbours) {
                // float alt = dist[u] + u.DistanceTo(v);
                float alt = dist[u] + CostToEnterTile(u.x, u.z, v.x, v.z);
                if (alt < dist[v]){
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }

        // If we get here, then either we found the shortest rout to 
        // our target or there is no route at all to our target.
        if (prev[target] == null){
            // No route between our "target" and the "source"!
            return;
        }

        List<Node> currentPath = new List<Node>();
        
        Node curr = target;

        // Step through the "prev" chain and add it to our path.
        while(curr != null){
            currentPath.Add(curr);
            curr = prev[curr];
        }

        // Right now, currentPath describes a route from our "target" to our "source"
        // So we need to invert it!
        currentPath.Reverse();
        selectedUnit.GetComponent<Unit>().currentPath = currentPath;
    }

}
