using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Wall[] walls;
    public bool levelComplete;
    void Start()
    {
        walls = GetComponentsInChildren<Wall>();
    }


    void Update()
    {
        if (walls.All(x => x.isHit == true))
        {
            levelComplete = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene ().buildIndex + 1);
        }

    }
}
