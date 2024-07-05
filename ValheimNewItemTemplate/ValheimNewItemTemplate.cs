using System;
using System.IO;
using BepInEx;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using UnityEngine;

// There's two ways of creating new items in Valheim (as far as I know, anyway):
//
// -------------------
// #1. Item Cloning
// -------------------
// Taking an existing game item, copying it, applying some changes, and then registering it is called item cloning.
// Cloning is of course more restrictive than option #2, but is sometimes easier and can save you a lot of time,
// work, and overhead if all you want is to make some small changes.
//
// The biggest restriction that comes with it is that you can only work with things that already exist within the game
// itself, or that have been already loaded by other mods.
//
// -------------------
// #2. Custom Item in Asset Bundle
// -------------------
// This is when you use Unity to create a brand-new item in an asset bundle, then load it in and register it. It has
// the advantage of being able to bring completely new models, textures, structures, etc. into the game. Most of the
// time you probably want to be doing things this way.

namespace ValheimNewItemTemplate
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.NotEnforced, VersionStrictness.None)]
    internal class ValheimNewItemTemplate : BaseUnityPlugin
    {
        public const string PluginGuid = "com.chebgonaz.valheimnewitemtemplate";
        public const string PluginName = "ValheimNewItemTemplate";
        public const string PluginVersion = "0.0.1";

        private string _vanillaPrefab = "SledgeDemolisher";
        private string _bundleName = "mybundle";
        private string _meshName = "mymesh";
        private string _materialName = "mymaterial";
        
        private Mesh _replacementMesh;
        private Material _replacementMaterial;

        private void Awake()
        {
            // First thing we do once the mod has loaded is load our asset bundle.
            // LoadAssetBundle();

            // Next, once the vanilla prefabs are available, we do the swap. 
            PrefabManager.OnVanillaPrefabsAvailable += DoOnVanillaPrefabsAvailable; // subscribe method to event
        }

        private void DoOnVanillaPrefabsAvailable()
        {
            PrefabManager.OnVanillaPrefabsAvailable -= DoOnVanillaPrefabsAvailable; // unsubscribe

            // -------------------
            // #1. Item Cloning
            // -------------------
            // Cloning is done when the OnVanillaPrefabsAvailable event is raised, because this is when all the
            // vanilla game stuff has finished loading. We can't clone stuff until it's loaded.
            //
            // Jotunn's cloning guide: https://valheim-modding.github.io/Jotunn/tutorials/items.html
            
            // Create and add a custom item based on SwordBlackmetal
            var fireAxeConfig = new ItemConfig();
            fireAxeConfig.Name = "Fire Sword Demo";
            fireAxeConfig.Description = "A silly sword.";
            fireAxeConfig.CraftingStation = CraftingStations.Workbench;
            fireAxeConfig.AddRequirement(new RequirementConfig("Stone", 1));
            fireAxeConfig.AddRequirement(new RequirementConfig("Wood", 1));

            var fireAxe = new CustomItem("DemoFireAxe", "AxeBronze", fireAxeConfig);
            
            // Just to demonstrate what's possible, we'll add a flaming effect to it and make it deal fire damage
            fireAxe.ItemDrop.m_itemData.m_shared.m_damages.m_fire = 25;

            // first, figure out where we want to add the effect. In this case: directly on the axe itself, so it
            var attachmentPoint = fireAxe.ItemPrefab.transform.Find("attach/Viking_Axe");
            
            // To add the fire effect, we're copying it from the fire sword
            // 1. Locate the prefab (like, the blueprint for the object)
            var fireSwordPrefab = PrefabManager.Instance.GetPrefab("SwordIronFire");
            // 2. Instantiate an instance of the object (basically, make a version of the sword)
            var fireSwordCopy = Instantiate(fireSwordPrefab);
            // 3. Locate the flaming effects of the sword, slightly adjust its position to fit the axe better, then
            //    assign it a new parent (our axe).
            var fireEffects = fireSwordCopy.transform.Find("attach/effects");
            fireEffects.position = attachmentPoint.position + Vector3.forward;
            fireEffects.SetParent(attachmentPoint);
            // 4. Destroy the copy, as we don't need it anymore
            Destroy(fireSwordCopy);
            
            // Now all changes are finished, add the item
            ItemManager.Instance.AddItem(fireAxe);
        }

        private void LoadAssetBundle()
        {
            var assetBundlePath = Path.Combine(Path.GetDirectoryName(Info.Location), _bundleName);
            var assetBundle = AssetUtils.LoadAssetBundle(assetBundlePath);
            try
            {
                _replacementMesh = assetBundle.LoadAsset<Mesh>(_meshName);
                _replacementMaterial = assetBundle.LoadAsset<Material>(_materialName);
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"Exception caught while loading assets: {ex}");
            }
            finally
            {
                assetBundle.Unload(false);
            }
        }
    }
}