using Cinemachine;
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
        [SerializeField] protected CinemachineVirtualCameraBase _virtualCamera;

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

        void FixedUpdate()
        {
            if (_canLerp)
            {
                transform.position = Vector3.Lerp(transform.position, _lerpPosition, _lerpVelocity * Time.fixedDeltaTime);
                if (Vector3.SqrMagnitude(transform.position - _lerpPosition) < 2)
                {
                    _gameReferee.GameStateMechanic(RS_GameStates.ANCHOR_SHIP);
                    _canLerp = false;
                }
            }
        }

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            ValidateReferences();
            #endif
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Cargo>() != null && other.GetComponent<Cargo>().IsLoaded)
            {
                other.GetComponent<Cargo>().IsLoaded= false;
                EliminateCargo(other.GetComponent<Cargo>());
            }
        }

        #endregion

        #region RuntimeMethods

        protected override void ValidateTrigger(Collider other)
        {
            switch (_gameReferee.GetGameState)
            {
                case RS_GameStates.CANNON_CARGO:
                    ValidateTriggerInCannonCargo(other);
                    break;
            }
        }

        protected override void ValidateTriggerInCannonCargo(Collider other)
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

        #region PublicMethods

        public void EliminateCargo(Cargo cargo)
        {
            switch (cargo.cargoType)
            {
                case CargoTypes.CREW_MEMBER:
                    _hasCrew = false;
                    break;
                case CargoTypes.FUEL:
                    _hasFuel = false;
                    break;
                case CargoTypes.MEDICINE:
                    _hasMedicine = false;
                    break;
                case CargoTypes.SCREW_PART:
                    _hasScrew = false;
                    break;
                case CargoTypes.SUPPLIES:
                    _hasSupplies = false;
                    break;
            }
        }

        public void AddCargo(Cargo cargo)
        {
            switch (cargo.cargoType)
            {
                case CargoTypes.CREW_MEMBER:
                    _hasCrew = true;
                    break;
                case CargoTypes.FUEL:
                    _hasFuel = true;
                    break;
                case CargoTypes.MEDICINE:
                    _hasMedicine = true;
                    break;
                case CargoTypes.SCREW_PART:
                    _hasScrew = true;
                    break;
                case CargoTypes.SUPPLIES:
                    _hasSupplies = true;
                    break;
            }
        }

        public virtual void Navigate()
        {

        }

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

        public CinemachineVirtualCameraBase GetVirtualCamera
        {
            get { return _virtualCamera; }
        }

        #endregion
    }
}