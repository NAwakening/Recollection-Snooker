using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NAwakening.RecollectionSnooker
{
    public class RS_MobileInputHandler : MobileInputHandler
    {
        #region References

        [SerializeField] protected GameObject _goTouchCursor;
        [SerializeField] protected Camera _camera;
        [SerializeField] protected RS_GameReferee _gameReferee;

        #endregion

        #region RuntimeVariables

        protected RaycastHit _raycastHit;
        protected Token _chosenToken;
        protected Token _contactToken;
        protected bool _movingToken;

        #endregion

        #region UnityMethods

        void Start()
        {
            InitializeMobileInputHandler();
        }

        private void FixedUpdate()
        {
            
        }

        #endregion

        #region LocalMethods

        protected override void HandleTouchInputAction(InputAction.CallbackContext value)
        {
            switch (_gameReferee.GetGameState)
            {
                case RS_GameStates.CHOOSE_TOKEN:
                    HandleTouchInChooseTokenByPlayer(value);
                    break;
                case RS_GameStates.CONTACT_POINT_TOKEN:
                    HandleTouchInContactPointTokenByPlayer(value);
                    break;
                case RS_GameStates.ORGANIZE_CARGO:
                    HandleTouchInOrganizCargo(value);
                    break;

            }
        }

        protected override void HandleRotateInputAction(InputAction.CallbackContext value)
        {
            switch (_gameReferee.GetGameState)
            {
                case RS_GameStates.FLICK_TOKEN:
                    HandleRotationInFlickTokenByPlayer(value);
                    break;
            }
        }

        protected override void HandleTiltInputAction(InputAction.CallbackContext value)
        {
            switch (_gameReferee.GetGameState)
            {
                case RS_GameStates.FLICK_TOKEN:
                    HandleRotationInFlickTokenByPlayer(value);
                    break;
            }
        }

        protected override void HandleTranslateInputAction(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                
            }
            else if (value.canceled)
            {

            }
        }

        #endregion

        #region HandleTouchActions

        protected void HandleTouchInChooseTokenByPlayer(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                if (Physics.Raycast(_camera.ScreenPointToRay(value.ReadValue<Vector2>()),out _raycastHit,50.0f,LayerMask.GetMask("Movable")))
                {
                    _chosenToken = _raycastHit.collider.gameObject.GetComponent<Token>();
                    if (_chosenToken as Cargo || _chosenToken as ShipPivot)
                    {
                        if (_chosenToken.IsAvalaibleForFlicking && transform.position.y >= -0.2f)
                        {
                            _goTouchCursor.SetActive(true);
                            _goTouchCursor.transform.position = _raycastHit.point;
                            _gameReferee.SetInteractedToken = _chosenToken;
                            _gameReferee.GameStateMechanic(RS_GameStates.CONTACT_POINT_TOKEN);
                        }
                    }
                }
                else
                {
                    _goTouchCursor.SetActive(false);
                }
            }
            else if (value.canceled)
            {
                _goTouchCursor.SetActive(false);
            }
        }

        protected void HandleTouchInContactPointTokenByPlayer(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                if (Physics.Raycast(_camera.ScreenPointToRay(value.ReadValue<Vector2>()), out _raycastHit, 50.0f, LayerMask.GetMask("Movable")))
                {
                    _contactToken = _raycastHit.collider.gameObject.GetComponent<Token>();
                    if (_contactToken == _chosenToken) 
                    {
                        _goTouchCursor.SetActive(true);
                        _goTouchCursor.transform.position = _raycastHit.point;
                        _gameReferee.GameStateMechanic(RS_GameStates.FLICK_TOKEN);
                    }
                }
                else
                {
                    _goTouchCursor.SetActive(false);
                }
            }
        }

        protected void HandleTouchInOrganizCargo(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                if (!_movingToken)
                {
                    if (Physics.Raycast(_camera.ScreenPointToRay(value.ReadValue<Vector2>()), out _raycastHit, 20.0f, LayerMask.GetMask("Movable")))
                    {
                        _chosenToken = _raycastHit.collider.gameObject.GetComponent<Token>();
                        if (_chosenToken.IsAvalaibleForFlicking)
                        {
                            _movingToken = true;
                            _chosenToken.SetHighlight(true);
                        }
                    }
                }
                else
                {
                    if (Physics.Raycast(_camera.ScreenPointToRay(value.ReadValue<Vector2>()), out _raycastHit, 20.0f, LayerMask.GetMask("ShipLoad")))
                    {
                        _chosenToken.gameObject.transform.position = _raycastHit.point;
                    }
                }
            }
            else if (value.canceled)
            {
                
            }
        }

        #endregion HandleTouchActions

        #region HandleRotationActions

        protected void HandleRotationInFlickTokenByPlayer(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                _gameReferee.GetFlag.transform.Rotate(new Vector3((value.ReadValue<Vector3>() != null ? value.ReadValue<Vector3>().x : value.ReadValue<Vector2>().x) * -4.0f, 0f, 0f), Space.Self);
            }
            else if (value.canceled)
            {

            }
        }

        #endregion HandleRotationActions

        #region GetterandSetter

        public bool MovingToken
        {
            get { return _movingToken; }
            set { _movingToken = value; }
        }

        public Token GetChosenToken
        {
            get { return _chosenToken; }
        }

        #endregion
    }
}