![QoL Logo](https://github.com/p1xel8ted/Cult-of-the-Lamb-Mod-Collection/blob/e197c9e251b87b5f2602deec890eacbcad7a4c53/thunderstore/icon.png?raw=true)

# Cult of QoL - A Collection

* Removes intros.
* Cheese the fishing mini-game. (No need to press anything apart from the initial cast).
* Remove extra buttons from menus (Discord, bug report, Twitch etc.)
* Collect tithes/inspire all at once. Inspire has some quirks I'm working on (the main one being when a follower levels up(?), it comes from the follower you initiated the conversation with, not the follower that's levelling up.)
* Reverse the damage change to the golden fleece introduced in 1.0.13.
* Double the rate the damage increases with the golden fleece.
* Halves (where possible) the cost of refining goods. Gets rounded up. i.e. something that costs 3 originally ends up being 2. Something that costs 10, becomes 5.
* Cure illness/exhaustion when a follower gains loyalty (the floaty white eyes thing).
* If you have multiple outhouses, followers should now go to the "less full" one more often than not.
* Unlock pre-order DLC and Twitch items (Plushie building, drops)
* You can now collect tithe from the oldies again. Brutal.
* Increase silo/fertilizer capacity to 32.
* Increase collection speed from certain structures (shrines, beds, outhouse, chest near main portal, windmill(?))

Each mod can be enabled/disabled individually from within the config.

## Installation

* Install [BepInExPack CultOfTheLamb](https://cult-of-the-lamb.thunderstore.io/package/BepInEx/BepInExPack_CultOfTheLamb/)
* Place the plugin DLL (CultOfTheLambMods.dll) into your "...\Cult of the Lamb\BepInEx\plugins" folder.

## Configuration

The configuration file is generated when you first run the game with the mod enabled. It can be found in the "...\Cult of the Lamb\BepInEx\config" folder.

## Issues, questions, etc.

Feel free to reach out to me on the channel below.

* [Cult of the Lamb Modding Discord](https://discord.gg/R73vhh8Q2F)

## Contributors

* p1xel8ted
* Matthew-X

## Changelog

### 1.5.0 - 02/09/2022

* Collection delays from the main shrine (credits to Matthew-X), the smaller shrines, the chest near the portal, the outhouses and the beds have been reduced to zero, otherwise shortened (otherwise animations look like junk).
* The seed and fertilizer silos can be expanded to 32 slots, which is the number of plots you can build around them. (credits to Matthew-X).
* Further adjustments to Inspire All - I have not found a consistent way to reproduce the issue. If it occurs (stand there dancing), save, exit to the menu and go back in.

### 1.4.0

* Made some adjustments to the outhouse stuff, see how it goes. Still a WIP.
* Added game speed manipulation. Left/Right arrows to increase/decrease in 0.25 increments. Up arrow resets it to default. Maximum (artifical cap) is 5, and good luck to you playing it at that speed.
* Improved the elderly extortion patch. Should now grey out when they've already been extorted, and only appear once you've unlocked that doctrine.

### 1.3.1

* Fixed the Twitch patches causing the game to lock you onto the portal platform, thingy.
* Modified golden fleece patch to be inline with changes from 1.0.13.

### 1.3

* Added ability to unlock Twitch items (will unlock on game load)
* Added being able to collect tithe from the oldies. Brutal.
* Added lumber/stone mine immortality.
* Added being able to extort the elderly. Brutal.

### 1.2

* Inspire all is somewhat fixed. Instead of being stuck sometimes, they'll move on to their next task.
* Added option to cure illness/exhaustion when a follower gains loyalty (the floaty white eyes thing).
* Fixed refinery config not being there.
* More

### 1.1

* Collect tithes/inspire all at once. Inspire has some quirks I'm working on.
* Reverse the 200% golden fleece cap.
* Double the rate the damage increases with the golden fleece.
* Halves (where possible) the cost of refining goods. Gets rounded up.

### 1.0 - Initial release

* Remove intros
* Cheese fishing min-game
* Remove button clutter (Discord, Bugs, Twitch etc..)
