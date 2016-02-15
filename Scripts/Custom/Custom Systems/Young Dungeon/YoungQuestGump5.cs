using System; using Server; using Server.Scripts.Commands;using Server.Gumps; using Server.Network;using Server.Items;using Server.Mobiles;namespace Server.Gumps
{ public class YoungQuestGump5 : Gump {
public static void Initialize() {
Server.Commands.Register( "YoungQuestGump5", AccessLevel.GameMaster, new CommandEventHandler( YoungQuestGump5_OnCommand ) );
}
private static void YoungQuestGump5_OnCommand( CommandEventArgs e )
{
e.Mobile.SendGump( new YoungQuestGump5( e.Mobile ) ); }
public YoungQuestGump5( Mobile owner ) : base( 50,50 )
{
//----------------------------------------------------------------------------------------------------
AddPage( 0 );AddImageTiled(  54, 33, 369, 400, 2624 );AddAlphaRegion( 54, 33, 369, 400 );AddImageTiled( 416, 39, 44, 389, 203 );
//--------------------------------------Window size bar--------------------------------------------
AddImage( 97, 49, 9005 );AddImageTiled( 58, 39, 29, 390, 10460 );AddImageTiled( 412, 37, 31, 389, 10460 );
AddLabel( 140, 60, 0x34, "Quest Offer" );
//----------------------/----------------------------------------------/
AddHtml( 107, 140, 300, 230, " < BODY > " +
"<BASEFONT COLOR=YELLOW>With a big smile upon his face, <BR>" +
"<BASEFONT COLOR=YELLOW>the wierd man greets you, while holding<BR>" +
"<BASEFONT COLOR=YELLOW>a cake in his hand.<BR>" +
"<BASEFONT COLOR=YELLOW><BR>" +
"<BASEFONT COLOR=YELLOW>'I knew it! Just like I expected... <BR>" +
"<BASEFONT COLOR=YELLOW>Quickly hand over the purple gems!'<BR>" +
"<BASEFONT COLOR=YELLOW>With a single stroke, he combined 15<BR>" +
"<BASEFONT COLOR=YELLOW>different colored gems.<BR>" +
"<BASEFONT COLOR=YELLOW>I can clearly see now a shiny little <BR>" +
"<BASEFONT COLOR=YELLOW>object in his hand.<BR>" +
"<BASEFONT COLOR=YELLOW><BR>" +
"<BASEFONT COLOR=YELLOW>'Here take this my friend, this deed<BR>"+
"<BASEFONT COLOR=YELLOW>is your reward. Explore it to receive<BR>" +
"<BASEFONT COLOR=YELLOW>goodies. Safe Travels young one!'<BR>" +
"</BODY>", false, true);
//----------------------/----------------------------------------------/
AddImage( 430, 9, 10441);AddImageTiled( 40, 38, 17, 391, 9263 );AddImage( 6, 25, 10421 );AddImage( 34, 12, 10420 );AddImageTiled( 94, 25, 342, 15, 10304 );AddImageTiled( 40, 427, 415, 16, 10304 );AddImage( -10, 314, 10402 );AddImage( 56, 150, 10411 );AddImage( 155, 120, 2103 );AddImage( 136, 84, 96 );AddButton( 225, 390, 0xF7, 0xF8, 0, GumpButtonType.Reply, 0 ); }
//----------------------/----------------------------------------------/
public override void OnResponse( NetState state, RelayInfo info ){ Mobile from = state.Mobile; switch ( info.ButtonID ) { case 0:{ break; }}}}}