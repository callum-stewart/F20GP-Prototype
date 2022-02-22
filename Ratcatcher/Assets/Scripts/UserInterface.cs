using TMPro;
using UnityEngine;

public class UserInterface : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI info;

    float timer;
    bool timerSet = false;

    private void Start()
    {
        setInfo("Where am I?", 10f);
    }

    private void Update()
    {
        // timer
        if (timerSet && timer > 0f)
            timer -= Time.deltaTime;
        else if (timerSet && timer <= 0f){
            info.text = "";
            timerSet = false;
        }

    }

    // set the info text
    // optional parameter to set a time before it disappears
    public void setInfo(string text, float duration = 0f) {
        info.text = text;

        timer = duration;
        timerSet = true;
    }
}
