using UnityEngine;
using UnityEngine.UI;

public class GameClock : MonoBehaviour
{
    public Text gameTime;
    public DayNightClock dayNightClock;
    public GameObject DayNightManager;
    
    private void OnAwake()
    {
        gameTime = gameObject.GetComponent<Text>();
        DayNightManager = GameObject.FindGameObjectWithTag("DayNightManager");
        gameTime.text = DayNightManager.GetComponent<DayNightClock>().WorldTime;
    }

}
