using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class DayNightClock : MonoBehaviour
{
    // References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;

    // Variables
    [SerializeField, Range(0, 24)] private float TimeOfDay; 

    public string WorldTime;
    private string hourString;
    private string minuteString;
    
    //Every 60 sec in real life, is 1 hour in game. 
    private const float REAL_SECOUNDS_PER_IN_GAME_HOUR = 60f; 

    private void Update()
    {
        if (Preset == null){
            return;
        }

        if (Application.isPlaying){
            TimeOfDay += Time.deltaTime / REAL_SECOUNDS_PER_IN_GAME_HOUR;
            
            // On UI timer.
            hourString = Mathf.Floor(TimeOfDay).ToString("00");
            minuteString = Mathf.Floor(((TimeOfDay) % 1f) * 60f).ToString("00");
            WorldTime = hourString + ":" + minuteString;

            // Clamping the time per full revolution to 24 hours.
            if (TimeOfDay >= 24){
                TimeOfDay = 0f;
            }
            UpdateLighting((TimeOfDay % 24f) / 24f);
        } else {
            UpdateLighting((TimeOfDay % 24f) / 24f);
        }
    }
    private void UpdateLighting(float timePercent){
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if (DirectionalLight != null) {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f - 90f), 170, 0)); 
        }
    }
    private void OnValidate(){
        if (DirectionalLight != null) {
            return;
        }

        if (Preset != null) {
            DirectionalLight = RenderSettings.sun;
        }else{
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach(Light light in lights){
                if (light.type == LightType.Directional){
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}
