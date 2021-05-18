using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using EGL = UnityEditor.EditorGUILayout;

[CustomEditor(typeof(ObjectRelocator))]
public class ObjectRelocatorEditor : Editor {

    private ObjectRelocator currentTarget;
    
    private SerializedObject owner;
    private SerializedProperty relocationList;

    private Color defaultBackgroundColor;
    
    private void OnEnable() {
        currentTarget = target as ObjectRelocator;
        owner = new SerializedObject(target);
        relocationList = owner.FindProperty("Positions");
        defaultBackgroundColor = GUI.backgroundColor;
    }

    public override void OnInspectorGUI() {
        
        EGL.LabelField("Size: " + relocationList.arraySize, CreateGUIStyle(18, Color.red, TextAnchor.UpperCenter));
        InsertSpace(1);
        if (GUILayout.Button("Draw lines to positions"))
            DrawLinesToPosition();
        
        InsertSpace(1);
        
        for (int i = 0; i < relocationList.arraySize; i++) {
            
            EGL.LabelField("Position: " + (i + 1), CreateGUIStyle(12, Color.green, TextAnchor.MiddleCenter));
            InsertSpace(1);
            
            SerializedProperty listIndex = relocationList.GetArrayElementAtIndex(i);
            
            
            SerializedProperty description = listIndex.FindPropertyRelative("Description");
            SerializedProperty useGameObjectAsPosition = listIndex.FindPropertyRelative("UseGameObjectAsPosition");
            
            SerializedProperty vectorPosition = listIndex.FindPropertyRelative("WorldPosition");
            SerializedProperty transformPosition = listIndex.FindPropertyRelative("TransformPosition");
            
            EGL.PropertyField(description);
            EGL.PropertyField(useGameObjectAsPosition);

            EGL.BeginHorizontal();
            
            if (useGameObjectAsPosition.boolValue)
                transformPosition.objectReferenceValue = EGL.ObjectField("Transform Position: ", transformPosition.objectReferenceValue, typeof(Transform), true);
            else 
                EGL.PropertyField(vectorPosition);
            
            
            
            if (GUILayout.Button("Clear")) {
                if(useGameObjectAsPosition.boolValue)
                    transformPosition.objectReferenceValue = null;
                else
                    vectorPosition.vector3Value = Vector3.zero;
                
            }
            
            EGL.EndHorizontal();

            EGL.BeginHorizontal();
            
            if (GUILayout.Button("Move here")) {
                Vector3 newPosition = useGameObjectAsPosition.boolValue ? (transformPosition.objectReferenceValue as Transform).position : vectorPosition.vector3Value;
                
                currentTarget.MoveToPosition(newPosition);
            }
            
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Remove from list")) {
                
                transformPosition.objectReferenceValue = null;
                vectorPosition.vector3Value = Vector3.zero;
                relocationList.DeleteArrayElementAtIndex(i);
            }
                
            
            GUI.backgroundColor = defaultBackgroundColor;
            EGL.EndHorizontal();
            InsertSpace(2); 
            
        }
        
        #region Knappar
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Add new position"))
            currentTarget.Positions.Add(new ObjectRelocator.RelocatorContext());
        if (GUILayout.Button("Clear list"))
            currentTarget.Positions.Clear();
        
        
        GUILayout.EndHorizontal();
        #endregion
        owner.ApplyModifiedProperties();
        owner.Update();

        if (GUI.changed)
            EditorUtility.SetDirty(target);


    }

    private void InsertSpace(int spaces) {
        for(int i = 0; i < spaces; i++)
            EGL.Space();
    }

    private GUIStyle CreateGUIStyle(int fontSize, Color color, TextAnchor alignment) {
        GUIStyle newGUIStyle = new GUIStyle();
        newGUIStyle.fontSize = fontSize;
        newGUIStyle.normal.textColor = color;
        newGUIStyle.alignment = alignment;
        newGUIStyle.richText = true;
        return newGUIStyle;
    }

    private void DrawLinesToPosition() {
        Vector3 start = currentTarget.transform.position;
        for (int i = 0; i < relocationList.arraySize; i++) {
            SerializedProperty listIndex = relocationList.GetArrayElementAtIndex(i);
            bool objectAsPosition = listIndex.FindPropertyRelative("UseGameObjectAsPosition").boolValue;
            Vector3 end = Vector3.zero;

            if (objectAsPosition) {
                Transform transform = listIndex.FindPropertyRelative("TransformPosition").objectReferenceValue as Transform;
                if (transform != null)
                    end = transform.position;
                else {
                    Debug.LogWarning("Object reference slot is null in GameObject with name: <color=red>" + currentTarget + "</color>");
                }
            }
                
            else
                end = listIndex.FindPropertyRelative("WorldPosition").vector3Value;

            Debug.DrawLine(start, end, Color.red, 5);

        }
    }
}
