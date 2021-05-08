using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene("EndStory");
    }
}
