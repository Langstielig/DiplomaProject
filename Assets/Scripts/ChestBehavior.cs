using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehavior : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimation()
    {
        animator.SetBool("isOpened", true);
    }

    public void GenerateScroll()
    {

    }
}
