using UnityEngine;

public class CameraController : MonoBehaviour{
    public float wasdPanSpeed = 10f;
    public float borderPanSpeed = 15f;
    public float panBorderThickness = 5f;
    public Vector2 panLimit;
    public int cameraXOffset = 10;
    public float scrollSpeed = 10f;
    public float minY = 6f;
    public float maxY = 12f;

    void Update(){        
        Vector3 pos = transform.position;
        float saveWASDSpeed = wasdPanSpeed;
        if (Input.GetKey("left shift")){
            wasdPanSpeed *= 1.5f;
        }
        if (Input.GetKey("w")){
            pos.z += wasdPanSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y >= Screen.height - panBorderThickness){
            pos.z += borderPanSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a")){
            pos.x -= wasdPanSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness){
            pos.x -= borderPanSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s")){
            pos.z -= wasdPanSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= panBorderThickness){
            pos.z -= borderPanSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d")){
            pos.x += wasdPanSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness){
            pos.x += borderPanSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime; 

        pos.x = Mathf.Clamp(pos.x, cameraXOffset-panLimit.x, cameraXOffset+panLimit.x);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);
        
        transform.position = pos;
        wasdPanSpeed = saveWASDSpeed;
    }
}
