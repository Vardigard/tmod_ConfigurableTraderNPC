using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using log4net;
using System.IO;
using System.Linq;

namespace ConfigurableTraderNPC
{
	public class ConfigurableTraderNPC : Mod
    {
        public static ModHotKey HotKey_AddItemIntoTraderList;
        public static ModHotKey HotKey_RemItemFromTraderList;
        public override void Load()
        {
            Config.Load();
            HotKey_AddItemIntoTraderList = RegisterHotKey("Add Item into Trader", "Y");
            HotKey_RemItemFromTraderList = RegisterHotKey("Rem Item from Trader", "U");
        }

        public override void Unload()
        {
            // Unload static references
            // You need to clear static references to assets (Texture2D, SoundEffects, Effects). 
            // In addition to that, if you want your mod to completely unload during unload, you need to clear static references to anything referencing your Mod class
            HotKey_AddItemIntoTraderList = null;
            HotKey_RemItemFromTraderList = null;
            Config.filename = null;
            Config.TraderItemListIDs = null;
            Player.list_TraderItemListIDs = null;
        }
        public class Trader : ModNPC
        {
            public override bool Autoload(ref string name)
            {
                name = "Nihiliminon";
                return mod.Properties.Autoload;
            }

            public override string Texture
            {
                get
                {
                    return "ConfigurableTraderNPC/ConfigurableTraderNPC";
                }
            }

            public override void SetDefaults()
            {
                //npc.name = "Custom Town NPC";   //the name displayed when hovering over the npc ingame.
                npc.townNPC = true; //This defines if the npc is a town Npc or not
                npc.friendly = true;  //this defines if the npc can hur you or not()
                npc.width = 18; //the npc sprite width
                npc.height = 46;  //the npc sprite height
                npc.aiStyle = 7; //this is the npc ai style, 7 is Pasive Ai
                npc.defense = 25;  //the npc defense
                npc.lifeMax = 250;// the npc life
                npc.HitSound = SoundID.NPCHit1;  //the npc sound when is hit
                npc.DeathSound = SoundID.NPCDeath1;  //the npc sound when he dies
                npc.knockBackResist = 0.5f;  //the npc knockback resistance
                Main.npcFrameCount[npc.type] = 23; //this defines how many frames the npc sprite sheet has
                NPCID.Sets.ExtraFramesCount[npc.type] = 9;
                NPCID.Sets.AttackFrameCount[npc.type] = 4;
                NPCID.Sets.DangerDetectRange[npc.type] = 150; //this defines the npc danger detect range
                NPCID.Sets.AttackType[npc.type] = 3; //this is the attack type,  0 (throwing), 1 (shooting), or 2 (magic). 3 (melee)
                NPCID.Sets.AttackTime[npc.type] = 30; //this defines the npc attack speed
                NPCID.Sets.AttackAverageChance[npc.type] = 10;//this defines the npc atack chance
                NPCID.Sets.HatOffsetY[npc.type] = 4; //this defines the party hat position
                animationType = NPCID.Guide;  //this copy the guide animation
                
            }

            /*public ConfigurableTraderNPC()
            {
            }
            */
            public override bool CanTownNPCSpawn(int numTownNPCs, int money) //Whether or not the conditions have been met for this town NPC to be able to move into town.
            {
                if (NPC.downedBoss1)  //so after the EoC is killed
                {
                    return true;
                }
                return false;
            }
            public override bool CheckConditions(int left, int right, int top, int bottom)    //Allows you to define special conditions required for this town NPC's house
            {
                return true;  //so when a house is available the npc will  spawn
            }
            public override string TownNPCName()     //Allows you to give this town NPC any name when it spawns
            {
                switch (WorldGen.genRand.Next(0))
                {
                    case 0:
                        return "Nihiliminon";
                    default:
                        return "Nihiliminon";
                }
            }

            public override void SetChatButtons(ref string button, ref string button2)  //Allows you to set the text for the buttons that appear on this town NPC's chat window.
            {
                button = "Buy Items";   //this defines the buy button name
            }
            public override void OnChatButtonClicked(bool firstButton, ref bool openShop) //Allows you to make something happen whenever a button is clicked on this town NPC's chat window. The firstButton parameter tells whether the first button or second button (button and button2 from SetChatButtons) was clicked. Set the shop parameter to true to open this NPC's shop.
            {

                if (firstButton)
                {
                    openShop = true;   //so when you click on buy button opens the shop
                }
            }

            public override void SetupShop(Chest shop, ref int nextSlot)       //Allows you to add items to this town NPC's shop. Add an item by setting the defaults of shop.item[nextSlot] then incrementing nextSlot.
            {
                var tmp_list = Config.TraderItemListIDs.Split(',').ToList();
                if (tmp_list.Count > 0)
                {
                    foreach (var tmp_id in tmp_list)
                    {
                        shop.item[nextSlot].SetDefaults(Int32.Parse(tmp_id));
                        if (shop.item[nextSlot].value != 0)
                        {
                            shop.item[nextSlot].shopCustomPrice = shop.item[nextSlot].value * Config.BuyoutPriceMult;
                        }
                        else
                        {
                            shop.item[nextSlot].shopCustomPrice = 1000;
                        }
                        nextSlot++;
                    }
                }
            }

            public override string GetChat()       //Allows you to give this town NPC a chat message when a player talks to it.
            {
                switch (Main.rand.Next(4))    //this are the messages when you talk to the npc
                {
                    case 0:
                        return "You wanna buy something?";
                    case 1:
                        return "What you want?";
                    case 2:
                        return "I like this house.";
                    case 3:
                        return "<I'm blue dabu di dabu dai>....OH HELLO THERE..";
                    default:
                        return "Go kill Skeletron.";

                }
            }
            public override void TownNPCAttackStrength(ref int damage, ref float knockback)//  Allows you to determine the damage and knockback of this town NPC attack
            {
                damage = 40;  //npc damage
                knockback = 2f;   //npc knockback
            }

            public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)  //Allows you to determine the cooldown between each of this town NPC's attack. The cooldown will be a number greater than or equal to the first parameter, and less then the sum of the two parameters.
            {
                cooldown = 5;
                randExtraCooldown = 10;
            }
        }
    }
}