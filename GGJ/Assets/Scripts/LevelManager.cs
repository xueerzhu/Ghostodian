using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public float levelEndDelay = 7.0f;
    public Wall[] walls;
    public Box[] boxes;
    public GameObject player;
    public bool levelComplete;

    private bool shouldPlayEndLevelSoundEffect = false;

    private bool shouldAdvanceToNextLevel = false;
    private bool shouldStartEndLevelWait = true;

    void Start()
    {
        walls = GetComponentsInChildren<Wall>();
        boxes = GetComponentsInChildren<Box>();

        PlayBackgroundMusic.Instance.Play();
        

    }


    void Update()
    {
        if (walls.All(x => x.isHit == true) && shouldStartEndLevelWait)
        {
            shouldStartEndLevelWait = false;
            levelComplete = true;
            StartCoroutine("WaitToEndLevel");
            Debug.Log("level complete is playing");
            GetComponent<AudioSource>().Play();
        }

        if (levelComplete && shouldAdvanceToNextLevel) {
            SceneManager.LoadScene(SceneManager.GetActiveScene ().buildIndex + 1);
        }

        if (Input.GetButtonDown("Replay"))  //TODO: no controller replay configured yet
        {
            RestartLevel();
        }

    }

    void RestartLevel()
    {
        // player.GetComponent<Movement>().ResetPlayer();
        // walls.ToList().ForEach(x => x.isHit = false);
        // boxes.ToList().ForEach(x => x.ResetBox());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator WaitToEndLevel()
    {
        yield return new WaitForSeconds(levelEndDelay);
        shouldAdvanceToNextLevel = true;
    }

}
