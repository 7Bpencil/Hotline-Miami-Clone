﻿namespace App.Engine.PhysicsEngine.RigidBody
{
    public class RigidCircle : RigidShape
    {
        private float radius;
        public float Radius { get => radius; set => radius = value; }
        
        public float Diameter => 2 * radius;

        private Vector center;
        public override Vector Center => center;
        
        private bool isStatic;
        public override bool IsStatic { get => isStatic; set => isStatic = value; }

        private bool canCollide;
        public override bool CanCollide { get => canCollide; set => canCollide = value; }

        private bool isCollided;
        public override bool IsCollided { get => isCollided; set => isCollided = value; }

        public override float BondRadius => radius;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="isStatic"></param>
        /// <param name="canCollide">should a collision be calculated</param>
        public RigidCircle(Vector center, float radius, bool isStatic, bool canCollide)
        {
            this.radius = radius;
            this.center = center;
            this.isStatic = isStatic;
            this.canCollide = canCollide;
        }

        public override void Move(Vector delta)
        {
            center += delta;
        }

        public override void Rotate(float delta) // Because it's meaningless
        {
        }
        
        public override RigidShape DeepCopy()
        {
            return new RigidCircle(center, radius, isStatic, canCollide);
        }
    }
}