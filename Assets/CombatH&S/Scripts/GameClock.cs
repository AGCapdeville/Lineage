using UnityEngine;
using TMPro;

public class GameClock : MonoBehaviour
{
    public DayNightClock dayNightClock;
    public GameObject DayNightManager;
    public TMP_Text clock;
    
    private void Start()
    {
        DayNightManager = GameObject.FindGameObjectWithTag("DayNightManager");
        clock = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (clock.text != DayNightManager.GetComponent<DayNightClock>().WorldTime){
            clock.SetText(DayNightManager.GetComponent<DayNightClock>().WorldTime);
        }
    }

}
