﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        Debug.Log("Start game!");
    }

}
