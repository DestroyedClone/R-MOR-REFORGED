using RoR2;
using UnityEngine;
using RoR2.Projectile;
using R2API;
using EntityStates.RMOR.Secondary;
using RMORMod.Content.RMORSurvivor.Components.Body;
using UnityEngine.AddressableAssets;
using RMORMod.Content.RMORSurvivor.Components.Projectiles;
using UnityEngine.Networking;
using RMORMod.Modules;

namespace RMORMod.Content.RMORSurvivor
{
    public class DroneSetup
    {
        public static GameObject droneProjectileGhost;
        public static void Init()
        {
            if (!DroneFollowerController.dronePrefab) DroneFollowerController.dronePrefab = CreateDroneFollower();
            if (!RMORTargetingController.enemyIndicatorPrefab) RMORTargetingController.enemyIndicatorPrefab = CreateEnemyIndicator();
        }

        private static GameObject CreateAllyIndicator()
        {
            GameObject indicator = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/PassiveHealing/WoodSpriteIndicator.prefab").WaitForCompletion().InstantiateClone("RMORMod_AllyIndicator", false);
            UnityEngine.Object.Destroy(indicator.GetComponentInChildren<RoR2.InputBindingDisplayController>());
            UnityEngine.Object.Destroy(indicator.GetComponentInChildren<TMPro.TextMeshPro>());

            Rewired.ComponentControls.Effects.RotateAroundAxis rot = indicator.GetComponentInChildren<Rewired.ComponentControls.Effects.RotateAroundAxis>();
            UnityEngine.Object.Destroy(rot);

            SpriteRenderer sr = indicator.GetComponentInChildren<SpriteRenderer>();
            sr.sprite = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texIndicatorDroneHeal.png");
            sr.color = new Color(189f / 255, 1f, 77f / 255f);
            sr.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            sr.transform.localScale = 0.5f * Vector3.one;

            return indicator;
        }
        private static GameObject CreateEnemyIndicator()
        {
            GameObject indicator = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Engi/EngiMissileTrackingIndicator.prefab").WaitForCompletion().InstantiateClone("RMORMod_EnemyIndicator", false);
            SpriteRenderer[] sr = indicator.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer s in sr)
            {
                if (s.name == "Base Core")
                {
                    s.color = new Color(0.556862745f, 0.682352941f, 0.690196078f);
                    break;
                }
            }
            return indicator;
        }

        private static GameObject CreateDroneProjectile()
        {
            GameObject droneProjectile = LegacyResourcesAPI.Load<GameObject>("prefabs/projectiles/EngiHarpoon").InstantiateClone("RMORMod_DroneProjectile", true);

            Shader hotpoo = LegacyResourcesAPI.Load<Shader>("Shaders/Deferred/hgstandard");
            //droneProjectileGhost = PrefabAPI.InstantiateClone(RMORMod.Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("DronePrefab"), "HANDMod_DroneProjectileGhost", false);


            MeshRenderer[] mr = droneProjectileGhost.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer m in mr)
            {
                if (m.name != "DronePropeller")
                {
                    m.material.shader = hotpoo;
                }
            }

            SkinnedMeshRenderer[] smr = droneProjectileGhost.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer m in smr)
            {
                if (m.name != "DronePropeller")
                {
                    m.material.shader = hotpoo;
                }
            }

            droneProjectileGhost.AddComponent<ProjectileGhostController>();

            droneProjectileGhost.layer = LayerIndex.noCollision.intVal;

            droneProjectile.GetComponent<ProjectileController>().ghostPrefab = droneProjectileGhost;

            Material droneMat = Modules.Materials.CreateHopooMaterial("DroneBody");
            Modules.Materials.SetEmission(droneMat, 3f, Color.white);
            droneProjectileGhost.GetComponentInChildren<SkinnedMeshRenderer>().material = droneMat;

            Collider[] collidersG = droneProjectileGhost.GetComponentsInChildren<Collider>();
            foreach (Collider cG in collidersG)
            {
                UnityEngine.Object.Destroy(cG);
            }

            RMORMod.Modules.ContentPacks.projectilePrefabs.Add(droneProjectile);

            UnityEngine.Object.Destroy(droneProjectile.GetComponent<ApplyTorqueOnStart>());
            UnityEngine.Object.Destroy(droneProjectile.GetComponent<MissileController>());
            ProjectileSteerTowardTarget pst = droneProjectile.AddComponent<ProjectileSteerTowardTarget>();
            pst.yAxisOnly = false;
            pst.rotationSpeed = 360f;

            ProjectileSimple ps = droneProjectile.AddComponent<ProjectileSimple>();
            ps.desiredForwardSpeed = 40f;
            ps.lifetime = 30f;
            ps.updateAfterFiring = true;
            ps.enableVelocityOverLifetime = false;
            ps.oscillate = true;
            ps.oscillateMagnitude = 6f;
            ps.oscillateSpeed = 1.5f;

            ProjectileSphereTargetFinder pstf = droneProjectile.AddComponent<ProjectileSphereTargetFinder>();
            pstf.lookRange = 90f;
            pstf.targetSearchInterval = 0.3f;
            pstf.onlySearchIfNoTarget = true;
            pstf.allowTargetLoss = false;
            pstf.testLoS = false;
            pstf.ignoreAir = false;
            pstf.flierAltitudeTolerance = Mathf.Infinity;

            UnityEngine.Object.Destroy(droneProjectile.GetComponent<AkEvent>());
            UnityEngine.Object.Destroy(droneProjectile.GetComponent<AkGameObj>());
            UnityEngine.Object.Destroy(droneProjectile.GetComponent<ProjectileSingleTargetImpact>());

            ProjectileStickOnImpact stick = droneProjectile.AddComponent<ProjectileStickOnImpact>();
            stick.ignoreWorld = true;
            stick.ignoreCharacters = false;
            stick.alignNormals = false;


            Collider[] colliders = droneProjectile.GetComponentsInChildren<Collider>();
            foreach (Collider c in colliders)
            {
                UnityEngine.Object.Destroy(c);
            }
            SphereCollider sc = droneProjectile.AddComponent<SphereCollider>();
            sc.radius = 0.6f;
            sc.contactOffset = 0.01f;

            droneProjectile.AddComponent<Components.DroneProjectile.DroneDamageController>();
            droneProjectile.AddComponent<Components.DroneProjectile.DroneCollisionController>();

            ProjectileController pc = droneProjectile.GetComponent<ProjectileController>();
            pc.allowPrediction = false;

            //droneProjectile.layer = LayerIndex.collideWithCharacterHullOnly.intVal;

            return droneProjectile;
        }

        private static GameObject CreateDroneFollower()
        {
            Shader hotpoo = LegacyResourcesAPI.Load<Shader>("Shaders/Deferred/hgstandard");
            GameObject droneFollower = PrefabAPI.InstantiateClone(RMORMod.Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("DroneFollowerPrefab"), "RMORMod_DroneFollower", false);

            MeshRenderer[] meshes = droneFollower.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer m in meshes)
            {
                if (m.name != "DronePropeller")
                {
                    m.material.shader = hotpoo;
                }
            }

            droneFollower.layer = LayerIndex.noCollision.intVal;

            Material droneMat = Modules.Materials.CreateHopooMaterial("DroneBody");
            Modules.Materials.SetEmission(droneMat, 3f, Color.white);
            droneFollower.GetComponentInChildren<SkinnedMeshRenderer>().material = droneMat;

            SkinnedMeshRenderer[] smr = droneFollower.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer m in smr)
            {
                if (m.name != "DronePropeller")
                {
                    m.material.shader = hotpoo;
                }
            }

            return droneFollower;
        }
    }
}
