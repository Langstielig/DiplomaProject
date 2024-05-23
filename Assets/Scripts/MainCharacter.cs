using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainCharacter : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    private CharacterController characterController;
    private Animator animator;
    private int damage = 1;
    private bool isTab = false;
    private bool isEsc = false;

    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private Camera thirdPersonCamera;

    [SerializeField] private UnityEvent OnLadderEnter;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name != "ChooseCharacter")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        thirdPersonCamera.enabled = false;
        if (SceneManager.GetActiveScene().name == "ChooseCharacter")
        {
            firstPersonCamera.enabled = false;
        }
        Vector3 startPosition = transform.position;
        startPosition.y = 0;
        characterController.Move(startPosition);
        DataHolder.mainCharacterHealth = 5;
        DataHolder.mainCharacterExperiencePoints = 0;
        DataHolder.mainCharacterLevel = 1;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "ChooseCharacter")
        {
            ChangeAnimation();
            ChangeCamera();
        }
    }

    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name != "ChooseCharacter")
        {
            isTab = CheckTabButton(isTab);
            isEsc = CheckEscBatton(isEsc);
            if (!isTab && !isEsc)
            {
                Move();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy" && !Input.GetMouseButton(1))
        {
            GameObject gameAspects = GameObject.FindGameObjectWithTag("GameAspects");
            if (gameAspects != null)
            {
                GameAspects gameAspectsBehavior = gameAspects.GetComponent<GameAspects>();
                gameAspectsBehavior.RemoveHeart();
            }
        }

        if(other.gameObject.tag == "Potion")
        {
            Destroy(other.gameObject);
            DataHolder.countOfPotions++;
            GameObject intentory = GameObject.FindGameObjectWithTag("Inventory");
            if(intentory != null)
            {
                Inventory inventoryBehavior = intentory.GetComponent<Inventory>();
                inventoryBehavior.AddPotion();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Enemy" && Input.GetMouseButtonDown(0))
        {
            EnemyBehavior enemy = other.gameObject.GetComponent<EnemyBehavior>();
            if(enemy != null)
            {
                enemy.ChangeToHitAnimation();
                enemy.GetHit(damage);
            }
        }

        if(other.gameObject.tag == "Chest" && Input.GetKeyDown(KeyCode.E))
        {
            ChestBehavior chest = other.gameObject.GetComponent<ChestBehavior>();
            if(chest != null)
            {
                chest.ChangeAnimation();
            }
        }

        if (other.gameObject.tag == "NextLevel" && Input.GetKeyDown(KeyCode.E))
        {
            //OnLadderEnter.Invoke();
        }
    }

    private bool CheckTabButton(bool isTab)
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            return !isTab;
        }
        else
        {
            return isTab;
        }
    }

    private bool CheckEscBatton(bool isEsc)
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            return !isEsc;
        }
        else
        {
            return isEsc;
        }
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        moveDirection.y -= 9.81f * Time.fixedDeltaTime;

        characterController.Move(moveDirection * speed * Time.fixedDeltaTime);
    }

    private void ChangeAnimation()
    {
        animator.SetBool("isForwardMove", Input.GetKey(KeyCode.W));
        animator.SetBool("isBackwardMove", Input.GetKey(KeyCode.S));
        animator.SetBool("isLeftMove", Input.GetKey(KeyCode.A));
        animator.SetBool("isRightMove", Input.GetKey(KeyCode.D));
        animator.SetBool("isAttack", Input.GetMouseButtonDown(0));
        animator.SetBool("isDefend", Input.GetMouseButton(1));
        animator.SetBool("isInteract", Input.GetKeyDown(KeyCode.E));
    }

    private void ChangeCamera()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            firstPersonCamera.enabled = !firstPersonCamera.enabled;
            thirdPersonCamera.enabled = !thirdPersonCamera.enabled;
        }
    }
}
