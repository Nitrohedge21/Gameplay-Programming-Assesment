using UnityEngine;
public class GameManager : MonoBehaviour
{
    GameObject canvas;
    void Start()
    {
        canvas = GameObject.Find("Canvas (UI)");
        //Finds the canvas object.
    }
    void Update()
    {
        isGameOver();
    }
    
    bool isGameOver()
    {
        //If the goldAmount reaches 68, activates the victory screen image and ends the game.
        if (IncreaseValue.goldAmount == 68)
        {
            canvas.transform.GetChild(1).gameObject.SetActive(true);
            Time.timeScale = 0;
            return true;
        }
        else
        {
            return false;
        }
    }
}
