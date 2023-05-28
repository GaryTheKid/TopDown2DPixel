/* Last Edition: 05/28/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   The utility class for common functions
 * Last Edition:
 *   Just Created.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utilities
{
    public class Math
    {
        public static float GetRandomDegree()
        {
            return UnityEngine.Random.Range(0.0f, 360.0f);
        }

        public static Vector2 GetRandomDirectionV2()
        {
            return new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        }

        public static Vector3 GetRandomDirectionV3()
        {
            return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        }

        public static float GetSizeFromTwoVerts(Vector3 v1, Vector3 v2)
        {
            return Mathf.Abs(v1.x - v2.x) * Mathf.Abs(v1.y - v2.y);
        }

        public static Vector2 RadianToVector2(float radian)
        {
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }

        public static Vector2 DegreeToVector2(float degree)
        {
            return RadianToVector2(degree * Mathf.Deg2Rad);
        }

        public static float Vector2ToDegree(Vector2 v2)
        {
            return Vector2.SignedAngle(Vector2.up, v2) + 90f;
        }

        public static Vector3 RadianToVector3(float radian)
        {
            return new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0f);
        }

        public static Vector3 DegreeToVector3(float degree)
        {
            return RadianToVector3(degree * Mathf.Deg2Rad);
        }
    }

    public class Common
    {
        public static Vector3 GetMouseScreenPosition()
        {
            return Mouse.current.position.ReadValue();
        }
        public static Vector3 GetMouseWorldPosition()
        {
            var currMousePos = GetMouseScreenPosition();
            Vector3 v3 = GetMousePostionWorldWithZ(new Vector3(currMousePos.x, currMousePos.y), Camera.main);
            v3.z = 0f;
            return v3;
        }
        public static Vector3 GetMousePostionWorldWithZ()
        {
            var currMousePos = GetMouseScreenPosition();
            return GetMousePostionWorldWithZ(new Vector3(currMousePos.x, currMousePos.y), Camera.main);
        }
        public static Vector3 GetMousePostionWorldWithZ(Camera worldCamera)
        {
            var currMousePos = GetMouseScreenPosition();
            return GetMousePostionWorldWithZ(new Vector3(currMousePos.x, currMousePos.y), worldCamera);
        }
        public static Vector3 GetMousePostionWorldWithZ(Vector3 screenPos, Camera worldCamera) 
        {
            screenPos.z = Mathf.Abs(worldCamera.transform.position.z);
            Vector3 worldPos = worldCamera.ScreenToWorldPoint(screenPos);
            return worldPos;
        }

        public static Ray GetScreenPointRay()
        {
            return Camera.main.ScreenPointToRay(GetMouseScreenPosition());
        }

        public static Vector2 GetScreenWorldPoint2D() 
        {
            return Camera.main.ScreenToWorldPoint(GetMouseScreenPosition());
        }

        public static float GetMouseRotationEulerAngle(Vector3 origin)
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            Vector3 aimDir = (mousePosition - origin).normalized;
            return Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        }

        public static float GetEulerAngleBetweenPoints(Vector3 origin, Vector3 end)
        {
            Vector3 dir = (end - origin).normalized;
            return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }

        public static float ConvertAngle(float angle)
        {
            if (angle < 0)
            {
                angle += 360;
            }
            else if (angle >= 360)
            {
                angle -= 360;
            }
            return angle;
        }

        public static float ConvertAngleToClockwise(float angle)
        {
            float convertedAngle = 90 - angle;
            if (convertedAngle < 0)
            {
                convertedAngle += 360;
            }
            return convertedAngle;
        }

        public static float GetEulerAngleBetweenPointsClockWise(Vector3 origin, Vector3 end)
        {
            return ConvertAngleToClockwise(ConvertAngle(GetEulerAngleBetweenPoints(origin, end)));
        }

        public static System.Object GetObjectCopyFromInstance(System.Object obj)
        {
            return Activator.CreateInstance(obj.GetType());
        }
    }

    public class Printer<T>
    {
        public static void PrintList(List<T> list) 
        {
            if (list == null)
                return;

            foreach (T element in list)
                Debug.Log(element);
        }
    }
}

