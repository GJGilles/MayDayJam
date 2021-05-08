using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class StartCutscene : MonoBehaviour
    {

        public void GoToScene()
        {
            SceneManager.LoadScene("StartStory");
        }
    }
}