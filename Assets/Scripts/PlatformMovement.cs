using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float m_LoopTime = 2;
    private float m_CurrentPos = 0;
    public Transform m_PointA;
    public Transform m_PointB;

    private int m_Forwards = 1;


    private void Update()
    {
        transform.position = Vector2.Lerp(m_PointA.position, m_PointB.position, m_CurrentPos);

        if (m_CurrentPos <= 0)
        {
            m_Forwards = 1;
            m_CurrentPos = 0;
        }

        if (m_CurrentPos >= 1)
        {
            m_Forwards = -1;
            m_CurrentPos = 1;
        }

        m_CurrentPos += m_Forwards * 2 * Time.deltaTime / m_LoopTime;
    }

}
