using System.Globalization;
using TMPro;
using UnityEngine;

public class ScoreScreenController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreLabel;
    
    private void Start()
    {
        scoreLabel.text = PlayerPrefs.GetInt("SCORE", 0).ToString(CultureInfo.InvariantCulture);
    }
}
