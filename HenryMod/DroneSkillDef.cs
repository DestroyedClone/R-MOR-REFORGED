using RMORMod.Content.RMORSurvivor.Components.Body;
using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace RMORMod
{
    public class DroneSkillDef : SkillDef
	{
        public TargetingMode targetingMode = TargetingMode.EnemiesAndAllies;

        public enum TargetingMode
        {
            EnemiesAndAllies,
            EnemiesOnly
        }
    }
}
