using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NAwakening.RecollectionSnooker
{
    #region Enums

    public enum MonsterPartType
    {
        LIMB,
        HEAD
    }

    #endregion

    #region Structs


    #endregion

    public class MonsterPart : Token
    {
        #region Knobs

        [Header("Knobs / Parameters")]
        public MonsterPartType monsterPartType;

        #endregion

        #region RuntimeVariables

        protected bool _startLerp;

        #endregion

        #region UnityMethods

        void Start()
        {
            base.InitializeToken();
        }

        void Update()
        {
            if (_startLerp)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3 (transform.position.x, transform.position.y - 5, transform.position.z), _lerpVelocity * Time.deltaTime);
                if (Vector3.SqrMagnitude(transform.position - new Vector3(transform.position.x, - 5, transform.position.z)) < 0.1f)
                {
                    _canLerp = true;
                    transform.position = new Vector3(_lerpPosition.x, _lerpPosition.y - 5, _lerpPosition.z);
                    _startLerp = false;
                }
            }
            if (_canLerp)
            {
                transform.position = Vector3.Lerp(transform.position, _lerpPosition, _lerpVelocity * Time.deltaTime);
                if (Vector3.SqrMagnitude(transform.position - _lerpPosition) < 0.1f)
                {
                    _canLerp = false;
                    if (monsterPartType == MonsterPartType.LIMB)
                    {
                        StateMechanic(TokenStateMechanic.SET_PHYSICS);
                    }
                    else
                    {
                        StateMechanic(TokenStateMechanic.SET_RIGID);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            ValidateReferences();
            #endif
        }

        #endregion

        #region RuntimeMethods


        #endregion

        #region GettersSetters

        public bool StartLerp
        {
            set { _startLerp = value; }
        }

        #endregion
    }
}