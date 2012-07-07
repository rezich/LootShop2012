using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using LootSystem;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.IO;


namespace LootMaker {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
	}

	public class Modifier2 : Item.Modifier {

		public new static Modifier2.ListType List = new Modifier2.ListType();

		public Modifier2(string name, Item.Modifier.Type kind, List<string> tags) {
			Name = name;
			Kind = kind;
			Tags = tags;
			List.Add(this);
		}

		public Modifier2() {
			List.Add(this);
		}

		public class ListType : ObservableCollection<LootMaker.Modifier2> {
			public static void Load(string fileName) {
				Modifier2.List.Clear();
				Modifier2.ListType loadedData = Modifier2.ListType.LoadFromFile(fileName);
				foreach (Modifier2 m in loadedData) {
					Modifier2.List.Add(m);
					// NO IDEA WHY THE FUCK THE NEXT LINE OF CODE WORKS, BUT IT DOES
					// (update: slept on it, figured it out, should probably rewrite other stuff to make it not needed but w/e)
					Modifier2.List.RemoveAt(Modifier2.List.Count - 1);
				}
			}
			public static ListType LoadFromFile(string fileName) {
				XmlRootAttribute root = new XmlRootAttribute("ArrayOfModifier");
				XmlSerializer reader = new XmlSerializer(typeof(Modifier2.ListType), root);
				RenameHack(fileName, true);
				System.IO.StreamReader file = new System.IO.StreamReader(fileName);
				ListType item = new ListType();
				item = (ListType)reader.Deserialize(file);
				file.Close();
				RenameHack(fileName, false);
				return item;
			}

			public void Save(string fileName) {
				XmlRootAttribute root = new XmlRootAttribute("ArrayOfModifier");
				XmlSerializer writer = new XmlSerializer(typeof(Modifier2.ListType), root);
				System.IO.StreamWriter file = new System.IO.StreamWriter(fileName);
				writer.Serialize(file, this);
				file.Close();
				RenameHack(fileName, false);
			}

			public static void RenameHack(string fileName, bool loading) {
				StreamReader reader = new StreamReader(fileName);
				string content = reader.ReadToEnd();
				reader.Close();

				content = loading ? Regex.Replace(content, "<Modifier", "<Modifier2") : Regex.Replace(content, "<Modifier2", "<Modifier");
				content = loading ? Regex.Replace(content, "</Modifier", "</Modifier2") : Regex.Replace(content, "</Modifier2", "</Modifier");

				StreamWriter writer = new StreamWriter(fileName);
				writer.Write(content);
				writer.Close();
			}
		}
	}
}
