using System; using Server; using Server.Scripts.Commands;using Server.Gumps; using Server.Network;using Server.Items;using Server.Mobiles;namespace Server.Gumps
{ public class YoungQuestGump2 : Gump {
public static void Initialize() {
Server.Commands.Register( "YoungQuestGump2", AccessLevel.GameMaster, new CommandEventHandler( YoungQuestGump2_OnCommand ) );
}
private static void YoungQuestGump2_OnCommand( CommandEventArgs e )
{
e.Mobile.SendGump( new YoungQuestGump2( e.Mobile ) ); }
public YoungQuestGump2( Mobile owner ) : base( 50,50 )
{
//----------------------------------------------------------------------------------------------------
AddPage( 0 );AddImageTiled(  54, 33, 369, 400, 2624 );AddAlphaRegion( 54, 33, 369, 400 );AddImageTiled( 416, 39, 44, 389, 203 );
//--------------------------------------Window size bar--------------------------------------------
AddImage( 97, 49, 9005 );AddImageTiled( 58, 39, 29, 390, 10460 );AddImageTiled( 412, 37, 31, 389, 10460 );
AddLabel( 140, 60, 0x34, "Quest Offer" );
//----------------------/----------------------------------------------/
AddHtml( 107, 140, 300, 230, " < BODY > " +
"<BASEFONT COLOR=YELLOW>Hello again young one.<BR>" +
"<BASEFONT COLOR=YELLOW><BR>" +
"<BASEFONT COLOR=YELLOW>Oh you brought me the Blue Gems!<BR>" +
"<BASEFONT COLOR=YELLOW>You have done a wonderful job, but <BR>" +
"<BASEFONT COLOR=YELLOW>unfortunately I need more now.<BR>" +
"<BASEFONT COLOR=YELLOW><BR>" +
"<BASEFONT COLOR=YELLOW>Here, have this key and access the next<BR>" +
"<BASEFONT COLOR=YELLOW>area. Try to gather 5 red balls of <BR>" +
"<BASEFONT COLOR=YELLOW>knowledge from the monsters living <BR>" +
"<BASEFONT COLOR=YELLOW>there and hand them back to me.<BR>" +
"<BASEFONT COLOR=YELLOW><BR>" +
"<BASEFONT COLOR=YELLOW>Good Luck!<BR>" +
"<BASEFONT COLOR=YELLOW><BR>" +
"<BASEFONT COLOR=YELLOW><BR>" +
"</BODY>", false, true);
//----------------------/----------------------------------------------/
AddImage( 430, 9, 10441);AddImageTiled( 40, 38, 17, 391, 9263 );AddImage( 6, 25, 10421 );AddImage( 34, 12, 10420 );AddImageTiled( 94, 25, 342, 15, 10304 );AddImageTiled( 40, 427, 415, 16, 10304 );AddImage( -10, 314, 10402 );AddImage( 56, 150, 10411 );AddImage( 155, 120, 2103 );AddImage( 136, 84, 96 );AddButton( 225, 390, 0xF7, 0xF8, 0, GumpButtonType.Reply, 0 ); }
//----------------------/----------------------------------------------/
public override void OnResponse( NetState state, RelayInfo info ){ Mobile from = state.Mobile; switch ( info.ButtonID ) { case 0:{ break; }}}}}