using Cinemachine;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject Player;
    public Transform PlayerTransform;
    public CinemachineFreeLook vcam;

    // Use this for initialization
    void Start()
    {
        vcam = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null)
        {
            Player = GameObject.FindWithTag("Player");
            if (Player != null)
            {
                PlayerTransform = Player.transform;
                vcam.LookAt = PlayerTransform;
                vcam.Follow = PlayerTransform;
            }
        }
    }
}

