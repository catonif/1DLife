using System;
using System.Collections.Generic;
using System.Diagnostics;

class Program {

	static int habitatSize = 32;

	static List<bool> cells = new List<bool>(habitatSize);
	static bool GetCell(int index) => cells[Pos(index)];
	static void SetCell(int index, bool value) => cells[Pos(index)] = value;

	static int Pos(int i) => i < 0 ? habitatSize - 1 : i >= habitatSize ? 0 : i;

	static int neighDistance = 0;

	static Random rnd = new Random();

	static bool renderIn2D = true;

	static bool grandTour = true;

	static void Main(string[] args) {

		Debug.WriteLine("Project started!");

		habitatSize = Console.WindowWidth - 1;

		void Help(string error) {
			Console.WriteLine($"err: {error}\n" +
				"1 arg: neighbour distance\n" +
				"2 arg: rule in binary or in decimal with the $ before of it\n" +
				"3 arg (optional): render in 2D or not (Y/N)");
			args = new string[] { };
		}

		while (true) {

			if (args.Length == 0) {
				Console.Write("instert args: ");
				args = Console.ReadLine().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			}

			if (args.Length > 0) {
				if (int.TryParse(args[0], out int i)) neighDistance = i;
				else {
					Help("didnt get a numeric value for neighbours distance");
					continue;
				}
			}
			if (args.Length > 1) {
				grandTour = args[1] == "gt";
				if (!grandTour) {
					int ruleLenght = neighDistance * 4 + 2;
					values = new bool[ruleLenght];
					string rule = args[1];
					if (args[1][0] == '$') {
						string number = rule.Substring(1);
						if (int.TryParse(number, out int i)) {
							rule = Convert.ToString(i, 2);
							while (rule.Length < ruleLenght) {
								rule = '0' + rule;
							}
						} else {
							Help("no numeric value after $");
							continue;
						}
					}
					if (rule.Length < ruleLenght) {
						Help("not enough digits for the rule");
						continue;
					} else if (rule.Length > ruleLenght) {
						Help($"number it too big ($0-${(1 << ruleLenght) - 1})");
						continue;
					}
					SetFromString(rule);
				}
			} else {
				Help("no rule");
				continue;
			}
			if (args.Length > 2) {
				if (args[2].Length != 1) {
					Help("not a Y/N answer for render 2D");
					continue;
				}
				char c = args[2].ToLower()[0];
				if (c == 'y') renderIn2D = true;
				else if (c == 'n') renderIn2D = false;
				else {
					Help("not a Y/N answer for render 2D");
					continue;
				}
			}

			if (!grandTour) Console.ReadKey(true);

			if (grandTour) {

				for (int i = 0; i < (1 << (values.Length + 4)); i++) {
					if (i % (1 << 4) == 0) {
						string rule = Convert.ToString(i >> 4, 2);
						int ruleLenght = neighDistance * 4 + 2;
						while (rule.Length < ruleLenght) {
							rule = '0' + rule;
						}
						SetFromString(rule);
						Console.WriteLine($"grand tour: now at rule {rule}");
						Fill();
						Randomize();
						Render();
					} else {
						GenerateNeighboursCount();
						Render();
					}
				}

			} else {
				Fill();
				Randomize();
				Render();

				while (Console.ReadKey(true).KeyChar != 'q') {
					GenerateNeighboursCount();
					Render();
				}
			}

			args = new string[] { };
			

		}

	}

	static void SetFromString(string rule) {
		values = new bool [neighDistance * 4 + 2];
		for (int i = 0; i < rule.Length; i++) {
			if (int.TryParse(rule[i] + string.Empty, out int v)) values[i] = v != 0;
		}
	}

	static int CountNeighbours(int startIndex) {
		int c = 0;
		for (int i = 1; i <= neighDistance; i++) {
			c += GetCell(startIndex + i) ? 1 : 0;
			c += GetCell(startIndex - i) ? 1 : 0;
		} return c;
	}

	static bool[] values = { false, true, true, false, true, false };

	static void GenerateNeighboursCount() {
		for (int i = 0; i < cells.Count; i++) cells[i] = values[CountNeighbours(i) * 2 + (GetCell(i) ? 0 : 1)];
	}

	static void Fill() { cells.Clear(); for (int i = 0; i < habitatSize; i++) cells.Add(false); }

	static void Randomize() { for (int i = 0; i < cells.Count; i++) cells[i] = rnd.Next(2) == 0; }

	static void Render() {
		if (!renderIn2D) Console.CursorLeft = 0;
		cells.ForEach(b => { Console.BackgroundColor = b ? ConsoleColor.White : ConsoleColor.Black; Console.Write(' '); });
		Console.BackgroundColor = ConsoleColor.Black;
		if (renderIn2D) {
			Console.WriteLine();
		}
	}

}
