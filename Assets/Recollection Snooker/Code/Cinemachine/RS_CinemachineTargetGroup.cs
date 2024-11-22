using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace NAwakening
{
    public class RS_CinemachineTargetGroup : CinemachineTargetGroup
    {
        public void ClearTargets()
        {
            //for (int i = m_Targets.Length - 1; i >= 0; i--)
            //{
            //    RemoveMember(m_Targets[i].target);
            //}
            m_Targets = new Target[0];
            //m_Targets.Clear();
        }
    }
}