using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobile : MonoBehaviour
{
    private static Mobile instance;
    private bool isMobile;

    void Awake()
    {
        isMobile = false;

        // If an instance already exists and it's not this one, destroy this object
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set the instance to this object and mark it to not be destroyed
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public bool GetIsMobile()
    {
        return isMobile;
    }

    public void SetIsMobile(bool isMobile)
    {
        this.isMobile = isMobile;
    }
}
