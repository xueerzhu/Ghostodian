using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Wall[] walls;
    public Box[] boxes;
    public GameObject player;
    public bool levelComplete;
    void Start()
    {
        walls = GetComponentsInChildren<Wall>();
        boxes = GetComponentsInChildren<Box>();

    }


    void Update()
    {
        if (walls.All(x => x.isHit == true))
        {
            levelComplete = true;
            //SceneManager.LoadScene(SceneManager.GetActiveScene ().buildIndex + 1);
        }

        if (Input.GetButtonDown("Replay"))  //TODO: no controller replay configured yet
        {
            RestartLevel();
        }

    }

    void RestartLevel()
    {
        player.GetComponent<Movement>().ResetPlayer();
        walls.ToList().ForEach(x => x.isHit = false);
        boxes.ToList().ForEach(x => x.ResetBox());
    }
}
