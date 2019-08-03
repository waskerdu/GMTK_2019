using Malee;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

#pragma warning disable 0660, 0661
public static class Utilities
{
    public static float Map(float value, float minA, float maxA, float minB, float maxB)
    {
        return (value - minA) / (maxA - minA) * (maxB - minB) + minB;
    }

    /// <summary>
    /// Returns 2D vector coordinates given radius and degrees
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="degrees"></param>
    /// <returns></returns>
    public static Vector2 PolarToCartesian(float radius, float degrees)
    {
        float theta = Mathf.Deg2Rad * degrees;
        float x = Mathf.Sin(theta) * radius;
        float y = Mathf.Cos(theta) * radius;
        return new Vector2(x, y);
    }

    /// <summary>
    /// returns new vector2 where x = radius and y = degrees
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static Vector2 CartesianToPolar(float x, float y)
    {
        float radius = Mathf.Sqrt(x * x + y * y);
        float theta = Mathf.Atan(x / y);
        float degrees = Mathf.Rad2Deg * theta;
        return new Vector2(radius, degrees);
    }

    #region Vector 2 Serializer
    /// <summary>
    /// Serializes Vector2 data to simple x and y floats for use in data persistence.
    /// <para>
    /// Moving to and from Vector2Serializer and Vector2 is implicit. Simply do: Vector2 vec2 = vec2SerializerInstance;
    /// </para>
    /// </summary>
    [Serializable]
    public struct Vector2Serializer
    {
        public float x;
        public float y;

        public void Fill(Vector3 v3)
        {
            x = v3.x;
            y = v3.y;
        }

        public static implicit operator Vector2(Vector2Serializer v3s)
        {
            return new Vector2(v3s.x, v3s.y);
        }

        public static implicit operator Vector2Serializer(Vector2 v2)
        {
            Vector2Serializer vec = new Vector2Serializer();
            vec.Fill(v2);
            return vec;
        }

        public static bool operator ==(Vector2Serializer lhs, Vector2Serializer rhs)
        {
            return (
                lhs.x == rhs.x &&
                lhs.y == rhs.y
            );
        }

        public static bool operator !=(Vector2Serializer lhs, Vector2Serializer rhs)
        {
            return !(lhs == rhs);
        }
    }
    #endregion

    #region Vector 3 Serializer
    /// <summary>
    /// Serializes Vector3 data to simple x, y, and z floats for use in data persistence.
    /// <para>
    /// Moving to and from Vector3Serializer and Vector3 is implicit. Simply do: Vector3 vec3 = vec3SerializerInstance;
    /// </para>
    /// </summary>
    [Serializable]
    public struct Vector3Serializer
    {
        public float x;
        public float y;
        public float z;

        public void Fill(Vector3 v3)
        {
            x = v3.x;
            y = v3.y;
            z = v3.z;
        }

        public static implicit operator Vector3(Vector3Serializer v3s)
        {
            return new Vector3(v3s.x, v3s.y, v3s.z);
        }

        public static implicit operator Vector3Serializer(Vector3 v3)
        {
            Vector3Serializer vec = new Vector3Serializer();
            vec.Fill(v3);
            return vec;
        }

        public static bool operator ==(Vector3Serializer lhs, Vector3Serializer rhs)
        {
            return (
                lhs.x == rhs.x &&
                lhs.y == rhs.y &&
                lhs.z == rhs.z
            );
        }

        public static bool operator !=(Vector3Serializer lhs, Vector3Serializer rhs)
        {
            return !(lhs == rhs);
        }
    }
    #endregion

    #region Quaternion Serializer
    /// <summary>
    /// Serializes Quaternion data to simple x, y, z, and w floats for use in data persistence.
    /// <para>
    /// Moving to and from QuaternionSerializer and Quaternion is implicit. Simply do: Quaternion quat = quaternionSerializerInstance;
    /// </para>
    /// </summary>
    [Serializable]
    public struct QuaternionSerializer
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public void Fill(Quaternion q)
        {
            x = q.x;
            y = q.y;
            z = q.z;
            w = q.w;
        }

        public static implicit operator Quaternion(QuaternionSerializer v2s)
        {
            return new Quaternion(v2s.x, v2s.y, v2s.z, v2s.w);
        }

        public static implicit operator QuaternionSerializer(Quaternion quat)
        {
            QuaternionSerializer quaternion = new QuaternionSerializer();
            quaternion.Fill(quat);
            return quaternion;
        }

        public static bool operator ==(QuaternionSerializer lhs, QuaternionSerializer rhs)
        {
            return (
                lhs.x == rhs.x &&
                lhs.y == rhs.y &&
                lhs.z == rhs.z &&
                lhs.w == rhs.w
            );
        }

        public static bool operator !=(QuaternionSerializer lhs, QuaternionSerializer rhs)
        {
            return !(lhs == rhs);
        }

        public Quaternion GetQuaternion
        { get { return new Quaternion(x, y, z, w); } }
    }
    #endregion
}
