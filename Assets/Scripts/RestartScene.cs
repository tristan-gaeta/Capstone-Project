using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    // Start is called before the first frame update
    public static void RestartTheScene() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}
