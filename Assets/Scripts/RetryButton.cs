using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class RetryButton: MonoBehaviour
    {

        public void StartGame()
        {
            SceneManager.LoadScene("Game");
        }
    }
}