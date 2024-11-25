using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NAwakening.RecollectionSnooker
{
    #region Enums


    #endregion

    #region Structs


    #endregion

    public class ShipPivot : Token
    {
        #region Knobs


        #endregion

        #region References

        #endregion

        #region RuntimeVariables

        #endregion

        #region UnityMethods

        void Start()
        {
            base.InitializeToken();
        }

        void Update()
        {

        }

        //private void OnCollisionEnter(Collision other)
        //{
        //    ValidateCollision(other);
        //}

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            ValidateReferences();
            #endif
        }

        private void OnTriggerEnter(Collider other)
        {
            ValidateTrigger(other);
        }

        #endregion

        #region RuntimeMethods

        protected override void ValidateTrigger(Collider other)
        {
            switch (_gameReferee.GetGameState)
            {
                case RS_GameStates.FLICK_TOKEN:
                    ValidateTriggerWithFlag(other);
                    break;
                case RS_GameStates.CANNON_BY_NAVIGATION:
                    ValidateTriggerInCannonNavigation(other);
                    break;
            }
        }

        protected override void ValidateTriggerInCannonNavigation(Collider other)
        {
            if (other.gameObject.CompareTag("Island"))
            {
                _gameReferee.MoveToLeaveCargoAtIsland = true;
            }
            else if (other.gameObject.GetComponent<Token>() != null)
            {
                _gameReferee.GetTargetGroup.AddMember(other.gameObject.transform, 1f, 1f);
            }
        }

        #endregion

        #region PublicMethods

        #endregion

        #region GettersSetters

        #endregion
    }
}