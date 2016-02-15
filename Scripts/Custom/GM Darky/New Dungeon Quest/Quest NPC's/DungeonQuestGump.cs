using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{
   public class DungeonQuestGump : Gump
   {
      public static void Initialize()
      {
         Commands.Register( "DungeonQuestGump", AccessLevel.GameMaster, new CommandEventHandler( DungeonQuestGump_OnCommand ) );
      }

      private static void DungeonQuestGump_OnCommand( CommandEventArgs e )
      {
         e.Mobile.SendGump( new DungeonQuestGump( e.Mobile ) );
      }

      public DungeonQuestGump( Mobile owner ) : base( 50,50 )
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
"<BASEFONT COLOR=YELLOW><CENTER>Welcome to the Quest of Dungeon Silence!</CENTER><BR><BR>" +
"<BASEFONT COLOR=YELLOW>Greetings there... You look lost my friend. Perhaps I can tell you a story to help you find your way back?<BR>" +
"<BASEFONT COLOR=YELLOW>Stay a while and listen...<BR><BR>" +
"<BASEFONT COLOR=YELLOW>*As the ghost begins to speak you slowly start to feel drowsy, you are getting less and less keen of your environment. " +
"<BASEFONT COLOR=YELLOW>Somehow you know you should stay sharp in here... But the harmonious melody of the ghost voice and the passion of with which he speaks " +
"<BASEFONT COLOR=YELLOW>makes you slowly unaware of your surroundings. There is only the ghost... and his story...*<BR><BR>" +
"<BASEFONT COLOR=YELLOW>A long time ago... so long ago that the days turned into weeks, the weeks into years and the years into decades. Longer ago then you have lived " +
"<BASEFONT COLOR=YELLOW>and probably also longer ago then the birth of the father of your fathers father. This long ago a brother and a sister where an adventures " +
"<BASEFONT COLOR=YELLOW>couple. They wandered the lands without fear looking for action, treasure and adventures worth writing songs about.<BR><BR>" +
"<BASEFONT COLOR=YELLOW>Together they walked into dangerous adventures, discovering hidden treasure, fighting the foulest creatures of this world and, most of all, " +
"<BASEFONT COLOR=YELLOW>spending every precious waking moment together which they could find. As the brother loved his sister dearly.. so did the sister love " +
"<BASEFONT COLOR=YELLOW>the brother equally, if not more.<BR><BR>" +
"<BASEFONT COLOR=YELLOW>But as with all brave people, courage and stupidity are a devious pair, misfortune caught up with them...<BR><BR>" +
"<BASEFONT COLOR=YELLOW>One day the sister, Jalinde, found a secret passage leading to a dungeon they had never heard about on all their travels. Brave as they " +
"<BASEFONT COLOR=YELLOW>where they lighted up a torch and stepped foot inside. Inside here they found anomaly's they had never seen before. And they traveled " +
"<BASEFONT COLOR=YELLOW>over all the land and sea in all directions the wind blows. Wary of the unknown but drunk with the adrenaline rush for a new challenge " +
"<BASEFONT COLOR=YELLOW>they managed to fight their way in deeper and deeper.<BR><BR>" +
"<BASEFONT COLOR=YELLOW>Scavenging the dungeon for days and days, fighting horrible monsters and defeating them against all odds, they ran into the most unlikely " +
"<BASEFONT COLOR=YELLOW>person to be found in a dungeon such as these.<BR><BR>" +
"<BASEFONT COLOR=YELLOW>A lonely Bard was sitting on a piece of rock amidst hordes of monsters cheerfully playing his lute and singing with the most sweetest " +
"<BASEFONT COLOR=YELLOW>voice they had ever heared. As they approached him with caution the Bard summoned them closer and welcomed them to sit with him for a while.<BR><BR> " +
"<BASEFONT COLOR=YELLOW>'Greetings Te'Ar and Jalinde', spoke the Bard,'Welcome into my humble home, I call it Silence. And let me tell you why.' Te'Ar and Jalinde spoke " +
"<BASEFONT COLOR=YELLOW>with the Bard for many hours. Enjoying the hospitality and safety that his presence brought. The Bard told them how he came to this Dungeon " +
"<BASEFONT COLOR=YELLOW>already when he was a young boy. And how he managed to learn the songs that would even make the most dreaded monsters here slumber into silence. " +
"<BASEFONT COLOR=YELLOW>The Bard taught them how to survive in the dungeon and where the most challenging parts where to be found. Te'Ar and Jalinde returned many times " +
"<BASEFONT COLOR=YELLOW>to visit the Bard and explore more of Silence. And also they started to love it's mystic. But as most of these story's do not have a happy " +
"<BASEFONT COLOR=YELLOW>end I am afraid I can not be anything less but unoriginal. As faith was soon to struck down in all it's terrible fury on our siblings...<BR><BR>'" +
"<BASEFONT COLOR=YELLOW>It was the sixth day of the eleventh hour at the time of small moon when Te'Ar discovered a hidden door. Unable to open it they consulted the Bard. " +
"<BASEFONT COLOR=YELLOW>'Do <B>not</B> enter that room siblings!', screamed the Bard, 'As it is my home and I like no dwellers apart myself to visit it!' Shocked by " +
"<BASEFONT COLOR=YELLOW>his sudden outburst Te'Ar and Jalinde promised the Bard not to enter and left with a sour feeling inside. That evening Jalinde spoke to her " +
"<BASEFONT COLOR=YELLOW>brother:'Te'Ar, I think in there we will find all the answers we seek. We must find a way inside.' 'No my dearest sister', spoke Te'Ar, 'did you not " +
"<BASEFONT COLOR=YELLOW>see the Bard is the master of his domain and we are only allowed access at his demand? I sincerely feel we should not cross any man in his own" +
"<BASEFONT COLOR=YELLOW>home if one desires so.' Jalinda nodded... but that night she sneaked away from her brother... <BR><BR>" +
"<BASEFONT COLOR=YELLOW>After a while, without to much troubles, Jalinde found the hidden door again. And after trying to get it open for a while she finally discovered " +
"<BASEFONT COLOR=YELLOW>how to get in. What she saw no one knows. What she discovered, I think, we should not know. All I know is that the Bard found her. And slayed her. " +
"<BASEFONT COLOR=YELLOW>He then rushed to her brother and told him to leave at once. Never to return and never to show his face again or else he would find the same faith.<BR><BR>" +
"<BASEFONT COLOR=YELLOW>This, my friends, is the story and Jalinde and Te'Ar... What happened to Te'Ar you ask me? *Sighs* Te'Ar still try's to free his sister. " +
"<BASEFONT COLOR=YELLOW>Many years have passed since he fled away as a coward from the Bard. But now I am afraid his soul will not rest untill he free's his sister. " +
"<BASEFONT COLOR=YELLOW>Yes young one... I indeed am what remains of who was once Te'Ar. And I indeed am forced to linger here forever until I find a way to free my sister.<BR><BR>" +
"<BASEFONT COLOR=YELLOW>*Sighs* If only I had been brave when it mattered...<BR><BR> " +
"<BASEFONT COLOR=YELLOW>You would... You would like to aid me? I need a key to unlock the doors you see.. But in this spirit form I can not perform the tasks ahead... " +
"<BASEFONT COLOR=YELLOW>If truly you wish to aid me with this, and grant this old ghost the love of his sister again, then bring me ten pieces of Dark Iron Wire. " +
"<BASEFONT COLOR=YELLOW>The Wisps inside here hold them sometimes... Kill them and you will find what I need... But I have to warn you my friend...<BR><BR> " +
"<BASEFONT COLOR=YELLOW>This shall just be the beginning..." +
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
               				from.SendMessage( "Safe Travels" );
               				break;
            			}
         		}
      		}
   	}
}