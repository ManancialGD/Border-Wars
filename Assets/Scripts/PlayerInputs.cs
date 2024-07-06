using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    private Vector2 input;
    private Vector2 lastInput = new Vector2 (0, 0);

    void Update()
    {
        InputUpdate();
    }

    /// <summary>
    /// This will make sure that the input does not have gravity
    /// 
    /// This works detecting if the input is getting bigger, it will set to 1 if is positive, if is negative will set to 0
    /// If is decreasing, then it will be -1 if it's negative, but if it's positive, it's set to 0
    /// 
    /// Why? maybe a controller checker here would be grate, if it's using controller, this would be ignored
    /// But if it's keyboard, it's really bad the gravity, since the button is pressed or not pressed, there's no inbetween
    /// </summary>
    private void InputUpdate()
    {
        if (Input.GetAxis("Horizontal") > lastInput.x)
        {
            if (Input.GetAxis("Horizontal") > 0.01f) input.x = 1;
            else if (Input.GetAxis("Horizontal") < -0.01f) input.x = 0;
        }
        else if (Input.GetAxis("Horizontal") < lastInput.x)
        {
            if (Input.GetAxis("Horizontal") < -0.01f) input.x = -1;
            else if (Input.GetAxis("Horizontal") > 0.01f) input.x = 0;
        }

        if (Input.GetAxis("Vertical") > lastInput.y)
        {
            if (Input.GetAxis("Vertical") > 0.01f) input.y = 1;
            else if (Input.GetAxis("Vertical") < -0.01f) input.y = 0;
        }
        else if (Input.GetAxis("Vertical") < lastInput.y)
        {
            if (Input.GetAxis("Vertical") < -0.01f) input.y = -1;
            else if (Input.GetAxis("Vertical") > 0.01f) input.y = 0;
        }

        lastInput.x = Input.GetAxis("Horizontal");
        lastInput.y = Input.GetAxis("Vertical");
    }

    public Vector2 GetPlayerInput()
    {
        return input;
    }
}
