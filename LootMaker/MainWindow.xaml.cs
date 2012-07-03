using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
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
using LootSystem;

namespace LootMaker {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		public MainWindow() {
			InitializeComponent();
			lbModifiers.DataContext = Item.Modifier.List;
		}

		private void btnModifiersNew_Click(object sender, RoutedEventArgs e) {
			string name = "UNNAMED_";
			Item.Modifier taken = null;
			int num = 0;

			do {
				taken = (from m in Item.Modifier.List
						 where m.Name == name + num.ToString()
						 select m).FirstOrDefault<Item.Modifier>();
				if (taken != null) num++;
			} while (taken != null);

			Item.Modifier newModifier = new Item.Modifier(name + num.ToString(), Item.Modifier.Type.Adjective, new List<string>());
			lbModifiers.SelectedItem = newModifier;
			tbModifierName.Focus();
			tbModifierName.SelectAll();
		}

		private void btnModifiersDelete_Click(object sender, RoutedEventArgs e) {
			Item.Modifier selectedItem = (Item.Modifier)lbModifiers.SelectedItem;
			Item.Modifier.List.Remove(selectedItem);
			if (((Item.Modifier.ListType)lbModifiers.DataContext).Count == 0) return;
			lbModifiers.SelectedItem = ((Item.Modifier.ListType)lbModifiers.DataContext)[((Item.Modifier.ListType)lbModifiers.DataContext).Count - 1];
		}

		private void lbModifiers_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			Item.Modifier selectedItem = (Item.Modifier)lbModifiers.SelectedItem;
			if (selectedItem == null) {
				tcModifiers.IsEnabled = false;
				btnModifiersDelete.IsEnabled = false;
				return;
			}
			tcModifiers.IsEnabled = true;
			btnModifiersDelete.IsEnabled = true;
		}

		private void btnModifierTagsAdd_Click(object sender, RoutedEventArgs e) {
			Item.Modifier selectedItem = (Item.Modifier)lbModifiers.SelectedItem;
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

		private void btnModifierTagsRemove_Click(object sender, RoutedEventArgs e) {
			Item.Modifier selectedModifier = (Item.Modifier)lbModifiers.SelectedItem;
			string selectedItem = (string)lbModifierTags.SelectedItem;

			selectedModifier.Tags.Remove(selectedItem);
			lbModifierTags.Items.Refresh();
			selectedItem = (string)lbModifierTags.SelectedItem;
			if (selectedItem == null) {
				btnModifierTagsRemove.IsEnabled = false;
				return;
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			cbModifierKind.ItemsSource = Enum.GetValues(typeof(Item.Modifier.Type)).Cast<Item.Modifier.Type>().ToList<Item.Modifier.Type>();
			//foreach (Item.Modifier.Type t in Enum.GetValues(typeof(Item.Modifier.Type)).Cast<Item.Modifier.Type>().ToList<Item.Modifier.Type>()) cbModifierKind.Items.Add(t.ToString());
		}

		private void btnSave_Click(object sender, RoutedEventArgs e) {
			Item.Modifier.List.Save();
		}

		private void btnLoad_Click(object sender, RoutedEventArgs e) {
			Item.Modifier.List.Clear();
			//Item.Modifier.List = new Item.Modifier.ListType();
			//Item.Modifier.List = Item.Modifier.ListType.Load();
			Item.Modifier.ListType loadedData = Item.Modifier.ListType.Load();
			foreach (Item.Modifier m in loadedData) {
				((Item.Modifier.ListType)lbModifiers.DataContext).Add(m);
				// NO IDEA WHY THE FUCK THE NEXT LINE OF CODE WORKS, BUT IT DOES
				((Item.Modifier.ListType)lbModifiers.DataContext).RemoveAt(((Item.Modifier.ListType)lbModifiers.DataContext).Count - 1);
			}
		}
	}

}
