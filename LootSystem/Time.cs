using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LootSystem {
	public class Time {
		public enum MonthName {
			January = 1,
			February,
			March,
			April,
			May,
			June,
			July,
			August,
			September,
			October,
			November,
			December
		}
		public enum TimeOfDayName {
			EarlyMorning,
			Morning,
			Afternoon,
			Evening,
			Night
		}
		public const int MonthsPerYear = 12;
		public const int DaysPerMonth = 30;
		public const int DaysPerWeek = 7;
		public int Year;

		public int Month {
			get {
				return month;
			}
			set {
				month = value;
				while (month > MonthsPerYear) {
					month -= MonthsPerYear;
					Year++;
				}
			}
		}
		private int month;

		public int Day {
			get {
				return day;
			}
			set {
				day = value;
				while (day > DaysPerMonth) {
					day -= DaysPerMonth;
					Month++;
				}
			}
		}
		private int day;

		public int TimeOfDay {
			get {
				return timeOfDay;
			}
			set {
				timeOfDay = value;
				int timesOfDay = ((Time.TimeOfDayName[])EnumHelper.GetValues<Time.TimeOfDayName>()).Length;
				while (timeOfDay > timesOfDay - 1) {
					timeOfDay -= timesOfDay;
					Day++;
				}
			}
		}
		public int timeOfDay;

		public override string ToString() {
			return (((TimeOfDayName)TimeOfDay).ToString() + " " + Day + " " + ((MonthName)Month).ToString() + " " + Year);
		}

		public Time() {
			Year = 1000;
			Month = 1;
			Day = 1;
		}
	}
}
