using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Security.Cryptography;

namespace NAwakening.RecollectionSnooker
{
    #region Enums


    #endregion

    #region Structs

    [System.Serializable]
    public struct GameplayAttributes
    {
        public bool isAvailableForFlicking;
        public bool isOutOfTheBoard;
        public bool isBeingDragged;
    }

    #endregion

    [RequireComponent(typeof(RS_TokenFiniteStateMachine))]
    public class Token : MonoBehaviour
    {
        #region Knobs


        #endregion

        #region References

        [SerializeField,HideInInspector] protected RS_TokenFiniteStateMachine _tokenPhysicalFSM;
        [SerializeField] protected CinemachineFreeLook _freeLookCamera;
        [SerializeField] protected RS_GameReferee _gameReferee;
        [SerializeField] protected Transform _flagTransformValues;

        #endregion

        #region RuntimeVariables

        [Header("Runtime Variables")]
        [SerializeField] protected GameplayAttributes _gameplayAttributes;

        protected Transform _flagTransform;
        [SerializeField] protected Flag _contactedFlag;

        #endregion

        #region UnityMethods

        void Start()
        {
            InitializeToken();
        }

        void FixedUpdate()
        {
            
        }

        private void OnCollisionEnter(Collision other)
        {
            //ValidateCollision(other);
        }

        private void OnTriggerEnter(Collider other)
        {
            ValidateTrigger(other);
        }

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            ValidateReferences();
            #endif
        }

        #endregion

        #region RuntimeMethods

        protected virtual void ValidateReferences()
        {
            if (_gameReferee == null)
            {
                _gameReferee = GameObject.FindAnyObjectByType<RS_GameReferee>();
            }
            if (_flagTransformValues == null)
            {
                _flagTransformValues = transform.GetChild(1).transform;
            }
            if(_freeLookCamera == null)
            {
                _freeLookCamera = transform.GetComponentInChildren<CinemachineFreeLook>();
            }
        }

        protected virtual void InitializeToken()
        {
            if (_tokenPhysicalFSM == null)
            {
                _tokenPhysicalFSM = GetComponent<RS_TokenFiniteStateMachine>();
            }
            ValidateReferences();
        }

        protected virtual void ValidateTrigger(Collider other)
        {
            switch (_gameReferee.GetGameState)
            {
                case RS_GameStates.FLICK_TOKEN:
                    ValidateTriggerWithFlag(other);
                    break;
            }
        }

        protected virtual void ValidateTriggerWithFlag(Collider other)
        {
            if ((this as Cargo || this as ShipPivot) && IsAvalaibleForFlicking)
            {
                if (other.gameObject.CompareTag("Flag"))
                {
                    _flagTransform = other.gameObject.transform;
                    _flagTransformValues.forward = _flagTransform.forward;
                    _flagTransformValues.position = _flagTransform.position;
                    _flagTransformValues.Rotate(_flagTransformValues.right, -90f, Space.Self);
                    _tokenPhysicalFSM.ThrowTokenAtSpecifcPosition(_flagTransformValues.forward * (Mathf.Abs(_contactedFlag.DeltaXDegrees + 1f) * 5.0f), other.gameObject.transform.position);

                    if (this as Cargo)
                    {
                        _gameReferee.GameStateMechanic(RS_GameStates.CANNON_CARGO);
                    }
                    else
                    {
                        _gameReferee.GameStateMechanic(RS_GameStates.CANNON_BY_NAVIGATION);
                    }
                    
                }
            }
        }

        protected virtual void ValidateTriggerInCannon(Collider other)
        {
            
        }

        #endregion

        #region PublicMethods

        public void StateMechanic(TokenStateMechanic value)
        {
            _tokenPhysicalFSM.StateMechanic(value);
        }

        public void SetHighlight(bool value)
        {
            _tokenPhysicalFSM.SetHighlight(value);
        }

        #endregion

        #region GettersSetters

        public bool IsStill
        {
            get { return _tokenPhysicalFSM.IsStill; }
        }

        public bool IsAvalaibleForFlicking
        {
            get { return _gameplayAttributes.isAvailableForFlicking; }
            set { _gameplayAttributes.isAvailableForFlicking = value; }
        }

        public CinemachineFreeLook GetFreeLookCamera
        {
            get { return _freeLookCamera; }
        }

        #endregion
    }
}