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
    private int currentLevel = 1;
    private int damage = 1;

    private EnemyBehavior closeEnemyBehavior = null;
    private ChestBehavior closeChestBehavior = null;
    private GameObject ladder = null;

    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private Camera thirdPersonCamera;

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
    }

    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name != "ChooseCharacter")
        {
            GameObject intentory = GameObject.FindGameObjectWithTag("Inventory");
            Inventory inventoryBehavior = intentory.GetComponent<Inventory>();

            GameObject pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
            PauseMenuScript pauseMenuBehavior = pauseMenu.GetComponent<PauseMenuScript>();

            if (!inventoryBehavior.isOpen && !pauseMenuBehavior.isOpen)
            {
                Move();
                ChangeAnimation();
                ChangeCamera();
                Hit();
                InteractWithObject();
            }

            if(DataHolder.mainCharacterHealth == 0)
            {
                Defeate();
            }

            if(DataHolder.mainCharacterLevel != currentLevel)
            {
                LevelUp();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            closeEnemyBehavior = other.GetComponent<EnemyBehavior>();
            if (!Input.GetMouseButton(1))
            {
                Invoke("GetHit", 1);
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

        if (other.gameObject.tag == "Chest")
        {
            closeChestBehavior = other.GetComponent<ChestBehavior>();
        }

        if(other.gameObject.tag == "NextLevel")
        {
            ladder = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            closeEnemyBehavior = null;
        }

        if(other.gameObject.tag == "Chest")
        {
            closeChestBehavior = null;
        }

        if(other.gameObject.tag == "NextLevel")
        {
            ladder = null;
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

    private void Hit()
    {
        if (closeEnemyBehavior != null && Input.GetMouseButtonDown(0) && !closeEnemyBehavior.isHit)
        {
            closeEnemyBehavior.ChangeToHitAnimation();
            closeEnemyBehavior.GetHit(damage);
        }
    }

    private void InteractWithObject()
    {
        if(closeChestBehavior != null && Input.GetKeyDown(KeyCode.E) && !closeChestBehavior.isOpened)
        {
            int index = closeChestBehavior.OpenChest();
            GameObject intentory = GameObject.FindGameObjectWithTag("Inventory");
            if (intentory != null)
            {
                Inventory inventoryBehavior = intentory.GetComponent<Inventory>();
                inventoryBehavior.AddScroll(index);
            }
        }

        if(ladder != null && Input.GetKeyDown(KeyCode.E))
        {
            DataHolder.sceneNumber++;
            SceneManager.LoadScene(DataHolder.sceneNumber);
        }
    }

    private void Defeate()
    {
        SceneManager.LoadScene(5);
    }

    private void LevelUp()
    {
        damage++;
        currentLevel = DataHolder.mainCharacterLevel;
    }

    private void GetHit()
    {
        GameObject gameAspects = GameObject.FindGameObjectWithTag("GameAspects");
        if (gameAspects != null)
        {
            GameAspects gameAspectsBehavior = gameAspects.GetComponent<GameAspects>();
            gameAspectsBehavior.RemoveHeart();
        }
    }
}
