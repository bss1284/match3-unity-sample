using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BSS.Extension {
    public static class Vector3Extension {
        #region Set,Add 함수
        public static Vector3 SetX(this Vector3 vector,float x) {
            return new Vector3(x, vector.y , vector.z);
        }
        public static Vector3 SetY(this Vector3 vector, float y) {
            return new Vector3(vector.x, y, vector.z);
        }
        public static Vector3 SetZ(this Vector3 vector, float z) {
            return new Vector3(vector.x, vector.y, z);
        }
        public static Vector3 AddX(this Vector3 vector, float x) {
            return vector+new Vector3(x, 0f, 0f);
        }
        public static Vector3 AddY(this Vector3 vector, float y) {
            return vector+new Vector3(0f, y, 0f);
        }
        public static Vector3 AddZ(this Vector3 vector, float z) {
            return vector+new Vector3(0f, 0f, z);
        }
        #endregion
    }
    public static class Vector2Extension {
        #region Set,Add 함수
        public static Vector2 SetX(this Vector2 vector, float x) {
            return new Vector2(x, vector.y);
        }
        public static Vector2 SetY(this Vector2 vector, float y) {
            return new Vector2(vector.x, y);
        }
        public static Vector2 AddX(this Vector2 vector, float x) {
            return new Vector2(x, vector.y);
        }
        public static Vector2 AddY(this Vector2 vector, float y) {
            return new Vector2(vector.x, y);
        }
        #endregion
    }


    public static class Vector2Ex {
        /// <summary>
        /// 현재 Vector에서 다른 Vector간의 방향을 구합니다. (단위:Degree)
        /// </summary>
        public static float GetDirectionDegree(Vector2 origin, Vector2 lookAt) {
            Vector2 direction = lookAt - origin;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle < 0f) angle += 360f;
            return angle;
        }
        public static Vector2 InverseScale(Vector2 origin, Vector2 other) {
            if (other.x.Equals(0f) || other.y.Equals(0f)) {
                Debug.LogError("Can not divide by 0");
                return Vector2.zero;
            }
            return new Vector2(origin.x / other.x, origin.y / other.y);
        }

        public static Vector2 Random(float minX, float maxX, float minY, float maxY) {
            return new Vector2(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY));
        }
        public static Vector2 Spread(Vector2 vector, float val) {
            return new Vector2(UnityEngine.Random.Range(vector.x-val, vector.x + val), UnityEngine.Random.Range(vector.y - val, vector.y + val));
        }
    }
    public static class Vector3Ex {
        /// <summary>
        /// 현재 Vector에서 다른 Vector간의 방향을 구합니다. (단위:Degree)
        /// </summary>
        public static float GetDirectionDegree(Vector3 origin, Vector3 lookAt) {
            Vector3 direction = lookAt - origin;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle < 0f) angle += 360f;
            return angle;
        }

        public static Vector3 InverseScale(Vector3 origin, Vector3 other) {
            if (other.x.Equals(0f) || other.y.Equals(0f) || other.z.Equals(0f)) {
                Debug.LogError("Can not divide by 0");
                return Vector2.zero;
            }
            return new Vector3(origin.x / other.x, origin.y / other.y, origin.z / other.z);
        }
    }
}
