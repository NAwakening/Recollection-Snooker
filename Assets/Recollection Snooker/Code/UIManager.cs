using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace NAwakening.RecollectionSnooker
{
    public class UIManager : MonoBehaviour
    {
        #region References

        [SerializeField] protected RS_GameReferee _gameReferee;
        [SerializeField] protected GameObject _contactpointPanel, _flickTokenPanel;
        [SerializeField] protected RawImage[] _hearts;
        [SerializeField] protected GameObject _pausePanel, _pauseButton;

        #endregion

        #region RuntimeVariables

        protected int _heartid;
        protected bool _pause;

        #endregion

        #region PublicMethods

        public void ReturnToChoseToken()
        {
            _gameReferee.GameStateMechanic(RS_GameStates.CHOOSE_TOKEN);
        }

        public void ReturnToContactPoint()
        {
            _gameReferee.GameStateMechanic(RS_GameStates.CONTACT_POINT_TOKEN);
        }

        public void Pause()
        {
            if (!_pause)
            {
                _pause = true;
                _pausePanel.SetActive(true);
                _pauseButton.SetActive(false);
                Time.timeScale = 0.0f;
            }
            else
            {
                _pause = false;
                _pausePanel.SetActive(false);
                _pauseButton.SetActive(true);
                Time.timeScale = 1.0f;
            }
        }

        public void ActivateContactPointPanel()
        {
            _contactpointPanel.SetActive(true);
        }

        public void ActivateFlickTokenPanel()
        {
            _flickTokenPanel.SetActive(true);
        }

        public void DeactivateContactPointPanel()
        {
            _contactpointPanel.SetActive(false);
        }

        public void DeactivateFlickTokenPanel()
        {
            _flickTokenPanel.SetActive(false);
        }

        public void ReloadScene(int sceneId)
        {
            SceneManager.LoadScene(sceneId);
        }

        public void HeartLost()
        {
            _hearts[_heartid].color = Color.black;
            _heartid++;
        }

        public void Quit()
        {
            Application.Quit();
        }

        #endregion
    }
}

