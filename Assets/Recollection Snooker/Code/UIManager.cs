using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NAwakening.RecollectionSnooker
{
    public class UIManager : MonoBehaviour
    {
        #region References

        [SerializeField] protected RS_GameReferee _gameReferee;
        [SerializeField] protected GameObject _contactpointPanel, _flickTokenPanel;

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

        public void LoadScene(int sceneId)
        {
            LoadScene(sceneId);
        }

        public void Quit()
        {
            Application.Quit();
        }

        #endregion
    }
}

