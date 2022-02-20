using UnityEngine;
using TMPro;
using LastBastion.Dialogue;
public class ChoiceSlider : MonoBehaviour
{
    private DialogueContent.ChoiceSlider choice;
    private TextMeshProUGUI text;
    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = choice.Text;
        DialogueContent.ChoiceSlider.OnValueChanged += ChoiceSlider_OnValueChanged;
    }


    private void ChoiceSlider_OnValueChanged(DialogueContent.ChoiceSlider currentSlider)
    {
        if (currentSlider == choice)
        {
            text.text = choice.Text;
        }
    }
    public void Init(DialogueContent.ChoiceSlider choice)
    {
        this.choice = choice;
    }
}
