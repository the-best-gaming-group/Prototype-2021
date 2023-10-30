//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(DialogueObject))]
//public class DialogueObjectEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        DialogueObject dialogueObject = (DialogueObject)target;

//        if (GUILayout.Button("Refresh"))
//        {
//            RefreshResponseEvents(dialogueObject);
//        }
//    }

//    private void RefreshResponseEvents(DialogueObject dialogueObject)
//    {
//        if (dialogueObject.Responses == null || dialogueObject.Responses.Length == 0)
//        {
//            return;
//        }

//        SerializedObject serializedObject = new SerializedObject(dialogueObject);

//        serializedObject.Update();

//        SerializedProperty responseEvents = serializedObject.FindProperty("responseEvents");

//        if (responseEvents == null || responseEvents.arraySize != dialogueObject.Responses.Length)
//        {
//            responseEvents.arraySize = dialogueObject.Responses.Length;
//        }

//        for (int i = 0; i < dialogueObject.Responses.Length; i++)
//        {
//            SerializedProperty responseEvent = responseEvents.GetArrayElementAtIndex(i);
//            responseEvent.FindPropertyRelative("name").stringValue = dialogueObject.Responses[i].ResponseText;
//        }

//        serializedObject.ApplyModifiedProperties();
//    }
//}
