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

        [SerializeField] protected float _lerpVelocity;

        #endregion

        #region References

        [SerializeField] protected RS_TokenFiniteStateMachine _tokenPhysicalFSM;
        [SerializeField] protected CinemachineFreeLook _freeLookCamera;
        [SerializeField] protected RS_GameReferee _gameReferee;

        #endregion

        #region RuntimeVariables

        [Header("Runtime Variables")]
        [SerializeField] protected GameplayAttributes _gameplayAttributes;

        [SerializeField] protected Flag _contactedFlag;
        protected bool _canLerp;
        protected Vector3 _lerpPosition;

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
            //if (_flagTransformValues == null)
            //{
            //    _flagTransformValues = transform.GetChild(1).transform;
            //}
            //if(_freeLookCamera == null)
            //{
            //    _freeLookCamera = transform.GetComponentInChildren<CinemachineFreeLook>();
            //}
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
                    _tokenPhysicalFSM.ThrowTokenAtSpecifcPosition(other.gameObject.transform.GetChild(0).forward * (Mathf.Abs(_contactedFlag.DeltaXDegrees + 1f) * 5.0f), other.gameObject.transform.position);
                    Debug.DrawRay(other.gameObject.transform.GetChild(0).position, other.gameObject.transform.GetChild(0).forward * (Mathf.Abs(_contactedFlag.DeltaXDegrees + 1f) * 5.0f), Color.yellow, 5.0f);

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

        protected virtual void ValidateTriggerInCannonCargo(Collider other)
        {
            
        }

        protected virtual void ValidateTriggerInCannonNavigation(Collider other)
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

        public bool CanLerp
        {
            set { _canLerp = value; }
            get { return _canLerp; }
        }

        public Vector3 SetLerpPosition
        {
            set { _lerpPosition = value; }
        }

        #endregion
    }
}