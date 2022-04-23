using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class Math
    {
        public static Vector2 GetRandomDirectionV2()
        {
            return new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        }

        public static Vector3 GetRandomDirectionV3()
        {
            return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        }
        public static Vector2 RadianToVector2(float radian)
        {
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }

        public static Vector2 DegreeToVector2(float degree)
        {
            return RadianToVector2(degree * Mathf.Deg2Rad);
        }
    }

    public class Common
    {
        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 v3 = GetMousePostionWorldWithZ(Input.mousePosition, Camera.main);
            v3.z = 0f;
            return v3;
        }
        public static Vector3 GetMousePostionWorldWithZ()
        {
            return GetMousePostionWorldWithZ(Input.mousePosition, Camera.main);
        }
        public static Vector3 GetMousePostionWorldWithZ(Camera worldCamera)
        {
            return GetMousePostionWorldWithZ(Input.mousePosition, worldCamera);
        }
        public static Vector3 GetMousePostionWorldWithZ(Vector3 screenPos, Camera worldCamera) 
        {
            screenPos.z = 999f;
            Vector3 worldPos = worldCamera.ScreenToWorldPoint(screenPos);
            return worldPos;
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

