using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NAwakening.RecollectionSnooker
{
    public class Radar : MonoBehaviour
    {
        #region References

        [SerializeField] protected RS_GameReferee _gameReferee;
        [SerializeField] protected SphereCollider _sphereCollider;

        #endregion

        #region UnityMethods

        private void FixedUpdate()
        {
            _sphereCollider.radius += 0.1f * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Cargo>() != null && !other.GetComponent<Cargo>().IsLoaded)
            {
                _sphereCollider.radius = 0.5f;
                _gameReferee.DeactivateRadar(other.GetComponent<Cargo>());
            }
        }

        #endregion
    }
}

