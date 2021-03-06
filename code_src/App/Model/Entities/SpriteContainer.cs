using App.Engine;
using App.Engine.Physics;

namespace App.Model.Entities
{
    public class SpriteContainer
    {
        public Sprite Content;
        public Vector CenterPosition;
        public float Angle;
        public bool IsEmpty;

        public SpriteContainer(Sprite startContent, Vector startCenterPosition, float startAngle)
        {
            Content = startContent;
            CenterPosition = startCenterPosition;
            Angle = startAngle;
        }

        public void ClearContent()
        {
            Content = null;
            IsEmpty = true;
        }
    }
}