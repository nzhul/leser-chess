using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public string VictoryText = "VICTORY";
    public string DefeatText = "DEFEAT";

    public Text OutcomeText;
    public Text OutcomeReasonText;

    public void UpdateText(Faction winner, string reason)
    {
        this.OutcomeText.text = this.GetOutcomeText(winner);
        this.OutcomeReasonText.text = reason;
    }

    public void ActivateWithDelay()
    {
        gameObject.SetActive(true);
        StartCoroutine(Activate());
    }

    IEnumerator Activate()
    {
        yield return new WaitForSeconds(1);
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    private string GetOutcomeText(Faction winner)
    {
        if (winner == Faction.Human)
        {
            return VictoryText;
        }
        else
        {
            return DefeatText;
        }
    }
}