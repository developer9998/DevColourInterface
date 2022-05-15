using System;
using System.Collections.Generic;
using System.Text;
using ComputerInterface;
using ComputerInterface.Interfaces;

namespace DevColourInterface
{
	public class ColourInterfaceEntry : IComputerModEntry
	{
		public string EntryName => "ColourInterface";

		public Type EntryViewType => typeof(ColourInterfaceView);
	}
}
