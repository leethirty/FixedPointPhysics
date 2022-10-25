using BlueNoah.Math.FixedPoint;
using System.Runtime.CompilerServices;

namespace BlueNoah.PhysicsEngine
{
    public partial class FixedPointIntersection
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool PointInSphere(FixedPointVector3 point,FixedPointVector3 position, FixedPoint64 radius)
        {
            return (point - position).sqrMagnitude <= radius * radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FixedPointVector3 ClosestPointWithPointAndSphere(FixedPointVector3 point, FixedPointVector3 position, FixedPoint64 radius)
        {
            if ((point - position).sqrMagnitude <= radius * radius)
            {
                return point;
            }
            return  (point - position).normalized * radius;
        }

        //Check point whether in AABB.
        //GamePhysics
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool PointInAABB(FixedPointVector3 point, FixedPointVector3 min, FixedPointVector3 max)
        {
            if (point.x < min.x || point.y < min.y || point.y < min.y)
            {
                return false;
            }
            if (point.x > max.x || point.y > max.y || point.y > max.y)
            {
                return false;
            }
            return true;
        }

        //ClosestPoint between Point and AABB.
        //GamePhysics
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FixedPointVector3 ClosestPointWithPointAndAABB(FixedPointVector3 point, FixedPointVector3 min, FixedPointVector3 max)
        {
            var result = point;
            result.x = FixedPointMath.Clamp(result.x, min.x, max.x);
            result.y = FixedPointMath.Clamp(result.y, min.y, max.y);
            result.z = FixedPointMath.Clamp(result.z, min.z, max.z);
            return result;
        }
        //Check point whether in OBB.
        //GamePhysics
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool PointInOBB(FixedPointVector3 point , FixedPointVector3 position, FixedPointVector3 halfSize, FixedPointMatrix fixedPointMatrix)
        {
            var dir = point - position;
            if (PointInOBBAxis(dir, new FixedPointVector3(fixedPointMatrix.M11, fixedPointMatrix.M12, fixedPointMatrix.M13), halfSize.x))
            {
                return false;
            }
            if (PointInOBBAxis(dir, new FixedPointVector3(fixedPointMatrix.M21, fixedPointMatrix.M22, fixedPointMatrix.M23), halfSize.y))
            {
                return false;
            }
            if (PointInOBBAxis(dir, new FixedPointVector3(fixedPointMatrix.M31, fixedPointMatrix.M32, fixedPointMatrix.M33), halfSize.z))
            {
                return false;
            }
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool PointInOBBAxis(FixedPointVector3 dir,FixedPointVector3 axis,FixedPoint64 axisDis)
        {
            var distance = FixedPointVector3.Dot(dir, axis);
            return (distance < -axisDis || distance > axisDis);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FixedPointVector3 ClosestPointWithPointAndOBB(FixedPointVector3 point, FixedPointVector3 position, FixedPointVector3 halfSize, FixedPointMatrix fixedPointMatrix)
        {
            var dir = point - position;
            return position + PointDistanceOBBAxis(dir, new FixedPointVector3(fixedPointMatrix.M11, fixedPointMatrix.M12, fixedPointMatrix.M13), halfSize.x)
                + PointDistanceOBBAxis(dir, new FixedPointVector3(fixedPointMatrix.M21, fixedPointMatrix.M22, fixedPointMatrix.M23), halfSize.y)
                + PointDistanceOBBAxis(dir, new FixedPointVector3(fixedPointMatrix.M31, fixedPointMatrix.M32, fixedPointMatrix.M33), halfSize.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static FixedPointVector3 PointDistanceOBBAxis(FixedPointVector3 dir, FixedPointVector3 axis, FixedPoint64 axisDis)
        {
            var distance = FixedPointVector3.Dot(dir, axis);
            return FixedPointMath.Clamp(distance,-axisDis,axisDis) * axis;
        }



    }
}