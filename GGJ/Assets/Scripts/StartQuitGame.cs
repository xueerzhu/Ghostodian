using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartQuitGame : MonoBehaviour
{
    public Transform quitGameTransform;
    public Transform startGameTransform;

    public Transform playerTransform;

    private void Update() {

        if (Vector3.Distance(playerTransform.position, startGameTransform.position) < 0.7f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene ().buildIndex + 1);
        }
        else if (Vector3.Distance(playerTransform.position, quitGameTransform.position) < 0.7f)
        {
            Debug.Log("quit");
            Application.Quit();
        }
    }
    
}
