using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

namespace NAwakening.RecollectionSnooker
{
    public class CinemachineMobileInputProvider : CinemachineInputProvider
    {
        #region Parameters

        public bool enableCameraRig;
        public bool enableVerticalMovement;

        #endregion

        #region PublicMethods

        public override float GetAxisValue(int axis)
        {
            if (enabled && enableCameraRig)
            {
                var action = ResolveForPlayer(axis, axis == 2 ? ZAxis : XYAxis);
                if (action != null)
                {
                    switch (axis)
                    {
                        case 0: return action.ReadValue<Vector3>().y;
                        case 1: return enableVerticalMovement ? action.ReadValue<Vector3>().x : 0f;
                        case 2: return action.ReadValue<float>();
                    }
                }
            }
            return 0;
        }

        #endregion
    }
}

