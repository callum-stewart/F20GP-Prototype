using TMPro;
using UnityEngine;

public class UserInterface : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI info;
    [SerializeField]
    private TextMeshProUGUI controls;
    private string controlText =
        @"Move Forward - W
Move Backwards - S
Move Left - A
Move Right - D
Flashlight - Right Click

Use Mouse To Look Around
Use E to interact
Press I to See This Again";

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

        if (Input.GetKey(KeyCode.I))
            controls.text = controlText;
        else if(Input.GetKeyUp(KeyCode.I))
            controls.text = "";
    }

    // set the info text
    // optional parameter to set a time before it disappears
    public void setInfo(string text, float duration = 0f) {
        info.text = text;

        timer = duration;
        timerSet = true;
    }
}
