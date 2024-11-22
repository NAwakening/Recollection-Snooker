using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace NAwakening.RecollectionSnooker
{
    #region Enums


    #endregion

    #region Structs


    #endregion

    public class Ship : Token
    {
        #region Knobs


        #endregion

        #region References

        [SerializeField] protected Transform[] _loadingCargoPositions;

        #endregion

        #region RuntimeVariables

        protected bool _hasCrew, _hasScrew, _hasFuel, _hasMedicine, _hasSupplies;
        public Cargo cargoToLoad;

        #endregion

        #region UnityMethods

        void Start()
        {
            base.InitializeToken();
        }

        void Update()
        {

        }

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            ValidateReferences();
            #endif
        }

        protected override void ValidateTrigger(Collider other)
        {
            switch (_gameReferee.GetGameState)
            {
                case RS_GameStates.CANNON_CARGO:
                    ValidateTriggerInCannon(other);
                    break;
            }
        }

        protected override void ValidateTriggerInCannon(Collider other)
        {
            if (other.GetComponent<Cargo>() != null && !other.GetComponent<Cargo>().IsLoaded)
            {
                if (_gameReferee.CheckIfCargoCanBeLoaded(other.GetComponent<Cargo>()))
                {
                    cargoToLoad = other.GetComponent<Cargo>();
                }
            }
        }

        #endregion

        #region RuntimeMethods


        #endregion

        #region PublicMethods

        #endregion

        #region GettersSetters

        public bool GetHasCrew
        {
            get { return _hasCrew; }
        }

        public bool GetHasScrew
        {
            get { return _hasScrew; }
        }

        public bool GetHasFuel
        {
            get { return _hasFuel; }
        }

        public bool GetHasMedicine
        {
            get { return _hasMedicine;}
        }

        public bool GetHasSupplies
        {
            get { return _hasSupplies; }
        }

        public Transform[] GetLoadingCargoPositions
        {
            get { return _loadingCargoPositions; }
        }

        #endregion
    }
}