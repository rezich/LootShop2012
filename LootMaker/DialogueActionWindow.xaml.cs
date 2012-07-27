using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LootSystem;

namespace LootMaker {
	/// <summary>
	/// Interaction logic for DialogueAction.xaml
	/// </summary>
	public partial class DialogueActionWindow : Window {

		public string Speaker {
			get { return cbSpeaker.Text == "" ? null : cbSpeaker.Text; }
		}

		public string Text {
			get { return tbText.Text; }
		}

		public DialogueActionWindow() {
			this.InitializeComponent();
		}

		public DialogueActionWindow(DialogueAction action) {
			this.InitializeComponent();
			cbSpeaker.Text = action.Speaker;
			tbText.Text = action.Text;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			/*Window mainWindow = Application.Current.MainWindow;
			this.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth) / 2;
			this.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight) / 2;*/
		}

		private void btnOK_Click(object sender, RoutedEventArgs e) {
			DialogResult = true;
		}
	}
}