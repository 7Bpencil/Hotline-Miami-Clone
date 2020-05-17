using System;
using System.Collections.Generic;
using App.Engine.Audio;
using App.Engine.Physics;

namespace App.Model.Entities.Weapons
{
    public class GrenadeLauncher : Weapon
    {
        private readonly Random r;
        private readonly string fireSoundPath;
        private readonly string fireSoundPath3D;

        private readonly string name;
        public override string Name => name;
        
        private readonly int capacity;
        public override int MagazineCapacity => capacity;
        
        private readonly float bulletWeight;
        public override float BulletWeight => bulletWeight;
        
        private int ammo;
        public override int AmmoAmount => ammo;

        private readonly int firePeriod;
        private int ticksFromLastFire;

        public GrenadeLauncher(int ammo)
        {
            name = "Grenade-Launcher";
            capacity = 6;
            firePeriod = 12;
            ticksFromLastFire = firePeriod + 1;
            bulletWeight = 1f;
            this.ammo = ammo;
            r = new Random();
            
            fireSoundPath = @"event:/gunfire/2D/GL_FIRE";
            fireSoundPath3D = @"event:/gunfire/3D/GL_FIRE_3D";
        }

        public override void IncrementTick() => ticksFromLastFire++;
        
        public override List<AbstractProjectile> Fire(Vector gunPosition, CustomCursor cursor)
        {
            var spray = new List<AbstractProjectile>();
            var direction = (cursor.Position - gunPosition).Normalize();
            var position = gunPosition + direction * 40;

            spray.Add(new Grenade(position, direction * 40, 500, cursor.Position, 96, 2));
            
            ammo--;
            ticksFromLastFire = 0;
            AudioEngine.PlayNewInstance(fireSoundPath);
            cursor.MoveBy(direction.GetNormal() * r.Next(-30, 30) + new Vector(r.Next(2, 2), r.Next(2, 2)));

            return spray;
        }
        
        public override List<AbstractProjectile> Fire(Vector gunPosition, Vector targetPosition)
        {
            var spray = new List<AbstractProjectile>();
            var direction = (targetPosition - gunPosition).Normalize();
            var position = gunPosition + direction * 40;
            
            spray.Add(new Grenade(position, direction * 40, 500, targetPosition, 96, 2));
            
            ammo--;
            ticksFromLastFire = 0;
            AudioEngine.PlayNewInstance(fireSoundPath3D, gunPosition);

            return spray;
        }
        
        public override void AddAmmo(int amount)
        {
            if (amount > ammo) ammo = amount;
        }

        public override bool IsReady => ticksFromLastFire >= firePeriod && ammo > 0;
    }
}