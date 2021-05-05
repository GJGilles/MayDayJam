using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{

    public int m_MoveSteps = 400;
    public int m_CurrentStep = 0;
    public Transform m_PointA;
    public Transform m_PointB;

    private int m_Forwards = 1;


    private void FixedUpdate()
    {
        transform.position = Vector2.Lerp(m_PointA.position, m_PointB.position, (float)m_CurrentStep / (float)m_MoveSteps);

        if (m_CurrentStep == 0)
        {
            m_Forwards = 1;
        }

        if (m_CurrentStep == m_MoveSteps)
        {
            m_Forwards = -1;
        }

        m_CurrentStep += m_Forwards;
    }

}
