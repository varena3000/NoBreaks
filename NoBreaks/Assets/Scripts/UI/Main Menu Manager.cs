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
        //Message to console as quit will not occur in unity editor
        Debug.Log("Application is quitting...");

        // Close the program
        Application.Quit();

        if (Application.isEditor)
        {
            // Only for testing funtionality in Editor
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}

