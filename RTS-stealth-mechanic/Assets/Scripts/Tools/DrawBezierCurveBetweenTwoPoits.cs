using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tools
{
    public class DrawBezierCurveBetweenTwoPoits : MonoBehaviour
    {
        [SerializeField] Transform _firstObject;
        [SerializeField] Transform _secondObject;

        private void OnDrawGizmos()
        {
            float halfHeight = _firstObject.position.y - _secondObject.position.y;
            Vector3 offset = Vector3.up * halfHeight;
            Handles.DrawBezier(_firstObject.position, _secondObject.position, _firstObject.position - offset, _secondObject.position + offset, Color.blue, EditorGUIUtility.whiteTexture, 2f);
        }


    }
}