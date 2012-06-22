using System;
using System.Collections.Generic;
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
using LootShop;

namespace LootMaker {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		Item.Modifier selectedModifier = null;

		public MainWindow() {
			InitializeComponent();
			
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

			new Item.Modifier(name + num.ToString(), null);
			lbModifiers.Items.Add(name + num.ToString());
		}

		private void lbModifiers_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			//MessageBox.Show("OMG");
			//selectedModifier = e.
		}
	}
}
