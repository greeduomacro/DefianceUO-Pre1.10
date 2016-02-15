Update on 8/11/04:
This is a small clean up & update for the CTF and DD games.  If you already have CTF/DD on your shard you will NOT need to resetup your games to use the new version of the scripts.  The only thing you must do after unextracting is re-add all of your signup stones (and relink them to the game stone).  Please delete DDJoin.cs, CTFJoin.cs, and CTFJoinGump.cs from previous isntallations.

What changed? The game no longer infinitely restarts itself.  The game now runs once for it's set length and then stops.  If someone reaches the maxscore before the game time is over, it will end early.  You can use [startgame and [stopgame (see below) to control the game as you see fit.  When the game is not running, players will be unable to fight (or heal) each other, and none of the flags/waypoints will work.
Also, Notoriety.cs is now much less cluttered with CTF stuff.  There are only 2 places, (both are clearly marked) where CTF stuff is in there.  There is a comment in CTFGame.cs where you can add code to give prizes to members of the winning team.

**** Installation ****
Unextract to your scripts folder. Then make SURE you delete your "Scripts/Misc/Notoriety.cs" file. This is very important....
Follow the instructions below to setup your game.

**** Setup Instuctions ****
Okay, this is a fairly large system.. there's a lot of little thigns and you'll want to play around with it before dropping it right on to your shard... Here is a "crash" course in how to set it up.

Before setting up the script on the shard, you may want to change the Coords in the "GameRegion.cs" script. The coords in there now are some nice places me & alkiser found a while ago on the default map... but you might want to use something else.

Capture the Flag:
1) [add CTFGame put it someplace out of the way.. this is used for controling game options only, players dont need to see it.
2) [props or double click the game stone and setup the properties of the game (like how many teams, etc).
3) [add CTFFlag, this is the flag so it should go someplace in your "arena"
4) Double click or do [props on the Flag, you will get a props menu... FIRST Set the TeamID to the Team Number (0, 1, 2, 3) etc you want the team to have. (This should never be >= the number of teams in the game)
5) Next set the Flag's Game property to the game stone you added in step 1.
6) Set all the other properties how you want, Note:set the flag's Hue to the color you want the team to wear (a blue flag means everyone on the team will wear a blue robe).

*) Repeat steps 3-6 for each team

Now you may [add gamejoinstone all around your shard to allow players to join the game. These must also be linked via props to the game stone from step 1.
You may also [add ctfscoreboard all over your shard (and dont forget to link them to the gamestone) so your players can see the scores. You might also want to add one in the arena.

Double Donimation:
First off, what is double domination?
This is a game I originally saw in Unreal on the Xbox. Basically 2 teams fight to control 2 way points at the same time, to control the point they must stand on it for 10 seconds without the other team touching it. Once they control both points at the same time for 30 seconds, they "score" and then the game resets.
It's very fast paced, but if teams are too large it can get out of hand.

The setup for double domination is pretty similar to CTF, with a few exceptions:

1) For each team you need to add a DDTeamControl (similar to the gamestone this will not be used by players) to control the DDTeams since they dont have flags. Otherwise, it acts the same as a ctfflag (including hue...).
2) You must add 2 DDWayPoints in your arena. DO NOT FORGET TO DO [inc z 5 on them so you can see the whole thing (its rather large). And you of course have to link them to your CTFGame (yes its still a CTFGame even though is DoubleDom)... These points must also be linked to each other. Your DD Game can have as many teams as you want, but it can only have 2 way points.

You should also be able to use the CTFScoreBoard for DD games.

Also included in this package is an "AutoSupply Stone" which when double clicked gives a player items based on their skills, its designed to make it so when a player dies he/she can re-equip very quickly.

Commands:
[endgame : (target ganestone) will end the current game
[startgame <reset> : will start the game, or if the game is already running, will restart it.  The paramemter reset should be true (or yes) if you want everyoen to reselect their teams, or false (or no) to keep the current teams.  [startgame true to reset/rebalance teams.
[t and [team : These commands allow players to talk to the entire team ( no matter where they are )

New to this package is the LeaveGameGate. [add leavegamegate Do not forget to [props and set the target location & map for the gate to take players. When stepped on the gate will remove the player for any/all games they are currently in, remove their game robe, and teleport them to the desired location. 

When you die in a CTF or DD Game you are resurrected automatically and taken back to your team's "home." A good straegy for this is to make the home a place somewhere outside of the battle area where the player can re-equip in peace, and then go through a gate to enter the game. This also prevents people from restocking on the stones when they didn't die.

Also this is provided AS IS, I am usually too busy to provide support for it. Please don't try to contact about it. There is a thread for it on runuo.com script submissions where you can seek support.

Thanks, enjoy.
~Zippy