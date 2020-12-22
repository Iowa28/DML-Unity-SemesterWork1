using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int goldMaxCount;
    [SerializeField]
    private int currentGold;
    [SerializeField]
    private Text goldText;

    public void AddGold(int goldToAdd)
    {
        if (currentGold < goldMaxCount)
        {
            currentGold += goldToAdd;
            goldText.text = "Gold: " + currentGold;

            if (currentGold == goldMaxCount)
            {
                goldText.text = "All gold is found!";
            }
        }
    }
}
