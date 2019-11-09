using System;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using log4net;
using System.Collections.Generic;

namespace ConfigurableTraderNPC
{
    public static class Config
    {
        public static string filename = "ConfigurableTraderNPC.json";

        public static int BuyoutPriceMult = 100;
        public static string TraderItemListIDs = "";

        static string ConfigPath = Path.Combine(Main.SavePath, "Mod Configs", filename);

        static Preferences Configuration = new Preferences(ConfigPath);

        public static void Load()
        {
            //Main.NewText("Load config");
            bool success = ReadConfig();

            if (!success)
            {
                ErrorLogger.Log("Failed to read ConfigurableTraderNPC's config file! Recreating config...");
                CreateConfig();
            }
        }

        public static bool ReadConfig()
        {
            //Main.NewText("Read config...");
            if (Configuration.Load())
            {
                //Main.NewText("Loaded config");
                Configuration.Get("BuyoutPriceMult", ref BuyoutPriceMult);
                if (BuyoutPriceMult <= 10)
                {
                    BuyoutPriceMult = 10;
                }
                Configuration.Get("TraderItemListIDs", ref TraderItemListIDs);
                //Main.NewText("TraderItemListIDs = " + TraderItemListIDs);
                return true;
            }
            else
            {
                //Main.NewText("NOT Loaded config");
                return false;
            }
        }

        static void CreateConfig()
        {
            Configuration.Clear();
            Configuration.Put("BuyoutPriceMult", BuyoutPriceMult);
            Configuration.Put("TraderItemListIDs", TraderItemListIDs);
            Configuration.Save();
        }

        public static bool SaveConfig
        {
            get
            {
                Configuration.Put("TraderItemListIDs", TraderItemListIDs);
                if (Configuration.Save())
                {
                    //Main.NewText("Save config");
                    return true;
                }
                else
                {
                    //Main.NewText("DID NOT Save config");
                    return false;
                }
            }
        }
    }
}