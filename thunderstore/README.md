![QoL Logo](https://github.com/p1xel8ted/Cult-of-the-Lamb-Mod-Collection/blob/e197c9e251b87b5f2602deec890eacbcad7a4c53/thunderstore/icon.png?raw=true)

# Cult of QoL - A Collection

* Removes intros.
* Cheese the fishing mini-game. (No need to press anything apart from the initial cast).
* Remove extra buttons from menus (Discord, bug report, Twitch etc.)
* Collect tithes/inspire all at once (unless they're in prison, sleeping, using the bathroom or dissenting.)
* Reverse the damage change to the golden fleece introduced in 1.0.13.
* Double the rate the damage increases with the golden fleece.
* Halves (where possible) the cost of refining goods. Gets rounded up. i.e. something that costs 3 originally ends up being 2. Something that costs 10, becomes 5.
* Cure illness/exhaustion when a follower gains loyalty (the floaty white eyes thing).
* Unlock pre-order DLC and Twitch items (Plushie building, drops)
* You can now collect tithe from the oldies again. Brutal.
* Increase silo/fertilizer capacity to 32.
* Increase collection speed from certain structures (shrines, beds, chest near main portal, windmill(?))
* Turn off propaganda speakers of a night. Despite the flame being on, they don't use fuel. Working on the flame part.
* Make the days longer. The default is 2x, and that already feels long.
* Manipulate game speed, or halt it. Not to be confused with day length, this makes EVERYTHING faster.). Left/Right arrow keys to change, default speed is the up key.
* Disable game over mechanic.
* Multiple tarot rare draw chance multiplier by 3 (goes from around 20% to 60%).
* Lumber/stone mines life span can be increased by 50%, 100% or unlimited.

Each mod can be enabled/disabled individually from within the config. There might be stuff I've missed.

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

## Known Issues

* Speeding up time can break menus (doubling up), and transitions (stuck behind barrier for example.) - You need to do a full game restart to correct it.
* Doubling shrine capacitiy isnt working on main shrines.

## Changelog

### 1.8.0 - 11/09/2022

* Lumberjack stations now have their loot speed increased when collecting from the chest. Its not totally 0, because it makes the animations look like junk.
* Added option for chests to make them no longer require the button to be held down.
* Added option for chests to enable them to automatically give you the loot when you're nearby.


### 1.7.0 - 10/09/2022

* Added option to multiply your tarot card draw luck by 3. This doesnt mean every draw will be a rare+, you just have a higher chance of drawing one (goes from around 20% to 60%).
* Added option to disable the game over mechanic (user request).
* Added option to increase lifespan of lumber/mines by 50%, as 100% felt way too long.
* After being inspired with inspire all, any remaining glowy eyed followers should now level as well.

### 1.6.0 - 05/09/2022

* Added 0 to the speed choices, halts everything and becomes 2d photo mode effectively.
* Added ability to extend the day length.
* Added the lighthouse "shrine" and the lonely shack shrine to the fast collect structures.
* Added checks to ensure we're not inspiring/extorting when sleeping/dissenting/bathroom/prison.
* Inspire All "should" be fixed now.
* Added option to double lumber/mine stations age instead of only lasting forever.

* Removed anything outhouse related as they're being put into a seperate outhouse specific mod.

* WIP - Option to double the storage capacity of shrines (not quite done, the main one ignores it for some reason).

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
