using System;
using System.Collections.Generic;
using System.Text;
using ComputerInterface;
using ComputerInterface.ViewLib;
using GorillaLocomotion;
using GorillaNetworking;
using UnityEngine;
using Photon.Pun;

namespace DevColourInterface
{
	public class ColourInterfaceView : ComputerView
	{
		const string highlightColor = "FF51FFff"; // thanks paint.net

		private readonly UISelectionHandler _selectionHandler;

		float Colour1;
		float Colour2;
		float Colour3;

		float FixedColour1;
		float FixedColour2;
		float FixedColour3;

		float _Colour1;
		float _Colour2;
		float _Colour3;

        float _FixedColour1;
        float _FixedColour2;
		float _FixedColour3;

        bool isRGBisNotHSV = true;
        float RGBRed;
		float RGBGreen;
		float RGBBlue;

		float[] rgbList = { 0, 28, 57, 85, 113, 142, 170, 198, 227, 255 };

		bool hasSavedFile = false;

		public ColourInterfaceView()
		{
            _selectionHandler = new UISelectionHandler(EKeyboardKey.Up, EKeyboardKey.Down, EKeyboardKey.Enter);
                               // the max zero indexed entry (2 entries - 1 since zero indexed)
            _selectionHandler.MaxIdx = 1;
			// when the "selection" key is pressed (we set it to enter above)
			_selectionHandler.OnSelected += OnEntrySelected;
			// since you quite often want to have an indicator of the selected item
			// I added helper function for that.
			// Basically you specify the prefix and suffix added to the selected item
			// and an prefix and suffix if the item isn't selected
			_selectionHandler.ConfigureSelectionIndicator($"<color=#{highlightColor}>></color> ", "", "  ", "");
		}

		public override void OnShow(object[] args)
		{
			base.OnShow(args);
			// changing the Text property will fire an PropertyChanged event
			// which lets the computer know the text has changed and update it
			UpdateScreen();
		}

		void UpdateScreen()
		{
			// when your text function isn't that complex
			// you can use this method which creates a string builder
			// passes it via the specified callback function and sets the text at the end
			SetText(str =>
			{
				str.BeginCenter();
				str.MakeBar('-', SCREEN_WIDTH, 0, "ffffff10");
				str.AppendClr("ColourInterface", highlightColor).EndColor().AppendLine();
				str.AppendLine("By dev9998");
				str.MakeBar('-', SCREEN_WIDTH, 0, "ffffff10");
				str.EndAlign().AppendLines(1);

				switch (Plugin.instance.Page)
                {
					case 0:
						DrawBody(str);
						break;
					case 1:
						DrawSetMonke(str);
						break;
					case 2:
						DrawBodyConvert(str);
                        break;
					case 3:
						DrawConvertResults(str);
						if (hasSavedFile)
						{
							str.BeginCenter();
							str.AppendLine();
							str.BeginColor(highlightColor);
							str.AppendLine("Saved data successfully");
							str.EndColor();
							str.EndAlign();
						}
						else
						{
							str.BeginCenter();
							str.AppendLine();
							str.AppendLine("Save data (Option 1)");
							str.EndAlign();
						}
						break;
				}
			});
		}

		void DrawConvertResults(StringBuilder str)
        {
			str.BeginCenter();
			str.AppendLine();
			str.AppendLine("Results from conversion");
			str.AppendLine();

			if (isRGBisNotHSV)
            {
				str.AppendLine($"{(_Colour1)} ▶ {(RGBRed)} (Red)");
				str.AppendLine($"{(_Colour2)} ▶ {(RGBGreen)} (Green)");
				str.AppendLine($"{(_Colour3)} ▶ {(RGBBlue)} (Blue)");
			}
			str.EndAlign();
		}
		void DrawBody(StringBuilder str)
		{
			str.BeginCenter();
			str.AppendLine();
			str.AppendLine("Select your option");
			str.AppendLine();

			str.AppendLine(_selectionHandler.GetIndicatedText(0, $"<color={("white")}>Change Colour</color>"));
			str.AppendLine(_selectionHandler.GetIndicatedText(1, $"<color={("white")}>Convert Colour</color>"));

			str.EndAlign();
		}
		void DrawBodyConvert(StringBuilder str)
		{
			str.BeginCenter();
			str.AppendLine();
			str.AppendLine("Type your colour (0-9)");
			str.AppendLine();

			if (Plugin.instance.ColourStage2 == 0)
			{
				str.AppendClr(_Colour1.ToString(), highlightColor).EndColor().AppendLine();
				str.AppendClr(_Colour2.ToString(), "FFFFFFFF").EndColor().AppendLine();
				str.AppendClr(_Colour3.ToString(), "FFFFFFFF").EndColor().AppendLine();
			}
			else
			if (Plugin.instance.ColourStage2 == 1)
			{
				str.AppendClr(_Colour1.ToString(), "FFFFFFFF").EndColor().AppendLine();
				str.AppendClr(_Colour2.ToString(), highlightColor).EndColor().AppendLine();
				str.AppendClr(_Colour3.ToString(), "FFFFFFFF").EndColor().AppendLine();
			}
			else
			if (Plugin.instance.ColourStage2 == 2)
			{
				str.AppendClr(_Colour1.ToString(), "FFFFFFFF").EndColor().AppendLine();
				str.AppendClr(_Colour2.ToString(), "FFFFFFFF").EndColor().AppendLine();
				str.AppendClr(_Colour3.ToString(), highlightColor).EndColor().AppendLine();
			}

			str.EndAlign();
		}

		void DrawSetMonke(StringBuilder str)
        {
			str.BeginCenter();
			str.AppendLine();
			str.AppendLine("Type your colour (0-9)");
			str.AppendLine();

			if (Plugin.instance.ColourStage == 0)
            {
				str.AppendClr(Colour1.ToString(), highlightColor).EndColor().AppendLine();
				str.AppendClr(Colour2.ToString(), "FFFFFFFF").EndColor().AppendLine();
				str.AppendClr(Colour3.ToString(), "FFFFFFFF").EndColor().AppendLine();
			}
			else
			if (Plugin.instance.ColourStage == 1)
			{
				str.AppendClr(Colour1.ToString(), "FFFFFFFF").EndColor().AppendLine();
				str.AppendClr(Colour2.ToString(), highlightColor).EndColor().AppendLine();
				str.AppendClr(Colour3.ToString(), "FFFFFFFF").EndColor().AppendLine();
			}
			else
			if (Plugin.instance.ColourStage == 2)
			{
				str.AppendClr(Colour1.ToString(), "FFFFFFFF").EndColor().AppendLine();
				str.AppendClr(Colour2.ToString(), "FFFFFFFF").EndColor().AppendLine();
				str.AppendClr(Colour3.ToString(), highlightColor).EndColor().AppendLine();
			}

			str.EndAlign();

		}

		private void OnEntrySelected(int index)
		{
			if (Plugin.instance.Page == 0)
            {
				switch (index)
				{
					case 0:
						Plugin.instance.Page = 1;
						UpdateScreen();
						break;
					case 1:
						Plugin.instance.Page = 2;
						UpdateScreen();
						break;
				}
			}
		}

		public override void OnKeyPressed(EKeyboardKey key)
		{
			if (Plugin.instance.Page == 3)
            {
				if (_selectionHandler.HandleKeypress(key))
				{
					UpdateScreen();
					return;
				}

				if (key == EKeyboardKey.Back)
				{
					Plugin.instance.ColourStage2 = 0;
					Plugin.instance.Page = 0;
					_Colour1 = 0;
					_Colour2 = 0;
					_Colour3 = 0;
					_FixedColour1 = 0;
					_FixedColour2 = 0;
					_FixedColour3 = 0;
					hasSavedFile = false;
					UpdateScreen();
				}

				if (key == EKeyboardKey.Option1)
				{
					hasSavedFile = true;
					Plugin.instance.SaveFile(RGBRed, RGBGreen, RGBBlue, _Colour1, _Colour2, _Colour3);
					UpdateScreen();
				}
			}
			else
			if (Plugin.instance.Page == 0)
			{
				if (_selectionHandler.HandleKeypress(key))
				{
					UpdateScreen();
					return;
				}

				if (key == EKeyboardKey.Back)
				{
					Plugin.instance.ColourStage = 0;
					ReturnView();
				}
			}
			else
			if (Plugin.instance.Page == 1)
            {
				if (key == EKeyboardKey.Back)
				{
					if (Plugin.instance.ColourStage == 0 || Plugin.instance.ColourStage == 3)
					{
						Plugin.instance.ColourStage = 0;
						Plugin.instance.Page = 0;
						Colour1 = 0f;
						FixedColour1 = 0f;
						Colour2 = 0f;
						FixedColour2 = 0f;
						Colour3 = 0f;
						FixedColour3 = 0f;
						Plugin.instance.UpdatePreviewColours();
						UpdateScreen();
					}
					else
					if (Plugin.instance.ColourStage == 2)
					{
						Plugin.instance.ColourStage = 1;
						Colour2 = 0f;
						FixedColour2 = 0f;
						UpdateScreen();
					}
					else
					if (Plugin.instance.ColourStage == 1)
					{
						Plugin.instance.ColourStage = 0;
						Colour1 = 0f;
						FixedColour1 = 0f;
						Colour2 = 0f;
						FixedColour2 = 0f;
						UpdateScreen();
					}
				}
			}
			else
			if (Plugin.instance.Page == 2)
			{
				if (key == EKeyboardKey.Back)
				{
					if (Plugin.instance.ColourStage2 == 0 || Plugin.instance.ColourStage2 == 3)
					{
						Plugin.instance.ColourStage2 = 0;
						Plugin.instance.Page = 0;
						_Colour1 = 0f;
						_FixedColour1 = 0f;
						_Colour2 = 0f;
						_FixedColour2 = 0f;
						_Colour3 = 0f;
						_FixedColour3 = 0f;
						UpdateScreen();
					}
					else
					if (Plugin.instance.ColourStage2 == 2)
					{
						Plugin.instance.ColourStage2 = 1;
						_Colour2 = 0f;
						_FixedColour2 = 0f;
						UpdateScreen();
					}
					else
					if (Plugin.instance.ColourStage2 == 1)
					{
						Plugin.instance.ColourStage2 = 0;
						_Colour1 = 0f;
						_FixedColour1 = 0f;
						_Colour2 = 0f;
						_FixedColour2 = 0f;
						UpdateScreen();
					}
				}
			}

			if (Plugin.instance.Page == 1)
            {
				if (Plugin.instance.ColourStage == 0)
                {
					if (key.IsNumberKey())
					{
						int numChar = (int)key;

						SetBasicColour(0, 1, numChar);
					}
				}
				else if (Plugin.instance.ColourStage == 1)
				{
					if (key.IsNumberKey())
					{
						int numChar = (int)key;

						SetBasicColour(1, 2, numChar);
					}
				}
				else if (Plugin.instance.ColourStage == 2)
				{
					if (key.IsNumberKey())
					{
						int numChar = (int)key;

						SetBasicColour(2, 3, numChar);
					}
				}
				if (Plugin.instance.ColourStage == 3)
				{
					Color trueColor = new Color(FixedColour1, FixedColour2, FixedColour3);

					PlayerPrefs.SetFloat("redValue", trueColor.r);
					PlayerPrefs.SetFloat("greenValue", trueColor.g);
					PlayerPrefs.SetFloat("blueValue", trueColor.b);

					GorillaTagger.Instance.UpdateColor(trueColor.r, trueColor.g, trueColor.b);
					PlayerPrefs.Save();

					if (PhotonNetwork.InRoom)
					{
						GorillaTagger.Instance.myVRRig.photonView.RPC("InitializeNoobMaterial", RpcTarget.All, new object[]
						{
							trueColor.r,
							trueColor.g,
							trueColor.b,
							GorillaComputer.instance.leftHanded
						}); ;
					}

					Plugin.instance.UpdatePreviewColoursFloat(trueColor.r, trueColor.g, trueColor.b);
					Plugin.instance.UpdateText();
					Colour1 = 0f;
					Colour2 = 0f;
					Colour3 = 0f;
					FixedColour1 = 0f;
					FixedColour2 = 0f;
					FixedColour3 = 0f;
					Plugin.instance.Page = 0;
					Plugin.instance.ColourStage = 0;
				}
				UpdateScreen();
			}

			if (Plugin.instance.Page == 2)
			{
				if (Plugin.instance.ColourStage2 == 0)
				{
					if (key.IsNumberKey())
					{
						int numChar = (int)key;

						SetBasicColour2(0, 1, numChar);
					}
				}
				else if (Plugin.instance.ColourStage2 == 1)
				{
					if (key.IsNumberKey())
					{
						int numChar = (int)key;

						SetBasicColour2(1, 2 , numChar);
					}
				}
				else if (Plugin.instance.ColourStage2 == 2)
				{
					if (key.IsNumberKey())
					{
						int numChar = (int)key;

						SetBasicColour2(2, 3, numChar);
					}
				}
				if (Plugin.instance.ColourStage2 == 3)
				{
					ConvertMonkeToRGB(_Colour1, "R");
					ConvertMonkeToRGB(_Colour2, "G");
					ConvertMonkeToRGB(_Colour3, "B");
					Plugin.instance.Page = 3;
				}
				UpdateScreen();
			}

		}
		public void ConvertMonkeToRGB(float ColourToChange, string Change)
		{
			//my code is still kinda bad 8/21/2022

			float ColorToActuallySetForFucksSakeMyCodeIsSoMessy = rgbList[(int)ColourToChange];

			switch (Change)
            {
				case "R":
					RGBRed = ColorToActuallySetForFucksSakeMyCodeIsSoMessy;
					break;
				case "G":
					RGBGreen = ColorToActuallySetForFucksSakeMyCodeIsSoMessy;
					break;
				case "B":
					RGBBlue = ColorToActuallySetForFucksSakeMyCodeIsSoMessy;
					break;
			}
		}

		public void SetBasicColour(int ColourStage1, int ColourStage2, int ColourToSet)
		{
			switch (Plugin.instance.ColourStage)
            {
				case 0:
					Colour1 = ColourToSet;
					break;
				case 1:
					Colour2 = ColourToSet;
					break;
				case 2:
					Colour3 = ColourToSet;
					break;
			}

			if (Plugin.instance.ColourStage == ColourStage1)
				Plugin.instance.ColourStage = ColourStage2;

			FixedColour1 = Colour1 / 9f;
			FixedColour2 = Colour2 / 9f;
			FixedColour3 = Colour3 / 9f;
		}

		public void SetBasicColour2(int ColourStage1, int ColourStage2, int ColourToSet)
		{
			switch (Plugin.instance.ColourStage2)
			{
				case 0:
					_Colour1 = ColourToSet;
					break;
				case 1:
					_Colour2 = ColourToSet;
					break;
				case 2:
					_Colour3 = ColourToSet;
					break;
			}

			if (Plugin.instance.ColourStage2 == ColourStage1)
				Plugin.instance.ColourStage2 = ColourStage2;

			_FixedColour1 = Colour1 / 9f;
			_FixedColour2 = Colour2 / 9f;
			_FixedColour3 = Colour3 / 9f;
		}
	}
}
