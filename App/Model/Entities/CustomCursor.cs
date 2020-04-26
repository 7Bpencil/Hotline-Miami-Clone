using App.Engine;
using App.Engine.Physics;
using App.Engine.Physics.RigidShape;

namespace App.Model.Entities
{
    public class CustomCursor
    {
        private RigidCircle shape;
        private Sprite sprite;

        public Vector Position => shape.Center;

        public CustomCursor(RigidCircle shape, Sprite sprite)
        {
            this.shape = shape;
            this.sprite = sprite;
        }

        public void MoveBy(Vector delta) => shape.MoveBy(delta);
    }
}