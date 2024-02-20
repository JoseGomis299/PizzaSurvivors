using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace ProjectUtils.Helpers
{
    public static class Helpers 
    {
        private static Camera _camera;
        public static Camera Camera
        {
            get
            {
                if(_camera == null) _camera = Camera.main;
                return _camera;
            }
        }
    
        private static PointerEventData _eventDataCurrentPosition;
        private static List<RaycastResult> _results;

        public static bool PointerIsOverUi()
        {
            _eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            _results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
            return _results.Count > 0;
        }

        public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera, out var result);
            return result;
        }

        public static void DeleteChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
            
            transform.DetachChildren();
        }

        /// <summary>Returns the angle in degrees between this position and the mouse position</summary>
        public static float GetAngleToPointer(this Vector3 position)
        {
            Vector3 attackDirection = Input.mousePosition;
            attackDirection = Camera.ScreenToWorldPoint(attackDirection);
            attackDirection.z = position.z;
            attackDirection = (attackDirection-position).normalized;

            float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
            while (angle<0) angle += 360;

            return angle;
        }
        /// <summary><para>Returns a vector with the 4 corners of the screen</para>
        /// <param name="targetObjectScale"> The scale of the object we want to be at the point</param>
        /// <param name="borderModification"> Multiplier for the borders to be bigger or smaller</param>
        /// <param name="targetObjectDistance"> (ONLY USE IN PERSPECTIVE MODE) The distance of the object we want to be at the point</param>
        /// </summary>
        public static Vector3[] GetBounds(this Camera cam, float targetObjectScale = 1, float borderModification = 1, float targetObjectDistance = 0)
        {
            Vector3 dist = Camera.WorldToScreenPoint(Camera.transform.forward * (Camera.nearClipPlane + targetObjectScale + targetObjectDistance));
            dist = Camera.ScreenToWorldPoint(dist);
        
            if (Camera.orthographic) 
            {
                var rightTopBounds = Camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)) + dist;
                rightTopBounds *= borderModification;
                var leftTopBounds = Camera.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)) + dist;
                leftTopBounds *= borderModification;
                var rightBotBounds = Camera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)) + dist;
                rightBotBounds *= borderModification;
                var leftBotBounds = Camera.ScreenToWorldPoint(new Vector3(0, 0, 0)) + dist;
                leftBotBounds *= borderModification;
                return new Vector3[] { leftBotBounds, rightBotBounds, rightTopBounds, leftTopBounds };
            }
        
            Vector3[] res = new Vector3[4];
            Ray ray = Camera.ScreenPointToRay(new Vector3(0, 0, 0)+new Vector3(-Screen.width, -Screen.height, 0)* (borderModification-1));
            Plane plane = new Plane(Camera.transform.forward, dist);
            plane.Raycast(ray, out var distance);
            res[0] = ray.GetPoint(distance);
                    
            ray = Camera.ScreenPointToRay(new Vector3(Screen.width, 0, 0)+new Vector3(Screen.width, -Screen.height, 0)* (borderModification-1));
            plane.Raycast(ray, out distance);
            res[1] = ray.GetPoint(distance);
                    
            ray = Camera.ScreenPointToRay(new Vector3(Screen.width, Screen.height, 0)* borderModification);
            plane.Raycast(ray, out distance);
            res[2] = ray.GetPoint(distance);
                    
            ray = Camera.ScreenPointToRay(new Vector3(0, Screen.height, 0)+new Vector3(-Screen.width, Screen.height, 0)* (borderModification-1));
            plane.Raycast(ray, out distance);
            res[3] = ray.GetPoint(distance);
            return res;
        }
        
        private struct MyVector
        {
            public Vector3 startPoint;
            public Vector3 direction;
            public float distance;
        }
        private static readonly MyVector[] _vectors = new MyVector[4];
        /// <summary><para>Returns a random point in the bounds of the screen</para>
        /// <param name="targetObjectScale"> The scale of the object we want to be at the point</param>
        /// <param name="borderModification"> Multiplier for the borders to be bigger or smaller</param>
        /// <param name="distanceToTarget"> (ONLY USE IN PERSPECTIVE MODE) The distance of the object we want to be at the point</param>
        /// </summary>
        public static Vector3 GetRandomPointInBounds(this Camera cam, float targetObjectScale = 1, float borderModification = 1, float distanceToTarget = 0)
        {
            Vector3[] bounds = cam.GetBounds(targetObjectScale, borderModification, distanceToTarget);
            for (var i = 0; i < bounds.Length; i++)
            {
                _vectors[i] = new MyVector
                {
                    startPoint = bounds[i],
                    direction = (bounds[(i+1)%bounds.Length] - bounds[i]).normalized,
                    distance = Vector3.Distance(bounds[i], bounds[(i+1)%bounds.Length])
                };
            }
            MyVector vector = _vectors[Random.Range(0, 4)];
            return vector.startPoint + vector.direction * (vector.distance * Random.value);
        }
        
        public static Vector2 Rotate(this Vector2 v, float angle)
        {
            float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
            float cos = Mathf.Cos(angle * Mathf.Deg2Rad);
        
            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }
        
        public static bool IsHimOrHisChild(Transform transform, Transform parent)
        {
            if (transform == parent) return true;
            if (transform.parent == null) return false;
            return IsHimOrHisChild(transform.parent, parent);
        }

        public static void DrawWiredRectangle(Vector3 position, Vector3 size, float angle, Color color)
        {
            Gizmos.color = color;

            Vector2 lb = - size / 2f ;
            lb = lb.Rotate(angle) + (Vector2) position;
            
            Vector2 rb =  new Vector3(size.x / 2f, -size.y / 2f) ;
            rb = rb.Rotate(angle) + (Vector2) position;

            Vector2 rt =  size / 2f ;
            rt = rt.Rotate(angle) + (Vector2) position;
            
            Vector2 lt = new Vector3(-size.x / 2f, size.y / 2f) ;
            lt = lt.Rotate(angle) + (Vector2) position;
            

            //LeftBottom to RightBottom
            Gizmos.DrawLine(lb, rb);
            
            //RightBottom to RightTop
            Gizmos.DrawLine(rb, rt);
            
            //RightTop to LeftTop
            Gizmos.DrawLine(rt, lt);
            
            //LeftTop to LeftBottom
            Gizmos.DrawLine(lt, lb);
        }
        
        public static void DrawWiredCapsule(Vector2 position, Vector3 size, float angle, CapsuleDirection2D orientation, Color color)
        {
            Gizmos.color = color;

            var rectSize = orientation == CapsuleDirection2D.Horizontal ? new Vector2(size.x-size.y, size.y) : new Vector2(size.x, size.y-size.x);
            DrawWiredRectangle(position, rectSize, angle, color);
            
            if (orientation == CapsuleDirection2D.Horizontal)
            {
                Gizmos.DrawWireSphere(position + new Vector2(size.x / 2f - size.y / 2f, 0).Rotate(angle), size.y / 2f);
                Gizmos.DrawWireSphere(position + new Vector2(-size.x / 2f + size.y / 2f, 0).Rotate(angle), size.y / 2f);
            }
            else
            {
                Gizmos.DrawWireSphere(position + new Vector2(0, -size.x / 2f + size.y / 2f).Rotate(angle), size.x / 2f);
                Gizmos.DrawWireSphere(position + new Vector2(0, size.x / 2f - size.y / 2f).Rotate(angle), size.x / 2f);
            }
        }
        
        public static Quaternion RandomRotation()
        {
            return Quaternion.Euler(0, 0, Random.Range(0, 360));
        }
    }
}
