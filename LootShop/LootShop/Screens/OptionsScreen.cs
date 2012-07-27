using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LootShop {
	class OptionsScreen : GenericMenu {
		List<ScreenResolution> resolutions = new List<ScreenResolution>();
		int curResolution = 0;
		bool fullscreen = false;

		List<double> titleSafeModes = new List<double> {
			0.1,
			0.2,
			0.3,
			0.4,
			0.5,
			0.6,
			0.7,
			0.8,
			0.9,
			1.0
		};
		int titleSafeIndex = 0;

		Entry resolutionEntry;
		Entry fullscreenEntry;
		Entry titleSafeEntry;

		public OptionsScreen()
			: base("Options", true, false) {
				Centered = true;
			foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes) {
				resolutions.Add(new ScreenResolution(dm.Width, dm.Height, dm.AspectRatio));
			}

			curResolution = resolutions.IndexOf((from r in resolutions
												 where r.Width == Resolution.RealWidth && r.Height == Resolution.RealHeight
												 select r).FirstOrDefault());
			titleSafeIndex = titleSafeModes.IndexOf(Resolution.TitleSafeAreaScale);
			fullscreen = Resolution.IsFullscreen;

			MenuEntries.Add(new HeadingEntry("Graphics"));

			resolutionEntry = new Entry("Resolution");
			resolutionEntry.SwipeLeft += DecrementResolution;
			resolutionEntry.SwipeRight += IncrementResolution;
#if XBOX
			resolutionEntry.Enabled = false;
#endif
			MenuEntries.Add(resolutionEntry);

			fullscreenEntry = new Entry("Fullscreen");
			fullscreenEntry.Selected += ToggleFullscreen;
#if XBOX
			fullscreenEntry.Enabled = false;
#endif
			MenuEntries.Add(fullscreenEntry);

			titleSafeEntry = new Entry("Safe Zone");
			titleSafeEntry.SwipeLeft += DecrementTitleSafeMode;
			titleSafeEntry.SwipeRight += IncrementTitleSafeMode;
			titleSafeEntry.Enabled = Resolution.HasTitleSafeArea;
			MenuEntries.Add(titleSafeEntry);

			Entry applySettingsEntry = new Entry("Apply settings");
			applySettingsEntry.Selected += ApplySettings;
			MenuEntries.Add(applySettingsEntry);

			UpdateEntryText();
		}

		void IncrementResolution(object sender, PlayerIndexEventArgs e) {
			curResolution++;
			if (curResolution >= resolutions.Count) curResolution -= resolutions.Count;
			UpdateEntryText();
		}

		void DecrementResolution(object sender, PlayerIndexEventArgs e) {
			curResolution--;
			if (curResolution < 0) curResolution += resolutions.Count;
			UpdateEntryText();
		}

		void ToggleFullscreen(object sender, PlayerIndexEventArgs e) {
			fullscreen = !fullscreen;
			UpdateEntryText();
		}

		void IncrementTitleSafeMode(object sender, PlayerIndexEventArgs e) {
			titleSafeIndex++;
			if (titleSafeIndex >= titleSafeModes.Count) titleSafeIndex -= titleSafeModes.Count;
			Resolution.TitleSafeAreaScale = titleSafeModes[titleSafeIndex];
			Resolution.DirtyMatrix = true;
			UpdateEntryText();
		}

		void DecrementTitleSafeMode(object sender, PlayerIndexEventArgs e) {
			titleSafeIndex--;
			if (titleSafeIndex < 0) titleSafeIndex += titleSafeModes.Count;
			Resolution.TitleSafeAreaScale = titleSafeModes[titleSafeIndex];
			Resolution.DirtyMatrix = true;
			UpdateEntryText();
		}

		void ApplySettings(object sender, PlayerIndexEventArgs e) {
			Resolution.SetResolution(resolutions[curResolution].Width, resolutions[curResolution].Height, fullscreen);
		}

		void UpdateEntryText() {
			resolutionEntry.Text2 = resolutions[curResolution].ToString();
			fullscreenEntry.Text2 = (fullscreen ? "ON" : "OFF");
			titleSafeEntry.Text2 = (titleSafeModes[titleSafeIndex] * 10).ToString();
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.BeginSpriteBatch();
			ScreenManager.SpriteBatch.Draw(GameSession.Current.Pixel, Resolution.Rectangle, Color.Black * TransitionAlphaSquared);
			ScreenManager.SpriteBatch.End();
			base.Draw(gameTime);
		}

		struct ScreenResolution {
			public int Width;
			public int Height;
			public float AspectRatio;

			public ScreenResolution(int width, int height, float aspectRatio) {
				Width = width;
				Height = height;
				AspectRatio = aspectRatio;
			}

			public override string ToString() {
				return Width.ToString() + "x" + Height.ToString() + (AspectRatio != (16f / 9f) ? "*" : "");
			}
		}
	}
}
