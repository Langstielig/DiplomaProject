using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehavior : MonoBehaviour
{
    private Animator animator;
    public bool isOpened = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public int OpenChest()
    {
        ChangeAnimation();
        int index = GenerateScroll();
        isOpened = true;
        return index;
    }

    public void ChangeAnimation()
    {
        animator.SetBool("isOpened", true);
    }

    public int GenerateScroll()
    {
        bool flag = true;
        int index = 0;

        while(flag)
        {
            index = Random.Range(0, 7);
            Debug.Log("Index is " + index);
            flag = DataHolder.scrolls[index];
        }

        DataHolder.scrolls[index] = true;
        return index;
    }
}
