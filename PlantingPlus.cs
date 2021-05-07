using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using BepInEx;
using BepInEx.Configuration;
using System.IO;

namespace PlantingPlus
{
    [BepInPlugin("com.bkeyes93.PlantingPlus", "PlantingPlus", "1.4.5")]
    public class PlantingPlus : BaseUnityPlugin
    {
        public const string MODNAME = "PlantingPlus";
        public const string AUTHOR = "bkeyes93";
        public const string GUID = "com.bkeyes93.PlantingPlus";
        public const string VERSION = "1.4.5";

        public void Awake()
        {
            PlantingPlus.modEnabled = base.Config.Bind<bool>("General", "Enabled", true, "Enable this mod");
            PlantingPlus.nexusID = base.Config.Bind<int>("General", "NexusID", 274, "Nexus mod ID for updates");
            PlantingPlus.enableOtherResources = base.Config.Bind<bool>("General", "EnableOtherResources", true, "Enable planting resources other than berries");
            PlantingPlus.resourcesSpawnEmpty = base.Config.Bind<bool>("General", "ResourcesSpawnEmpty", false, "Pickable resources will spawn empty rather than full");
            PlantingPlus.requireCultivation = base.Config.Bind<bool>("General", "RequireCultivation", false, "Pickable resources can only be planted on cultivated ground");
            PlantingPlus.placeAnywhere = base.Config.Bind<bool>("General", "PlaceAnywhere", false, "Allow resources to be placed anywhere. This will only apply to bushes and trees");
            PlantingPlus.enforceBiomes = base.Config.Bind<bool>("General", "EnforceBiomes", false, "Only allow resources to be placed in their respective biome");
            PlantingPlus.alternateIcons = base.Config.Bind<bool>("General", "AlternateIcons", false, "User berry icons in the cultivator menu rather than the default ones");
            PlantingPlus.enableCustomRespawnTimes = base.Config.Bind<bool>("General", "EnableCustomRespawnTimes", true, "Enable custom respawn times for pickable resources");
            PlantingPlus.enableCustomResourceAmounts = base.Config.Bind<bool>("General", "EnableCustomResourceAmounts", true, "Enable custom resource amounts for pickable resources");
            PlantingPlus.raspberryCost = base.Config.Bind<int>("General", "RaspberryCost", 10, "Number of raspberries required to place a raspberry bush");
            PlantingPlus.blueberryCost = base.Config.Bind<int>("General", "BlueberryCost", 10, "Number of blueberries required to place a blueberry bush");
            PlantingPlus.cloudberryCost = base.Config.Bind<int>("General", "CloudberryCost", 10, "Number of cloudberries required to place a cloudberry bush");
            PlantingPlus.mushroomCost = base.Config.Bind<int>("General", "MushroomCost", 5, "Number of mushrooms required to place a pickable mushroom spawner");
            PlantingPlus.yellowMushroomCost = base.Config.Bind<int>("General", "YellowMushroomCost", 5, "Number of yellow mushrooms required to place a pickable yellow mushroom spawner");
            PlantingPlus.blueMushroomCost = base.Config.Bind<int>("General", "BlueMushroomCost", 5, "Number of blue mushrooms required to place a pickable blue mushroom spawner");
            PlantingPlus.thistleCost = base.Config.Bind<int>("General", "ThistleCost", 5, "Number of thistle required to place a pickable thistle spawner");
            PlantingPlus.dandelionCost = base.Config.Bind<int>("General", "DandelionCost", 5, "Number of dandelion required to place a pickable dandelion spawner");
            PlantingPlus.birchCost = base.Config.Bind<int>("General", "BirchCost", 1, "Number of birch cones required to place a birch sapling");
            PlantingPlus.oakCost = base.Config.Bind<int>("General", "OakCost", 1, "Number of oak seeds required to place an oak sapling");
            PlantingPlus.ancientCost = base.Config.Bind<int>("General", "AncientCost", 1, "Number of ancient seeds required to place an ancient sapling");
            PlantingPlus.raspberryRespawnTime = base.Config.Bind<int>("General", "RaspberryRespawnTime", 300, "Number of minutes it takes for a raspberry bush to respawn berries");
            PlantingPlus.blueberryRespawnTime = base.Config.Bind<int>("General", "BlueberryRespawnTime", 300, "Number of minutes it takes for a blueberry bush to respawn berries");
            PlantingPlus.cloudberryRespawnTime = base.Config.Bind<int>("General", "CloudberryRespawnTime", 300, "Number of minutes it takes for a cloudberry bush to respawn berries");
            PlantingPlus.mushroomRespawnTime = base.Config.Bind<int>("General", "MushroomRespawnTime", 240, "Number of minutes it takes for mushrooms to respawn");
            PlantingPlus.yellowMushroomRespawnTime = base.Config.Bind<int>("General", "YellowMushroomRespawnTime", 240, "Number of minutes it takes for yellow mushrooms to respawn");
            PlantingPlus.blueMushroomRespawnTime = base.Config.Bind<int>("General", "BlueMushroomRespawnTime", 240, "Number of minutes it takes for blue mushrooms to respawn");
            PlantingPlus.thistleRespawnTime = base.Config.Bind<int>("General", "ThistleRespawnTime", 240, "Number of minutes it takes for thistle to respawn");
            PlantingPlus.dandelionRespawnTime = base.Config.Bind<int>("General", "DandelionRespawnTime", 240, "Number of minutes it takes for dandelion to respawn");
            PlantingPlus.birchGrowthTime = base.Config.Bind<float>("General", "BirchGrowthTime", 3000f, "Number of seconds it takes for a birch tree to grow from a birch sapling (will take at least 10 seconds after planting to grow)");
            PlantingPlus.oakGrowthTime = base.Config.Bind<float>("General", "OakGrowthTime", 3000f, "Number of seconds it takes for an oak tree to grow from an oak sapling (will take at least 10 seconds after planting to grow)");
            PlantingPlus.ancientGrowthTime = base.Config.Bind<float>("General", "AncientGrowthTime", 3000f, "Number of seconds it takes for an ancient tree to grow from an ancient sapling (will take at least 10 seconds after planting to grow)");
            PlantingPlus.raspberryResourceAmount = base.Config.Bind<int>("General", "RaspberryResourceAmount", 1, "Number of berries a raspberry bush will spawn");
            PlantingPlus.blueberryResourceAmount = base.Config.Bind<int>("General", "BlueberryResourceAmount", 1, "Number of berries a blueberry bush will spawn");
            PlantingPlus.cloudberryResourceAmount = base.Config.Bind<int>("General", "CloudberryResourceAmount", 1, "Number of berries a cloudberry bush will spawn");
            PlantingPlus.mushroomResourceAmount = base.Config.Bind<int>("General", "MushroomResourceAmount", 1, "Number of mushrooms a pickable mushroom spawner will spawn");
            PlantingPlus.yellowMushroomResourceAmount = base.Config.Bind<int>("General", "YellowMushroomResourceAmount", 1, "Number of yellow mushrooms a pickable yellow mushroom spawner will spawn");
            PlantingPlus.blueMushroomResourceAmount = base.Config.Bind<int>("General", "BlueMushroomResourceAmount", 1, "Number of blue mushrooms a pickable blue mushroom spawner will spawn");
            PlantingPlus.thistleResourceAmount = base.Config.Bind<int>("General", "ThistleResourceAmount", 1, "Number of thistle a pickable thistle spawner will spawn");
            PlantingPlus.dandelionResourceAmount = base.Config.Bind<int>("General", "DandelionResourceAmount", 1, "Number of dandelion a pickable dandelion spawner will spawn");

            PlantingPlus.birchMinScale = base.Config.Bind<float>("General", "BirchMinScale", 0.5f, "The minimum scaling factor used to scale a birch tree upon growth");
            PlantingPlus.birchMaxScale = base.Config.Bind<float>("General", "BirchMaxScale", 2f, "The maximum scaling factor used to scale a birch tree upon growth");
            PlantingPlus.oakMinScale = base.Config.Bind<float>("General", "OakMinScale", 0.5f, "The minimum scaling factor used to scale an oak tree upon growth");
            PlantingPlus.oakMaxScale = base.Config.Bind<float>("General", "OakMaxScale", 2f, "The maximum scaling factor used to scale an oak tree upon growth");
            PlantingPlus.ancientMinScale = base.Config.Bind<float>("General", "AncientMinScale", 0.5f, "The minimum scaling factor used to scale an ancient tree upon growth");
            PlantingPlus.ancientMaxScale = base.Config.Bind<float>("General", "AncientMaxScale", 2f, "The maximum scaling factor used to scale an ancient tree upon growth");

            UnityEngine.Object.DontDestroyOnLoad(this);

            if (!PlantingPlus.modEnabled.Value)
            {
                this.enabled = false;
            }
            else
            {
                Harmony harmony = new Harmony("com.bkeyes93.PlantingPlus");
                harmony.PatchAll();
            }

        }
        public void Update()
        {
            // Get the pickable resource objects.
            foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
            {
                if (gameObject.name == "RaspberryBush")
                    raspberryBushObject = gameObject;
                if (gameObject.name == "BlueberryBush")
                    blueberryBushObject = gameObject;
                if (gameObject.name == "CloudberryBush")
                    cloudberryBushObject = gameObject;

                if (PlantingPlus.enableOtherResources.Value)
                {
                    if (gameObject.name == "Pickable_Mushroom")
                        pickableMushroomObject = gameObject;
                    if (gameObject.name == "Pickable_Mushroom_yellow")
                        pickableYellowMushroomObject = gameObject;
                    if (gameObject.name == "Pickable_Mushroom_blue")
                        pickableBlueMushroomObject = gameObject;
                    if (gameObject.name == "Pickable_Thistle")
                        pickableThistleObject = gameObject;
                    if (gameObject.name == "Pickable_Dandelion")
                        pickableDandelionObject = gameObject;
                    if (gameObject.name == "Oak1")
                        oakTree1Object = gameObject;
                    if (gameObject.name == "Birch1")
                        birchTree1Object = gameObject;
                    if (gameObject.name == "Birch2")
                        birchTree2Object = gameObject;
                    if (gameObject.name == "SwampTree1")
                        swampTree1Object = gameObject;
                    if (gameObject.name == "PineTree_Sapling")
                        pineTreeSaplingObject = gameObject;
                    if (gameObject.name == "Beech_Sapling")
                        beechTreeSaplingObject = gameObject;
                }

                if (raspberryBushObject != null && blueberryBushObject != null && cloudberryBushObject != null && ((pickableMushroomObject != null && pickableYellowMushroomObject != null && pickableBlueMushroomObject != null && pickableThistleObject != null && pickableDandelionObject != null && oakTree1Object != null && birchTree1Object != null && birchTree2Object != null && swampTree1Object != null && pineTreeSaplingObject != null && beechTreeSaplingObject != null) || !PlantingPlus.enableOtherResources.Value))
                    break;
            }

            if (raspberryBushObject == null || blueberryBushObject == null || cloudberryBushObject == null || (PlantingPlus.enableOtherResources.Value && (pickableMushroomObject == null || pickableYellowMushroomObject == null || pickableBlueMushroomObject == null || pickableThistleObject == null || pickableDandelionObject == null || oakTree1Object == null || birchTree1Object == null || birchTree2Object == null || swampTree1Object == null || pineTreeSaplingObject == null || beechTreeSaplingObject == null)))
                return;

            // Create new sapling prefabs.
            if (PlantingPlus.enableOtherResources.Value)
            {
                if (birchTreeSaplingObject == null)
                {
                    isCloningPrefab = true;
                    birchTreeSaplingObject = UnityEngine.Object.Instantiate<GameObject>(pineTreeSaplingObject);
                    birchTreeSaplingObject.name = "Birch_Sapling";
                    UnityEngine.Object.DontDestroyOnLoad(PlantingPlus.birchTreeSaplingObject);
                    birchTreeSaplingObject.hideFlags = HideFlags.HideInHierarchy;
                    isCloningPrefab = false;
                }

                if (oakTreeSaplingObject == null)
                {
                    isCloningPrefab = true;
                    oakTreeSaplingObject = UnityEngine.Object.Instantiate<GameObject>(beechTreeSaplingObject);
                    oakTreeSaplingObject.name = "Oak_Sapling";
                    UnityEngine.Object.DontDestroyOnLoad(PlantingPlus.oakTreeSaplingObject);
                    oakTreeSaplingObject.hideFlags = HideFlags.HideInHierarchy;
                    isCloningPrefab = false;
                }

                if (swampTreeSaplingObject == null)
                {
                    isCloningPrefab = true;
                    swampTreeSaplingObject = UnityEngine.Object.Instantiate<GameObject>(pineTreeSaplingObject);
                    swampTreeSaplingObject.name = "Ancient_Sapling";
                    UnityEngine.Object.DontDestroyOnLoad(PlantingPlus.swampTreeSaplingObject);
                    swampTreeSaplingObject.hideFlags = HideFlags.HideInHierarchy;
                    isCloningPrefab = false;
                }
            }

            // If enabled, change the respawn times for pickable resources.
            if (PlantingPlus.enableCustomRespawnTimes.Value)
            {
                raspberryBushObject.GetComponent<Pickable>().m_respawnTimeMinutes = PlantingPlus.raspberryRespawnTime.Value;
                blueberryBushObject.GetComponent<Pickable>().m_respawnTimeMinutes = PlantingPlus.blueberryRespawnTime.Value;
                cloudberryBushObject.GetComponent<Pickable>().m_respawnTimeMinutes = PlantingPlus.cloudberryRespawnTime.Value;

                if (PlantingPlus.enableOtherResources.Value)
                {
                    pickableMushroomObject.GetComponent<Pickable>().m_respawnTimeMinutes = PlantingPlus.mushroomRespawnTime.Value;
                    pickableYellowMushroomObject.GetComponent<Pickable>().m_respawnTimeMinutes = PlantingPlus.yellowMushroomRespawnTime.Value;
                    pickableBlueMushroomObject.GetComponent<Pickable>().m_respawnTimeMinutes = PlantingPlus.blueMushroomRespawnTime.Value;
                    pickableThistleObject.GetComponent<Pickable>().m_respawnTimeMinutes = PlantingPlus.thistleRespawnTime.Value;
                    pickableDandelionObject.GetComponent<Pickable>().m_respawnTimeMinutes = PlantingPlus.dandelionRespawnTime.Value;
                }
            }

            // If enabled, change the resource amounts for pickable resources.
            if (PlantingPlus.enableCustomResourceAmounts.Value)
            {
                raspberryBushObject.GetComponent<Pickable>().m_amount = PlantingPlus.raspberryResourceAmount.Value;
                blueberryBushObject.GetComponent<Pickable>().m_amount = PlantingPlus.blueberryResourceAmount.Value;
                cloudberryBushObject.GetComponent<Pickable>().m_amount = PlantingPlus.cloudberryResourceAmount.Value;

                if (PlantingPlus.enableOtherResources.Value)
                {
                    pickableMushroomObject.GetComponent<Pickable>().m_amount = PlantingPlus.mushroomResourceAmount.Value;
                    pickableYellowMushroomObject.GetComponent<Pickable>().m_amount = PlantingPlus.yellowMushroomResourceAmount.Value;
                    pickableBlueMushroomObject.GetComponent<Pickable>().m_amount = PlantingPlus.blueMushroomResourceAmount.Value;
                    pickableThistleObject.GetComponent<Pickable>().m_amount = PlantingPlus.thistleResourceAmount.Value;
                    pickableDandelionObject.GetComponent<Pickable>().m_amount = PlantingPlus.dandelionResourceAmount.Value;
                }
            }

            // Get the resource items.
            if (ObjectDB.instance == null)
                return;

            List<GameObject>.Enumerator enumerator = ObjectDB.instance.m_items.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.GetComponent<ItemDrop>().name == "Raspberry")
                    raspberryItem = enumerator.Current.GetComponent<ItemDrop>();
                if (enumerator.Current.GetComponent<ItemDrop>().name == "Blueberries")
                    blueberriesItem = enumerator.Current.GetComponent<ItemDrop>();
                if (enumerator.Current.GetComponent<ItemDrop>().name == "Cloudberry")
                    cloudberryItem = enumerator.Current.GetComponent<ItemDrop>();
                if (enumerator.Current.GetComponent<ItemDrop>().name == "Cultivator")
                    cultivatorItem = enumerator.Current.GetComponent<ItemDrop>();

                if (PlantingPlus.enableOtherResources.Value)
                {
                    if (enumerator.Current.GetComponent<ItemDrop>().name == "Mushroom")
                        mushroomItem = enumerator.Current.GetComponent<ItemDrop>();
                    if (enumerator.Current.GetComponent<ItemDrop>().name == "MushroomYellow")
                        yellowMushroomItem = enumerator.Current.GetComponent<ItemDrop>();
                    if (enumerator.Current.GetComponent<ItemDrop>().name == "MushroomBlue")
                        blueMushroomItem = enumerator.Current.GetComponent<ItemDrop>();
                    if (enumerator.Current.GetComponent<ItemDrop>().name == "Thistle")
                        thistleItem = enumerator.Current.GetComponent<ItemDrop>();
                    if (enumerator.Current.GetComponent<ItemDrop>().name == "Dandelion")
                        dandelionItem = enumerator.Current.GetComponent<ItemDrop>();
                }

                if (raspberryItem != null && blueberriesItem != null && cloudberryItem != null && cultivatorItem != null && ((mushroomItem != null && yellowMushroomItem != null && blueMushroomItem != null && thistleItem != null && dandelionItem != null) || !PlantingPlus.enableOtherResources.Value))
                    break;
            }

            if (raspberryItem == null || blueberriesItem == null || cloudberryItem == null || cultivatorItem == null || (PlantingPlus.enableOtherResources.Value && (mushroomItem == null || yellowMushroomItem == null || blueMushroomItem == null || thistleItem == null || dandelionItem == null)))
                return;

            if (PlantingPlus.enableOtherResources.Value && (birchTreeSaplingObject == null || oakTreeSaplingObject == null || swampTreeSaplingObject == null || birchConeObject == null || oakSeedsObject == null || ancientSeedsObject == null))
                return;

            // Create and add piece components to the pickable resource objects.
            raspberryBushPiece = raspberryBushObject.AddComponent<Piece>();
            blueberryBushPiece = blueberryBushObject.AddComponent<Piece>();
            cloudberryBushPiece = cloudberryBushObject.AddComponent<Piece>();
            raspberryBushPiece.m_name = "Raspberry Bush";
            raspberryBushPiece.m_description = "Plant raspberries to grow raspberry bushes";
            raspberryBushPiece.m_category = Piece.PieceCategory.Misc;
            raspberryBushPiece.m_cultivatedGroundOnly = PlantingPlus.requireCultivation.Value;
            raspberryBushPiece.m_groundOnly = !PlantingPlus.placeAnywhere.Value;
            raspberryBushPiece.m_groundPiece = !PlantingPlus.placeAnywhere.Value;
            blueberryBushPiece.m_name = "Blueberry Bush";
            blueberryBushPiece.m_description = "Plant blueberries to grow blueberry bushes";
            blueberryBushPiece.m_category = Piece.PieceCategory.Misc;
            blueberryBushPiece.m_cultivatedGroundOnly = PlantingPlus.requireCultivation.Value;
            blueberryBushPiece.m_groundOnly = !PlantingPlus.placeAnywhere.Value;
            blueberryBushPiece.m_groundPiece = !PlantingPlus.placeAnywhere.Value;
            cloudberryBushPiece.m_name = "Cloudberry Bush";
            cloudberryBushPiece.m_description = "Plant cloudberries to grow cloudberry bushes";
            cloudberryBushPiece.m_category = Piece.PieceCategory.Misc;
            cloudberryBushPiece.m_cultivatedGroundOnly = PlantingPlus.requireCultivation.Value;
            cloudberryBushPiece.m_groundOnly = !PlantingPlus.placeAnywhere.Value;
            cloudberryBushPiece.m_groundPiece = !PlantingPlus.placeAnywhere.Value;

            if (PlantingPlus.enableOtherResources.Value)
            {
                pickableMushroomPiece = pickableMushroomObject.AddComponent<Piece>();
                pickableYellowMushroomPiece = pickableYellowMushroomObject.AddComponent<Piece>();
                pickableBlueMushroomPiece = pickableBlueMushroomObject.AddComponent<Piece>();
                pickableMushroomPiece.m_name = "Pickable Mushrooms";
                pickableMushroomPiece.m_description = "Plant mushrooms to grow more pickable mushrooms";
                pickableMushroomPiece.m_category = Piece.PieceCategory.Misc;
                pickableMushroomPiece.m_cultivatedGroundOnly = PlantingPlus.requireCultivation.Value;
                pickableMushroomPiece.m_groundOnly = true;
                pickableMushroomPiece.m_groundPiece = true;
                pickableYellowMushroomPiece.m_name = "Pickable Yellow Mushrooms";
                pickableYellowMushroomPiece.m_description = "Plant yellow mushrooms to grow more pickable yellow mushrooms";
                pickableYellowMushroomPiece.m_category = Piece.PieceCategory.Misc;
                pickableYellowMushroomPiece.m_cultivatedGroundOnly = PlantingPlus.requireCultivation.Value;
                pickableYellowMushroomPiece.m_groundOnly = true;
                pickableYellowMushroomPiece.m_groundPiece = true;
                pickableBlueMushroomPiece.m_name = "Pickable Blue Mushrooms";
                pickableBlueMushroomPiece.m_description = "Plant blue mushrooms to grow more pickable blue mushrooms";
                pickableBlueMushroomPiece.m_category = Piece.PieceCategory.Misc;
                pickableBlueMushroomPiece.m_cultivatedGroundOnly = PlantingPlus.requireCultivation.Value;
                pickableBlueMushroomPiece.m_groundOnly = true;
                pickableBlueMushroomPiece.m_groundPiece = true;
                pickableThistlePiece = pickableThistleObject.AddComponent<Piece>();
                pickableThistlePiece.m_name = "Pickable Thistle";
                pickableThistlePiece.m_description = "Plant thistle to grow more pickable thistle";
                pickableThistlePiece.m_category = Piece.PieceCategory.Misc;
                pickableThistlePiece.m_cultivatedGroundOnly = PlantingPlus.requireCultivation.Value;
                pickableThistlePiece.m_groundOnly = true;
                pickableThistlePiece.m_groundPiece = true;
                pickableDandelionPiece = pickableDandelionObject.AddComponent<Piece>();
                pickableDandelionPiece.m_name = "Pickable Dandelion";
                pickableDandelionPiece.m_description = "Plant dandelion to grow more pickable dandelion";
                pickableDandelionPiece.m_category = Piece.PieceCategory.Misc;
                pickableDandelionPiece.m_cultivatedGroundOnly = PlantingPlus.requireCultivation.Value;
                pickableDandelionPiece.m_groundOnly = true;
                pickableDandelionPiece.m_groundPiece = true;

                birchTreeSaplingObject.GetComponent<Piece>().m_name = "Birch Sapling";
                oakTreeSaplingObject.GetComponent<Piece>().m_name = "Oak Sapling";
                swampTreeSaplingObject.GetComponent<Piece>().m_name = "Ancient Sapling";
                birchTreeSaplingObject.GetComponent<Piece>().m_groundOnly = !PlantingPlus.placeAnywhere.Value;
                birchTreeSaplingObject.GetComponent<Piece>().m_groundPiece = !PlantingPlus.placeAnywhere.Value;
                birchTreeSaplingObject.GetComponent<Piece>().m_cultivatedGroundOnly = PlantingPlus.requireCultivation.Value;
                oakTreeSaplingObject.GetComponent<Piece>().m_groundOnly = !PlantingPlus.placeAnywhere.Value;
                oakTreeSaplingObject.GetComponent<Piece>().m_groundPiece = !PlantingPlus.placeAnywhere.Value;
                oakTreeSaplingObject.GetComponent<Piece>().m_cultivatedGroundOnly = PlantingPlus.requireCultivation.Value;
                swampTreeSaplingObject.GetComponent<Piece>().m_groundOnly = !PlantingPlus.placeAnywhere.Value;
                swampTreeSaplingObject.GetComponent<Piece>().m_groundPiece = !PlantingPlus.placeAnywhere.Value;
                swampTreeSaplingObject.GetComponent<Piece>().m_cultivatedGroundOnly = PlantingPlus.requireCultivation.Value;
                birchTreeSaplingObject.GetComponent<Plant>().m_name = "Birch Sapling";
                oakTreeSaplingObject.GetComponent<Plant>().m_name = "Oak Sapling";
                swampTreeSaplingObject.GetComponent<Plant>().m_name = "Ancient Sapling";
                birchTreeSaplingObject.GetComponent<Plant>().m_grownPrefabs = new GameObject[] { birchTree1Object, birchTree2Object };
                oakTreeSaplingObject.GetComponent<Plant>().m_grownPrefabs = new GameObject[] { oakTree1Object };
                swampTreeSaplingObject.GetComponent<Plant>().m_grownPrefabs = new GameObject[] { swampTree1Object };
                birchTreeSaplingObject.GetComponent<Plant>().m_growTime = PlantingPlus.birchGrowthTime.Value;
                birchTreeSaplingObject.GetComponent<Plant>().m_growTimeMax = PlantingPlus.birchGrowthTime.Value;
                oakTreeSaplingObject.GetComponent<Plant>().m_growTime = PlantingPlus.oakGrowthTime.Value;
                oakTreeSaplingObject.GetComponent<Plant>().m_growTimeMax = PlantingPlus.oakGrowthTime.Value;
                swampTreeSaplingObject.GetComponent<Plant>().m_growTime = PlantingPlus.ancientGrowthTime.Value;
                swampTreeSaplingObject.GetComponent<Plant>().m_growTimeMax = PlantingPlus.ancientGrowthTime.Value;

                birchTreeSaplingObject.GetComponent<Plant>().m_needCultivatedGround = PlantingPlus.requireCultivation.Value;
                oakTreeSaplingObject.GetComponent<Plant>().m_needCultivatedGround = PlantingPlus.requireCultivation.Value;
                swampTreeSaplingObject.GetComponent<Plant>().m_needCultivatedGround = PlantingPlus.requireCultivation.Value;

                if (PlantingPlus.placeAnywhere.Value)
                {
                    typeof(Plant).GetField("m_roofMask", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, Physics.IgnoreRaycastLayer);
                    typeof(Plant).GetField("m_spaceMask", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, Physics.IgnoreRaycastLayer);

                }

                // Set scaling factors to prevent planted trees from growing too big.
                birchTreeSaplingObject.GetComponent<Plant>().m_minScale = PlantingPlus.birchMinScale.Value;
                birchTreeSaplingObject.GetComponent<Plant>().m_maxScale = PlantingPlus.birchMaxScale.Value;
                oakTreeSaplingObject.GetComponent<Plant>().m_minScale = PlantingPlus.oakMinScale.Value;
                oakTreeSaplingObject.GetComponent<Plant>().m_maxScale = PlantingPlus.oakMaxScale.Value;
                swampTreeSaplingObject.GetComponent<Plant>().m_minScale = PlantingPlus.ancientMinScale.Value;
                swampTreeSaplingObject.GetComponent<Plant>().m_maxScale = PlantingPlus.ancientMaxScale.Value;
            }

            // If enabled, require pickable resources to be placed in their respective biomes.
            if (PlantingPlus.enforceBiomes.Value)
            {
                raspberryBushPiece.m_onlyInBiome = Heightmap.Biome.Meadows;
                blueberryBushPiece.m_onlyInBiome = Heightmap.Biome.BlackForest;
                cloudberryBushPiece.m_onlyInBiome = Heightmap.Biome.Plains;

                if (PlantingPlus.enableOtherResources.Value)
                {
                    pickableMushroomPiece.m_onlyInBiome = Heightmap.Biome.Meadows | Heightmap.Biome.BlackForest;
                    pickableYellowMushroomPiece.m_onlyInBiome = Heightmap.Biome.BlackForest | Heightmap.Biome.Swamp;
                    pickableBlueMushroomPiece.m_onlyInBiome = Heightmap.Biome.BlackForest | Heightmap.Biome.Swamp;
                    pickableThistlePiece.m_onlyInBiome = Heightmap.Biome.BlackForest | Heightmap.Biome.Swamp;
                    pickableDandelionPiece.m_onlyInBiome = Heightmap.Biome.Meadows;

                    birchTreeSaplingObject.GetComponent<Piece>().m_onlyInBiome = Heightmap.Biome.Meadows | Heightmap.Biome.Plains;
                    oakTreeSaplingObject.GetComponent<Piece>().m_onlyInBiome = Heightmap.Biome.Meadows | Heightmap.Biome.Plains;
                    swampTreeSaplingObject.GetComponent<Piece>().m_onlyInBiome = Heightmap.Biome.Swamp;

                    birchTreeSaplingObject.GetComponent<Plant>().m_biome = Heightmap.Biome.Meadows | Heightmap.Biome.Plains;
                    oakTreeSaplingObject.GetComponent<Plant>().m_biome = Heightmap.Biome.Meadows | Heightmap.Biome.Plains;
                    swampTreeSaplingObject.GetComponent<Plant>().m_biome = Heightmap.Biome.Swamp;
                }
            }
            else
            {
                if (PlantingPlus.enableOtherResources.Value)
                {
                    birchTreeSaplingObject.GetComponent<Piece>().m_onlyInBiome = Heightmap.Biome.None;
                    oakTreeSaplingObject.GetComponent<Piece>().m_onlyInBiome = Heightmap.Biome.None;
                    swampTreeSaplingObject.GetComponent<Piece>().m_onlyInBiome = Heightmap.Biome.None;

                    birchTreeSaplingObject.GetComponent<Plant>().m_biome = (Heightmap.Biome.Meadows | Heightmap.Biome.Swamp | Heightmap.Biome.Mountain | Heightmap.Biome.Mountain | Heightmap.Biome.BlackForest | Heightmap.Biome.Plains | Heightmap.Biome.AshLands | Heightmap.Biome.DeepNorth | Heightmap.Biome.Ocean | Heightmap.Biome.Mistlands | Heightmap.Biome.BiomesMax);
                    oakTreeSaplingObject.GetComponent<Plant>().m_biome = (Heightmap.Biome.Meadows | Heightmap.Biome.Swamp | Heightmap.Biome.Mountain | Heightmap.Biome.Mountain | Heightmap.Biome.BlackForest | Heightmap.Biome.Plains | Heightmap.Biome.AshLands | Heightmap.Biome.DeepNorth | Heightmap.Biome.Ocean | Heightmap.Biome.Mistlands | Heightmap.Biome.BiomesMax);
                    swampTreeSaplingObject.GetComponent<Plant>().m_biome = (Heightmap.Biome.Meadows | Heightmap.Biome.Swamp | Heightmap.Biome.Mountain | Heightmap.Biome.Mountain | Heightmap.Biome.BlackForest | Heightmap.Biome.Plains | Heightmap.Biome.AshLands | Heightmap.Biome.DeepNorth | Heightmap.Biome.Ocean | Heightmap.Biome.Mistlands | Heightmap.Biome.BiomesMax);
                }
            }

            // Add resource requirements to pickable resource pieces.
            raspberryBushPiece.m_resources = new Piece.Requirement[1];
            raspberryBushPiece.m_resources[0] = new Piece.Requirement();
            raspberryBushPiece.m_resources[0].m_resItem = raspberryItem;
            raspberryBushPiece.m_resources[0].m_amount = PlantingPlus.raspberryCost.Value;
            raspberryBushPiece.m_resources[0].m_recover = false;
            blueberryBushPiece.m_resources = new Piece.Requirement[1];
            blueberryBushPiece.m_resources[0] = new Piece.Requirement();
            blueberryBushPiece.m_resources[0].m_resItem = blueberriesItem;
            blueberryBushPiece.m_resources[0].m_amount = PlantingPlus.blueberryCost.Value;
            blueberryBushPiece.m_resources[0].m_recover = false;
            cloudberryBushPiece.m_resources = new Piece.Requirement[1];
            cloudberryBushPiece.m_resources[0] = new Piece.Requirement();
            cloudberryBushPiece.m_resources[0].m_resItem = cloudberryItem;
            cloudberryBushPiece.m_resources[0].m_amount = PlantingPlus.cloudberryCost.Value;
            cloudberryBushPiece.m_resources[0].m_recover = false;

            if (PlantingPlus.enableOtherResources.Value)
            {
                pickableMushroomPiece.m_resources = new Piece.Requirement[1];
                pickableMushroomPiece.m_resources[0] = new Piece.Requirement();
                pickableMushroomPiece.m_resources[0].m_resItem = mushroomItem;
                pickableMushroomPiece.m_resources[0].m_amount = PlantingPlus.mushroomCost.Value;
                pickableMushroomPiece.m_resources[0].m_recover = false;
                pickableYellowMushroomPiece.m_resources = new Piece.Requirement[1];
                pickableYellowMushroomPiece.m_resources[0] = new Piece.Requirement();
                pickableYellowMushroomPiece.m_resources[0].m_resItem = yellowMushroomItem;
                pickableYellowMushroomPiece.m_resources[0].m_amount = PlantingPlus.yellowMushroomCost.Value;
                pickableYellowMushroomPiece.m_resources[0].m_recover = false;
                pickableBlueMushroomPiece.m_resources = new Piece.Requirement[1];
                pickableBlueMushroomPiece.m_resources[0] = new Piece.Requirement();
                pickableBlueMushroomPiece.m_resources[0].m_resItem = blueMushroomItem;
                pickableBlueMushroomPiece.m_resources[0].m_amount = PlantingPlus.blueMushroomCost.Value;
                pickableBlueMushroomPiece.m_resources[0].m_recover = false;
                pickableThistlePiece.m_resources = new Piece.Requirement[1];
                pickableThistlePiece.m_resources[0] = new Piece.Requirement();
                pickableThistlePiece.m_resources[0].m_resItem = thistleItem;
                pickableThistlePiece.m_resources[0].m_amount = PlantingPlus.thistleCost.Value;
                pickableThistlePiece.m_resources[0].m_recover = false;
                pickableDandelionPiece.m_resources = new Piece.Requirement[1];
                pickableDandelionPiece.m_resources[0] = new Piece.Requirement();
                pickableDandelionPiece.m_resources[0].m_resItem = dandelionItem;
                pickableDandelionPiece.m_resources[0].m_amount = PlantingPlus.dandelionCost.Value;
                pickableDandelionPiece.m_resources[0].m_recover = false;

                birchTreeSaplingObject.GetComponent<Piece>().m_resources[0] = new Piece.Requirement();
                birchTreeSaplingObject.GetComponent<Piece>().m_resources[0].m_resItem = birchConeObject.GetComponent<ItemDrop>();
                birchTreeSaplingObject.GetComponent<Piece>().m_resources[0].m_amount = PlantingPlus.birchCost.Value;
                oakTreeSaplingObject.GetComponent<Piece>().m_resources[0] = new Piece.Requirement();
                oakTreeSaplingObject.GetComponent<Piece>().m_resources[0].m_resItem = oakSeedsObject.GetComponent<ItemDrop>();
                oakTreeSaplingObject.GetComponent<Piece>().m_resources[0].m_amount = PlantingPlus.oakCost.Value;
                swampTreeSaplingObject.GetComponent<Piece>().m_resources[0] = new Piece.Requirement();
                swampTreeSaplingObject.GetComponent<Piece>().m_resources[0].m_resItem = ancientSeedsObject.GetComponent<ItemDrop>();
                swampTreeSaplingObject.GetComponent<Piece>().m_resources[0].m_amount = PlantingPlus.ancientCost.Value;

                // Adding drops to trees.
                DropTable.DropData birchDropData = new DropTable.DropData();
                birchDropData.m_item = birchConeObject;
                birchDropData.m_stackMin = 1;
                birchDropData.m_stackMax = 5;
                birchTree1Object.GetComponent<TreeBase>().m_dropWhenDestroyed.m_drops.Add(birchDropData);
                birchTree2Object.GetComponent<TreeBase>().m_dropWhenDestroyed.m_drops.Add(birchDropData);
                DropTable.DropData oakDropData = new DropTable.DropData();
                oakDropData.m_item = oakSeedsObject;
                oakDropData.m_stackMin = 1;
                oakDropData.m_stackMax = 5;
                oakTree1Object.GetComponent<TreeBase>().m_dropWhenDestroyed.m_drops.Add(oakDropData);
                DropTable.DropData ancientDropData = new DropTable.DropData();
                ancientDropData.m_item = ancientSeedsObject;
                ancientDropData.m_stackMin = 1;
                ancientDropData.m_stackMax = 5;
                swampTree1Object.GetComponent<TreeBase>().m_dropWhenDestroyed.m_drops.Add(ancientDropData);

                birchTree1Object.GetComponent<TreeBase>().m_dropWhenDestroyed.m_dropChance = 1;
                birchTree1Object.GetComponent<TreeBase>().m_dropWhenDestroyed.m_oneOfEach = true;
                birchTree1Object.GetComponent<TreeBase>().m_dropWhenDestroyed.m_dropMin = 1;
                birchTree1Object.GetComponent<TreeBase>().m_dropWhenDestroyed.m_dropMax = 3;
                birchTree2Object.GetComponent<TreeBase>().m_dropWhenDestroyed.m_dropChance = 1;
                birchTree2Object.GetComponent<TreeBase>().m_dropWhenDestroyed.m_oneOfEach = true;
                birchTree2Object.GetComponent<TreeBase>().m_dropWhenDestroyed.m_dropMin = 1;
                birchTree2Object.GetComponent<TreeBase>().m_dropWhenDestroyed.m_dropMax = 3;

                oakTree1Object.GetComponent<TreeBase>().m_dropWhenDestroyed.m_dropChance = 1;
                oakTree1Object.GetComponent<TreeBase>().m_dropWhenDestroyed.m_oneOfEach = true;
                oakTree1Object.GetComponent<TreeBase>().m_dropWhenDestroyed.m_dropMin = 1;
                oakTree1Object.GetComponent<TreeBase>().m_dropWhenDestroyed.m_dropMax = 3;

                swampTree1Object.GetComponent<TreeBase>().m_dropWhenDestroyed.m_dropChance = 1;
                swampTree1Object.GetComponent<TreeBase>().m_dropWhenDestroyed.m_oneOfEach = true;
                swampTree1Object.GetComponent<TreeBase>().m_dropWhenDestroyed.m_dropMin = 1;
                swampTree1Object.GetComponent<TreeBase>().m_dropWhenDestroyed.m_dropMax = 3;
            }

            // Get place effect objects.
            foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
            {
                if (gameObject.name == "vfx_Place_wood_pole")
                    placeWoodPoleVfx = gameObject;
                if (gameObject.name == "sfx_build_cultivator")
                    buildCultivatorSfx = gameObject;
                if (placeWoodPoleVfx != null && buildCultivatorSfx != null)
                    break;
            }

            if (placeWoodPoleVfx == null || buildCultivatorSfx == null)
                return;

            // Add place effects to pieces. 
            raspberryBushPiece.m_placeEffect.m_effectPrefabs = new EffectList.EffectData[2];
            raspberryBushPiece.m_placeEffect.m_effectPrefabs[0] = new EffectList.EffectData();
            raspberryBushPiece.m_placeEffect.m_effectPrefabs[0].m_prefab = placeWoodPoleVfx;
            raspberryBushPiece.m_placeEffect.m_effectPrefabs[0].m_enabled = true;
            raspberryBushPiece.m_placeEffect.m_effectPrefabs[1] = new EffectList.EffectData();
            raspberryBushPiece.m_placeEffect.m_effectPrefabs[1].m_prefab = buildCultivatorSfx;
            raspberryBushPiece.m_placeEffect.m_effectPrefabs[1].m_enabled = true;
            blueberryBushPiece.m_placeEffect.m_effectPrefabs = new EffectList.EffectData[2];
            blueberryBushPiece.m_placeEffect.m_effectPrefabs[0] = new EffectList.EffectData();
            blueberryBushPiece.m_placeEffect.m_effectPrefabs[0].m_prefab = placeWoodPoleVfx;
            blueberryBushPiece.m_placeEffect.m_effectPrefabs[0].m_enabled = true;
            blueberryBushPiece.m_placeEffect.m_effectPrefabs[1] = new EffectList.EffectData();
            blueberryBushPiece.m_placeEffect.m_effectPrefabs[1].m_prefab = buildCultivatorSfx;
            blueberryBushPiece.m_placeEffect.m_effectPrefabs[1].m_enabled = true;
            cloudberryBushPiece.m_placeEffect.m_effectPrefabs = new EffectList.EffectData[2];
            cloudberryBushPiece.m_placeEffect.m_effectPrefabs[0] = new EffectList.EffectData();
            cloudberryBushPiece.m_placeEffect.m_effectPrefabs[0].m_prefab = placeWoodPoleVfx;
            cloudberryBushPiece.m_placeEffect.m_effectPrefabs[0].m_enabled = true;
            cloudberryBushPiece.m_placeEffect.m_effectPrefabs[1] = new EffectList.EffectData();
            cloudberryBushPiece.m_placeEffect.m_effectPrefabs[1].m_prefab = buildCultivatorSfx;
            cloudberryBushPiece.m_placeEffect.m_effectPrefabs[1].m_enabled = true;

            if (PlantingPlus.enableOtherResources.Value)
            {
                pickableMushroomPiece.m_placeEffect.m_effectPrefabs = new EffectList.EffectData[2];
                pickableMushroomPiece.m_placeEffect.m_effectPrefabs[0] = new EffectList.EffectData();
                pickableMushroomPiece.m_placeEffect.m_effectPrefabs[0].m_prefab = placeWoodPoleVfx;
                pickableMushroomPiece.m_placeEffect.m_effectPrefabs[0].m_enabled = true;
                pickableMushroomPiece.m_placeEffect.m_effectPrefabs[1] = new EffectList.EffectData();
                pickableMushroomPiece.m_placeEffect.m_effectPrefabs[1].m_prefab = buildCultivatorSfx;
                pickableMushroomPiece.m_placeEffect.m_effectPrefabs[1].m_enabled = true;
                pickableYellowMushroomPiece.m_placeEffect.m_effectPrefabs = new EffectList.EffectData[2];
                pickableYellowMushroomPiece.m_placeEffect.m_effectPrefabs[0] = new EffectList.EffectData();
                pickableYellowMushroomPiece.m_placeEffect.m_effectPrefabs[0].m_prefab = placeWoodPoleVfx;
                pickableYellowMushroomPiece.m_placeEffect.m_effectPrefabs[0].m_enabled = true;
                pickableYellowMushroomPiece.m_placeEffect.m_effectPrefabs[1] = new EffectList.EffectData();
                pickableYellowMushroomPiece.m_placeEffect.m_effectPrefabs[1].m_prefab = buildCultivatorSfx;
                pickableYellowMushroomPiece.m_placeEffect.m_effectPrefabs[1].m_enabled = true;
                pickableBlueMushroomPiece.m_placeEffect.m_effectPrefabs = new EffectList.EffectData[2];
                pickableBlueMushroomPiece.m_placeEffect.m_effectPrefabs[0] = new EffectList.EffectData();
                pickableBlueMushroomPiece.m_placeEffect.m_effectPrefabs[0].m_prefab = placeWoodPoleVfx;
                pickableBlueMushroomPiece.m_placeEffect.m_effectPrefabs[0].m_enabled = true;
                pickableBlueMushroomPiece.m_placeEffect.m_effectPrefabs[1] = new EffectList.EffectData();
                pickableBlueMushroomPiece.m_placeEffect.m_effectPrefabs[1].m_prefab = buildCultivatorSfx;
                pickableBlueMushroomPiece.m_placeEffect.m_effectPrefabs[1].m_enabled = true;
                pickableThistlePiece.m_placeEffect.m_effectPrefabs = new EffectList.EffectData[2];
                pickableThistlePiece.m_placeEffect.m_effectPrefabs[0] = new EffectList.EffectData();
                pickableThistlePiece.m_placeEffect.m_effectPrefabs[0].m_prefab = placeWoodPoleVfx;
                pickableThistlePiece.m_placeEffect.m_effectPrefabs[0].m_enabled = true;
                pickableThistlePiece.m_placeEffect.m_effectPrefabs[1] = new EffectList.EffectData();
                pickableThistlePiece.m_placeEffect.m_effectPrefabs[1].m_prefab = buildCultivatorSfx;
                pickableThistlePiece.m_placeEffect.m_effectPrefabs[1].m_enabled = true;
                pickableDandelionPiece.m_placeEffect.m_effectPrefabs = new EffectList.EffectData[2];
                pickableDandelionPiece.m_placeEffect.m_effectPrefabs[0] = new EffectList.EffectData();
                pickableDandelionPiece.m_placeEffect.m_effectPrefabs[0].m_prefab = placeWoodPoleVfx;
                pickableDandelionPiece.m_placeEffect.m_effectPrefabs[0].m_enabled = true;
                pickableDandelionPiece.m_placeEffect.m_effectPrefabs[1] = new EffectList.EffectData();
                pickableDandelionPiece.m_placeEffect.m_effectPrefabs[1].m_prefab = buildCultivatorSfx;
                pickableDandelionPiece.m_placeEffect.m_effectPrefabs[1].m_enabled = true;
            }

            // Load and set icons for the pieces.
            if (!PlantingPlus.alternateIcons.Value)
            {
                Texture2D raspberryBushPieceTexture = new Texture2D(64, 64, TextureFormat.ARGB32, false);
                raspberryBushPieceTexture.LoadImage(Properties.Resources.raspberryBushPieceIcon);
                raspberryBushPieceTexture.Apply();
                raspberryBushPiece.m_icon = Sprite.Create(raspberryBushPieceTexture, new Rect(0, 0, 64, 64), new Vector2(0, 0));
                Texture2D blueberryBushPieceTexture = new Texture2D(64, 64, TextureFormat.ARGB32, false);
                blueberryBushPieceTexture.LoadImage(Properties.Resources.blueberryBushPieceIcon);
                blueberryBushPieceTexture.Apply();
                blueberryBushPiece.m_icon = Sprite.Create(blueberryBushPieceTexture, new Rect(0, 0, 64, 64), new Vector2(0, 0));
                Texture2D cloudberryBushPieceTexture = new Texture2D(64, 64, TextureFormat.ARGB32, false);
                cloudberryBushPieceTexture.LoadImage(Properties.Resources.cloudberryBushPieceIcon);
                cloudberryBushPieceTexture.Apply();
                cloudberryBushPiece.m_icon = Sprite.Create(cloudberryBushPieceTexture, new Rect(0, 0, 64, 64), new Vector2(0, 0));
            }
            else
            {
                raspberryBushPiece.m_icon = raspberryItem.m_itemData.GetIcon();
                blueberryBushPiece.m_icon = blueberriesItem.m_itemData.GetIcon();
                cloudberryBushPiece.m_icon = cloudberryItem.m_itemData.GetIcon();
            }

            if (PlantingPlus.enableOtherResources.Value)
            {
                pickableMushroomPiece.m_icon = mushroomItem.m_itemData.GetIcon();
                pickableYellowMushroomPiece.m_icon = yellowMushroomItem.m_itemData.GetIcon();
                pickableBlueMushroomPiece.m_icon = blueMushroomItem.m_itemData.GetIcon();
                pickableThistlePiece.m_icon = thistleItem.m_itemData.GetIcon();
                pickableDandelionPiece.m_icon = dandelionItem.m_itemData.GetIcon();
            }

            // Add the pickable resource objects to the cultivator piecetable.
            cultivatorItem.m_itemData.m_shared.m_buildPieces.m_pieces.Add(raspberryBushObject);
            cultivatorItem.m_itemData.m_shared.m_buildPieces.m_pieces.Add(blueberryBushObject);
            cultivatorItem.m_itemData.m_shared.m_buildPieces.m_pieces.Add(cloudberryBushObject);

            if (PlantingPlus.enableOtherResources.Value)
            {
                cultivatorItem.m_itemData.m_shared.m_buildPieces.m_pieces.Add(pickableMushroomObject);
                cultivatorItem.m_itemData.m_shared.m_buildPieces.m_pieces.Add(pickableYellowMushroomObject);
                cultivatorItem.m_itemData.m_shared.m_buildPieces.m_pieces.Add(pickableBlueMushroomObject);
                cultivatorItem.m_itemData.m_shared.m_buildPieces.m_pieces.Add(pickableThistleObject);
                cultivatorItem.m_itemData.m_shared.m_buildPieces.m_pieces.Add(pickableDandelionObject);

                cultivatorItem.m_itemData.m_shared.m_buildPieces.m_pieces.Add(birchTreeSaplingObject);
                cultivatorItem.m_itemData.m_shared.m_buildPieces.m_pieces.Add(oakTreeSaplingObject);
                cultivatorItem.m_itemData.m_shared.m_buildPieces.m_pieces.Add(swampTreeSaplingObject);
            }

            // This is enabled so that we can remove pieces for the pickable resources.
            cultivatorItem.m_itemData.m_shared.m_buildPieces.m_canRemovePieces = true;

            // Once everything has loaded we can stop execution of Update.
            Debug.Log("[Planting+ v1.4.5 finished loading]");
            this.enabled = false;
        }

        public static GameObject raspberryBushObject = null;
        public static GameObject blueberryBushObject = null;
        public static GameObject cloudberryBushObject = null;
        public static GameObject pickableMushroomObject = null;
        public static GameObject pickableYellowMushroomObject = null;
        public static GameObject pickableBlueMushroomObject = null;
        public static GameObject pickableThistleObject = null;
        public static GameObject pickableDandelionObject = null;
        public static ItemDrop raspberryItem = null;
        public static ItemDrop blueberriesItem = null;
        public static ItemDrop cloudberryItem = null;
        public static ItemDrop cultivatorItem = null;
        public static ItemDrop mushroomItem = null;
        public static ItemDrop yellowMushroomItem = null;
        public static ItemDrop blueMushroomItem = null;
        public static ItemDrop thistleItem = null;
        public static ItemDrop dandelionItem = null;
        public static GameObject placeWoodPoleVfx = null;
        public static GameObject buildCultivatorSfx = null;
        public static Piece raspberryBushPiece = null;
        public static Piece blueberryBushPiece = null;
        public static Piece cloudberryBushPiece = null;
        public static Piece pickableMushroomPiece = null;
        public static Piece pickableYellowMushroomPiece = null;
        public static Piece pickableBlueMushroomPiece = null;
        public static Piece pickableThistlePiece = null;
        public static Piece pickableDandelionPiece = null;

        public static GameObject birchConeObject = null;
        public static GameObject oakSeedsObject = null;
        public static GameObject ancientSeedsObject = null;
        public static GameObject birchTreeSaplingObject = null;
        public static GameObject oakTreeSaplingObject = null;
        public static GameObject birchTree1Object = null;
        public static GameObject birchTree2Object = null;
        public static GameObject oakTree1Object = null;
        public static GameObject swampTree1Object = null;
        public static GameObject pineTreeSaplingObject = null;
        public static GameObject beechTreeSaplingObject = null;
        public static GameObject swampTreeSaplingObject = null;
        public static bool isCloningPrefab = false;



        public static ConfigEntry<bool> modEnabled;
        public static ConfigEntry<int> nexusID;
        public static ConfigEntry<bool> enableOtherResources;
        public static ConfigEntry<bool> resourcesSpawnEmpty;
        public static ConfigEntry<bool> requireCultivation;
        public static ConfigEntry<bool> placeAnywhere;
        public static ConfigEntry<bool> enforceBiomes;
        public static ConfigEntry<bool> alternateIcons;
        public static ConfigEntry<bool> enableCustomRespawnTimes;
        public static ConfigEntry<bool> enableCustomResourceAmounts;
        public static ConfigEntry<int> raspberryCost;
        public static ConfigEntry<int> blueberryCost;
        public static ConfigEntry<int> cloudberryCost;
        public static ConfigEntry<int> mushroomCost;
        public static ConfigEntry<int> yellowMushroomCost;
        public static ConfigEntry<int> blueMushroomCost;
        public static ConfigEntry<int> thistleCost;
        public static ConfigEntry<int> dandelionCost;
        public static ConfigEntry<int> birchCost;
        public static ConfigEntry<int> oakCost;
        public static ConfigEntry<int> ancientCost;
        public static ConfigEntry<int> raspberryRespawnTime;
        public static ConfigEntry<int> blueberryRespawnTime;
        public static ConfigEntry<int> cloudberryRespawnTime;
        public static ConfigEntry<int> mushroomRespawnTime;
        public static ConfigEntry<int> yellowMushroomRespawnTime;
        public static ConfigEntry<int> blueMushroomRespawnTime;
        public static ConfigEntry<int> thistleRespawnTime;
        public static ConfigEntry<int> dandelionRespawnTime;
        public static ConfigEntry<float> birchGrowthTime;
        public static ConfigEntry<float> oakGrowthTime;
        public static ConfigEntry<float> ancientGrowthTime;

        public static ConfigEntry<float> birchMinScale;
        public static ConfigEntry<float> birchMaxScale;
        public static ConfigEntry<float> oakMinScale;
        public static ConfigEntry<float> oakMaxScale;
        public static ConfigEntry<float> ancientMinScale;
        public static ConfigEntry<float> ancientMaxScale;

        public static ConfigEntry<int> raspberryResourceAmount;
        public static ConfigEntry<int> blueberryResourceAmount;
        public static ConfigEntry<int> cloudberryResourceAmount;
        public static ConfigEntry<int> mushroomResourceAmount;
        public static ConfigEntry<int> yellowMushroomResourceAmount;
        public static ConfigEntry<int> blueMushroomResourceAmount;
        public static ConfigEntry<int> thistleResourceAmount;
        public static ConfigEntry<int> dandelionResourceAmount;
    }

    [HarmonyLib.HarmonyPatch(typeof(UnityEngine.Object), "Instantiate", new System.Type[] { typeof(Object), typeof(Vector3), typeof(Quaternion) })]
    public static class PlacePiece_Hook
    {
        [HarmonyLib.HarmonyPostfix]
        public static void Postfix(Object __result)
        {
            if (PlantingPlus.resourcesSpawnEmpty.Value)
            {
                bool piecePlaced = (bool)typeof(TerrainModifier).GetField("m_triggerOnPlaced", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
                if (piecePlaced && (__result is GameObject))
                {
                    GameObject gameObject = (GameObject)__result;
                    if (gameObject.name.StartsWith("RaspberryBush") || gameObject.name.StartsWith("BlueberryBush") || gameObject.name.StartsWith("CloudberryBush") || gameObject.name.StartsWith("Pickable_Mushroom") || gameObject.name.StartsWith("Pickable_Mushroom_yellow") || gameObject.name.StartsWith("Pickable_Mushroom_blue") || gameObject.name.StartsWith("Pickable_Thistle") || gameObject.name.StartsWith("Pickable_Dandelion"))
                    {
                        MethodInfo Pickable_SetPicked = typeof(Pickable).GetMethod("SetPicked", BindingFlags.NonPublic | BindingFlags.Instance, null, new System.Type[] { typeof(System.Boolean) }, null);
                        Pickable_SetPicked.Invoke(gameObject.GetComponent<Pickable>(), new object[] { (System.Boolean)true });
                    }
                }
            }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(Player), "SetLocalPlayer")]
    public static class Player_SetLocalPlayer_Hook
    {
        [HarmonyLib.HarmonyPostfix]
        public static void Postfix(HashSet<string> ___m_knownRecipes, int ___m_removeRayMask)
        {
            // Add new pieces to player's known pieces.
            if (PlantingPlus.raspberryBushPiece != null && PlantingPlus.blueberryBushPiece != null && PlantingPlus.cloudberryBushPiece != null)
            {
                if (!___m_knownRecipes.Contains(PlantingPlus.raspberryBushPiece.m_name))
                    ___m_knownRecipes.Add(PlantingPlus.raspberryBushPiece.m_name);
                if (!___m_knownRecipes.Contains(PlantingPlus.blueberryBushPiece.m_name))
                    ___m_knownRecipes.Add(PlantingPlus.blueberryBushPiece.m_name);
                if (!___m_knownRecipes.Contains(PlantingPlus.cloudberryBushPiece.m_name))
                    ___m_knownRecipes.Add(PlantingPlus.cloudberryBushPiece.m_name);

                //Player.m_localPlayer.AddKnownPiece(ValheimBerryPlantingMod.raspberryBushPiece);
                //Player.m_localPlayer.AddKnownPiece(ValheimBerryPlantingMod.blueberryBushPiece);
                //Player.m_localPlayer.AddKnownPiece(ValheimBerryPlantingMod.cloudberryBushPiece);
            }

            if (PlantingPlus.enableOtherResources.Value && (PlantingPlus.pickableMushroomPiece != null && PlantingPlus.pickableYellowMushroomPiece != null && PlantingPlus.pickableBlueMushroomPiece != null && PlantingPlus.pickableThistlePiece != null && PlantingPlus.pickableDandelionPiece != null && PlantingPlus.birchTreeSaplingObject != null && PlantingPlus.oakTreeSaplingObject != null && PlantingPlus.swampTreeSaplingObject != null))
            {
                if (!___m_knownRecipes.Contains(PlantingPlus.pickableMushroomPiece.m_name))
                    ___m_knownRecipes.Add(PlantingPlus.pickableMushroomPiece.m_name);
                if (!___m_knownRecipes.Contains(PlantingPlus.pickableYellowMushroomPiece.m_name))
                    ___m_knownRecipes.Add(PlantingPlus.pickableYellowMushroomPiece.m_name);
                if (!___m_knownRecipes.Contains(PlantingPlus.pickableBlueMushroomPiece.m_name))
                    ___m_knownRecipes.Add(PlantingPlus.pickableBlueMushroomPiece.m_name);
                if (!___m_knownRecipes.Contains(PlantingPlus.pickableThistlePiece.m_name))
                    ___m_knownRecipes.Add(PlantingPlus.pickableThistlePiece.m_name);
                if (!___m_knownRecipes.Contains(PlantingPlus.pickableDandelionPiece.m_name))
                    ___m_knownRecipes.Add(PlantingPlus.pickableDandelionPiece.m_name);

                if (!___m_knownRecipes.Contains(PlantingPlus.birchTreeSaplingObject.GetComponent<Piece>().m_name))
                    ___m_knownRecipes.Add(PlantingPlus.birchTreeSaplingObject.GetComponent<Piece>().m_name);
                if (!___m_knownRecipes.Contains(PlantingPlus.oakTreeSaplingObject.GetComponent<Piece>().m_name))
                    ___m_knownRecipes.Add(PlantingPlus.oakTreeSaplingObject.GetComponent<Piece>().m_name);
                if (!___m_knownRecipes.Contains(PlantingPlus.swampTreeSaplingObject.GetComponent<Piece>().m_name))
                    ___m_knownRecipes.Add(PlantingPlus.swampTreeSaplingObject.GetComponent<Piece>().m_name);

                //Player.m_localPlayer.AddKnownPiece(ValheimBerryPlantingMod.pickableMushroomPiece);
                //Player.m_localPlayer.AddKnownPiece(ValheimBerryPlantingMod.pickableYellowMushroomPiece);
                //Player.m_localPlayer.AddKnownPiece(ValheimBerryPlantingMod.pickableBlueMushroomPiece);
                //Player.m_localPlayer.AddKnownPiece(ValheimBerryPlantingMod.pickableThistlePiece);
            }
        }
    }

    [HarmonyPatch(typeof(ItemDrop), "Awake")]
    public static class ItemDrop_Awake_Hook
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            if (PlantingPlus.isCloningPrefab)
                return false;
            else
                return true;
        }
    }

    [HarmonyPatch(typeof(ItemDrop), "Start")]
    public static class ItemDrop_Start_Hook
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            if (PlantingPlus.isCloningPrefab)
                return false;
            else
                return true;
        }
    }

    [HarmonyPatch(typeof(ZNetView), "Awake")]
    public static class ZNetView_Awake_Hook
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            if (PlantingPlus.isCloningPrefab)
                return false;
            else
                return true;
        }
    }

    [HarmonyPatch(typeof(ObjectDB), "Awake")]
    public static class ObjectDB_Awake_Hook
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            if (PlantingPlus.enableOtherResources.Value)
            {
                while (PlantingPlus.birchConeObject == null || PlantingPlus.oakSeedsObject == null || PlantingPlus.ancientSeedsObject == null)
                {
                    foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
                    {
                        if (gameObject.name == "PineCone" && PlantingPlus.birchConeObject == null)
                        {
                            PlantingPlus.isCloningPrefab = true;
                            PlantingPlus.birchConeObject = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                            UnityEngine.Object.DontDestroyOnLoad(PlantingPlus.birchConeObject);
                            PlantingPlus.birchConeObject.hideFlags = HideFlags.HideInHierarchy;
                            PlantingPlus.birchConeObject.name = "BirchCone";
                            PlantingPlus.isCloningPrefab = false;
                        }

                        if (gameObject.name == "BeechSeeds" && PlantingPlus.oakSeedsObject == null)
                        {
                            PlantingPlus.isCloningPrefab = true;
                            PlantingPlus.oakSeedsObject = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                            UnityEngine.Object.DontDestroyOnLoad(PlantingPlus.oakSeedsObject);
                            PlantingPlus.oakSeedsObject.hideFlags = HideFlags.HideInHierarchy;
                            PlantingPlus.oakSeedsObject.name = "OakSeeds";
                            PlantingPlus.isCloningPrefab = false;
                        }

                        if (gameObject.name == "BeechSeeds" && PlantingPlus.ancientSeedsObject == null)
                        {
                            PlantingPlus.isCloningPrefab = true;
                            PlantingPlus.ancientSeedsObject = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                            UnityEngine.Object.DontDestroyOnLoad(PlantingPlus.ancientSeedsObject);
                            PlantingPlus.ancientSeedsObject.hideFlags = HideFlags.HideInHierarchy;
                            PlantingPlus.ancientSeedsObject.name = "AncientSeeds";
                            PlantingPlus.isCloningPrefab = false;
                        }
                    }

                    if (PlantingPlus.birchConeObject != null && PlantingPlus.oakSeedsObject != null && PlantingPlus.ancientSeedsObject != null)
                    {
                        PlantingPlus.birchConeObject.GetComponent<ItemDrop>().name = "BirchCone";
                        PlantingPlus.birchConeObject.GetComponent<ItemDrop>().m_itemData.m_shared.m_name = "Birch Cone";
                        PlantingPlus.birchConeObject.GetComponent<ItemDrop>().m_itemData.m_shared.m_description = "Plant it to grow a birch tree.";
                        PlantingPlus.oakSeedsObject.GetComponent<ItemDrop>().name = "OakSeeds";
                        PlantingPlus.oakSeedsObject.GetComponent<ItemDrop>().m_itemData.m_shared.m_name = "Oak Seeds";
                        PlantingPlus.oakSeedsObject.GetComponent<ItemDrop>().m_itemData.m_shared.m_description = "Plant them to grow an oak tree.";
                        PlantingPlus.ancientSeedsObject.GetComponent<ItemDrop>().name = "AncientSeeds";
                        PlantingPlus.ancientSeedsObject.GetComponent<ItemDrop>().m_itemData.m_shared.m_name = "Ancient Seeds";
                        PlantingPlus.ancientSeedsObject.GetComponent<ItemDrop>().m_itemData.m_shared.m_description = "Plant them to grow an ancient tree.";
                    }
                }

                if (!ObjectDB.instance.m_items.Contains(PlantingPlus.birchConeObject))
                    ObjectDB.instance.m_items.Add(PlantingPlus.birchConeObject);
                if (!ObjectDB.instance.m_items.Contains(PlantingPlus.oakSeedsObject))
                    ObjectDB.instance.m_items.Add(PlantingPlus.oakSeedsObject);
                if (!ObjectDB.instance.m_items.Contains(PlantingPlus.ancientSeedsObject))
                    ObjectDB.instance.m_items.Add(PlantingPlus.ancientSeedsObject);
            }
        }
    }

    [HarmonyPatch(typeof(ObjectDB), "CopyOtherDB")]
    public static class ObjectDB_CopyOtherDB_Hook
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            if (PlantingPlus.enableOtherResources.Value)
            {
                while (PlantingPlus.birchConeObject == null || PlantingPlus.oakSeedsObject == null || PlantingPlus.ancientSeedsObject == null)
                {
                    foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
                    {
                        if (gameObject.name == "PineCone" && PlantingPlus.birchConeObject == null)
                        {
                            PlantingPlus.isCloningPrefab = true;
                            PlantingPlus.birchConeObject = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                            UnityEngine.Object.DontDestroyOnLoad(PlantingPlus.birchConeObject);
                            PlantingPlus.birchConeObject.hideFlags = HideFlags.HideInHierarchy;
                            PlantingPlus.birchConeObject.name = "BirchCone";
                            PlantingPlus.isCloningPrefab = false;
                        }

                        if (gameObject.name == "BeechSeeds" && PlantingPlus.oakSeedsObject == null)
                        {
                            PlantingPlus.isCloningPrefab = true;
                            PlantingPlus.oakSeedsObject = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                            UnityEngine.Object.DontDestroyOnLoad(PlantingPlus.oakSeedsObject);
                            PlantingPlus.oakSeedsObject.hideFlags = HideFlags.HideInHierarchy;
                            PlantingPlus.oakSeedsObject.name = "OakSeeds";
                            PlantingPlus.isCloningPrefab = false;
                        }

                        if (gameObject.name == "BeechSeeds" && PlantingPlus.ancientSeedsObject == null)
                        {
                            PlantingPlus.isCloningPrefab = true;
                            PlantingPlus.ancientSeedsObject = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                            UnityEngine.Object.DontDestroyOnLoad(PlantingPlus.ancientSeedsObject);
                            PlantingPlus.ancientSeedsObject.hideFlags = HideFlags.HideInHierarchy;
                            PlantingPlus.ancientSeedsObject.name = "AncientSeeds";
                            PlantingPlus.isCloningPrefab = false;
                        }
                    }

                    if (PlantingPlus.birchConeObject != null && PlantingPlus.oakSeedsObject != null && PlantingPlus.ancientSeedsObject != null)
                    {
                        PlantingPlus.birchConeObject.GetComponent<ItemDrop>().name = "BirchCone";
                        PlantingPlus.birchConeObject.GetComponent<ItemDrop>().m_itemData.m_shared.m_name = "Birch Cone";
                        PlantingPlus.birchConeObject.GetComponent<ItemDrop>().m_itemData.m_shared.m_description = "Plant it to grow a birch tree.";
                        PlantingPlus.oakSeedsObject.GetComponent<ItemDrop>().name = "OakSeeds";
                        PlantingPlus.oakSeedsObject.GetComponent<ItemDrop>().m_itemData.m_shared.m_name = "Oak Seeds";
                        PlantingPlus.oakSeedsObject.GetComponent<ItemDrop>().m_itemData.m_shared.m_description = "Plant them to grow an oak tree.";
                        PlantingPlus.ancientSeedsObject.GetComponent<ItemDrop>().name = "AncientSeeds";
                        PlantingPlus.ancientSeedsObject.GetComponent<ItemDrop>().m_itemData.m_shared.m_name = "Ancient Seeds";
                        PlantingPlus.ancientSeedsObject.GetComponent<ItemDrop>().m_itemData.m_shared.m_description = "Plant them to grow an ancient tree.";
                    }
                }

                if (!ObjectDB.instance.m_items.Contains(PlantingPlus.birchConeObject))
                    ObjectDB.instance.m_items.Add(PlantingPlus.birchConeObject);
                if (!ObjectDB.instance.m_items.Contains(PlantingPlus.oakSeedsObject))
                    ObjectDB.instance.m_items.Add(PlantingPlus.oakSeedsObject);
                if (!ObjectDB.instance.m_items.Contains(PlantingPlus.ancientSeedsObject))
                    ObjectDB.instance.m_items.Add(PlantingPlus.ancientSeedsObject);
            }
        }
    }

    [HarmonyPatch(typeof(ZNetScene), "Awake")]
    public static class ZNetScene_Awake_Hook
    {
        [HarmonyPrefix]
        public static void HarmonyPrefix(ZNetScene __instance)
        {
            if (PlantingPlus.enableOtherResources.Value)
            {
                if (__instance != null && PlantingPlus.birchConeObject != null && PlantingPlus.oakSeedsObject != null && PlantingPlus.ancientSeedsObject != null && PlantingPlus.birchTreeSaplingObject != null && PlantingPlus.oakTreeSaplingObject != null && PlantingPlus.swampTreeSaplingObject != null)
                {
                    __instance.m_prefabs.Add(PlantingPlus.birchConeObject);
                    __instance.m_prefabs.Add(PlantingPlus.oakSeedsObject);
                    __instance.m_prefabs.Add(PlantingPlus.ancientSeedsObject);
                    __instance.m_prefabs.Add(PlantingPlus.birchTreeSaplingObject);
                    __instance.m_prefabs.Add(PlantingPlus.oakTreeSaplingObject);
                    __instance.m_prefabs.Add(PlantingPlus.swampTreeSaplingObject);
                }
            }
        }
    }

    // This patch will allow the cultivator to remove Pickable objects.
    [HarmonyLib.HarmonyPatch(typeof(Player), "RemovePiece")]
    public static class Player_RemovePiece_Hook
    {
        [HarmonyLib.HarmonyPrefix]
        public static bool Prefix(Player __instance, ZSyncAnimation ___m_zanim, ref bool __result)
        {
            if (__instance.GetRightItem().m_shared.m_name == "$item_cultivator")
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(GameCamera.instance.transform.position, GameCamera.instance.transform.forward, out raycastHit, 50f, LayerMask.GetMask(new string[] { "item", "piece_nonsolid", "terrain" })) && Vector3.Distance(raycastHit.point, __instance.m_eye.position) < __instance.m_maxPlaceDistance)
                {
                    Pickable pickable = raycastHit.collider.GetComponentInParent<Pickable>();
                    if (pickable == null /*&& raycastHit.collider.GetComponent<Heightmap>()*/)
                    {
                        float num = 999999f;
                        ZNetView znv = null;
                        Dictionary<ZDO, ZNetView> zdo_instances = typeof(ZNetScene).GetField("m_instances", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(ZNetScene.instance) as Dictionary<ZDO, ZNetView>;
                        foreach(KeyValuePair<ZDO, ZNetView> entry in zdo_instances)
                        {
                            if (entry.Value.gameObject.GetComponent<Pickable>())
                            {
                                float num2 = Mathf.Sqrt(((entry.Key.GetPosition().x - raycastHit.point.x) * (entry.Key.GetPosition().x - raycastHit.point.x)) + ((entry.Key.GetPosition().z - raycastHit.point.z) * (entry.Key.GetPosition().z - raycastHit.point.z)));
                                if (num2 <= 0.5 && num2 <= num)
                                {
                                    num = num2;
                                    znv = entry.Value;
                                }
                            }
                        }
                        if (znv)
                        {
                            pickable = znv.gameObject.GetComponent<Pickable>();
                        }
                    }

                    if (pickable != null)
                    {
                        ZNetView component = pickable.GetComponent<ZNetView>();
                        if (component == null)
                        {
                            __result = false;
                           return false;
                        }

                        component.ClaimOwnership();
                        __instance.m_removeEffects.Create(pickable.transform.position, Quaternion.identity, null, 1f);
                        ZNetScene.instance.Destroy(pickable.gameObject);
                        ItemDrop.ItemData rightItem = __instance.GetRightItem();
                        if (rightItem != null)
                        {
                            __instance.FaceLookDirection();
                            ___m_zanim.SetTrigger(rightItem.m_shared.m_attack.m_attackAnimation);
                        }
                        __result = true;
                        return false;
                    }

                }
                __result = false;
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    // These patches fix an in game bug where pick time gets reset upon loading into a chunk with a pickable resource.
    [HarmonyPatch(typeof(Pickable), "SetPicked")]
    public static class FixPickableTime
    {
        public class PickState
        {
            public long picked_time;
            public bool picked;
        }

        [HarmonyPrefix]
        public static void Prefix(bool picked, ZNetView ___m_nview, bool ___m_picked, ref PickState __state)
        {
            __state = new PickState();
            __state.picked_time = ___m_nview.GetZDO().GetLong("picked_time", 0L);
            __state.picked = ___m_picked;
        }

        [HarmonyPostfix]
        public static void Postfix(bool picked, ZNetView ___m_nview, bool ___m_picked, ref PickState __state)
        {
            //if we're not changing the state, don't change trhe pick time.
            bool flag = __state != null && __state.picked == ___m_picked && ___m_nview.IsOwner();
            if (flag)
            {
                ___m_nview.GetZDO().Set("picked_time", __state.picked_time);
            }
        }
    }
}
