using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI.Chat;
using static Terraria.ModLoader.ModContent;

namespace ConfigurableTraderNPC
{
    public class Player : ModPlayer
    {
        public static List<string> list_TraderItemListIDs = new List<string>();
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if ((ConfigurableTraderNPC.HotKey_AddItemIntoTraderList.JustPressed || ConfigurableTraderNPC.HotKey_RemItemFromTraderList.JustPressed) && Main.mouseItem.netID != 0)
            {
                Main.NewText("id = " + Main.mouseItem.netID + ", value = " + Main.mouseItem.value);
                if (Config.ReadConfig())
                {
                    //Main.NewText("type = " + Main.mouseItem.type + ", name = " + Main.mouseItem.Name + ", netID = " + Main.mouseItem.netID);
                    //int a_len = Config.TraderItemListIDs.Length;
                    //Main.NewText("a_len = " + a_len);

                    //Get holding item id
                    int item_id = Main.mouseItem.netID;
                    list_TraderItemListIDs = Config.TraderItemListIDs.Split(',').ToList();

                    if (ConfigurableTraderNPC.HotKey_AddItemIntoTraderList.JustPressed)
                    {
                        //If config file did not contain item id
                        //if (!Config.TraderItemListIDs.Contains(item_id.ToString()))
                        if (!list_TraderItemListIDs.Contains(item_id.ToString()))
                        {
                            //Add to config
                            if (Config.TraderItemListIDs.Length != 0)
                            {
                                Config.TraderItemListIDs += "," + item_id.ToString();
                            } 
                            else
                            {
                                Config.TraderItemListIDs += item_id.ToString();
                            }
                            //If sucess save, then print in chat
                            if (Config.SaveConfig)
                            {
                                Main.NewText("Added " + Main.mouseItem.Name + " to config");
                            }
                            //Print in chat anyway, yeah
                            else
                            {
                                Main.NewText("NOT Added " + Main.mouseItem.Name + " to config !!!!!");
                            }
                        }
                        else
                        {
                            Main.NewText("Item " + Main.mouseItem.Name + " already IN config");
                        }
                    }

                    if (ConfigurableTraderNPC.HotKey_RemItemFromTraderList.JustPressed)
                    {
                        //If config file contain item id
                        //if (Config.TraderItemListIDs.Contains(item_id.ToString()))
                        if (list_TraderItemListIDs.Contains(item_id.ToString()))
                        {
                            //Convert string to list
                            //list_TraderItemListIDs = Config.TraderItemListIDs.Split(',').ToList();
                            //Remove item id from config
                            list_TraderItemListIDs.Remove(item_id.ToString());
                            //... and convert it back to string (stupid, i know)
                            Config.TraderItemListIDs = String.Join(",", list_TraderItemListIDs);
                            if (Config.SaveConfig)
                            {
                                Main.NewText("Removed " + Main.mouseItem.Name + " from config");
                            }
                            else
                            {
                                Main.NewText("NOT removed " + Main.mouseItem.Name + " from config !!!!!");
                            }
                        }
                        else
                        {
                            Main.NewText("Item " + Main.mouseItem.Name + " is NOT IN config");
                        }
                    }
                }
                else
                {
                    Main.NewText(Config.filename + " did not readed :(");
                }
            }
        }
    }
}