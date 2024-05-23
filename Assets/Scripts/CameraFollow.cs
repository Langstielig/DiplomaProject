using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    //[SerializeField] private GameObject mainCharacter;
    [SerializeField, Min(0f)] private float sensitivity;
    [SerializeField, Min(0f)] private float maxYAngle;

    //private Vector3 offset;
    private float rotationX;
    private bool isTab = false;
    private bool isEsc = false;

    private void Update()
    {
        if(SceneManager.GetActiveScene().name != "ChooseCharacter")
        {
            isTab = CheckTabButton(isTab);
            isEsc = CheckEscButton(isEsc);
            if (!isTab && !isEsc)
            {
                Move();
            }
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

    private bool CheckEscButton(bool isEsc)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.parent.Rotate(Vector3.up * mouseX * sensitivity);

        rotationX -= mouseY * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -maxYAngle, maxYAngle);
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }
}
