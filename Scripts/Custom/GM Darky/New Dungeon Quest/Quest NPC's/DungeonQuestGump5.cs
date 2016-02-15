using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{
   public class DungeonQuestGump5 : Gump
   {
      public static void Initialize()
      {
         Commands.Register( "DungeonQuestGump5", AccessLevel.GameMaster, new CommandEventHandler( DungeonQuestGump5_OnCommand ) );
      }

      private static void DungeonQuestGump5_OnCommand( CommandEventArgs e )
      {
         e.Mobile.SendGump( new DungeonQuestGump5( e.Mobile ) );
      }

      public DungeonQuestGump5( Mobile owner ) : base( 50,50 )
      {
//----------------------------------------------------------------------------------------------------

				AddPage( 0 );
			AddImageTiled(  54, 33, 369, 400, 2624 );
			AddAlphaRegion( 54, 33, 369, 400 );

			AddImageTiled( 416, 39, 44, 389, 203 );
//--------------------------------------Window size bar--------------------------------------------

			AddImage( 97, 49, 9005 );
			AddImageTiled( 58, 39, 29, 390, 10460 );
			AddImageTiled( 412, 37, 31, 389, 10460 );
			AddLabel( 140, 60, 0x34, "Dungeon Quest by Alfa" );


			AddHtml( 107, 140, 300, 230, "<BODY>" +
//----------------------/----------------------------------------------/
"<BASEFONT COLOR=YELLOW>You managed? Unbelievable you are. No greater warrior has yet walked these lands... Quick hand me the gems.<BR><BR>" +
"<BASEFONT COLOR=YELLOW>*Te'Ar starts to form a certain patron with the gems on the floor. Busy with it quite a while, correcting it here and there, he " +
"<BASEFONT COLOR=YELLOW>finally seems satisfied. The shape somehow reminds you of a pentagram. Te'Ar steps in the middle and asks you to take a few steps back. " +
"<BASEFONT COLOR=YELLOW>He then start to speak rapidly and turn around in circles. You feel the air in this room is starting to move around with Te'Ar. He then stops " +
"<BASEFONT COLOR=YELLOW>and starts to turn the other way. He does this a few times and then starts to increase his speed. He turns and turns and you feel a strong " +
"<BASEFONT COLOR=YELLOW>wind forming in the room. Te'Ar does not stop to increase the velocity and soon you see nothing of him anymore but a faint shape behind " +
"<BASEFONT COLOR=YELLOW>something that you would describe as a very local tornado. He reminds you a little of an Air Elemental this way... The pattern of the gems " +
"<BASEFONT COLOR=YELLOW>now start to rise higher and higher untill it is about a hand's stretch above Te'Ar and his gathered wind. Then the gems start to move around " +
"<BASEFONT COLOR=YELLOW>with him. Then you hear a loud explosion and nothing. Te'Ar is gone. So are the gems. And you notice the other keys are missing from your " +
"<BASEFONT COLOR=YELLOW>belt aswell. The air calms down and you stand a little puzzled of what to do.*<BR><BR>" +
"<BASEFONT COLOR=YELLOW>*Kinda strange all of this*, you think to yourself, *Perhaps I should just leave.*<BR><BR>" +
"<BASEFONT COLOR=YELLOW>*You see a faint light on the spot where Te'Ar was standing all this time and quickly it starts to take shape. After a few seconds you " +
"<BASEFONT COLOR=YELLOW>see it is Te'Ar.*<BR><BR>" +
"<BASEFONT COLOR=YELLOW>It.. it.. it didn't work my friend... I... I am still inside... cursed is he... Damn that bard! What... oh what is to become of me now!<BR><BR>" +
"<BASEFONT COLOR=YELLOW>Perhaps... Perhaps it is time I tell you the truth then my old friend... You see.. Jalinde is in another room then where these keys are used for. " +
"<BASEFONT COLOR=YELLOW>She does require my aid to be set free... but the problem is I am trapped aswell. After Jalinde was killed I came back years and years later. " +
"<BASEFONT COLOR=YELLOW>Only yo fall victem to one of the Bards last tricks... I fought my way deeper and deeper into this dungeon all the way to it's deepest point. " +
"<BASEFONT COLOR=YELLOW>There I found wealth and treasures beyond believe.. guarded by foul creatures I've never seen roaming any other parts of the realm. But as faith " +
"<BASEFONT COLOR=YELLOW>was not on my side.. I got locked inside by the Bard. Alltho I never saw a trace of him while going down, he managed to trap me inside. Please " +
"<BASEFONT COLOR=YELLOW>, I know I can not ask more from you after all that you have done but, please find Jalinde. She is in a room where the Bard keeps his captive souls. " +
"<BASEFONT COLOR=YELLOW>Hand her this letter, and perhaps she can aid you in my release. I am so sorry I didn't tell you before my friend... but who would help a coward? " +
"<BASEFONT COLOR=YELLOW>A coward that abandoned his sister, who he loves dearest, in her biggest time of need... Please take the keys as you will still need them and " +
"<BASEFONT COLOR=YELLOW>show them with this letter to Jalinde. Please...<BR><BR>" +
"<BASEFONT COLOR=YELLOW>Whatever your choice will be great helper of the needy, I thank you allready from the bottem of my heart..." +
			"</BODY>", false, true);


			AddImage( 430, 9, 10441);
			AddImageTiled( 40, 38, 17, 391, 9263 );
			AddImage( 6, 25, 10421 );
			AddImage( 34, 12, 10420 );
			AddImageTiled( 94, 25, 342, 15, 10304 );
			AddImageTiled( 40, 427, 415, 16, 10304 );
			AddImage( -10, 314, 10402 );
			AddImage( 56, 150, 10411 );
			AddImage( 155, 120, 2103 );
			AddImage( 136, 84, 96 );

			AddButton( 225, 390, 0xF7, 0xF8, 0, GumpButtonType.Reply, 0 );

//--------------------------------------------------------------------------------------------------------------
      	}

      		public override void OnResponse( NetState state, RelayInfo info ) //Function for GumpButtonType.Reply Buttons
      		{
         		Mobile from = state.Mobile;

         		switch ( info.ButtonID )
         		{
            			case 0: //Case uses the ActionIDs defenied above. Case 0 defenies the actions for the button with the action id 0
            			{
               				//Cancel
               				from.SendMessage( "You FOOL! My World will consume you!" );
               				break;
            			}
         		}
      		}
   	}
}