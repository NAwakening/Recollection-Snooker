using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NAwakening.Game;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using System.Threading;
using JetBrains.Annotations;

namespace NAwakening.RecollectionSnooker
{
    #region Enum

    public enum RS_GameStates
    {
        //START OF THE GAME
        SHOW_THE_LAYOUT_TO_THE_PLAYER,
        //PLAYER ORDINARY TURN STATES
        CHOOSE_TOKEN,
        CONTACT_POINT_TOKEN,
        FLICK_TOKEN,
        CANNON_CARGO,
        MOVE_COUNTER_BY_SANCTION,
        ORGANIZE_CARGO,
        CANNON_BY_NAVIGATION,
        NAVIGATING_SHIP,
        ANCHOR_SHIP,
        LEAVE_CARGO_AT_ISLAND,
        //END OF THE TURN
        SHIFT_MONSTER_PARTS,
        //META MECHANICS
        VICTORY,
        FAILURE
    }

    #endregion

    public class RS_GameReferee : GameReferee
    {
        #region References

        [Header("Token References")]
        [SerializeField] protected Cargo[] allCargoOfTheGame;
        [SerializeField] protected MonsterPart[] allMonsterPartOfTheGame;
        [SerializeField] protected Ship ship;
        [SerializeField] protected ShipPivot shipPivot;
        [SerializeField] protected MonsterPart monsterHead;

        [Header("Camera References")]
        [SerializeField] protected CinemachineFreeLook tableFreeLookCamera;
        [SerializeField] protected CinemachineFreeLook targetgroupCamera;
        [SerializeField] protected RS_CinemachineTargetGroup targetGroup;

        [Header("Flags")]
        [SerializeField] protected GameObject _flag;
        [SerializeField] protected TextMeshProUGUI debugText;

        [Header("Other")]
        [SerializeField] protected GameObject _radar;
        [SerializeField] protected UIManager _uiManager;

        #endregion

        #region RuntimeVariables

        protected new RS_GameStates _gameState;
        protected bool _moveToMoveCounter, _moveToOrganizeCargo;
        protected CinemachineFreeLook _currentFreeLookCamera;
        protected Token _interactedToken;
        [SerializeField] protected bool _isAllCargoStill;
        protected int _life = 6;
        protected int _cargoToLoad;
        protected RaycastHit _rayCastHit;
        protected Vector3 _tokenSpawnPosition;

        #endregion

        #region UnityMethods

        void Start()
        {
            InitializeGameReferee();
        }

        private void FixedUpdate()
        {
            Debug.Log(_gameState);
            switch (_gameState)
            {
                case RS_GameStates.SHOW_THE_LAYOUT_TO_THE_PLAYER:
                    ExecutingShowTheLayoutToThePlayerState();
                    break;
                case RS_GameStates.CHOOSE_TOKEN:
                    ExecutingChooseTokenState();
                    break;
                case RS_GameStates.CONTACT_POINT_TOKEN:
                    ExecutingContactPointTokenState();
                    break;
                case RS_GameStates.FLICK_TOKEN:
                    ExecutingFlickTokenState();
                    break;
                case RS_GameStates.CANNON_CARGO:
                    ExecutingCanonCargoState();
                    break;
                case RS_GameStates.MOVE_COUNTER_BY_SANCTION:
                    ExecutingMoveCounterBySanctionState();
                    break;
                case RS_GameStates.ORGANIZE_CARGO:
                    ExecutingOrganizeCargoState();
                    break;
                case RS_GameStates.CANNON_BY_NAVIGATION:
                    ExecutingCanonByNavigationState();
                    break;
                case RS_GameStates.NAVIGATING_SHIP:
                    ExecutingNavigatingShipState();
                    break;
                case RS_GameStates.ANCHOR_SHIP:
                    ExecutingAnchorShipState();
                    break;
                case RS_GameStates.LEAVE_CARGO_AT_ISLAND:
                    ExecutingLeaveCargoAtIslandState();
                    break;
                case RS_GameStates.SHIFT_MONSTER_PARTS:
                    ExecutingShiftMonsterPartsState();
                    break;
                case RS_GameStates.VICTORY:
                    ExecutingVictoryState();
                    break;
                case RS_GameStates.FAILURE:
                    ExecutingFailureState();
                    break;
            }
        }

        #endregion

        #region PublicMethods

        public void DebugInMobiles(string value)
        {
            debugText.text = value;
        }

        public void GameStateMechanic(RS_GameStates toNextState)
        {
            switch (toNextState)
            {
                case RS_GameStates.CHOOSE_TOKEN:
                    if (_gameState == RS_GameStates.SHOW_THE_LAYOUT_TO_THE_PLAYER || _gameState == RS_GameStates.SHIFT_MONSTER_PARTS || _gameState == RS_GameStates.CONTACT_POINT_TOKEN)
                    {
                        FinalizeCurrentState(toNextState);
                    }
                    break;
                case RS_GameStates.CONTACT_POINT_TOKEN:
                    if (_gameState == RS_GameStates.CHOOSE_TOKEN || _gameState == RS_GameStates.FLICK_TOKEN)
                    {
                        FinalizeCurrentState(toNextState);
                    }
                    break;
                case RS_GameStates.FLICK_TOKEN:
                    if(_gameState == RS_GameStates.CONTACT_POINT_TOKEN)
                    {
                        FinalizeCurrentState(toNextState);
                    }
                    break;
                case RS_GameStates.CANNON_CARGO:
                    if(_gameState == RS_GameStates.FLICK_TOKEN)
                    {
                        FinalizeCurrentState(toNextState);
                    }
                    break;
                case RS_GameStates.MOVE_COUNTER_BY_SANCTION:
                    if (_gameState == RS_GameStates.CANNON_CARGO)
                    {
                        FinalizeCurrentState(toNextState);
                    }
                    break;
                case RS_GameStates.ORGANIZE_CARGO:
                    if (_gameState == RS_GameStates.CANNON_CARGO || _gameState == RS_GameStates.MOVE_COUNTER_BY_SANCTION)
                    {
                        FinalizeCurrentState(toNextState);
                    }
                    break;
                case RS_GameStates.CANNON_BY_NAVIGATION:
                    if (_gameState == RS_GameStates.FLICK_TOKEN)
                    {
                        FinalizeCurrentState(toNextState);
                    }
                    break;
                case RS_GameStates.NAVIGATING_SHIP:
                    if (_gameState == RS_GameStates.CANNON_BY_NAVIGATION)
                    {
                        FinalizeCurrentState(toNextState);
                    }
                    break;
                case RS_GameStates.ANCHOR_SHIP:
                    if (_gameState == RS_GameStates.NAVIGATING_SHIP)
                    {
                        FinalizeCurrentState(toNextState);
                    }
                    break;
                case RS_GameStates.LEAVE_CARGO_AT_ISLAND:
                    if (_gameState == RS_GameStates.ANCHOR_SHIP)
                    {
                        FinalizeCurrentState(toNextState);
                    }
                    break;
                case RS_GameStates.SHIFT_MONSTER_PARTS:
                    if (_gameState == RS_GameStates.CANNON_CARGO || _gameState == RS_GameStates.MOVE_COUNTER_BY_SANCTION || _gameState == RS_GameStates.ORGANIZE_CARGO || _gameState == RS_GameStates.ANCHOR_SHIP)
                    {
                        FinalizeCurrentState(toNextState);
                    }
                    break;
                case RS_GameStates.VICTORY:
                    if (_gameState == RS_GameStates.ANCHOR_SHIP)
                    {
                        FinalizeCurrentState(toNextState);
                    }
                    break;
                case RS_GameStates.FAILURE:
                    if (_gameState == RS_GameStates.MOVE_COUNTER_BY_SANCTION)
                    {
                        FinalizeCurrentState(toNextState);
                    }
                    break;
            }
        }

        public void DeactivateRadar(Cargo closestCargo)
        {
            closestCargo.SetHighlight(false);
            closestCargo.IsAvalaibleForFlicking = false;
            _radar.SetActive(false);
        }

        public bool CheckIfCargoCanBeLoaded(Cargo cargoToCheck)
        {
            foreach (Cargo cargo in allCargoOfTheGame)
            {
                if(cargo.IsLoaded || cargo.IsOnIsland)
                {
                    if(cargo.cargoType == cargoToCheck.cargoType)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void CargoLoaded()
        {
            _cargoToLoad--;
            if (_cargoToLoad == 0)
            {
                GameStateMechanic(RS_GameStates.SHIFT_MONSTER_PARTS);
            }
        }

        #endregion

        #region RuntimeMethods

        protected void FinalizeCurrentState(RS_GameStates toNextState)
        {
            FinalizeState();
            _gameState = toNextState;
            InitializeState();
        }

        protected bool IsAllCargoStill()
        {
            _isAllCargoStill = true;
            foreach (Token token in allCargoOfTheGame)
            {
                if (!token.IsStill)
                {
                    _isAllCargoStill = false;
                    break;
                }
            }
            return _isAllCargoStill;
        }

        protected override void InitializeGameReferee()
        {
            _gameState = RS_GameStates.SHOW_THE_LAYOUT_TO_THE_PLAYER;
            InitializeState();
        }

        protected void InitializeState()
        {
            switch (_gameState)
            {
                case RS_GameStates.SHOW_THE_LAYOUT_TO_THE_PLAYER:
                    InitializeShowTheLayoutToThePlayerState();
                    break;
                case RS_GameStates.CHOOSE_TOKEN:
                    InitializeChooseTokenState();
                    break;
                case RS_GameStates.CONTACT_POINT_TOKEN:
                    InitializeContactPointTokenState();
                    break;
                case RS_GameStates.FLICK_TOKEN:
                    InitializeFlickTokenState();
                    break;
                case RS_GameStates.CANNON_CARGO:
                    InitializeCanonCargoState();
                    break;
                case RS_GameStates.MOVE_COUNTER_BY_SANCTION:
                    InitializeMoveCounterBySanctionState();
                    break;
                case RS_GameStates.ORGANIZE_CARGO:
                    InitializeOrganizeCargoState();
                    break;
                case RS_GameStates.CANNON_BY_NAVIGATION:
                    InitializeCanonByNavigationState();
                    break;
                case RS_GameStates.NAVIGATING_SHIP:
                    InitializeNavigatingShipState();
                    break;
                case RS_GameStates.ANCHOR_SHIP:
                    InitializeAnchorShipState();
                    break;
                case RS_GameStates.LEAVE_CARGO_AT_ISLAND:
                    InitializeLeaveCargoAtIslandState();
                    break;
                case RS_GameStates.SHIFT_MONSTER_PARTS:
                    InitializeShiftMonsterPartsState();
                    break;
                case RS_GameStates.VICTORY:
                    InitializeVictoryState();
                    break;
                case RS_GameStates.FAILURE:
                    InitializeFailureState();
                    break;
            }
        }

        protected void FinalizeState()
        {
            switch (_gameState)
            {
                case RS_GameStates.SHOW_THE_LAYOUT_TO_THE_PLAYER:
                    FinalizeShowTheLayoutToThePlayerState();
                    break;
                case RS_GameStates.CHOOSE_TOKEN:
                    FinalizeChooseTokenState();
                    break;
                case RS_GameStates.CONTACT_POINT_TOKEN:
                    FinalizeContactPointTokenState();
                    break;
                case RS_GameStates.FLICK_TOKEN:
                    FinalizeFlickTokenState();
                    break;
                case RS_GameStates.CANNON_CARGO:
                    FinalizeCanonCargoState();
                    break;
                case RS_GameStates.MOVE_COUNTER_BY_SANCTION:
                    FinalizeMoveCounterBySanctionState();
                    break;
                case RS_GameStates.ORGANIZE_CARGO:
                    FinalizeOrganizeCargoState();
                    break;
                case RS_GameStates.CANNON_BY_NAVIGATION:
                    FinalizeCanonByNavigationState();
                    break;
                case RS_GameStates.NAVIGATING_SHIP:
                    FinalizeNavigatingShipState();
                    break;
                case RS_GameStates.ANCHOR_SHIP:
                    FinalizeAnchorShipState();
                    break;
                case RS_GameStates.LEAVE_CARGO_AT_ISLAND:
                    FinalizeLeaveCargoAtIslandState();
                    break;
                case RS_GameStates.SHIFT_MONSTER_PARTS:
                    FinalizeShiftMonsterPartsState();
                    break;
                case RS_GameStates.VICTORY:
                    FinalizeVictoryState();
                    break;
                case RS_GameStates.FAILURE:
                    FinalizeFailureState();
                    break;
            }
        }

        protected bool Checkposition(Vector3 position, float radius)
        {
            if (Physics.SphereCast(new Vector3(position.x, position.y + 5.0f, position.z), radius, -transform.up ,out _rayCastHit, 5.0f))
            {
                if(_rayCastHit.collider.gameObject.GetComponent<Token>() != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        protected void DebugInConsole(string value)
        {
            Debug.Log(
                    gameObject.name + ": " +
                    this.name + " - " +
                    value
                );
        }

        #endregion

        #region FiniteStateMachineMethods

        #region ShowTheLayoutToThePlayer

        protected void InitializeShowTheLayoutToThePlayerState()
        {
            foreach (Token cargo in allCargoOfTheGame)
            {
                while (true)
                {
                    _tokenSpawnPosition = new Vector3(Random.Range(-15.0f, 15.0f), 0.1f, Random.Range(-15.0f, 15.0f));
                    if (Checkposition(_tokenSpawnPosition, 0.7f))
                    {
                        cargo.gameObject.transform.position = _tokenSpawnPosition;
                        break;
                    }
                }
                cargo.StateMechanic(TokenStateMechanic.SET_SPOOKY);
            }
            
            if (_currentFreeLookCamera != null)
            {
                _currentFreeLookCamera.Priority = 1;
            }
            _currentFreeLookCamera = tableFreeLookCamera;
            _currentFreeLookCamera.Priority = 1000;
            GameStateMechanic(RS_GameStates.CHOOSE_TOKEN);
        }

        protected void ExecutingShowTheLayoutToThePlayerState()
        {

        }

        protected void FinalizeShowTheLayoutToThePlayerState()
        {

        }

        #endregion ShowTheLayoutToThePlayer

        #region ChooseToken

        protected void InitializeChooseTokenState()
        {
            _radar.SetActive(true);
            foreach (Token cargo in allCargoOfTheGame)
            {
                cargo.StateMechanic(TokenStateMechanic.SET_SPOOKY);
            }
            foreach (Token monster in allMonsterPartOfTheGame)
            {
                monster.StateMechanic(TokenStateMechanic.SET_SPOOKY);
            }
            ship.StateMechanic(TokenStateMechanic.SET_SPOOKY);
            monsterHead.StateMechanic(TokenStateMechanic.SET_SPOOKY);
            //shipPivot.StateMechanic(TokenStateMechanic.SET_SPOOKY);

            if (_currentFreeLookCamera != null)
            {
                _currentFreeLookCamera.Priority = 1;
            }
            _currentFreeLookCamera = tableFreeLookCamera;
            _currentFreeLookCamera.Priority = 1000;

            foreach (Cargo cargo in allCargoOfTheGame)
            {
                if (!cargo.IsLoaded && !cargo.IsOnIsland)
                {
                    cargo.SetHighlight(true);
                    cargo.IsAvalaibleForFlicking = true;
                }
                if (ship.GetHasCrew && cargo.cargoType == CargoTypes.CREW_MEMBER)
                {
                    cargo.SetHighlight(false);
                    cargo.IsAvalaibleForFlicking = false;
                }
                else if (ship.GetHasScrew && cargo.cargoType == CargoTypes.SCREW_PART)
                {
                    cargo.SetHighlight(false);
                    cargo.IsAvalaibleForFlicking = false;
                }
                else if (ship.GetHasMedicine && cargo.cargoType == CargoTypes.MEDICINE)
                {
                    cargo.SetHighlight(false);
                    cargo.IsAvalaibleForFlicking = false;
                }
                else if (ship.GetHasFuel && cargo.cargoType == CargoTypes.FUEL)
                {
                    cargo.SetHighlight(false);
                    cargo.IsAvalaibleForFlicking = false;
                }
                else if (ship.GetHasSupplies && cargo.cargoType == CargoTypes.SUPPLIES)
                {
                    cargo.SetHighlight(false);
                    cargo.IsAvalaibleForFlicking = false;
                }
            }
        }

        protected void ExecutingChooseTokenState()
        {

        }

        protected void FinalizeChooseTokenState()
        {
            _currentFreeLookCamera.Priority = 1;
        }

        #endregion ChooseToken

        #region ContactPointToken

        protected void InitializeContactPointTokenState()
        {
            _interactedToken.StateMechanic(TokenStateMechanic.SET_PHYSICS);
            _currentFreeLookCamera = _interactedToken.GetFreeLookCamera;
            _currentFreeLookCamera.Priority = 1000;
            foreach (Cargo cargo in allCargoOfTheGame)
            {
                cargo.SetHighlight(false);
                cargo.IsAvalaibleForFlicking = false;
            }
            _interactedToken.IsAvalaibleForFlicking = true;
            _uiManager.ActivateContactPointPanel();
        }

        protected void ExecutingContactPointTokenState()
        {
            
        }

        protected void FinalizeContactPointTokenState()
        {
            _uiManager.DeactivateContactPointPanel();
        }

        #endregion ContactPointToken

        #region FlickToken

        protected void InitializeFlickTokenState()
        {
            _currentFreeLookCamera.gameObject.GetComponent<CinemachineMobileInputProvider>().enableCameraRig = false;
            _currentFreeLookCamera.m_YAxis.Value = 0;
            _flag.gameObject.SetActive(true);
            _uiManager.ActivateFlickTokenPanel();
        }

        protected void ExecutingFlickTokenState()
        {

        }

        protected void FinalizeFlickTokenState()
        {
            _flag.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            _flag.gameObject.SetActive(false);
            foreach (Cargo cargo in allCargoOfTheGame)
            {
                if (cargo.IsOnIsland)
                {
                    cargo.StateMechanic(TokenStateMechanic.SET_RIGID);
                }
                else
                {
                    cargo.StateMechanic(TokenStateMechanic.SET_PHYSICS);
                }
            }
            foreach (Token monster in allMonsterPartOfTheGame)
            {
                monster.StateMechanic(TokenStateMechanic.SET_PHYSICS);
            }
            ship.StateMechanic(TokenStateMechanic.SET_PHYSICS);
            monsterHead.StateMechanic(TokenStateMechanic.SET_RIGID);
            //shipPivot.StateMechanic(TokenStateMechanic.SET_PHYSICS);
            _currentFreeLookCamera.Priority = 1;
            _uiManager.DeactivateFlickTokenPanel();
            _currentFreeLookCamera.gameObject.GetComponent<CinemachineMobileInputProvider>().enableCameraRig = true;
            _interactedToken.IsAvalaibleForFlicking = false;
        }

        #endregion FlickToken

        #region CanonCargo

        protected void InitializeCanonCargoState()
        {
            _currentFreeLookCamera = targetgroupCamera;
            targetGroup.AddMember(_interactedToken.gameObject.transform, 1f, 1f);
            _currentFreeLookCamera.Priority = 1000;
            
        }

        protected void ExecutingCanonCargoState()
        {
            if (IsAllCargoStill())
            {
                if (_moveToMoveCounter)
                {
                    GameStateMechanic(RS_GameStates.MOVE_COUNTER_BY_SANCTION);
                }
                else if (_moveToOrganizeCargo)
                {
                    GameStateMechanic(RS_GameStates.ORGANIZE_CARGO);
                }
                else
                {
                    GameStateMechanic(RS_GameStates.SHIFT_MONSTER_PARTS);
                }
            }
        }

        protected void FinalizeCanonCargoState()
        {
            targetGroup.ClearTargets(); // :D
            _moveToMoveCounter = false;
            _interactedToken = null;
        }

        #endregion CanonCargo

        #region MoveCounterBySanction

        protected void InitializeMoveCounterBySanctionState()
        {
            _currentFreeLookCamera = tableFreeLookCamera;
            _currentFreeLookCamera.Priority = 1000;

            _life--;

            // Mover el contador

            if(_life == 0)
            {
                GameStateMechanic(RS_GameStates.FAILURE);
            }

            else
            {
                if (_moveToOrganizeCargo)
                {
                    GameStateMechanic(RS_GameStates.ORGANIZE_CARGO);
                }
                else
                {
                    GameStateMechanic(RS_GameStates.SHIFT_MONSTER_PARTS);
                }
            }
        }

        protected void ExecutingMoveCounterBySanctionState()
        {

        }

        protected void FinalizeMoveCounterBySanctionState()
        {
            _currentFreeLookCamera.Priority = 1;
        }

        #endregion CanonCargo

        #region OrganizeCargo

        protected void InitializeOrganizeCargoState()
        {
            //resetear la rotacion en x y z de la ship
            // volver la rotación de los tokens igual a la rotacción del barco
            _currentFreeLookCamera = ship.GetFreeLookCamera;
            _currentFreeLookCamera.Priority = 1000;
            _currentFreeLookCamera.gameObject.GetComponent<CinemachineMobileInputProvider>().enableCameraRig = false;
            ship.StateMechanic(TokenStateMechanic.SET_RIGID);
            ship.cargoToLoad.IsLoaded = true;
            ship.cargoToLoad = null;
            foreach (Cargo cargo in allCargoOfTheGame)
            {
                cargo.StateMechanic(TokenStateMechanic.SET_SPOOKY);
                if (cargo.IsLoaded)
                {
                    _cargoToLoad++;
                    cargo.IsAvalaibleForFlicking = true;
                    switch (cargo.cargoType)
                    {
                        case CargoTypes.CREW_MEMBER:
                            cargo.gameObject.transform.position = ship.GetLoadingCargoPositions[0].position;
                            break;
                        case CargoTypes.FUEL:
                            cargo.gameObject.transform.position = ship.GetLoadingCargoPositions[1].position;
                            break;
                        case CargoTypes.MEDICINE:
                            cargo.gameObject.transform.position = ship.GetLoadingCargoPositions[2].position;
                            break;
                        case CargoTypes.SUPPLIES:
                            cargo.gameObject.transform.position = ship.GetLoadingCargoPositions[3].position;
                            break;
                        case CargoTypes.SCREW_PART:
                            cargo.gameObject.transform.position = ship.GetLoadingCargoPositions[4].position;
                            break;
                    }
                }
            }
            foreach (Token monster in allMonsterPartOfTheGame)
            {
                monster.StateMechanic(TokenStateMechanic.SET_SPOOKY);
            }
            monsterHead.StateMechanic(TokenStateMechanic.SET_SPOOKY);
            //shipPivot.StateMechanic(TokenStateMechanic.SET_SPOOKY);
        }

        protected void ExecutingOrganizeCargoState()
        {
            
        }

        protected void FinalizeOrganizeCargoState()
        {
            _currentFreeLookCamera.Priority = 1;
            _moveToOrganizeCargo = false;
            foreach (Cargo cargo in allCargoOfTheGame)
            {
                if (cargo.IsOnIsland)
                {
                    cargo.StateMechanic(TokenStateMechanic.SET_RIGID);
                }
                else
                {
                    cargo.StateMechanic(TokenStateMechanic.SET_PHYSICS);
                }
            }
            foreach (Token monster in allMonsterPartOfTheGame)
            {
                monster.StateMechanic(TokenStateMechanic.SET_PHYSICS);
            }
            ship.StateMechanic(TokenStateMechanic.SET_PHYSICS);
            monsterHead.StateMechanic(TokenStateMechanic.SET_RIGID);
            //shipPivot.StateMechanic(TokenStateMechanic.SET_PHYSICS);
            _currentFreeLookCamera.gameObject.GetComponent<CinemachineMobileInputProvider>().enableCameraRig = true;
        }

        #endregion OrganizeCargo

        #region CanonByNavigation

        protected void InitializeCanonByNavigationState()
        {

        }

        protected void ExecutingCanonByNavigationState()
        {
            if (IsAllCargoStill())
            {
                GameStateMechanic(RS_GameStates.NAVIGATING_SHIP);
            }
        }

        protected void FinalizeCanonByNavigationState()
        {
            _currentFreeLookCamera.Priority = 1;
        }

        #endregion CanonByNavigation

        #region NavigatingShip

        protected void InitializeNavigatingShipState()
        {

        }

        protected void ExecutingNavigatingShipState()
        {
            
        }

        protected void FinalizeNavigatingShipState()
        {
            
        }

        #endregion NavigatingShip

        #region AnchorShip

        protected void InitializeAnchorShipState()
        {

        }

        protected void ExecutingAnchorShipState()
        {

        }

        protected void FinalizeAnchorShipState()
        {

        }

        #endregion AnchorShip

        #region LeaveCargoAtIsland

        protected void InitializeLeaveCargoAtIslandState()
        {

        }

        protected void ExecutingLeaveCargoAtIslandState()
        {

        }

        protected void FinalizeLeaveCargoAtIslandState()
        {

        }

        #endregion LeaveCargoAtIsland

        #region ShiftMonsterParts

        protected void InitializeShiftMonsterPartsState()
        {
            _currentFreeLookCamera = tableFreeLookCamera;
            _currentFreeLookCamera.Priority = 10000;
            foreach (Token monster in allMonsterPartOfTheGame)
            {
                while (true)
                {
                    _tokenSpawnPosition = new Vector3(Random.Range(-15.0f, 15.0f), 0.1f, Random.Range(-15.0f, 15.0f));
                    if (Checkposition(_tokenSpawnPosition, 2f))
                    {
                        monster.gameObject.transform.position = _tokenSpawnPosition;
                        break;
                    }
                }
            }
            while (true)
            {
                _tokenSpawnPosition = new Vector3(Random.Range(-10.0f, 10.0f), 0.1f, Random.Range(-10.0f, 10.0f));
                if (Checkposition(_tokenSpawnPosition, 4f))
                {
                    monsterHead.gameObject.transform.position = _tokenSpawnPosition;
                    break;
                }
            }
            GameStateMechanic(RS_GameStates.CHOOSE_TOKEN);
        }

        protected void ExecutingShiftMonsterPartsState()
        {

        }

        protected void FinalizeShiftMonsterPartsState()
        {

        }

        #endregion ShiftMonsterParts

        #region Victory

        protected void InitializeVictoryState()
        {

        }

        protected void ExecutingVictoryState()
        {

        }

        protected void FinalizeVictoryState()
        {

        }

        #endregion Victory

        #region Failure

        protected void InitializeFailureState()
        {

        }

        protected void ExecutingFailureState()
        {

        }

        protected void FinalizeFailureState()
        {

        }

        #endregion Failure

        #endregion FiniteStateMachineMethods

        #region GettersAndSetters

        public RS_GameStates GetGameState
        {
            get { return _gameState; }
        }

        public Token SetInteractedToken
        {
            set { _interactedToken = value; }
        }

        public GameObject GetFlag
        {
            get { return _flag; }
        }

        public RS_CinemachineTargetGroup GetTargetGroup
        {
            get { return targetGroup; }

        }

        public bool SetMoveToMoveCounter
        {
            set { _moveToMoveCounter = value; }
        }

        public bool SetMoveToOrganizeCargo
        {
            set { _moveToOrganizeCargo = value; }
        }

        #endregion
    }
}