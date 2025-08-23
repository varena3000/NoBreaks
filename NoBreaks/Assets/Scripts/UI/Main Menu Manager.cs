using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuManager : MonoBehaviour
{
    //Title:Start Menu in Unity
    //Author: Brackeys
    //Date: 21-08-25
    //Code Version: N/A
    //Available at: https://www.youtube.com/watch?v=zc8ac_qUXQY 

    public void StartGame ()
    {
        // Load the next scene referencing the build index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnApplicationQuit()
    {
        // Close the program
        Application.Quit();
    }
}

