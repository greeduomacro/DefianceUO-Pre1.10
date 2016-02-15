using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using Server.Gumps;
using Server.Prompts;

namespace Server.Mobiles
{

public class ReplyNPC : BaseCreature
{

private ArrayList m_TriggerList;
private int m_ListenRange;
private string m_SpamMsg;
private bool m_SpamActive;
private bool m_SpamOnEnterRange;
private double m_MinimumSpamDelay;
private double m_MaximumSpamDelay;

[CommandProperty(AccessLevel.GameMaster)]
public int ListenRange
{
get { return m_ListenRange; }
set { m_ListenRange = value; }
}

[CommandProperty(AccessLevel.GameMaster)]
public string SpamMsg
{
get { return m_SpamMsg; }
set { m_SpamMsg = value; }
}

[CommandProperty(AccessLevel.GameMaster)]
public bool SpamActive
{
get { return m_SpamActive; }
set
{
if (!m_SpamActive)
new SpamTimer(this).Start();
m_SpamActive = value;
}
}
[CommandProperty(AccessLevel.GameMaster)]
public bool SpamOnEnterRange
{
get { return m_SpamOnEnterRange; }
set { m_SpamOnEnterRange = value; }
}

[CommandProperty(AccessLevel.GameMaster)]
public double MinimumSpamDelay
{
get { return m_MinimumSpamDelay; }
set { m_MinimumSpamDelay = value; }
}

[CommandProperty(AccessLevel.GameMaster)]
public double MaximumSpamDelay
{
get { return m_MaximumSpamDelay; }
set { m_MaximumSpamDelay = value; }
}

public ArrayList TriggerList
{
get { return m_TriggerList; }
set { m_TriggerList = value; }
}

[Constructable]
public ReplyNPC() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
{

m_TriggerList = new ArrayList();

SpeechHue = Utility.RandomDyedHue();
Hue = Utility.RandomSkinHue();

if (this.Female = Utility.RandomBool())
{
Body = 0x191;
Name = NameList.RandomName("female");
}
else
{
Body = 0x190;
Name = NameList.RandomName("male");
AddItem(new ShortPants(Utility.RandomNeutralHue()));
}

Item hair = new Item(Utility.RandomList(0x203B, 0x2049, 0x2048, 0x204A));
hair.Hue = Utility.RandomNeutralHue();
hair.Layer = Layer.Hair;
hair.Movable = false;
AddItem(hair);

if (Utility.RandomBool() && !this.Female)
{
Item beard = new Item(Utility.RandomList(0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D));

beard.Hue = hair.Hue;
beard.Layer = Layer.FacialHair;
beard.Movable = false;

AddItem(beard);
}

SetStr(61, 75);
SetDex(81, 95);
SetInt(86, 100);

SetDamage(10, 23);

SetSkill(SkillName.EvalInt, 100.0, 125);
SetSkill(SkillName.Magery, 100, 125);
SetSkill(SkillName.Meditation, 100, 125);
SetSkill(SkillName.MagicResist, 100, 125);
SetSkill(SkillName.Tactics, 100, 125);
SetSkill(SkillName.Macing, 100, 125);

Fame = 100;
Karma = 100;

AddItem(new Shoes(Utility.RandomNeutralHue()));
AddItem(new Shirt());

AddItem(new Robe(Utility.RandomNeutralHue()));
AddItem(new ThighBoots());

AI = AIType.AI_Vendor;
}

private class TriggerWord
{
private string m_Word;
private string m_Response;
private bool m_CaseSensitive;

public string Word
{
get { return m_Word; }
set { m_Word = value; }
}
public string Response
{
get { return m_Response; }
set { m_Response = value; }
}

public bool CaseSensitive
{
get { return m_CaseSensitive; }
set { m_CaseSensitive = value; }
}

public TriggerWord(string word, string response, bool caseSensitive)
{
this.m_Word = word;
this.m_Response = response;
this.m_CaseSensitive = caseSensitive;
}

public TriggerWord Clone()
{
return new TriggerWord(this.m_Word, this.m_Response, this.m_CaseSensitive);
}
}

public override void OnSpeech(SpeechEventArgs e)
{
Mobile from = e.Mobile;

if (!e.Handled && from is PlayerMobile && from.InRange(this.Location, m_ListenRange))
{
string speech, word;
foreach (TriggerWord tw in m_TriggerList)
{
speech = e.Speech;
word = tw.Word;

if(!tw.CaseSensitive)
{
speech = speech.ToLower();
word = word.ToLower();
}

if (speech.IndexOf(word) != -1)
{
RevealingAction();
SayTo(from, tw.Response);
break;
}
}
e.Handled = true;
}
base.OnSpeech(e);
}

public override void OnDoubleClick(Mobile from)
{
if (from is PlayerMobile && from.AccessLevel >= AccessLevel.GameMaster)
{
from.SendGump(new ReplyNPCGump(0, 100, 100, m_TriggerList, this));
}
}

public override void OnMovement(Mobile m, Point3D oldLocation)
{
if (m.Player && m.Alive && InRange(m, m_ListenRange) && !InRange(oldLocation, m_ListenRange) && InLOS(m) && m_SpamOnEnterRange)
{
Direction = GetDirectionTo(m);
RevealingAction();
SayTo(m, m_SpamMsg);
}
}

public ReplyNPC(Serial serial) : base( serial )
{
}

public override void Serialize(GenericWriter writer)
{
base.Serialize(writer);
writer.Write((int)1); // version

// Version 1 added the CaseSensitive member

int count = m_TriggerList.Count;

writer.Write(count);
for (int i = 0; i < count; i++)
{
TriggerWord tw = (TriggerWord)m_TriggerList[i];
writer.Write(tw.Word);
writer.Write(tw.Response);
writer.Write(tw.CaseSensitive.ToString());
}


writer.Write(m_ListenRange);
writer.Write(m_SpamMsg);
writer.Write(m_SpamActive);
writer.Write(m_SpamOnEnterRange);
writer.Write(m_MinimumSpamDelay);
writer.Write(m_MaximumSpamDelay);

}

public override void Deserialize(GenericReader reader)
{
base.Deserialize(reader);

m_TriggerList = new ArrayList();
int version = reader.ReadInt();

int count = reader.ReadInt();
for (int i = 0; i < count; i++)
{
string word = reader.ReadString();
string response = reader.ReadString();

bool caseSensitive;

if(version >= 1)
caseSensitive = Convert.ToBoolean(reader.ReadString());
else
caseSensitive = false;

m_TriggerList.Add(new TriggerWord(word, response, caseSensitive));
}
m_ListenRange = reader.ReadInt();
m_SpamMsg = reader.ReadString();
m_SpamActive = reader.ReadBool();
m_SpamOnEnterRange = reader.ReadBool();
m_MinimumSpamDelay = reader.ReadDouble();
m_MaximumSpamDelay = reader.ReadDouble();

if (m_SpamActive)
new SpamTimer(this).Start();
}
private class SpamTimer : Timer
{
private ReplyNPC m_Owner;

public SpamTimer(ReplyNPC owner) : base( TimeSpan.FromSeconds( owner.MinimumSpamDelay ), TimeSpan.FromSeconds( owner.MaximumSpamDelay ) )
{
this.m_Owner = owner;
}

protected override void OnTick()
{
if (m_Owner.SpamActive)
{
m_Owner.Say(m_Owner.SpamMsg);
}
else
{
this.Stop();
}
}
}

private class TriggerWordPrompt : Prompt
{
private ArrayList m_TriggerList;
private ReplyNPC m_Owner;

public TriggerWordPrompt(ArrayList triggerList, ReplyNPC owner)
{
this.m_TriggerList = triggerList;
this.m_Owner = owner;
}

public override void OnResponse(Mobile from, string text)
{
from.SendMessage("Type in the response");
from.Prompt = new ResponsePrompt(m_TriggerList, m_Owner, text);
}

public override void OnCancel(Mobile from)
{
from.SendGump(new ReplyNPCGump(0, 100, 100, m_TriggerList, m_Owner));
}
}

private class ResponsePrompt : Prompt
{
private ArrayList m_TriggerList;
private ReplyNPC m_Owner;
private string m_Word;

public ResponsePrompt(ArrayList triggerList, ReplyNPC owner, string word)
{
this.m_TriggerList = triggerList;
this.m_Owner = owner;
this.m_Word = word;
}

public override void OnResponse(Mobile from, string text)
{
TriggerWord tw = new TriggerWord(m_Word, text, false);
m_TriggerList.Add(tw);
from.SendGump(new ReplyNPCGump(0, 100, 100, m_TriggerList, m_Owner));
}

public override void OnCancel(Mobile from)
{
from.SendGump(new ReplyNPCGump(0, 100, 100, m_TriggerList, m_Owner));
}
}

private class ReplyNPCGump : Gump
{
private int m_Page;
private ArrayList m_TriggerList;
private ReplyNPC m_Owner;

public ReplyNPCGump(int page, int x, int y, ArrayList triggerList, ReplyNPC owner) : base( x, y )
{
this.m_Page = page;

this.m_TriggerList = new ArrayList();

// We want to clone each TriggerWord so that when someone cancels, the values do not save
for(int i = 0; i < triggerList.Count; i++)
{
this.m_TriggerList.Add((TriggerWord)(((TriggerWord)triggerList[i]).Clone()));
}

this.m_Owner = owner;

this.Closable = true;
this.Disposable = true;
this.Dragable = true;
this.Resizable = false;
this.AddPage(0);

this.AddBackground(0, 0, 400, 400, 9380);
if (m_Page > 0)
{
this.AddLabel(60, 375, 1149, @"Prev page");
this.AddButton(40, 380, 9706, 9707, (int)Buttons.Previous, GumpButtonType.Reply, 0);
}
if (m_TriggerList.Count > m_Page * 10 + 10)
{
this.AddLabel(270, 375, 1149, @"Next page");
this.AddButton(330, 380, 9702, 9703, (int)Buttons.Next, GumpButtonType.Reply, 0);
}

this.AddLabel(40, 40, 1259, @"Reply NPC Configure Gump");

this.AddLabel(100, 60, 1259, @"Add New Entry:");
this.AddButton(220, 60, 4014, 4015, (int)Buttons.Add, GumpButtonType.Reply, 0);

this.AddLabel(40, 80, 1259, @"Trigger Word:");
this.AddLabel(140, 80, 1259, @"Test:");
this.AddLabel(200, 80, 1259, @"Delete:");
this.AddLabel(260, 80, 1259, @"Case Sensitive:");

int j = 0;
for (int i = m_Page * 10; i < m_TriggerList.Count && j < 10; i++)
{

this.AddLabel(40, 100 + j * 20, 1149, @"" + ((TriggerWord)m_TriggerList[i]).Word);
this.AddButton(140, 100 + j * 20, 4011, 4012, 10 + i, GumpButtonType.Reply, 0);
this.AddButton(200, 100 + j * 20, 4017, 4018, (10 + i)*-1, GumpButtonType.Reply, 0);
this.AddCheck(290, 100 + j * 20, 0xD2, 0xD3, ((TriggerWord)m_TriggerList[i]).CaseSensitive, i);
j++;
}
this.AddButton(120, 340, 247, 248, (int)Buttons.OK, GumpButtonType.Reply, 0);
this.AddButton(220, 340, 241, 242, (int)Buttons.Cancel, GumpButtonType.Reply, 0);
}

public enum Buttons
{
Cancel = 0,
Previous = 1,
Next = 2,
OK = 3,
Add = 4,
}

// Updates the current page's case sensitivity checkboxes in the temporary array
private void UpdateCaseSensitivity(RelayInfo info)
{
for(int i = m_Page * 10, j = 0; i < m_TriggerList.Count && j < 10; i++, j++)
{
((TriggerWord)m_TriggerList[i]).CaseSensitive = info.IsSwitched(i);
}
}

public override void OnResponse(NetState state, RelayInfo info)
{
if (state == null || state.Mobile == null)
return;

UpdateCaseSensitivity(info);

if (info.ButtonID == (int)Buttons.Cancel)
{
state.Mobile.SendMessage("You canceled changing the npc, nothing has changed.");
}
else if (info.ButtonID == (int)Buttons.Previous && m_Page > 0)
{
state.Mobile.SendGump(new ReplyNPCGump(m_Page - 1, this.X, this.Y, m_TriggerList, m_Owner));
}
else if (info.ButtonID == (int)Buttons.Next)
{
state.Mobile.SendGump(new ReplyNPCGump(m_Page + 1, this.X, this.Y, m_TriggerList, m_Owner));
}
else if (info.ButtonID == (int)Buttons.OK)
{
m_Owner.TriggerList = m_TriggerList;
state.Mobile.SendMessage("The NPC has been updated.");
}
else if (info.ButtonID == (int)Buttons.Add)
{
state.Mobile.SendMessage("Type in the word you want to trigger a response.");
state.Mobile.Prompt = new TriggerWordPrompt(m_TriggerList, m_Owner);
}
else
{
if (info.ButtonID > 0)
{
//test
int testIndex = info.ButtonID - 10;

if (testIndex >= 0 && testIndex < m_TriggerList.Count)
{
m_Owner.RevealingAction();
m_Owner.SayTo(state.Mobile, ((TriggerWord)m_TriggerList[testIndex]).Response);
state.Mobile.SendGump(new ReplyNPCGump(m_Page, this.X, this.X, m_TriggerList, m_Owner));
}

}
else
{
//delete
int deleteIndex = (info.ButtonID*-1) - 10;

if (deleteIndex >= 0 && deleteIndex < m_TriggerList.Count)
{
m_TriggerList.RemoveAt(deleteIndex);
state.Mobile.SendGump(new ReplyNPCGump(0, this.X, this.X, m_TriggerList, m_Owner));
}
}
}
}
}
}
}