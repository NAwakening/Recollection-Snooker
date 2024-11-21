using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NAwakening.RecollectionSnooker
{
    public class Flag : MonoBehaviour
    {
        #region RuntimeVariables

        protected Vector3 _previousLocalRotation;
        protected Vector3 _actualLocalRotation;
        protected float _deltaDegrees;

        #endregion

        #region UnityMethods

        void Start()
        {
            _previousLocalRotation = transform.localRotation.eulerAngles;
            _deltaDegrees = 0f;
        }

        void Update()
        {
            _actualLocalRotation = transform.localRotation.eulerAngles;
            //if (_actualLocalRotation.x > 120f )
            //{
            //    _actualLocalRotation.x = 120f;
            //    transform.localRotation = Quaternion.Euler(_actualLocalRotation);
            //}
        }

        private void LateUpdate()
        {
            _previousLocalRotation = _actualLocalRotation;
        }

        #endregion

        #region Getters

        public float DeltaXDegrees
        {
            get { return _actualLocalRotation.x - _previousLocalRotation.x; }
        }

        #endregion
    }
}

