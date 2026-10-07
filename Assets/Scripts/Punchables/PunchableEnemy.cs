using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Enemy : MonoBehaviour, Punchable
{
    public int hp;
    public int maxReflections;
    public float punchForce = 1000f;
    private bool _isPunched;
    private Vector3 _punchAngle;
    private int _reflections;
    private Queue<Vector3> _rayPositions;
    private LineRenderer _lineRenderer;
    public void Hook()
    {
        throw new System.NotImplementedException();
    }

    public void SetupPunch(Vector3 angle)
    {
        _punchAngle = angle;
        _isPunched = true;
    }

    public void Punch()
    {
        // TODO: trace line?
        Rigidbody rigidBody = gameObject.AddComponent<Rigidbody>();
        rigidBody.AddForce(_punchAngle * punchForce, ForceMode.Impulse);
    }

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        if (_isPunched)
        {
            DrawRay();
            _isPunched = false;
        }
    }

    private void DrawRay()
    {
        _rayPositions = new Queue<Vector3>();
        _reflections = 0;
        GetRayPositions(transform.position, _punchAngle);
        _lineRenderer.positionCount = _rayPositions.Count;
        _lineRenderer.SetPositions(_rayPositions.ToArray());
    }

    private void GetRayPositions(Vector3 start, Vector3 angle)
    {
        _rayPositions.Enqueue(start);
        RaycastHit hit;
        if (Physics.Raycast(start, angle, out hit))
        {
            if (hit.collider && _reflections < maxReflections)
            {
                _reflections++;
                Vector3 reflectedAngle = Vector3.Reflect(angle, hit.normal);
                GetRayPositions(hit.point, reflectedAngle);
            }
        }
    }
}
