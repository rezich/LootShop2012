using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;

namespace LootSystem {
	public class Cutscene {
		public string Name;
		public static List<Cutscene> List = new List<Cutscene>();
		public List<CutsceneAction> Actions = new List<CutsceneAction>();

		public Cutscene(string name, List<CutsceneAction> actions) {
			Name = name;
			Actions = actions;
			List.Add(this);
		}
		public Cutscene() {
			List.Add(this);
		}

		public static Cutscene Lookup(string name) {
			Cutscene cutscene = (from c in List
								 where c.Name == name
								 select c).FirstOrDefault<Cutscene>();
			return cutscene;
		}

		public static void Load() {
			List.Clear();
			List<Cutscene> loadedData = LoadFromFile();
			foreach (Cutscene c in loadedData) {
				List.Add(c);
				// NO IDEA WHY THE FUCK THE NEXT LINE OF CODE WORKS, BUT IT DOES
				// (update: slept on it, figured it out, should probably rewrite other stuff to make it not needed but w/e)
				List.RemoveAt(List.Count - 1);
			}
		}
		public static List<Cutscene> LoadFromFile() {
			//StreamWriter writer;
			Stream stream;
			Assembly assembly = Assembly.GetExecutingAssembly();
			List<Cutscene> item = new List<Cutscene>();

			try {
				stream = assembly.GetManifestResourceStream("LootSystem.Cutscenes.xml");
			}
			catch {
				throw new Exception("Aw shit son!");
			}
			XmlSerializer serializer = new XmlSerializer(typeof(List<Cutscene>));
			XmlReader reader = XmlReader.Create(stream);
			item = (List<Cutscene>)serializer.Deserialize(reader);

			return item;
		}
	}

	[XmlInclude(typeof(DialogueAction))]
	public abstract class CutsceneAction {
		public CutsceneAction() { }
	}
	
	public class DialogueAction : CutsceneAction {
		public string Text;
		public string Speaker;
		public DialogueAction() {
		}
		public DialogueAction(string text) {
			Text = text;
		}
		public DialogueAction(string text, string speaker) {
			Text = text;
			Speaker = speaker;
		}
	}
}
