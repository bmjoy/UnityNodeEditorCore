﻿using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Base class for all nodes </summary>
[Serializable]
public abstract class Node : ScriptableObject {
    public enum ShowBackingValue {
        /// <summary> Never show the backing value </summary>
        Never,
        /// <summary> Show the backing value only when the port does not have any active connections </summary>
        Unconnected,
        /// <summary> Always show the backing value </summary>
        Always
    }

    /// <summary> Parent <see cref="NodeGraph"/> </summary>
    [SerializeField] public NodeGraph graph;
    /// <summary> Position on the <see cref="NodeGraph"/> </summary>
    [SerializeField] public Vector2 position;
    /// <summary> Input <see cref="NodePort"/>s. It is recommended not to modify these at hand. Instead, see <see cref="InputAttribute"/> </summary>
    [SerializeField] public List<NodePort> inputs = new List<NodePort>();
    /// <summary> Output <see cref="NodePort"/>s. It is recommended not to modify these at hand. Instead, see <see cref="OutputAttribute"/> </summary>
    [SerializeField] public List<NodePort> outputs = new List<NodePort>();

    public int InputCount { get { return inputs.Count; } }
    public int OutputCount { get { return outputs.Count; } }

    protected void OnEnable() {
        NodeDataCache.UpdatePorts(this, inputs, outputs);
        Init();
    }

    /// <summary> Checks all connections for invalid references, and removes them. </summary>
    public void VerifyConnections() {
        for (int i = 0; i < InputCount; i++) {
            inputs[i].VerifyConnections();
        }
        for (int i = 0; i < OutputCount; i++) {
            outputs[i].VerifyConnections();
        }
    }

    /// <summary> Returns input or output port which matches fieldName </summary>
    public NodePort GetPortByFieldName(string fieldName) {
        NodePort port = GetOutputByFieldName(fieldName);
        if (port != null) return port;
        else return GetInputByFieldName(fieldName);
    }


    /// <summary> Returns output port which matches fieldName. Returns null if none found. </summary>
    public NodePort GetOutputByFieldName(string fieldName) {
        for (int i = 0; i < OutputCount; i++) {
            if (outputs[i].fieldName == fieldName) return outputs[i];
        }
        return null;
    }

    /// <summary> Returns input port which matches fieldName. Returns null if none found. </summary>
    public NodePort GetInputByFieldName(string fieldName) {
        for (int i = 0; i < InputCount; i++) {
            if (inputs[i].fieldName == fieldName) return inputs[i];
        }
        return null;
    }

    /// <summary> Return input value for a specified port. Returns fallback value if no ports are connected </summary>
    /// <param name="fieldName">Field name of requested input port</param>
    /// <param name="fallback">If no ports are connected, this value will be returned</param>
    public T GetInputByFieldName<T>(string fieldName, T fallback = default(T)) {
        NodePort port = GetInputByFieldName(fieldName);
        if (port != null && port.IsConnected) return port.GetInputValue<T>();
        else return fallback;
    }

    /// <summary> Return all input values for a specified port. Returns fallback value if no ports are connected </summary>
    /// <param name="fieldName">Field name of requested input port</param>
    /// <param name="fallback">If no ports are connected, this value will be returned</param>
    public T[] GetInputsByFieldName<T>(string fieldName, params T[] fallback) {
        NodePort port = GetInputByFieldName(fieldName);
        if (port != null && port.IsConnected) return port.GetInputValues<T>();
        else return fallback;
    }

    /// <summary> Returns a value based on requested port output. Should be overridden before used. </summary>
    /// <param name="port">The requested port.</param>
    public virtual object GetValue(NodePort port) {
        Debug.LogWarning("No GetValue(NodePort port) override defined for " + GetType());
        return null;
    }

    /// <summary> Initialize node. Called on creation. </summary>
    protected virtual void Init() { name = GetType().Name; }

    /// <summary> Called whenever a connection is being made between two <see cref="NodePort"/>s</summary>
    /// <param name="from">Output</param> <param name="to">Input</param>
    public virtual void OnCreateConnection(NodePort from, NodePort to) { }

    /// <summary> Disconnect everything from this node </summary>
    public void ClearConnections() {
        for (int i = 0; i < inputs.Count; i++) {
            inputs[i].ClearConnections();
        }
        for (int i = 0; i < outputs.Count; i++) {
            outputs[i].ClearConnections();
        }
    }

    public override int GetHashCode() {
        return JsonUtility.ToJson(this).GetHashCode();
    }

    /// <summary> Mark a serializable field as an input port. You can access this through <see cref="GetInputByFieldName(string)"/> </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class InputAttribute : Attribute {
        public ShowBackingValue backingValue;
        
        /// <summary> Mark a serializable field as an input port. You can access this through <see cref="GetInputByFieldName(string)"/> </summary>
        /// <param name="backingValue">Should we display the backing value for this port as an editor field? </param>
        public InputAttribute(ShowBackingValue backingValue = ShowBackingValue.Unconnected) { this.backingValue = backingValue; }
    }

    /// <summary> Mark a serializable field as an output port. You can access this through <see cref="GetOutputByFieldName(string)"/> </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class OutputAttribute : Attribute {
        /// <summary> Mark a serializable field as an output port. You can access this through <see cref="GetOutputByFieldName(string)"/> </summary>
        public OutputAttribute() { }
    }
}