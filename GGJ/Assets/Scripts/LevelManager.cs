using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public Wall[] walls;
    public bool levelComplete;
    void Start()
    {
        walls = GetComponentsInChildren<Wall>();
    }

    // Update is called once per frame
    void Update()
    {
        if (walls.All(x => x.isHit == true))
        {
            levelComplete = true;
            Debug.Log("levelComplete");
        }

    }
}
