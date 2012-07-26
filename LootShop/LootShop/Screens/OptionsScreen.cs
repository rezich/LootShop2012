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
			foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes) {
				resolutions.Add(new ScreenResolution(dm.Width, dm.Height));
			}

			curResolution = resolutions.IndexOf((from r in resolutions
												 where r.Width == Resolution.RealWidth && r.Height == Resolution.RealHeight
												 select r).FirstOrDefault());
			titleSafeIndex = titleSafeModes.IndexOf(Resolution.TitleSafeAreaScale);
			fullscreen = Resolution.IsFullscreen;

			resolutionEntry = new Entry("Resolution: " + resolutions[curResolution]);
			resolutionEntry.Selected += IncrementResolution;
			MenuEntries.Add(resolutionEntry);

			fullscreenEntry = new Entry("Fullscreen: " + (fullscreen ? "YES" : "NO"));
			fullscreenEntry.Selected += ToggleFullscreen;
			MenuEntries.Add(fullscreenEntry);

			titleSafeEntry = new Entry("Safe zone: " + titleSafeModes[titleSafeIndex].ToString());
			titleSafeEntry.Selected += IncrementTitleSafeMode;
			MenuEntries.Add(titleSafeEntry);

			Entry applySettingsEntry = new Entry("Apply settings");
			applySettingsEntry.Selected += ApplySettings;
			MenuEntries.Add(applySettingsEntry);

		}

		void IncrementResolution(object sender, PlayerIndexEventArgs e) {
			curResolution++;
			if (curResolution >= resolutions.Count) curResolution -= resolutions.Count;
			resolutionEntry.Text = "Resolution: " + resolutions[curResolution];
		}

		void ToggleFullscreen(object sender, PlayerIndexEventArgs e) {
			fullscreen = !fullscreen;
			fullscreenEntry.Text = "Fullscreen: " + (fullscreen ? "YES" : "NO");
		}

		void IncrementTitleSafeMode(object sender, PlayerIndexEventArgs e) {
			titleSafeIndex++;
			if (titleSafeIndex >= titleSafeModes.Count) titleSafeIndex -= titleSafeModes.Count;
			titleSafeEntry.Text = "Safe zone: " + titleSafeModes[titleSafeIndex].ToString();
			Resolution.TitleSafeAreaScale = titleSafeModes[titleSafeIndex];
			Resolution.DirtyMatrix = true;
		}

		void ApplySettings(object sender, PlayerIndexEventArgs e) {
			Resolution.SetResolution(resolutions[curResolution].Width, resolutions[curResolution].Height, fullscreen);
		}

		struct ScreenResolution {
			public int Width;
			public int Height;

			public ScreenResolution(int width, int height) {
				Width = width;
				Height = height;
			}

			public override string ToString() {
				return Width.ToString() + "x" + Height.ToString();
			}
		}
	}
}
