using System;
using Server;

namespace Server.Items
{
    public class AugustBook : RedBook
    {
        [Constructable]
        public AugustBook()
            : base("a book of rules", "a random admin", 5, false
      )
        {
            Hue = 600;

            Pages[0].Lines = new string[]
            {
             "Rule 1, Do not sit on",
             "the blue bench.",
             "This is for the village",
             "to idiot to sleep on",
            };

            Pages[1].Lines = new string[]
            {
             "Rule 2, Avoid the garden.",
             "The plants have an",
             "appetite for our flesh.",
            };

            Pages[2].Lines = new string[]
            {
             "Rule 3, Under no",
             "circumstance should you",
             "remove the dogs lead.",
             "He is a fast and vicious",
             "bugger that is impossible",
             "to catch.",
            };

            Pages[3].Lines = new string[]
            {
             "Rule 4, Please dont feed",
             "the fish.",
             "They are fat enough",
             "already.",
            };

            Pages[4].Lines = new string[]
            {
             "Rule 5, Never give a",
             "spider a book to guard.",
             "As he tends to die and",
             "leave it for someone",
             "to find.",
            };
        }

        public AugustBook(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}