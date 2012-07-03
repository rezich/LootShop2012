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
			Item.Modifier.List.Save(@"..\..\..\LootSystem\Data\Modifiers.xml");
		}

		private void btnLoad_Click(object sender, RoutedEventArgs e) {
			Item.Modifier.ListType.Load(@"..\..\..\LootSystem\Data\Modifiers.xml");
		}

		private void btnModifierTagsGood_Click(object sender, RoutedEventArgs e) {
			Item.Modifier selectedItem = (Item.Modifier)lbModifiers.SelectedItem;
			selectedItem.Tags.Remove("bad");
			selectedItem.Tags.Add("good");
			lbModifierTags.Items.Refresh();
		}

		private void btnModifierTagsNeutral_Click(object sender, RoutedEventArgs e) {
			Item.Modifier selectedItem = (Item.Modifier)lbModifiers.SelectedItem;
			selectedItem.Tags.Remove("bad");
			selectedItem.Tags.Remove("good");
			lbModifierTags.Items.Refresh();
		}

		private void btnModifierTagsBad_Click(object sender, RoutedEventArgs e) {
			Item.Modifier selectedItem = (Item.Modifier)lbModifiers.SelectedItem;
			selectedItem.Tags.Add("bad");
			selectedItem.Tags.Remove("good");
			lbModifierTags.Items.Refresh();
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
