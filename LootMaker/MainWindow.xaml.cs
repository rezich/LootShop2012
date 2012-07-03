﻿using System;
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

			Item.Modifier newModifier = new Item.Modifier(name + num.ToString(), new List<string>());
			lbModifiers.SelectedItem = newModifier;
			//lbModifiers.Items.Add(name + num.ToString());
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

		private void btnModifierTest_Click(object sender, RoutedEventArgs e) {
			Item.Modifier selectedItem = (Item.Modifier)lbModifiers.SelectedItem;
			if (selectedItem == null) {
				return;
			}
			selectedItem.Name = "POOPY PANTS";
		}

		private void btnModifierTagsAdd_Click(object sender, RoutedEventArgs e) {
			Item.Modifier selectedItem = (Item.Modifier)lbModifiers.SelectedItem;
			if (tbModifierTagsNew.Text != "" && tbModifierTagsNew.Text != null) {
				selectedItem.Tags.Add(tbModifierTagsNew.Text);
				lbModifierTags.Items.Refresh();
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
	}

}
