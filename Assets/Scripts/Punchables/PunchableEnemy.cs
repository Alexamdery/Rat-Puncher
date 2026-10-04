using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Enemy : MonoBehaviour, Punchable
{
    public int hp;
    public int maxReflections;
    public float punchForce;
    private bool m_isPunched;
    private Vector3 m_punchAngle;
    private int m_reflections;
    private List<Vector3> m_rayPositions;
    private LineRenderer m_lineRenderer;
    public void Hook()
    {
        throw new System.NotImplementedException();
    }

    public void SetupPunch(Vector3 angle)
    {
        m_punchAngle = angle;
        m_isPunched = true;
    }

    public void Punch()
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        if (m_isPunched)
        {
            DrawRay();
            m_isPunched = false;
        }
    }

    private void DrawRay()
    {
        m_rayPositions = new List<Vector3>();
        m_reflections = 0;
        GetRayPositions(transform.position, m_punchAngle);
        m_lineRenderer.positionCount = m_rayPositions.Count;
        m_lineRenderer.SetPositions(m_rayPositions.ToArray());
    }

    private void GetRayPositions(Vector3 start, Vector3 angle)
    {
        m_rayPositions.Add(start);
        RaycastHit hit;
        if (Physics.Raycast(start, angle, out hit))
        {
            if (hit.collider && m_reflections < maxReflections)
            {
                m_reflections++;
                Vector3 reflectedAngle = Vector3.Reflect(angle, hit.normal);
                GetRayPositions(hit.point, reflectedAngle);
            }
        }
    }
}
