﻿using UnityEngine;

namespace BasicNodes {
    public class Vector : Node {
        [Input] public float x, y, z;
        [Output] public Vector3 vector;

        public override object GetValue(NodePort port) {
            float x = GetInputByFieldName<float>("x", this.x);
            float y = GetInputByFieldName<float>("y", this.y);
            float z = GetInputByFieldName<float>("z", this.z);
            return new Vector3(x, y, z);
        }
    }
}