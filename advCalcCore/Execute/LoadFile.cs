using advCalcCore.Execute;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace advCalcCore.FileHandle
{
	public static class LoadFile
	{
		public static string Load(string path)
		{
			return File.ReadAllText(path);
		}

		public static Value ExecuteFile(string path)
		{
			return Code.Run(Load(path));
		}

		public static bool TryLoad(string path, out string content)
		{
			Console.WriteLine(Path.GetFullPath(path));

			if (!File.Exists(path))
			{
				content = "ERROR: File not found: " + path;
				return false;
			}

			content = File.ReadAllText(path);
			return true;
		}

	}
}
