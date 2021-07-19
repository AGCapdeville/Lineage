using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableTile : MonoBehaviour{
    
    public int tileX;
    public int tileZ; 
    public TileMap map;

    void OnMouseUp() {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        Debug.Log ("Clicked Tile: ["+tileX+","+tileZ+"]");
        map.GeneratePathTo(tileX, tileZ);
    }

}
