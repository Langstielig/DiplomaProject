using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private float maxDistanceToHit;
    [SerializeField] private float maxDistanceToMove;
    [SerializeField] private GameObject potion;

    private GameObject mainCharacter;
    private Animator animator;
    private int health = 3;

    private bool isHit = false;

    private void Start()
    {
        mainCharacter = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(health <= 0)
        {
            ChangeToDestroyAnimation();
            GameObject gameAspects = GameObject.FindGameObjectWithTag("GameAspects");
            if(gameAspects != null)
            {
                GameAspects gameAspectsBehavior = gameAspects.GetComponent<GameAspects>();
                gameAspectsBehavior.AddExperiencePoint();
            }
            Invoke("Destroy", 1);
        }
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(mainCharacter.transform.position, transform.position);
        if (distance <= maxDistanceToMove && distance > maxDistanceToHit)
        {
            ChangeToMoveAnimation();
            Move();
        }
        else if(distance <= maxDistanceToHit && !isHit) 
        {
            ChangeToAttackAnimation();
        }
    }

    private void Move()
    {
        transform.LookAt(mainCharacter.transform);
        transform.position = Vector3.MoveTowards(transform.position, mainCharacter.transform.position, speed);
    }

    private void ChangeToMoveAnimation()
    {
        animator.SetBool("isMove", true);
        animator.SetBool("isAttack", false);
        animator.SetBool("isHit", false);
    }

    private void ChangeToAttackAnimation()
    {
        animator.SetBool("isMove", false);
        animator.SetBool("isAttack", true);
        animator.SetBool("isHit", false);
    }

    public void ChangeToHitAnimation()
    {
        animator.SetBool("isMove", false);
        animator.SetBool("isAttack", false);
        animator.SetBool("isHit", true);
        isHit = true;
        Invoke("SetIsHitFalse", 1);
    }

    public void ChangeToDestroyAnimation()
    {
        animator.SetBool("isMove", false);
        animator.SetBool("isAttack", false);
        animator.SetBool("isHit", false);
        animator.SetBool("isDead", true);
        isHit = true;
    }

    private void SetIsHitFalse()
    {
        isHit = false;
    }

    public void GetHit(int damage)
    {
        health -= damage;
        Debug.Log("Enemy health " + health);
    }

    private void GeneratePotion()
    {
        //if(Random.Range(1, 10) > 6)
        //{
            GameObject newPotion = Instantiate(potion);
            newPotion.transform.position = transform.position;
        //}
    }

    private void Destroy()
    {
        GeneratePotion();
        Destroy(gameObject);
    }
}
