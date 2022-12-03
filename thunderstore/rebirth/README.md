![Rebirth Logo](https://raw.githubusercontent.com/p1xel8ted/Cult-of-the-Lamb-Mod-Collection/dd8e32aed18a1d789ff8e24e59a21f786620bc94/thunderstore/rebirth/icon.png?raw=true)

# Rebirth

* Followers can be reborn, allowing you to change their names, looks etc.
* First one is free; subsequent ones on the same follower require 25 Rebirth Tokens obtained from crusading. 
* * Source of the tokens is from killing enemies, destroying old bone piles (not the ones from enemies), dungeon chests and offering shrines and more.
* * The drop chance starts at 15% and scales with your dungeon luck modifier. 
* The followers' XP carries over, but there is a 10% chance they lose half of their XP and 1 level when reborn. There is a notification when this happens.
* Their old body stays; compost it, eat it. Up to you.
* Refine the tokens at your refineries for 15 bones!
* Send your followers on a mission to get tokens!
* Go on a quest yourself to get tokens!

## Donate

[![KoFiLogo](https://raw.githubusercontent.com/p1xel8ted/GYK-Mods-QMod/main/kofi-nexus-smaller.png)](https://ko-fi.com/p1xel8ted)

## Installation

* Install [BepInExPack CultOfTheLamb](https://cult-of-the-lamb.thunderstore.io/package/BepInEx/BepInExPack_CultOfTheLamb/)
* Install [COTL API](https://cult-of-the-lamb.thunderstore.io/package/xhayper/COTL_API/)
* Extract the contents of Rebirth & COTL API into your BepInEx folder "...\Cult of the Lamb\BepInEx\" folder.

If done correctly, you should have a Rebirth folder inside your plugins folder alongside the API DLL and its Assets folder.

## Configuration

Apart from On/Off, there is no configuration required.

## Issues, questions, etc.

Feel free to reach out to me on the channel below.

* [Cult of the Lamb Modding Discord](https://discord.gg/R73vhh8Q2F)

## Changelog

### 1.0.2 - 3/12/2022 - You must upate COTL_API to 0.1.12!

* Added support for changing config options from within the Cult of the Lamb Mod settings menu.

### 1.0.1 - 25/11/2022 - You must upate COTL_API to 0.1.10!

* Compatibility update for COTL API 0.1.10
* Added ability to rebirth elderly followers. Default is off. Can be changed in the config file.

### 1.0.0 - 17/10/2022 - You must upate COTL_API to 0.1.6!

* Moved the token from the common to rare pool of the offering shrine.
* Added token as a refinery item to the refinery. 15 bones for 1 token.
* Added a token mission to the Missionary structure.
* Re-activated the custom quest. Will be expanded a little (different zones) provided there is no further issues.
* Fixed followers being reset to level 1 on Rebirth. They now keep their level and XP provided they're lucky enough to not trigger the 10% chance of losing half their XP and 1 level.

### 0.1.3 - 25/09/2022 - You must upate COTL_API to 0.1.4!

* Disabled the quest until I get a chance to look at what broke (if the follower comes to you with it, it softlocks. Pressing ESC/B (on controller) appears to resolve it.)

### 0.1.201 - 25/09/2022

* Dependency update. Enforcing a version means I get less "it's not working messages".

### 0.1.1 - 25/09/2022

* The token now spawns as an actual item.
* The token can appear in the offering shrines.
* Each time you start the game it will add a quest to go collect a random amount (between 15 and 25) of them from one of the dungeons you have completed.

### 0.1.0 - 10/09/2022

* Initial release
