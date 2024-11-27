using NAwakening.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NAwakening.RecollectionSnooker
{
    public class ChangeScenes : MonoBehaviour
    {
        #region PublicMethods

        public void ChangeScene(int sceneId)
        {
            SceneManager.LoadScene(sceneId);
        }

        public void Quit()
        {
            Application.Quit();
        }

        #endregion
    }
}

