using BepInEx;
using BepInEx.Configuration;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VipChest
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.ClientMustHaveMod, VersionStrictness.Major)]
    internal class JotunnModStub : BaseUnityPlugin
    {
        public const string PluginGUID = "VipChest";
        public const string PluginName = "VipChest";
        public const string PluginVersion = "1.0.4";
        private ConfigEntry<int> CoinAmount;
        private ConfigEntry<int> ChestHeight;

        private void Awake()
        {
            CreateConfigValues();
            PrefabManager.OnVanillaPrefabsAvailable += AddClonedItems;
        }

        private void AddClonedItems()
        {
            try
            {
                var pieces = new List<CustomPiece>();

                var vipchest_personal = new CustomPiece("vipchest_personal", "piece_chest_private",
                    new PieceConfig
                    {
                        PieceTable = "Hammer",
                        Category = "Furniture",
                        Name = "Personal Chad Chest",
                        Requirements = new[]
                        {
                            new RequirementConfig
                            {
                                Item = "Coins",
                                Amount = 26973,
                                Recover = false
                            },
                            new RequirementConfig
                            {
                                Item = "SerpentScale",
                                Amount = 100,
                                Recover = false
                            },
                            new RequirementConfig
                            {
                                Item = "FineWood",
                                Amount = 50,
                                Recover = false
                            },
                            new RequirementConfig
                            {
                                Item = "Iron",
                                Amount = 30,
                                Recover = false
                            }
                        }
                    });

                pieces.Add(vipchest_personal);


                foreach (var piece in pieces)
                {
                    piece.Piece.GetComponent<WearNTear>().m_health *= 9999.99f;
                    piece.Piece.GetComponent<Container>().m_height = 2;
                    PieceManager.Instance.AddPiece(piece);
                }

            }
            catch (Exception ex)
            {
                Jotunn.Logger.LogError($"Error while adding cloned item: {ex.Message}");
                Jotunn.Logger.LogError(ex.StackTrace);
            }
            finally
            {
                // You want that to run only once, Jotunn has the item cached for the game session
                PrefabManager.OnVanillaPrefabsAvailable -= AddClonedItems;
            }
        }

        private void CreateConfigValues()
        {
            Config.SaveOnConfigSet = true;

            CoinAmount = Config.Bind("Server config", "CoinAmount", 26973, 
                new ConfigDescription("Coin Amount", null ,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));

            ChestHeight = Config.Bind("Server config", "ChestHeight", 2,
                new ConfigDescription("Chest Height",
                new AcceptableValueRange<int>(1, 10),
                new ConfigurationManagerAttributes { IsAdminOnly = true }));

            SynchronizationManager.OnConfigurationSynchronized += (obj, attr) =>
            {
                if (attr.InitialSynchronization)
                {
                    Jotunn.Logger.LogMessage("Initial Config sync event received");
                }
                else
                {
                    Jotunn.Logger.LogMessage("Config sync event received");
                }
            };

        }

    }



  
}