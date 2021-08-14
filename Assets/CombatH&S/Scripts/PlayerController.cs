using System;
using Mirror;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private CubeInputActions cubeInputActions;
    [SerializeField] public CharacterController controller;
    [SerializeField] private InputAction movement;
    [SerializeField] private InputAction run;
    [SerializeField] private InputAction attack;
    [SerializeField] private InputAction aim;
    [SerializeField] private CinemachineFreeLook cmf;

    // private Animator animator;
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float movementSpeedModifier = 2f;
    [SerializeField] private float rotationSpeed = 215f;

    // jump vars
    [SerializeField] public float jumpForce = 5.0f;
    [SerializeField] public float jumpImpulse = .5f;
    [SerializeField] public float gravity = -9.8f;
    [SerializeField] private float speedVert;
    [SerializeField] private bool isGrounded;

    [SerializeField] private Animator[] animators;
    [SerializeField] public GameObject enemy;
    
    // weapon vars
    [SerializeField] public Transform AxeObj;
    [SerializeField] public Transform BowObj;
    private bool[] WeaponsBools =
    {
        true, // axe
        false // bow
    };


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        speedVert = 0.0f; //player starts at rest
        isGrounded = true;

        if (isLocalPlayer)
        {
            cmf = CinemachineFreeLook.FindObjectOfType<CinemachineFreeLook>();
            cmf.LookAt = this.gameObject.transform;
            cmf.Follow = this.gameObject.transform;
        }

        AxeObj = gameObject.transform.GetChild(1).transform.GetChild(0);
        BowObj = gameObject.transform.GetChild(1).transform.GetChild(1);
        
        AxeObj.gameObject.SetActive(false);
        BowObj.gameObject.SetActive(false);
    }

    private void Awake()
    {
        cubeInputActions = new CubeInputActions();
        // controller = GetComponent<CharacterController>();
        controller = GetComponentInChildren<CharacterController>();
        animators = GetComponentsInChildren<Animator>();
    }

    private void OnEnable()
    {
        movement = cubeInputActions.Player.Movement;
        movement.Enable();

        run = cubeInputActions.Player.Run;
        run.Enable();

        attack = cubeInputActions.Player.Attack;
        attack.Enable();

        aim = cubeInputActions.Player.Aim;
        aim.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        run.Disable();
        attack.Disable();
        aim.Disable();
    }
    private void Update()
    {
        if (isLocalPlayer == false)
            return;
        
        if (Input.inputString != "")
        {
            // Debug.Log(Input.inputString);
            int number;
            bool isNumber = Int32.TryParse(Input.inputString, out number);
            if (isNumber)
            {
                weaponSelect(number);   
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            GameObject enemySpawn = Instantiate(enemy, transform.position + transform.forward*2, transform.rotation);
            NetworkServer.Spawn(enemySpawn);
        }

        bool attacking = Mathf.Approximately(attack.ReadValue<float>(), 1);
        bool aiming = Mathf.Approximately(aim.ReadValue<float>(), 1);

        animators[1].SetBool("Attacking", attacking);
        animators[1].SetBool("Aiming", aiming);
        animators[2].SetBool("Attacking", attacking);
        animators[2].SetBool("Aiming", aiming);
        
        MovePlayerTo();
        ToggleCur();
    }

    private void ToggleCur()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.Confined : CursorLockMode.Locked;
        }
    }

    private void MovePlayerTo()
    {
        var cameraForwardDirection = Camera.main.transform.forward;
        var cameraVector = Vector3.Scale(cameraForwardDirection, (Vector3.right + Vector3.forward));

        bool canRun = !StaminaBar.instance.IsStaminaEmpty();
        bool aiming = Mathf.Approximately(aim.ReadValue<float>(), 1);

        // Player Input
        bool running = Mathf.Approximately(run.ReadValue<float>(), 1);
        Vector2 inputVector = movement.ReadValue<Vector2>();
        
        Vector3 movementVector = new Vector3();
        movementVector.x = inputVector.x;
        movementVector.z = inputVector.y;
        bool moving = movementVector != Vector3.zero;

        Vector3 camF = Camera.main.transform.forward;
        Vector3 camR = Camera.main.transform.right;
        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        Vector3 newPlayerMovementVector;
        newPlayerMovementVector = camF * movementVector.z + camR * movementVector.x;

        if (moving && !running) // Walking
        {
            PlayerRotation(aiming, cameraVector, newPlayerMovementVector);
            controller.Move(newPlayerMovementVector * (0.5f * Time.deltaTime * 3.14f * movementSpeed) + PlayerTranslation(canRun));
            animators[0].SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
            animators[0].SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
        }
        else if (moving && running && canRun) // Running
        {
            PlayerRotation(aiming, cameraVector, newPlayerMovementVector);
            StaminaBar.instance.UseStamina(0.05f);
            controller.Move(newPlayerMovementVector * (0.5f * Time.deltaTime * 3.14f * movementSpeed * movementSpeedModifier) + PlayerTranslation(canRun));
            animators[0].SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
        }
        else if (moving && running && !canRun) // Attempting to run
        {
            PlayerRotation(aiming, cameraVector, newPlayerMovementVector);
            controller.Move(newPlayerMovementVector * (0.5f * Time.deltaTime * 3.14f * movementSpeed) + PlayerTranslation(canRun));
            animators[0].SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
        }
        else // Idle
        {
            controller.Move(PlayerTranslation(canRun));
            
            if (aiming)
                PlayerRotation(aiming, cameraVector, newPlayerMovementVector);
            animators[0].SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
        }
    }

    private Vector3 PlayerTranslation(bool canRun) //modify vertical motions
    {
        isGrounded = (controller.collisionFlags & CollisionFlags.Below) != 0;
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canRun) //jump
        {
            StaminaBar.instance.UseStamina(5.0f);
            speedVert = jumpForce;
            return transform.up * jumpImpulse;
        }
        else //falling
        {
            speedVert += gravity * Time.deltaTime;
            return transform.up * (speedVert * Time.deltaTime);
        }
    }
    
    private void PlayerRotation(bool aiming, Vector3 cameraVector, Vector3 movementVector)
    {
        Quaternion toRotation = aiming ? Quaternion.LookRotation(cameraVector, Vector3.up) : Quaternion.LookRotation(movementVector, Vector3.up); 
        controller.transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }

    private void weaponSelect(int number)
    {
        for (int i = 1; i < WeaponsBools.Length+1; i++)
        {
            WeaponsBools[i-1] = false;
            if (i == number)
            {
                WeaponsBools[i-1] = true;
            }
        }
        AxeObj.gameObject.SetActive(WeaponsBools[0]);
        BowObj.gameObject.SetActive(WeaponsBools[1]);
    }
}
