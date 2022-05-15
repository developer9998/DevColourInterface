using System;
using System.Collections.Generic;
using System.Text;
using ComputerInterface;
using ComputerInterface.Interfaces;
using Zenject;

namespace DevColourInterface
{
	internal class MainInstaller : Installer
	{
		public override void InstallBindings()
		{
			Container.Bind<IComputerModEntry>().To<ColourInterfaceEntry>().AsSingle();
		}
	}
}
