using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using LootSystem;

namespace LootMaker {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		public MainWindow() {
			InitializeComponent();
			lbModifiers.DataContext = LootMaker.Modifier2.List;
			lbCutscenes.DataContext = LootMaker.Cutscene2.List;
		}

		private void btnModifiersDelete_Click(object sender, RoutedEventArgs e) {
			LootMaker.Modifier2 selectedItem = (LootMaker.Modifier2)lbModifiers.SelectedItem;
			LootMaker.Modifier2.List.Remove(selectedItem);
			if (((LootMaker.Modifier2.ListType)lbModifiers.DataContext).Count == 0) return;
			lbModifiers.SelectedItem = ((LootMaker.Modifier2.ListType)lbModifiers.DataContext)[((LootMaker.Modifier2.ListType)lbModifiers.DataContext).Count - 1];
		}

		private void lbModifiers_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			LootMaker.Modifier2 selectedItem = (LootMaker.Modifier2)lbModifiers.SelectedItem;
			if (selectedItem == null) {
				tcModifiers.IsEnabled = false;
				btnModifiersDelete.IsEnabled = false;
				return;
			}
			tcModifiers.IsEnabled = true;
			btnModifiersDelete.IsEnabled = true;
		}

		private void lbCutscenes_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			LootMaker.Cutscene2 selectedItem = (LootMaker.Cutscene2)lbCutscenes.SelectedItem;
			if (selectedItem == null) {
				tcCutscenes.IsEnabled = false;
				btnModifiersDelete.IsEnabled = false;
				return;
			}
			tcCutscenes.IsEnabled = true;
			btnModifiersDelete.IsEnabled = true;
		}

		private void btnModifierTagsAdd_Click(object sender, RoutedEventArgs e) {
			LootMaker.Modifier2 selectedItem = (LootMaker.Modifier2)lbModifiers.SelectedItem;
			if (tbModifierTagsNew.Text != "" && tbModifierTagsNew.Text != null) {
				if (!selectedItem.Tags.Contains(tbModifierTagsNew.Text)) selectedItem.Tags.Add(tbModifierTagsNew.Text);
				lbModifierTags.Items.Refresh();
				tbModifierTagsNew.Text = "";
				tbModifierTagsNew.Focus();
			}
		}

		private void lbModifierTags_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			string selectedItem = (string)lbModifierTags.SelectedItem;
			if (selectedItem == null) {
				btnModifierTagsRemove.IsEnabled = false;
				return;
			}
			btnModifierTagsRemove.IsEnabled = true;
		}

		private void lbCutsceneActions_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			CutsceneAction selectedItem = (CutsceneAction)lbCutsceneActions.SelectedItem;
			if (selectedItem == null) {
				btnCutsceneActionsRemove.IsEnabled = false;
				return;
			}
			btnCutsceneActionsRemove.IsEnabled = true;
		}

		private void btnModifierTagsRemove_Click(object sender, RoutedEventArgs e) {
			LootMaker.Modifier2 selectedModifier = (LootMaker.Modifier2)lbModifiers.SelectedItem;
			string selectedItem = (string)lbModifierTags.SelectedItem;

			selectedModifier.Tags.Remove(selectedItem);
			lbModifierTags.Items.Refresh();
			selectedItem = (string)lbModifierTags.SelectedItem;
			if (selectedItem == null) {
				btnModifierTagsRemove.IsEnabled = false;
				return;
			}
		}

		private void btnCutsceneActionsRemove_Click(object sender, RoutedEventArgs e) {
			LootMaker.Cutscene2 selectedCutscene = (LootMaker.Cutscene2)lbCutscenes.SelectedItem;
			CutsceneAction selectedItem = (CutsceneAction)lbCutsceneActions.SelectedItem;

			selectedCutscene.Actions.Remove(selectedItem);
			lbCutsceneActions.Items.Refresh();
			selectedItem = (CutsceneAction)lbCutsceneActions.SelectedItem;
			if (selectedItem == null) {
				btnCutsceneActionsRemove.IsEnabled = false;
				return;
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			cbModifierKind.ItemsSource = Enum.GetValues(typeof(LootMaker.Modifier2.Type)).Cast<LootMaker.Modifier2.Type>().ToList<LootMaker.Modifier2.Type>();
			cbModifiersNewKind.ItemsSource = Enum.GetValues(typeof(LootMaker.Modifier2.Type)).Cast<LootMaker.Modifier2.Type>().ToList<LootMaker.Modifier2.Type>();
			cbModifiersNewKind.SelectedIndex = 0;
			//foreach (LootMaker.Modifier.Type t in Enum.GetValues(typeof(LootMaker.Modifier.Type)).Cast<LootMaker.Modifier.Type>().ToList<LootMaker.Modifier.Type>()) cbModifierKind.Items.Add(t.ToString());
		}

		private void btnSave_Click(object sender, RoutedEventArgs e) {
			LootMaker.Modifier2.List.Save(@"..\..\..\LootSystem\Modifiers.xml");
			LootMaker.Cutscene2.List.Save(@"..\..\..\LootSystem\Cutscenes.xml");
		}

		private void btnLoad_Click(object sender, RoutedEventArgs e) {
			if (File.Exists(@"..\..\..\LootSystem\Modifiers.xml")) LootMaker.Modifier2.ListType.Load(@"..\..\..\LootSystem\Modifiers.xml");
			else MessageBox.Show("Modifier XML file not found!", "Uh-oh!", MessageBoxButton.OK, MessageBoxImage.Error);
			if (File.Exists(@"..\..\..\LootSystem\Cutscenes.xml")) LootMaker.Cutscene2.ListType.Load(@"..\..\..\LootSystem\Cutscenes.xml");
			else MessageBox.Show("Cutscene XML file not found!", "Uh-oh!", MessageBoxButton.OK, MessageBoxImage.Error);
			btnSave.IsEnabled = true;
		}

		private void CommandBinding_ModifierGood(object sender, ExecutedRoutedEventArgs e) {
			LootMaker.Modifier2 selectedItem = (LootMaker.Modifier2)lbModifiers.SelectedItem;
			if (selectedItem == null) return;
			selectedItem.Tags.Remove("bad");
			if (!selectedItem.Tags.Contains("good")) selectedItem.Tags.Add("good");
			lbModifierTags.Items.Refresh();
		}

		private void CommandBinding_ModifierNeutral(object sender, ExecutedRoutedEventArgs e) {
			LootMaker.Modifier2 selectedItem = (LootMaker.Modifier2)lbModifiers.SelectedItem;
			if (selectedItem == null) return;
			selectedItem.Tags.Remove("bad");
			selectedItem.Tags.Remove("good");
			lbModifierTags.Items.Refresh();
		}

		private void CommandBinding_ModifierBad(object sender, ExecutedRoutedEventArgs e) {
			LootMaker.Modifier2 selectedItem = (LootMaker.Modifier2)lbModifiers.SelectedItem;
			if (selectedItem == null) return;
			if (!selectedItem.Tags.Contains("bad")) selectedItem.Tags.Add("bad");
			selectedItem.Tags.Remove("good");
			lbModifierTags.Items.Refresh();
		}

		private void CommandBinding_NewModifier(object sender, ExecutedRoutedEventArgs e) {
			string name = "UNNAMED_";
			LootMaker.Modifier2 taken = null;
			int num = 0;

			do {
				taken = (from m in LootMaker.Modifier2.List
						 where m.Name == name + num.ToString()
						 select m).FirstOrDefault<LootMaker.Modifier2>();
				if (taken != null) num++;
			} while (taken != null);

			LootMaker.Modifier2 newModifier = new LootMaker.Modifier2(name + num.ToString(), (LootMaker.Modifier2.Type)cbModifiersNewKind.SelectedItem, new List<string>());
			lbModifiers.SelectedItem = newModifier;
			tbModifierName.Focus();
			tbModifierName.SelectAll();
		}

		private void CommandBinding_NewCutscene(object sender, ExecutedRoutedEventArgs e) {
			string name = "UNNAMED_";
			LootMaker.Cutscene2 taken = null;
			int num = 0;

			do {
				taken = (from m in LootMaker.Cutscene2.List
						 where m.Name == name + num.ToString()
						 select m).FirstOrDefault<LootMaker.Cutscene2>();
				if (taken != null) num++;
			} while (taken != null);

			LootMaker.Cutscene2 newCutscene = new LootMaker.Cutscene2(name + num.ToString(), new List<CutsceneAction>());
			lbCutscenes.SelectedItem = newCutscene;
			tbCutsceneName.Focus();
			tbCutsceneName.SelectAll();
		}

		private void btnModifiersSort_Click(object sender, RoutedEventArgs e) {
			List<Modifier2> adjectives = (from adj in Modifier2.List
										  where adj.Kind == Item.Modifier.Type.Adjective
										  orderby adj.Name
										  select adj).ToList<Modifier2>();
			List<Modifier2> ofX = (from adj in Modifier2.List
								   where adj.Kind == Item.Modifier.Type.OfX
								   orderby adj.Name
								   select adj).ToList<Modifier2>();
			Modifier2.List.Clear();
			foreach (Modifier2 adj in adjectives) Modifier2.List.Add(adj);
			foreach (Modifier2 x in ofX) Modifier2.List.Add(x);
			//Modifier2.List.Sort(x => x.Name);
		}

		private void btnCutsceneActionsAddDialogue_Click(object sender, RoutedEventArgs e) {
			LootMaker.Cutscene2 selectedItem = (LootMaker.Cutscene2)lbCutscenes.SelectedItem;
			if (selectedItem == null) return;
			selectedItem.Actions.Add(new DialogueAction());
			lbCutsceneActions.Items.Refresh();
			lbCutsceneActions.SelectedIndex = lbCutsceneActions.Items.Count - 1;
			EditDialogueAction();
		}

		private void lbCutsceneActions_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
			if (lbCutsceneActions.SelectedItem == null) return;
			if (lbCutsceneActions.SelectedItem is DialogueAction) EditDialogueAction();
		}

		private void EditDialogueAction() {
			DialogueActionWindow window = null;
			window = new DialogueActionWindow((DialogueAction)lbCutsceneActions.SelectedItem);
			window.Owner = Window.GetWindow(this);
			if (window.ShowDialog() == true) {
				((DialogueAction)lbCutsceneActions.SelectedItem).Speaker = window.Speaker;
				((DialogueAction)lbCutsceneActions.SelectedItem).Text = window.Text;
				//selectedItem.Actions.Add(new DialogueAction(window.Text, window.Speaker));
				lbCutsceneActions.Items.Refresh();
			}
		}
	}

	// The following code courtesy http://michlg.wordpress.com/2010/01/16/listbox-automatically-scroll-currentitem-into-view/

	/// <summary>
	/// This class contains a few useful extenders for the ListBox
	/// </summary>
	public class ListBoxExtenders : DependencyObject {
		#region Properties

		public static readonly DependencyProperty AutoScrollToCurrentItemProperty = DependencyProperty.RegisterAttached("AutoScrollToCurrentItem", typeof(bool), typeof(ListBoxExtenders), new UIPropertyMetadata(default(bool), OnAutoScrollToCurrentItemChanged));

		/// <summary>
		/// Returns the value of the AutoScrollToCurrentItemProperty
		/// </summary>
		/// <param name="obj">The dependency-object whichs value should be returned</param>
		/// <returns>The value of the given property</returns>
		public static bool GetAutoScrollToCurrentItem(DependencyObject obj) {
			return (bool)obj.GetValue(AutoScrollToCurrentItemProperty);
		}

		/// <summary>
		/// Sets the value of the AutoScrollToCurrentItemProperty
		/// </summary>
		/// <param name="obj">The dependency-object whichs value should be set</param>
		/// <param name="value">The value which should be assigned to the AutoScrollToCurrentItemProperty</param>
		public static void SetAutoScrollToCurrentItem(DependencyObject obj, bool value) {
			obj.SetValue(AutoScrollToCurrentItemProperty, value);
		}

		#endregion

		#region Events

		/// <summary>
		/// This method will be called when the AutoScrollToCurrentItem
		/// property was changed
		/// </summary>
		/// <param name="s">The sender (the ListBox)</param>
		/// <param name="e">Some additional information</param>
		public static void OnAutoScrollToCurrentItemChanged(DependencyObject s, DependencyPropertyChangedEventArgs e) {
			var listBox = s as ListBox;
			if (listBox != null) {
				var listBoxItems = listBox.Items;
				if (listBoxItems != null) {
					var newValue = (bool)e.NewValue;

					var autoScrollToCurrentItemWorker = new EventHandler((s1, e2) => OnAutoScrollToCurrentItem(listBox, listBox.Items.CurrentPosition));

					if (newValue)
						listBoxItems.CurrentChanged += autoScrollToCurrentItemWorker;
					else
						listBoxItems.CurrentChanged -= autoScrollToCurrentItemWorker;
				}
			}
		}

		/// <summary>
		/// This method will be called when the ListBox should
		/// be scrolled to the given index
		/// </summary>
		/// <param name="listBox">The ListBox which should be scrolled</param>
		/// <param name="index">The index of the item to which it should be scrolled</param>
		public static void OnAutoScrollToCurrentItem(ListBox listBox, int index) {
			if (listBox != null && listBox.Items != null && listBox.Items.Count > index && index >= 0)
				listBox.ScrollIntoView(listBox.Items[index]);
		}

		#endregion
	}
}
