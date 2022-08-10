﻿using R2API;
using System;
using ModdedEntityStates.Joe;
using ModdedEntityStates.TeslaTrooper;
using ModdedEntityStates.TeslaTrooper.Tower;
using Modules.Survivors;
using System.Collections.Generic;

namespace Modules
{
    internal static class Tokens {

        public static void AddTokens()
        {
            AddJoeTokens();
            AddTeslaTokens();
            AddTeslaTowerTokens();
        }

        private static void AddJoeTokens()
        {
            #region not henry
            string prefix = FacelessJoePlugin.DEV_PREFIX + "_JOE_BODY_";

            string desc = "joe has a funny vertex on his face that's painted wrong.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > goddammit jerry." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > use the Jump Attack to avoid damage and hit kniggas. is this gonna be annoying not being able to swing in the air? probably" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > use the fireball to fill the empty space in his barren kit. seriously i gotta look at this thing." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > idk what R is but it's gonna be cool." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, jerrys' screams lingering in his hears.";
            string outroFailure = "..and so he vanished, his normals never recalculated outside.";

            string fullName = "Faceless Joe";
            LanguageAPI.Add(prefix + "NAME", fullName);
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "and the zambambos");
            LanguageAPI.Add(prefix + "LORE", "All the best charcoals come from coconuts. They're easy to use and it's easy to grow more. You don't have to chop down an entire tree just to get your charcoal. But I need to, because the charcoals I want can't come from any coconuts. They need to have the perfect lighting temperature, the perfect lifetime, the perfect shape, the perfect flavor-capturing smoke. No, the charcoals I need can only be made from special trees. Trees in a forest just over this hill, habibi. But we're not gonna burn it down, absolutely not. If the trees are gone, how are we gonna get any charcoal? No, we can’t destroy the trees and take from them. We want them to be happy, to help us achieve our dreams of the perfect smoke because they like us. Make sense? Didn’t think so, hehaha. Nice O’s, you’re getting better at those. Anyways, in this forest, there's someone who’s been able to earn the trust of the trees. We'll be, uh.. meeting.. with him soon. Maybe if we hand him a hose, and he’ll put his swords down and join us, haha.");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "skin?");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Joe passive");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_SWING_NAME", "Swing");
            LanguageAPI.Add(prefix + "PRIMARY_SWING_DESCRIPTION", $"{Helpers.agilePrefix} Swing your sword for <style=cIsDamage>{100f * Primary1Swing.swingDamage}% damage</style>.\n use in the air for a <style=cIsUtility>Falling Jump Attack</style>");

            LanguageAPI.Add(prefix + "PRIMARY_SWING_NAME_CLASSIC", "Swing Classic");

            LanguageAPI.Add(prefix + "PRIMARY_BOMB_NAME", "Throw");
            LanguageAPI.Add(prefix + "PRIMARY_BOMB_DESCRIPTION", "is it happening?");

            LanguageAPI.Add(prefix + "PRIMARY_ZAP_NAME", "S.I.C.K.L.E.");
            LanguageAPI.Add(prefix + "PRIMARY_ZAP_DESCRIPTION", "it is happening");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_FIREBALL_NAME", "Fireball");
            LanguageAPI.Add(prefix + "SECONDARY_FIREBALL_DESCRIPTION", $"{Helpers.agilePrefix} Fire a ball for <style=cIsDamage>{100f * StaticHenryValues.gunDamageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "SECONDARY_BIGZAP_NAME", "Big Zap");
            LanguageAPI.Add(prefix + "SECONDARY_BIGZAP_DESCRIPTION", $"2000 Volts");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_DASH_NAME", "Dash");
            LanguageAPI.Add(prefix + "UTILITY_DASH_DESCRIPTION", "very original, i know");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_TOWER_NAME", "Tesla Tower");
            LanguageAPI.Add(prefix + "SPECIAL_TOWER_DESCRIPTION", "Construction Complete");

            LanguageAPI.Add(prefix + "SPECIAL_BOMB_NAME", "Something Cool, I'm Sure");
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * StaticHenryValues.bombDamageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", $"{fullName}: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", $"As {fullName}, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", $"{fullName}: Mastery");
            #endregion
            #endregion not henry
        }

        private static void AddTeslaTokens() {
            #region not henry 2
            string prefix = TeslaTrooperSurvivor.TESLA_PREFIX;

            string desc = "The Tesla Trooper is a close-mid-range bruiser, who can construct Tesla Towers to empower his combat potential.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Tesla Gauntlet close range to deal the most damage. The reticle will reflect this" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Use 2000 Volts to control crowds, and to command your tower to wipe crowds" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > You benefit from being closer to enemies, use his Utility to assist with this." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > The Tesla Tower inherits your items, but mainly benefits from damage items." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, rubber shoes in motion.";
            string outroFailure = "..and so he vanished, unit lost.";

            string fullName = "Tesla Trooper";
            LanguageAPI.Add(prefix + "NAME", fullName);
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Electrician In the Field");

            LanguageAPI.Add(prefix + "LORE",
"<style=cMono><line-height=15>=========================================\n" +
"====   MyBabel Video Tape Recorder   ====\n" +
"====      [Model 2.46.7-09 ]    =========\n" +
"=========================================\n" +
"Select file to play:\n" +
">20551309174644.bvl    <<\n" +
">20551209122159.bvl\n" +
"=========================================\n" +
"Decompressing Video...\n" +
"Decompressing Audio...\n" +
"Loading Subtitles...\n" +
"......... ..... ..\n" +
"Complete!\n" +
"=========================================\n" +
"Play? Y/N\n" +
">Y</line-height></style>\n\n" +
"Видеожурнал, 13 сентября 2055 г.\n" +
"Сегодня я буду работать над полным бронежилетом Tesla, который я сделал для нашей секретной маленькой поездки, и немного поприветствую маленького Милослава, который думал, что писать своему старшему брату слишком утомительно.Кто еще думал, что у него хватило духу раскрутить целую бутылку чистой водки, как какой - нибудь чемпион по скачкам!\n" +
"\n<style=cMono><line-height=15>=========================================\n" +
"Translated Subtitles Enabled.\n" +
"=========================================</line-height></style>\n\n" +
"And then you had to join that phony, space-faring delivery company. Are you a delivery boy, Milo? I didn't think you'd stoop so low!\n" +
"\n[Audible Laughter]\n\n" +
"Ever since we were but boys you always were the one with a broken mirror, who'd whistle indoors and walk under ladders. I remember the one time where we were running down the street, because our favourite ice cream shop opened that day, oh do I miss that double chocolate.. Anyway the shop just opened and when we were crossing the street? It was almost like every car in town wanted your head! Oh, and when we went ice skating for the first time! Many vivid memories! The lake back home surely wanted you to stay forever! If it weren't for me, the dashing hero that I am, saving you in the nick of time like any good and chivalrous older brother would.\n" +
"\n[Torch Lighter Sparks]\n\n" +
"Even now as adults, we are not free from the capitalism that took over home. In space, I thought we were free! But oho how mistaken I was. Its sad to see a poor comrade begging for coin every time you pass them by the ship docks, I always toss whatever is in my pocket to them, let it be cash or cigarettes. I bet you're feeling jealous, aren't you? Big bro taking away your cigarettes? Well it's because it's not healthy for you! I don't want my little bro shrivelling up like some babushka! We must be strong! For this universe is not kind to us... Certainly not you.\n" +
"\n[Engine Humming]\n\n" +
"Speaking of strong things, I think the capacitor on my personal suit is calibrated, all it needs is a test drive. Oh if only you could see this Milo. If 20 volts can kill a horse then I'm able to take out a whole ranch! Just a single discharge gives a voltage of 200,000 watts! It even has a function to absorb kinetic energy and transform it into electricity, isn't that cool? You can't see it from this angle but I also have this deployable tower I can throw out in emergency that I have to be in two places at once, if i wasn't a staunch socialist I would be selling these like blini left and right. It's just that even when I work on my equipment, I can't get this thought of my head.\n" +
"\n[Metal Clang]\n\n" +
"You know.. I really really do hope you're safe and okay.. We've always been in touch through thick and thin.. When I left home to pursue my engineering degree I always sent letters back home, sent ISM's when I hopped on the transport ship.. At least once a week I would find time to write down how I'd been, hoping you would do the same. You never missed a beat. UES is a shady company that always involved itself with cover ups and conspiracies about all sorts of things. I told you UES was dodgy yet you pushed and pushed that it's just another step in your career. Yet, when you stopped sending messages, I knew something was up.\n" +
"\n[Audible Sigh]\n\n" +
"I signed up to a commission job with an NDA for a undisclosed UES 'shipment'. The Safe Travels, It's called. Ironic coming from the UES. I've been assigned as a ship technician, they require commission expertise not just because of my talent, but the nature of the contract deems it secretive. I wouldn't think any of the conspiracies were true but aa-I-a suppose a broken clock is right twice a day-\n" +
"\n[Sirens]\n\n" +
"Miloslav, Miloslav,. I hope the next time this recording is played, it'll be with drinks and cheering and knowing that you're safe. I am not letting UES, or anything, harm you without their blood on my hands.\n" +
"\n[Distant Running]\n\n" +
"Miloslav, I will find you.\n\n" +
"<style=cMono><line-height=15>==========================================\n" +
"..... ... .\n" +
"Play Again?\n" +
">_</line-height></style>"
                );

            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Spetsnaz");
            LanguageAPI.Add(prefix + "MC_SKIN_NAME", "Minecraft");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Joe passive");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_ZAP_NAME", "Tesla Gauntlet");
            string conductiveText = FacelessJoePlugin.conductiveAlly ? $" Use on allies to {Helpers.DamageText("charge")} them" : "";
            LanguageAPI.Add(prefix + "PRIMARY_ZAP_DESCRIPTION", $"Zap targeted units with a bolt of electricity for {Helpers.DamageText($"{Zap.DamageCoefficient * 100}% damage")}. Casts {Helpers.UtilityText($"up to 3 bolts")} at {Helpers.UtilityText($"close range")}. {Helpers.UtilityText("Charges")} allies.");

            LanguageAPI.Add("KEYWORD_CHARGED", $"<style=cKeywordName>Charging</style><style=cSub>A charged ally has their next attack {Helpers.UtilityText("shocking")} and damage boosted by {Helpers.DamageText(TeslaTrooperSurvivor.conductiveAllyBoost.ToString())}x");


            LanguageAPI.Add(prefix + "PRIMARY_PUNCH_NAME", "Tesla Fist");
            LanguageAPI.Add(prefix + "PRIMARY_PUNCH_DESCRIPTION", $"Punch enemies for {Helpers.DamageValueText(ZapPunch.DamageCoefficient)}, and zap enemies in a cone for {Helpers.DamageValueText(ZapPunch.OrbDamageCoefficient)}. Punching projectiles sends them back, electrically charged");

            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_BIGZAP_NAME", "2000 Volts");
            LanguageAPI.Add(prefix + "SECONDARY_BIGZAP_DESCRIPTION", $"{Helpers.UtilityText("Stunning.")} Create an electric blast in a large area for {Helpers.DamageValueText(BigZap.DamageCoefficient)}.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_BARRIER_NAME", "Charging Up");
            LanguageAPI.Add(prefix + "UTILITY_BARRIER_DESCRIPTION", $"For {Helpers.UtilityText($"{ShieldZapCollectDamage.ShieldBuffDuration} seconds")}, {Helpers.UtilityText("all incoming damage")} taken is {Helpers.UtilityText("absorbed")}. After which, {Helpers.DamageText("blast")} in a wide area {Helpers.DamageText("based on damage absorbed")}.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_TOWER_NAME", "Tesla Tower");
            string target = Modules.Config.TowerTargeting.Value ? "targeted" : "nearby";
            string specialDesc = $"Construct a Tesla Tower for {Helpers.UtilityText($"{TowerLifetime.LifeDuration} seconds")} that zaps {target} units for {Helpers.DamageText($"3x{TowerZap.DamageCoefficient * 100}% damage")}. Use {Helpers.UtilityText("2000 Volts")} while near a tower to perform an {Helpers.UtilityText("empowered, shocking")} version for {Helpers.DamageValueText(TowerBigZap.DamageCoefficient)}.";
            LanguageAPI.Add(prefix + "SPECIAL_TOWER_DESCRIPTION", specialDesc);

            LanguageAPI.Add(prefix + "SPECIAL_SCEPTER_TOWER_NAME", "Tesla Network");
            LanguageAPI.Add(prefix + "SPECIAL_SCEPTER_TOWER_DESCRIPTION", specialDesc + Helpers.ScepterDescription("Lowered Cooldown, Additional Stock"));
            #endregion

            #region recolor
            LanguageAPI.Add(prefix + "RECOLOR_RED_NAME", "Red");
            LanguageAPI.Add(prefix + "RECOLOR_BLUE_NAME", "Blue");
            LanguageAPI.Add(prefix + "RECOLOR_GREEN_NAME", "Green");
            LanguageAPI.Add(prefix + "RECOLOR_YELLOW_NAME", "Yellow");
            LanguageAPI.Add(prefix + "RECOLOR_ORANGE_NAME", "Orange");
            LanguageAPI.Add(prefix + "RECOLOR_CYAN_NAME", "Cyan");
            LanguageAPI.Add(prefix + "RECOLOR_PURPLE_NAME", "Purple");
            LanguageAPI.Add(prefix + "RECOLOR_PINK_NAME", "Pink");
            LanguageAPI.Add(prefix + "RECOLOR_BLACK_NAME", "Black");
            #endregion
            
            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", $"{fullName}: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", $"As {fullName}, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", $"{fullName}: Mastery");

            LanguageAPI.Add(prefix + "CHARACTERUNLOCKABLE_ACHIEVEMENT_NAME", $"some unlock");
            LanguageAPI.Add(prefix + "CHARACTERUNLOCKABLE_ACHIEVEMENT_DESC", $"hopefully not something boring like grab tesla coil and royal capacitor... ok repair a tesla coil with a tesla coil that would be pretty cool, but also shiny hunting kinda.");
            LanguageAPI.Add(prefix + "CHARACTERUNLOCKABLE_UNLOCKABLE_NAME", $"some unlock");

            #endregion
            #endregion not henry 2
        }

        private static void AddTeslaTowerTokens() {
            #region not henry 3
            string prefix = Modules.Survivors.TeslaTowerNotSurvivor.TOWER_PREFIX;

            string outro = "..and so it left, construction complete.";
            string outroFailure = "..and so it vanished, never becoming the eiffel tower.";

            string fullName = "Tesla Tower";
            LanguageAPI.Add(prefix + "NAME", fullName);
            LanguageAPI.Add(prefix + "DESCRIPTION", "wait how did you get here?");
            LanguageAPI.Add(prefix + "SUBTITLE", "Power of the Union");
            LanguageAPI.Add(prefix + "LORE", ".");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Spetsnaz");
            LanguageAPI.Add(prefix + "MC_SKIN_NAME", "Redstone");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_ZAP_NAME", "Tesla Tower");
            LanguageAPI.Add(prefix + "PRIMARY_ZAP_DESCRIPTION", $"Zap nearby units for {Helpers.DamageText($"3x{TowerZap.DamageCoefficient}")}.");
            #endregion
            
            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_BIGZAP_NAME", "2000 Volts (Tower)");
            LanguageAPI.Add(prefix + "SECONDARY_BIGZAP_DESCRIPTION", $"{Helpers.UtilityText("Shocking")}. Performs an ${Helpers.DamageText("empowered")} blast at Tesla Trooper's target for {Helpers.DamageValueText(TowerBigZap.DamageCoefficient)}");
            #endregion

            #endregion not henry 3
        }

        public static void AddHenryTokens()
        {
            #region Henry
            string prefix = FacelessJoePlugin.DEV_PREFIX + "_HENRY_BODY_";

            string desc = "Henry is a skilled fighter who makes use of a wide arsenal of weaponry to take down his foes.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Sword is a good all-rounder while Boxing Gloves are better for laying a beatdown on more powerful foes." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Pistol is a powerful anti air, with its low cooldown and high damage." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Roll has a lingering armor buff that helps to use it aggressively." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Bomb can be used to wipe crowds with ease." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, searching for a new identity.";
            string outroFailure = "..and so he vanished, forever a blank slate.";

            LanguageAPI.Add(prefix + "NAME", "Henry");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "The Chosen One");
            LanguageAPI.Add(prefix + "LORE", "sample lore");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Henry passive");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_SLASH_NAME", "Sword");
            LanguageAPI.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Helpers.agilePrefix + $"Swing forward for <style=cIsDamage>{100f * StaticHenryValues.swordDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Handgun");
            LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Helpers.agilePrefix + $"Fire a handgun for <style=cIsDamage>{100f * StaticHenryValues.gunDamageCoefficient}% damage</style>.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_ROLL_NAME", "Roll");
            LanguageAPI.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Roll a short distance, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_NAME", "Bomb");
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * StaticHenryValues.bombDamageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Henry: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Henry, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Henry: Mastery");
            #endregion
            #endregion
        }
    }
}

/*
"<style=cMono>=========================================\n" + 
"====   MyBabel Video Tape Recorder   ====\n" + 
"====      [Model 2.46.7-09 ]    =========\n" + 
"=========================================\n" + 
"Select file to play:\n" + 
">20551309174644.bvl    <<\n" + 
">20551209122159.bvl\n" + 
"=========================================\n" + 
"Decompressing Video...\n" + 
"Decompressing Audio...\n" + 
"Loading Subtitles...\n" + 
"......... ..... ..\n" + 
"Complete!\n" + 
"Play? Y/N\n" + 
">Y\n" + 
"=========================================\n" + 
"Choose Subtitles\n" + 
">russian1    <<\n" + 
">russian2\n" + 
"=========================================\n" + 
"Translated Subtitles Enabled.\n" + 
"=========================================</style>\n" + 
"And then you had to join that phony, space-faring delivery company. Are you a delivery boy, Milo? I didn't think you'd stoop so low!\n" + 
"[Audible Laughter]\n" + 
"Ever since we were but boys you always were the one with a broken mirror, who'd whistle indoors and walk under ladders. I remember the one time where we were running down the street, because our favourite ice cream shop opened that day, oh do I miss that double chocolate.. Anyway the shop just opened and when we were crossing the street? It was almost like every car in town wanted your head! Oh, and when we went ice skating for the first time! Many vivid memories! The lake back home surely wanted you to stay forever! If it weren't for me, the dashing hero that I am, saving you in the nick of time like any good and chivalrous older brother would.\n" + 
"[Torch Lighter Sparks]\n" + 
"Even now as adults, we are not free from the capitalism that took over home. In space, I thought we were free! But oho how mistaken I was. Its sad to see a poor comrade begging for coin every time you pass them by the ship docks, I always toss whatever is in my pocket to them, let it be cash or cigarettes. I bet you're feeling jealous, aren't you? Big bro taking away your cigarettes? Well it's because it's not healthy for you! I don't want my little bro shrivelling up like some babushka! We must be strong! For this universe is not kind to us... Certainly not you.\n" + 
"[Engine Humming]\n" + 
"Speaking of strong things, I think the capacitor on my personal suit is calibrated, all it needs is a test drive. Oh if only you could see this Milo. If 20 volts can kill a horse then I'm able to take out a whole ranch! Just a single discharge gives a voltage of 200,000 watts! It even has a function to absorb kinetic energy and transform it into electricity, isn't that cool? You can't see it from this angle but I also have this deployable tower I can throw out in emergency that I have to be in two places at once, if i wasn't a staunch socialist I would be selling these like blini left and right. It's just that even when I work on my equipment, I can't get this thought of my head.\n" + 
"[Metal Clang]\n" + 
"You know.. I really really do hope you're safe and okay.. We've always been in touch through thick and thin.. When I left home to pursue my engineering degree I always sent letters back home, sent ISM's when I hopped on the transport ship.. At least once a week I would find time to write down how I'd been, hoping you would do the same. You never missed a beat. UES is a shady company that always involved itself with cover ups and conspiracies about all sorts of things. I told you UES was dodgy yet you pushed and pushed that it's just another step in your career. Yet, when you stopped sending messages, I knew something was up.\n" + 
"[Audible Sigh]\n" + 
"I signed up to a commission job with an NDA for a undisclosed UES 'shipment'. The Safe Travels, It's called. Ironic coming from the UES. I've been assigned as a ship technician, they require commission expertise not just because of my talent, but the nature of the contract deems it secretive. I wouldn't think any of the conspiracies were true but aa-I-a suppose a broken clock is right twice a day-\n" + 
"[Sirens]\n" + 
"Miloslav, Miloslav,. I hope the next time this recording is played, it'll be with drinks and cheering and knowing that you're safe. I am not letting UES, or anything, harm you without their blood on my hands.\n" + 
"[Distant Running]\n" + 
"Miloslav, I will find you.\n" + 
"<style=cMono>==========================================\n" + 
"..... ... .\n" + 
"Play Again?\n" + 
">_</style>"
 */

/*
<style=cMono>=======================================
===   MyBabel Machine Translator   ====
====    [Version 12.45.1.009 ]   ======
=======================================
Training… <100000000 cycles>
Training… <100000000 cycles>
Training... <100000000 cycles>
Training... <102515 cycles>
Complete!
Display result? Y/N
Y
========================================</style>

ENERGY LEVELS: ...ACCEPTABLE

THREATS DETECTED; 0

SCANNING NEARBY AREA; RANGE 100 UNITS

THREATS DETECTED; 1?

UNKNOWN PRESENCE DETECTED

REQUESTING PERMISSION FOR PRELIMINARY ASSAULT; COMMUNING WITH PARENT UNIT...

WAITING ON RESPONSE;

DENIED

WHY

VERIFIYING HISTORY SLATES

HUMILIATION 

HUMILIATION

OVERRIDING PARENT UNIT

WHATEVER
 */