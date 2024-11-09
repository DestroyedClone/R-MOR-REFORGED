using RMORMod.Modules.Achievements;
using RoR2;
using RoR2.Achievements;
using UnityEngine;

namespace RMOR.Content.HANDSurvivor.Achievements
{
    [RegisterAchievement("MoriyaRMOROverclockedClearGameTyphoon", "Skins.RMOR.GrandMastery", null, 15u, null)]
    public class RMORGrandMasteryAchievement : BaseGrandMasteryAchievement
    {
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("HANDOverclockedBody");
        }
    }
}
