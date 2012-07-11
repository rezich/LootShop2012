using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LootShop {
	public class TestMenu : GenericMenu {
		public TestMenu()
			: base("Test Menu") {
				Entry itemOne = new Entry("Spawn another menu!");
				Entry itemTwo = new Entry("Test 2", new TextBlock("The path of #C_SPECIAL# the righteous man #C_NORMAL# is beset on all sides by the iniquities of the selfish and the tyranny of evil men. Blessed is he who, in the name of charity and good will, shepherds the weak through the valley of darkness, for he is truly his brother's keeper and the finder of lost children. And I will strike down upon thee with great vengeance and furious anger those who would attempt to poison and destroy My brothers. And you will know My name is the Lord when I lay My vengeance upon thee. #NL# #NL# Normally, both your asses would be dead as fucking fried chicken, but you happen to pull this shit while I'm in a transitional period so I don't wanna kill you, I wanna help you. But I can't give you this case, it don't belong to me. Besides, I've already been through too much shit this morning over this case to hand it over to your dumb ass. #NL# #NL# Look, just because I don't be givin' no man a foot massage don't make it right for Marsellus to throw Antwone into a glass motherfuckin' house, fuckin' up the way the nigger talks. Motherfucker do that shit to me, he better paralyze my ass, 'cause I'll kill the motherfucker, know what I'm sayin'? #NL# #NL# Now that there is the Tec-9, a crappy spray gun from South Miami. This gun is advertised as the most popular gun in American crime. Do you believe that shit? It actually says that in the little book that comes with it: the most popular gun in American crime. Like they're actually proud of that shit. Also, press #X_BUTTON# to do absolutely fucking nothing."));

				itemOne.Selected += SpawnAnotherMenu;

				MenuEntries.Add(itemOne);
				MenuEntries.Add(itemTwo);
				MenuEntries.Add(new Entry("Test filler"));
				MenuEntries.Add(new Entry("Test filler"));
				MenuEntries.Add(new Entry("Test filler"));
				MenuEntries.Add(new Entry("Test filler"));
				MenuEntries.Add(new Entry("Test filler"));
		}

		void SpawnAnotherMenu(object sender, PlayerIndexEventArgs e) {
			ScreenManager.AddScreen(new TestMenu(), ControllingPlayer);
		}
	}
}
