﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;

namespace LootShop {
	class PauseScreen : GenericMenu {
		public PauseScreen()
			: base("Paused", true, false) {
				Centered = true;
				Entry resumeEntry = new Entry("Resume");
				resumeEntry.Selected += OnCancel;
				resumeEntry.IsCancel = true;
				MenuEntries.Add(resumeEntry);
				Entry saveEntry = new Entry("Save game");
				saveEntry.Selected += OnSave;
				MenuEntries.Add(saveEntry);
				Entry optionsEntry = new Entry("Options");
				optionsEntry.Selected += OnOptions;
				MenuEntries.Add(optionsEntry);
				Entry quitEntry = new Entry("Quit game");
				quitEntry.Selected += OnQuit;
				MenuEntries.Add(quitEntry);
		}

		public override void Initialize() {
			base.Initialize();
			GameSession.Current.GamePause.Play(GameSession.Current.SoundEffectVolume, 0, 0);
			MediaPlayer.Pause();
		}

		protected override void OnCancel(Microsoft.Xna.Framework.PlayerIndex playerIndex) {
			MediaPlayer.Resume();
			base.OnCancel(playerIndex);
		}

		public void OnSave(object sender, PlayerIndexEventArgs e) {
			GameSession.Current.Campaign.Save("test.xml");
			OnCancel(sender, e);
		}

		void OnOptions(object sender, PlayerIndexEventArgs e) {
			ScreenManager.AddScreen(new OptionsScreen(), ControllingPlayer);
		}

		public void OnQuit(object sender, PlayerIndexEventArgs e) {
			GameSession.Current.StartFromSplashScreens();
		}
	}
}
