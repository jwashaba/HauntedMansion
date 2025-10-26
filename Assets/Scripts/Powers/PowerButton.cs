using UnityEngine;
using UnityEngine.UI;
using TMPro; // Only if using TextMeshPro

public class PowerButton : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text nameText;        // If using TextMeshPro
    public TMP_Text descriptionText; // If using TextMeshPro

    private CursedPower power;

    public void Setup(CursedPower p)
    {
        power = p;

        if (iconImage != null) iconImage.sprite = p.icon;
        if (nameText != null) nameText.text = p.powerName;
        if (descriptionText != null) descriptionText.text = p.description;

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        GameManagerScript.Instance.OnPowerChosen(power);
    }
}