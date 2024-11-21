using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace NAwakening.RecollectionSnooker
{
    #region Enums

    public enum TokenPhysicalStates
    {
        PHYSICAL,
        STATIC,   
        GHOST
    }

    public enum TokenStateMechanic
    {
        SET_PHYSICS,
        SET_RIGID,
        SET_SPOOKY
    }

    #endregion

    #region Struct

    [System.Serializable]
    public struct MaterialAssets
    {
        [SerializeField] public Material physicalMaterial;
        [SerializeField] public Material ghostMaterial;
        [SerializeField] public Material highlightPhysicalMaterial;
        [SerializeField] public Material highlightGhostMaterial;

    }

    #endregion

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(MeshRenderer))]
    public class RS_TokenFiniteStateMachine : MonoBehaviour
    {
        #region References

        [SerializeField,HideInInspector] protected Rigidbody _rigidbody;
        [SerializeField,HideInInspector] protected MeshCollider _meshCollider;
        [SerializeField,HideInInspector] protected MeshCollider _meshTrigger;
        [SerializeField,HideInInspector] protected MeshRenderer _meshRenderer;

        #endregion

        #region Assets

        [Header("Asset References")]
        public MaterialAssets materialAssets;

        #endregion

        #region RuntimeVariables

        [Header("Runtime Variables")]
        [SerializeField] protected TokenPhysicalStates _physicalState;
        [SerializeField] protected bool isStill;
        [SerializeField] protected bool isHighlighted;

        #endregion

        #region UnityMethods

        private void Start()
        {
            InitializeFiniteStateMachine();
        }

        private void FixedUpdate()
        {
            if (_physicalState == TokenPhysicalStates.PHYSICAL)
            {
                isStill = _rigidbody.GetAccumulatedForce().magnitude < 0.1f && _rigidbody.velocity.magnitude < 0.1f && _rigidbody.angularVelocity.magnitude < 0.1f;
            }
            else
            {
                isStill = false;
            }
        }

        #endregion

        #region RuntimeMethods

        protected virtual void InitializeFiniteStateMachine()
        {
            if (_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }
            if (_meshCollider == null)
            {
                _meshCollider = GetComponent<MeshCollider>();
            }
            if (_meshRenderer == null)
            {
                _meshRenderer = GetComponent<MeshRenderer>();
            }
        }

        #endregion

        #region PublicMethods

        public void StateMechanic(TokenStateMechanic value)
        {
            switch (value)
            {
                case TokenStateMechanic.SET_PHYSICS:
                    InitializePhysicalState();
                    break;
                case TokenStateMechanic.SET_RIGID:
                    InitializeStaticState();
                    break;
                case TokenStateMechanic.SET_SPOOKY:
                    InitializeGhostState();
                    break;
            }
        }

        public void ThrowToken(Vector3 directionForce)
        {
            _rigidbody.AddForce(directionForce, ForceMode.Impulse);
        }

        public void ThrowTokenAtSpecifcPosition(Vector3 directionForce, Vector3 contactPoint)
        {
            _rigidbody.AddForceAtPosition(directionForce, contactPoint, ForceMode.Impulse);
        }

        public void SetHighlight(bool value)
        {
            isHighlighted = value;
            if (isHighlighted)
            {
                if (_physicalState == TokenPhysicalStates.GHOST)
                {
                    _meshRenderer.material = materialAssets.highlightGhostMaterial;
                }
                else
                {
                    _meshRenderer.material = materialAssets.highlightPhysicalMaterial;
                }
            }
            else
            {
                if (_physicalState == TokenPhysicalStates.GHOST)
                {
                    _meshRenderer.material = materialAssets.ghostMaterial;
                }
                else
                {
                    _meshRenderer.material = materialAssets.physicalMaterial;
                }
            }
        }

        #endregion

        #region PrepareFiniteStateMachineStates

        protected virtual void InitializePhysicalState()
        {
            _rigidbody.isKinematic = false;
            _meshCollider.enabled = true;
            _physicalState = TokenPhysicalStates.PHYSICAL;
            _meshRenderer.material = materialAssets.physicalMaterial;
        }

        protected virtual void InitializeStaticState()
        {
            _rigidbody.isKinematic = true;
            _meshCollider.enabled = true;
            _physicalState = TokenPhysicalStates.STATIC;
            _meshRenderer.material = materialAssets.physicalMaterial;
        }

        protected virtual void InitializeGhostState()
        {
            _rigidbody.isKinematic = true;
            _meshCollider.enabled = false;
            _physicalState = TokenPhysicalStates.GHOST;
            _meshRenderer.material = materialAssets.ghostMaterial;
        }

        #endregion

        #region GettersAndSetters

        public TokenPhysicalStates GetPhysicalState
        {
            get { return _physicalState; }
        }

        public bool IsStill
        {
            get { return isStill; }
        }

        #endregion
    }
}