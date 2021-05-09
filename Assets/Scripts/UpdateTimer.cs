using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class UpdateTimer : MonoBehaviour
{
    public UnityEvent OnDone;

    private float time = 60f;
    private bool started = false;

    // Update is called once per frame
    void Update()
    {
        if (!started)
        {
            var input = Input.GetAxisRaw("Horizontal");
            if (input > 0.1 || input < -0.1)
                started = true;
        }
        else if ((time - Time.deltaTime) > 0)
        {
            time -= Time.deltaTime;

            int seconds = Mathf.FloorToInt(time);
            int mseconds = Mathf.FloorToInt((time - seconds) * 10);

            var asset = GetComponent<TMP_Text>();
            asset.text = string.Format("0:{0}.{1}", seconds.ToString("00"), mseconds);
        }
        else
        {
            var asset = GetComponent<TMP_Text>();
            asset.text = "0:00.0";
            OnDone.Invoke();
        }
    }
}
