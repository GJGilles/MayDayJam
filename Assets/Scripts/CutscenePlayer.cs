using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class CutscenePlayer : MonoBehaviour
{
    public List<GameObject> scenes = new List<GameObject>();
    public GameObject textArea;
    public float textSpeed = 0.1f; //char per sec
    public float pauseLength = 1f; // length of pause
    public float transSpeed = 0.5f; //opacity per sec

    private int index = 0;
    private GameObject current;

    private bool textDone = false;
    private float textTime = 0f;
    private float pauseTime = 0f;
    private float transTime = 0f;

    public UnityEvent OnDone;

    private void Start()
    {
        if (OnDone == null)
            OnDone = new UnityEvent();
    }

    void Update()
    {
        if (current == null)
        {
            current = scenes[index];
            index++;

            textDone = false;
            textTime = 0f;
            pauseTime = 0f;
            transTime = 0f;
        }
        else if (!textDone)
        {
            textTime += Time.deltaTime;

            var text = current.GetComponent<UnityEngine.UI.Text>().text;
            var len = Math.Min(Mathf.FloorToInt(textTime * textSpeed), text.Length);

            textArea.GetComponent<TMP_Text>().text = text.Substring(0, len);

            if (len >= text.Length)
                textDone = true;
        }
        else if (pauseTime < pauseLength)
        {
            pauseTime += Time.deltaTime;
        }
        else if (index >= scenes.Count)
        {
            OnDone.Invoke();
            Destroy(this);
        }
        else if (transTime < (1f / transSpeed))
        {
            transTime += Time.deltaTime;

            current.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1 - transTime * transSpeed);
            textArea.GetComponent<TMP_Text>().faceColor = new Color32(255, 255, 255, Convert.ToByte(Mathf.RoundToInt(255f * (1 - transTime * transSpeed))));
        }
        else
        {
            textArea.GetComponent<TMP_Text>().text = "";
            textArea.GetComponent<TMP_Text>().faceColor = new Color32(255, 255, 255, 255);

            current = null;
        }
    }
}
