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
        [SerializeField] protected GameObject _contactpointPanel, _flickTokenPanel, _organicaCargoPanel, _anchorShipPanel;
        [SerializeField] protected Image[] _hearts;
        [SerializeField] protected Sprite _emptyHeart;
        [SerializeField] protected GameObject _pausePanel, _pauseButton;
        [SerializeField] protected RS_MobileInputHandler _mobileInputHandler;

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

        public void ConfirmCargoLoad()
        {
            if (_mobileInputHandler.MovingToken)
            {
                _mobileInputHandler.MovingToken = false;
                _mobileInputHandler.GetChosenToken.SetHighlight(false);
                _mobileInputHandler.GetChosenToken.IsAvalaibleForFlicking = false;
                _mobileInputHandler.GetChosenToken.StateMechanic(TokenStateMechanic.SET_PHYSICS);
                _gameReferee.CargoLoaded();
            }
        }

        public void ConfirmAnchor()
        {
            if (_gameReferee.MoveToLeaveCargoAtIsland)
            {
                _gameReferee.GameStateMechanic(RS_GameStates.LEAVE_CARGO_AT_ISLAND);
            }
            else
            {
                _gameReferee.GameStateMechanic(RS_GameStates.SHIFT_MONSTER_PARTS);
            }
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

        public void ActivateOrganizePanel()
        {
            _organicaCargoPanel.SetActive(true);
        }

        public void ActivateAnchorPanel()
        {
            _anchorShipPanel.SetActive(true);
        }

        public void DeactivateContactPointPanel()
        {
            _contactpointPanel.SetActive(false);
        }

        public void DeactivateFlickTokenPanel()
        {
            _flickTokenPanel.SetActive(false);
        }

        public void DeactivateOrganizePanel()
        {
            _organicaCargoPanel.SetActive(false);
        }

        public void DeactivateAnchorPanel()
        {
            _anchorShipPanel.SetActive(false);
        }

        public void ReloadScene(int sceneId)
        {
            SceneManager.LoadScene(sceneId);
        }

        public void HeartLost()
        {
            _hearts[_heartid].sprite = _emptyHeart;
            _heartid++;
        }

        public void Quit()
        {
            Application.Quit();
        }

        #endregion
    }
}

