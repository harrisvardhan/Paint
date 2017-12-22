﻿using Leap.Unity.Attributes;
using Leap.Unity.Meshing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity.Animation {

  using IPositionSpline = ISpline<Vector3, Vector3>;

  public class SimpleSplineMeshGenerator : MonoBehaviour {

    [SerializeField, ImplementsInterface(typeof(IPositionSpline))]
    private MonoBehaviour _spline;
    public IPositionSpline spline {
      get { return _spline as IPositionSpline; }
    }

    [Tooltip("If the spline object has its transform baked in, check this to invert "
           + "the spline positions back into local space.")]
    public bool applyInverseSplineTransform = true;

    public float radius = 0.02f;

    [Header("Mesh Filter Output")]
    [QuickButton("Generate!", "generateIntoMeshFilter")]
    public MeshFilter _meshFilter;

    private PolyMesh _backingPolyMesh = null;
    private PolyMesh _polyMesh {
      get {
        if (_backingPolyMesh == null) {
          _backingPolyMesh = new PolyMesh(enableEdgeData: false);
        }
        return _backingPolyMesh;
      }
    }

    private void generateIntoMeshFilter() {
      if (_meshFilter == null) {
        Debug.LogError("Mesh filter is null; assign a mesh filter before generating.", this);
        return;
      }
      
      if (spline == null) {
        Debug.LogError("Spline is null; assign a spline behaviour before generating.", this);
        return;
      }

      Matrix4x4? applyTransform = null;
      if (applyInverseSplineTransform) {
        applyTransform = _spline.transform.localToWorldMatrix;
      }
      SplineUtil.FillPolyMesh(spline, _polyMesh,
                              applyTransform: applyTransform,
                              radius: radius);

      if (_meshFilter.sharedMesh == null) {
        _meshFilter.sharedMesh = new Mesh();
        _meshFilter.sharedMesh.name = "SplineMeshGenerator Spline Mesh";
      }
      _polyMesh.FillUnityMesh(_meshFilter.sharedMesh);
    }

  }

}
