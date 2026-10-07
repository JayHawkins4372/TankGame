using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    //call the pause function
    public void Pause()
    {
        //pause the game
        Time.timeScale = 0;
    }
    //call the resume function
    public void Resume()
    {
        //resume the game
        Time.timeScale = 1;
    }
}
