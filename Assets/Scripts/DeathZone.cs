using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public MainManager Manager;

    private void Update()
    {
        if (!Manager)
        {
            Manager = GameObject.Find("MainManager").GetComponent<MainManager>();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(other.gameObject);
        Manager.GameOver();
    }
}
