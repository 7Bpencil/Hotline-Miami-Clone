﻿using System.Collections.Generic;
using App.Engine.Physics;

namespace App.Model.Entities
{
    public abstract class Weapon
    {
        public abstract string Name { get; }
        public abstract int AmmoAmount { get;}
        public abstract int MagazineCapacity { get;}
        public abstract List<Bullet> Fire(Vector gunPosition, Vector listenerPosition, CustomCursor cursor);
        public abstract List<Bullet> Fire(Vector gunPosition, Vector sightDirection, Vector listenerPosition);
        public abstract void IncrementTick();
        public abstract float BulletWeight { get; }
        public abstract void AddAmmo(int amount);
        public abstract bool IsReady();
    }
}