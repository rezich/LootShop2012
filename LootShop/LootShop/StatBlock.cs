using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LootSystem;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LootShop {
	class StatBlock {

		public static void Draw(SpriteBatch spriteBatch, Item item) {

			Color color;

			switch (item.Rarity.Name) {
				case Item.RarityLevel.Type.Garbage:
					color = Color.Gray;
					break;
				case Item.RarityLevel.Type.Normal:
					color = Color.White;
					break;
				case Item.RarityLevel.Type.Magic:
					color = Color.DarkCyan;
					break;
				case Item.RarityLevel.Type.Rare:
					color = Color.Yellow;
					break;
				case Item.RarityLevel.Type.Legendary:
					color = Color.DarkViolet;
					break;
				case Item.RarityLevel.Type.Unique:
					color = Color.Violet;
					break;
				default:
					color = Color.Red;
					break;
			}
			Vector2 origin = new Vector2(Resolution.Right / 2, 100);

			SpriteFont lootFont = GameSession.Current.UIFontSmall;

			int width = 400;

			origin.X -= width / 2;

			int lineHeight = Convert.ToInt32(lootFont.MeasureString("W").Y * 0.8);
			int padding = 40;
			int line = 0;
			TextBlock name = new TextBlock(item.Name.ToUpper());
			List<string> nameList = name.WrappedText(lootFont, width);
			foreach (string n in nameList) {
				spriteBatch.DrawString(lootFont, n, origin + new Vector2(width / 2, line * lineHeight), color, 0.0f, new Vector2(lootFont.MeasureString(n).X, 0) / 2, 1.0f, SpriteEffects.None, 1.0f);
				line++;
			}
			line++;

			string type = (item.Rarity.Name == Item.RarityLevel.Type.Normal ? "" : item.Rarity.ToString() + " ") + item.Variety.Name;
			spriteBatch.DrawString(lootFont, type, origin + new Vector2(0, line * lineHeight), color, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
			spriteBatch.DrawString(lootFont, item.Variety.Slot.ToString().DeCamelCase(), origin + new Vector2(width, line * lineHeight), Color.White, 0.0f, new Vector2(lootFont.MeasureString(item.Variety.Slot.ToString().DeCamelCase()).X, 0), 1.0f, SpriteEffects.None, 1.0f);
			line += 2;

			// TODO: Condense this shit

			foreach (KeyValuePair<Item.Attribute.Type, double> kvp in item.StandardAttributes) {
				string key = kvp.Key.ToString().DeCamelCase();
				string num = ((Item.Attribute.Lookup(kvp.Key).Addition ? "+" : "") + kvp.Value.ToString() + (Item.Attribute.Lookup(kvp.Key).Percentage ? "%" : ""));
				if (Item.Attribute.Lookup(kvp.Key).NonstandardListing == null) {
					spriteBatch.DrawString(lootFont, key, origin + new Vector2(padding, line * lineHeight), Color.White);
					spriteBatch.DrawString(lootFont, num, origin + new Vector2(width - padding, line * lineHeight), Color.White, 0.0f, new Vector2(lootFont.MeasureString(num).X, 0), 1.0f, SpriteEffects.None, 1.0f);
				}
				else {
					string key2 = Item.Attribute.Lookup(kvp.Key).NonstandardListing.Replace("@", num);
					spriteBatch.DrawString(lootFont, key2, origin + new Vector2(width / 2, line * lineHeight), Color.White, 0.0f, new Vector2(lootFont.MeasureString(key2).X, 0), 1.0f, SpriteEffects.None, 1.0f);
				}
				line++;
			}
			line++;

			foreach (KeyValuePair<Item.Attribute.Type, double> kvp in item.NonstandardAttributes) {
				string key = kvp.Key.ToString().DeCamelCase();
				string num = ((Item.Attribute.Lookup(kvp.Key).Addition ? "+" : "") + kvp.Value.ToString() + (Item.Attribute.Lookup(kvp.Key).Percentage ? "%" : ""));
				if (Item.Attribute.Lookup(kvp.Key).NonstandardListing == null) {
					spriteBatch.DrawString(lootFont, key, origin + new Vector2(padding, line * lineHeight), Color.White);
					spriteBatch.DrawString(lootFont, num, origin + new Vector2(width - padding, line * lineHeight), Color.White, 0.0f, new Vector2(lootFont.MeasureString(num).X / 2, 0), 1.0f, SpriteEffects.None, 1.0f);
				}
				else {
					string key2 = Item.Attribute.Lookup(kvp.Key).NonstandardListing.Replace("@", num);
					spriteBatch.DrawString(lootFont, key2, origin + new Vector2(width / 2, line * lineHeight), Color.White, 0.0f, new Vector2(lootFont.MeasureString(key2).X / 2, 0), 1.0f, SpriteEffects.None, 1.0f);
				}
				line++;
			}
			if (item.NonstandardAttributes.Count > 0) line++;

			string reqLevel = "Required Level: " + item.Level;
			spriteBatch.DrawString(lootFont, reqLevel, origin + new Vector2(width / 2, line * lineHeight), Color.Gray, 0.0f, new Vector2(lootFont.MeasureString(reqLevel).X / 2, 0), 1.0f, SpriteEffects.None, 1.0f);
		}

	}
}
