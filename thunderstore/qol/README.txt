## Changelog

## 1.9.1 - 2/10/2022

* Updated for game version 1.0.17.
* Unlocked Twitch skins will no longer be removed if you disable the option to unlock them.
* Another attempt at fixing propoganda speakers being turned off during the day for a whole phase.

## 1.9.0 - 19/09/2022

* Updated for game version 1.0.16. They removed the Debug button from the main menu, so I've removed the code that removes it.
* When the propaganda speakers turn off of at night, their fire animation will now also turn off.
* Implemented a potential fix for speakers not turning back on.
* Added ability to replaced follower necklaces. The one they're wearing will drop to the ground.
* Added ability to receive notifications when items that use fuel run out.
* Followers who can level after Inspire All should now level. Will be added to others when I implement them.

## 1.8.1 - 13/09/2022

* Fixed the follower menu breaking in some instances with the healing bay mod on.

### 1.8.0 - 11/09/2022 - Some options have moved categories, I suggest a clean config file.

* Lumberjack stations now have their loot speed delay lowered when collecting from the chest. Its not totally 0 (its 0.01), because it makes the animations look like junk.
* Added option for chests to enable them to automatically give you the loot when you're nearby. 
* Added option to double the distance required before the loot is sent your way.
* Added option to set a trigger limit before loot is automatically retreived.
* Added the ability to "heal" exhausted followers in the healing bays.
* Added option to recieve a notification when one of the scarecrows catches a bird.
* Fixed the passive and main shrines from not having their capacity doubled.

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
* Added option to turn off propaganda speakers of a night. The fuel animation still runs for now but doesn't consume fuel.

* Removed anything outhouse related as they're being put into a separate outhouse-specific mod.

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
