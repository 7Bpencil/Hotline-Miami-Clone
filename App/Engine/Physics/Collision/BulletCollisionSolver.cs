using System;
using App.Engine.Physics.RigidBody;
using App.Model;

namespace App.Engine.Physics.Collision
{
    public static class BulletCollisionSolver
    {
        private static float[] AreCollide(Bullet bullet, RigidAABB rectangle)
        {
            var tMin = 0f;
            var tMax = float.PositiveInfinity;
            for (var i = 0; i < 2; i++)
            {
                if (Math.Abs(bullet.velocity[i]) < 0.01)
                {
                    if (bullet.position[i] < rectangle.MinPoint[i] || bullet.position[i] > rectangle.MaxPoint[i])
                        return null;
                }
                else
                {
                    var ood = 1.0f / bullet.velocity[i];
                    var t1 = (rectangle.MinPoint[i] - bullet.position[i]) * ood;
                    var t2 = (rectangle.MaxPoint[i] - bullet.position[i]) * ood;
                    if (t1 > t2)
                    {
                        var buf = t1;
                        t1 = t2;
                        t2 = buf;
                    }

                    tMin = Math.Max(tMin, t1);
                    tMax = Math.Min(tMax, t2);
                    if (tMin > tMax) return null;
                }
            }

            return new[] {tMin, tMax};
        }
        
        private static float[] AreCollide(Bullet bullet, RigidCircle circle)
        {
            var dX = bullet.position.X - circle.Center.X;
            var dY = bullet.position.Y - circle.Center.Y;
            var dVx = bullet.velocity.X;
            var dVy = bullet.velocity.Y;
            // t^2 * (dVx^2 + dVy^2) + 2t(dX * dVx + dY * dVy) + (dX^2 + dY^2 - R^2) = 0

            var a = dVx * dVx + dVy * dVy;
            var b = dX * dVx + dY * dVy;
            var D = 4 * (b * b - a * (dX * dX + dY * dY - circle.Radius * circle.Radius));
            if (D < 0) return null;

            var delA = 1 / (2 * a);
            var sqrtD = (float) Math.Sqrt(D);

            var t1 = (-2 * b - sqrtD) * delA;
            var t2 = (-2 * b + sqrtD) * delA;

            return new[] {t1, t2};
        }

        private static float[] AreCollideDynamically(Bullet bullet, RigidCircle circle, Vector circleVelocity)
        {
            var dX = bullet.position.X - circle.Center.X;
            var dY = bullet.position.Y - circle.Center.Y;
            var dVx = bullet.velocity.X - circleVelocity.X;
            var dVy = bullet.velocity.Y - circleVelocity.Y;
            // t^2 * (dVx^2 + dVy^2) + 2t(dX * dVx + dY * dVy) + (dX^2 + dY^2 - R^2) = 0

            var a = dVx * dVx + dVy * dVy;
            var b = dX * dVx + dY * dVy;
            var D = 4 * (b * b - a * (dX * dX + dY * dY - circle.Radius * circle.Radius));
            if (D < 0) return null;

            var delA = 1 / (2 * a);
            var sqrtD = (float) Math.Sqrt(D);

            var t1 = (-2 * b - sqrtD) * delA;
            var t2 = (-2 * b + sqrtD) * delA;

            // TODO there could be great optimization but it's really case specific
            if (t1 > 1) return null;

            var ist1 = t1 > 0;
            var ist2 = t2 > 0 && t2 < 1;
            
            if (ist1 && ist2) return new[] {t1, t2,};
            if (ist1) return new[] {t1};
            if (ist2) return new[] {t2};
            return null;
        }
    }
}