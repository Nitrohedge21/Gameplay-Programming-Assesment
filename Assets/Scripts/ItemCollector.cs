using UnityEngine;
using UnityEngine.UI;
public class ItemCollector : MonoBehaviour
{
    public Text scoreText;

    void Start()
    {
        IncreaseValue.goldAmount = 0;
        scoreText.text = "Golds : " + IncreaseValue.goldAmount;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Collectables")
        {
            IncreaseValue.goldAmount++;
            Destroy(other.gameObject);
            scoreText.text = "Golds : " + IncreaseValue.goldAmount;
        }
    }
}

//The part below was done thanks to Sam, he sorta explained what static classes are and why I should use it in this case.
//This was done in order to make the characters share the same value rather than having seperate ones.
public static class IncreaseValue
{
    private static int _goldAmount;
    public static int goldAmount
    {
        get
        {
            return _goldAmount;
        }
        set
        {
            _goldAmount = value;
        }
    }
}