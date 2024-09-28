using RoR2;
using RoR2.Achievements;
using UnityEngine;

namespace RMORMod.Content.RMOR.Achievements
{
    [RegisterAchievement("MoriyaRMORClearGameMonsoon", "Skins.RMOR.Mastery", null, 10U, null)]
    public class RMORMasteryAchievement : BasePerSurvivorClearGameMonsoonAchievement
    {
        public override BodyIndex LookUpRequiredBodyIndex()
        { 
            return BodyCatalog.FindBodyIndex("RMORBody");
        }
    }
}
