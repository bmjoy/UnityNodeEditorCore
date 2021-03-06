[![Discord](https://img.shields.io/discord/361769369404964864.svg)](https://discord.gg/qgPrHv4)
[![GitHub issues](https://img.shields.io/github/issues/Siccity/UnityNodeEditorCore.svg)](https://github.com/Siccity/UnityNodeEditorCore/issues)
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/Siccity/UnityNodeEditorCore/master/LICENSE.md)
[![GitHub Wiki](https://img.shields.io/badge/wiki-available-brightgreen.svg)](https://github.com/Siccity/UnityNodeEditorCore/wiki)

### UnityNodeEditorCore
Thinking of developing a node-based plugin? Then this is for you. You can download it as an archive and unpack to a new unity project, or connect it as git submodule.

With a minimal footprint, UNEC is ideal as a base for custom state machines, dialogue systems, decision makers etc.

![editor](https://user-images.githubusercontent.com/6402525/31379481-a9c15950-adae-11e7-91c4-387dd020261e.png)

### Key features of UnityNodeEditorCore:
* Lightweight in runtime
* Very little boilerplate code
* Strong separation of editor and runtime code
* No runtime reflection (unless you need to edit/build node graphs at runtime. In this case, all reflection is cached.)
* Does not rely on any 3rd party plugins
* Custom node inspector code is very similar to regular custom inspector code

### Node example:
```csharp
[System.Serializable]
public class MathNode : Node {
    // Adding [Input] or [Output] is all you need to do to register a field as a valid port on your node 
    [Input] public float a;
    [Input] public float b;
    // The value of an output node field is not used for anything, but could be used for caching output results
    [Output] public float result;
    [Output] public float sum;

    // UNEC will display this as an editable field - just like the normal inspector would
    public MathType mathType = MathType.Add;
    public enum MathType { Add, Subtract, Multiply, Divide}
    
    // GetValue should be overridden to return a value for any specified output port
    public override object GetValue(NodePort port) {

        // Get new a and b values from input connections. Fallback to field values if input is not connected
        float a = GetInputByFieldName<float>("a", this.a);
        float b = GetInputByFieldName<float>("b", this.b);

        // After you've gotten your input values, you can perform your calculations and return a value
        if (port.fieldName == "result")
            switch(mathType) {
                case MathType.Add: default: return a + b;
                case MathType.Subtract: return a - b;
                case MathType.Multiply: return a * b;
                case MathType.Divide: return a / b;
            }
        else if (port.fieldName == "sum") return a + b;
        else return 0f;
    }
}
```

Join the [Discord](https://discord.gg/qgPrHv4 "Join Discord server") server to leave feedback or get support.
Feel free to also leave suggestions/requests in the [issues](https://github.com/Siccity/UnityNodeEditorCore/issues "Go to Issues") page.

Projects using UnityNodeEditorCore:
* [Graphmesh](https://github.com/Siccity/Graphmesh "Go to github page")
