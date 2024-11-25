using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace NAwakening.RecollectionSnooker
{
    #region Enums

    public enum CargoTypes
    {
        CREW_MEMBER = 4,
        FUEL = 3,
        MEDICINE = 2,
        SUPPLIES = 1,
        SCREW_PART = 0
        //SCREW_PART,
        //SUPPLIES,
        //MEDICINE,
        //FUEL,
        //CREW_MEMBER
    }

    #endregion

    #region Structs


    #endregion

    public class Cargo : Token
    {
        #region Knobs

        [Header("Knobs / Parameters")]
        public CargoTypes cargoType;

        #endregion

        #region References

        #endregion

        #region RuntimeVariables

        [Header("Runtime Variables")]
        [SerializeField] protected bool isLoaded, isOnIsland;

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
                case RS_GameStates.CANNON_CARGO:
                    ValidateTriggerInCannonCargo(other);
                    break;
                case RS_GameStates.CANNON_BY_NAVIGATION:
                    ValidateTriggerInCannonNavigation(other); 
                    break;
            }
        }

        protected override void ValidateTriggerInCannonCargo(Collider other)
        {
            if (!isLoaded)
            {
                if (other.gameObject.CompareTag("MonsterLimb"))
                {
                    _gameReferee.SetMoveToMoveCounter = true;
                }
                else if (other.gameObject.CompareTag("Player"))
                {
                    _gameReferee.SetMoveToOrganizeCargo = true;
                }
                else if (other.gameObject.GetComponent<Token>() != null)
                {
                    _gameReferee.GetTargetGroup.AddMember(other.gameObject.transform, 1f, 1f);
                }
            }
        }

        protected override void ValidateTriggerInCannonNavigation(Collider other)
        {
            if (!isLoaded)
            {
                if (other.gameObject.CompareTag("MonsterLimb"))
                {
                    _gameReferee.SetMoveToMoveCounter = true;
                }
                else if (other.gameObject.GetComponent<Token>() != null)
                {
                    _gameReferee.GetTargetGroup.AddMember(other.gameObject.transform, 1f, 1f);
                }
            }
        }

        #endregion

        #region PublicMethods

        #endregion

        #region GettersSetters

        public bool IsLoaded
        {
            get { return isLoaded; }
            set { isLoaded = value; }
        }

        public bool IsOnIsland
        {
            get { return isOnIsland; }
            set { isOnIsland = value; }
        }

        #endregion
    }
}