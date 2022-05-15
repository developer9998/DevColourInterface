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

#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable IDE0017 // Simplify object initialization

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

				if (Plugin.Page == 0)
				{
					DrawBody(str);
				} 
				else
				if (Plugin.Page == 1)
				{
					DrawSetMonke(str);
				}
				else
				if (Plugin.Page == 2)
				{
					DrawBodyConvert(str);
				}
				else
				if (Plugin.Page == 3)
				{
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

			if (Plugin.ColourStage2 == 0)
			{
				str.AppendClr(_Colour1.ToString(), highlightColor).EndColor().AppendLine();
				str.AppendClr(_Colour2.ToString(), "FFFFFFFF").EndColor().AppendLine();
				str.AppendClr(_Colour3.ToString(), "FFFFFFFF").EndColor().AppendLine();
			}
			else
			if (Plugin.ColourStage2 == 1)
			{
				str.AppendClr(_Colour1.ToString(), "FFFFFFFF").EndColor().AppendLine();
				str.AppendClr(_Colour2.ToString(), highlightColor).EndColor().AppendLine();
				str.AppendClr(_Colour3.ToString(), "FFFFFFFF").EndColor().AppendLine();
			}
			else
			if (Plugin.ColourStage2 == 2)
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

			if (Plugin.ColourStage == 0)
            {
				str.AppendClr(Colour1.ToString(), highlightColor).EndColor().AppendLine();
				str.AppendClr(Colour2.ToString(), "FFFFFFFF").EndColor().AppendLine();
				str.AppendClr(Colour3.ToString(), "FFFFFFFF").EndColor().AppendLine();
			}
			else
			if (Plugin.ColourStage == 1)
			{
				str.AppendClr(Colour1.ToString(), "FFFFFFFF").EndColor().AppendLine();
				str.AppendClr(Colour2.ToString(), highlightColor).EndColor().AppendLine();
				str.AppendClr(Colour3.ToString(), "FFFFFFFF").EndColor().AppendLine();
			}
			else
			if (Plugin.ColourStage == 2)
			{
				str.AppendClr(Colour1.ToString(), "FFFFFFFF").EndColor().AppendLine();
				str.AppendClr(Colour2.ToString(), "FFFFFFFF").EndColor().AppendLine();
				str.AppendClr(Colour3.ToString(), highlightColor).EndColor().AppendLine();
			}

			str.EndAlign();

		}

		private void OnEntrySelected(int index)
		{
			if (Plugin.Page == 0)
            {
				switch (index)
				{
					case 0:
						Plugin.Page = 1;
						UpdateScreen();
						break;
					case 1:
						Plugin.Page = 2;
						UpdateScreen();
						break;
				}
			}
		}

		public override void OnKeyPressed(EKeyboardKey key)
		{
			if (Plugin.Page == 3)
            {
				if (_selectionHandler.HandleKeypress(key))
				{
					UpdateScreen();
					return;
				}

				if (key == EKeyboardKey.Back)
				{
					Plugin.ColourStage2 = 0;
					Plugin.Page = 0;
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
					Plugin.SaveFile(RGBRed, RGBGreen, RGBBlue, _Colour1, _Colour2, _Colour3);
					UpdateScreen();
				}
			}
			else
			if (Plugin.Page == 0)
			{
				if (_selectionHandler.HandleKeypress(key))
				{
					UpdateScreen();
					return;
				}

				if (key == EKeyboardKey.Back)
				{
					Plugin.ColourStage = 0;
					ReturnView();
				}
			}
			else
			if (Plugin.Page == 1)
            {
				if (key == EKeyboardKey.Back)
				{
					if (Plugin.ColourStage == 0 || Plugin.ColourStage == 3)
					{
						Plugin.ColourStage = 0;
						Plugin.Page = 0;
						Colour1 = 0f;
						FixedColour1 = 0f;
						Colour2 = 0f;
						FixedColour2 = 0f;
						Colour3 = 0f;
						FixedColour3 = 0f;
						Plugin.UpdatePreviewColours();
						UpdateScreen();
					}
					else
					if (Plugin.ColourStage == 2)
					{
						Plugin.ColourStage = 1;
						Colour2 = 0f;
						FixedColour2 = 0f;
						//Plugin.UpdatePreviewColoursFloat(FixedColour1, FixedColour2, FixedColour3);
						UpdateScreen();
					}
					else
					if (Plugin.ColourStage == 1)
					{
						Plugin.ColourStage = 0;
						Colour1 = 0f;
						FixedColour1 = 0f;
						Colour2 = 0f;
						FixedColour2 = 0f;
						//Plugin.UpdatePreviewColoursFloat(FixedColour1, FixedColour2, FixedColour3);
						UpdateScreen();
					}
				}
			}
			else
			if (Plugin.Page == 2)
			{
				if (key == EKeyboardKey.Back)
				{
					if (Plugin.ColourStage2 == 0 || Plugin.ColourStage2 == 3)
					{
						Plugin.ColourStage2 = 0;
						Plugin.Page = 0;
						_Colour1 = 0f;
						_FixedColour1 = 0f;
						_Colour2 = 0f;
						_FixedColour2 = 0f;
						_Colour3 = 0f;
						_FixedColour3 = 0f;
						UpdateScreen();
					}
					else
					if (Plugin.ColourStage2 == 2)
					{
						Plugin.ColourStage2 = 1;
						_Colour2 = 0f;
						_FixedColour2 = 0f;
						//Plugin.UpdatePreviewColoursFloat(FixedColour1, FixedColour2, FixedColour3);
						UpdateScreen();
					}
					else
					if (Plugin.ColourStage2 == 1)
					{
						Plugin.ColourStage2 = 0;
						_Colour1 = 0f;
						_FixedColour1 = 0f;
						_Colour2 = 0f;
						_FixedColour2 = 0f;
						//Plugin.UpdatePreviewColoursFloat(FixedColour1, FixedColour2, FixedColour3);
						UpdateScreen();
					}
				}
			}

			if (Plugin.Page == 1)
            {
				if (Plugin.ColourStage == 0)
                {
					if (key == EKeyboardKey.NUM0)
					{
						SetBasicColour(0, 1, 0);
					}
					if (key == EKeyboardKey.NUM1)
					{
						SetBasicColour(0, 1, 1);
					}
					if (key == EKeyboardKey.NUM2)
					{
						SetBasicColour(0, 1, 2);
					}
					if (key == EKeyboardKey.NUM3)
					{
						SetBasicColour(0, 1, 3);
					}
					if (key == EKeyboardKey.NUM4)
					{
						SetBasicColour(0, 1, 4);
					}
					if (key == EKeyboardKey.NUM5)
					{
						SetBasicColour(0, 1, 5);
					}
					if (key == EKeyboardKey.NUM6)
					{
						SetBasicColour(0, 1, 6);
					}
					if (key == EKeyboardKey.NUM7)
					{
						SetBasicColour(0, 1, 7);
					}
					if (key == EKeyboardKey.NUM8)
					{
						SetBasicColour(0, 1, 8);
					}
					if (key == EKeyboardKey.NUM9)
					{
						SetBasicColour(0, 1, 9);
					}
				}
				else
				if (Plugin.ColourStage == 1)
				{
					if (key == EKeyboardKey.NUM0)
					{
						SetBasicColour(1, 2, 0);
					}
					if (key == EKeyboardKey.NUM1)
					{
						SetBasicColour(1, 2, 1);
					}
					if (key == EKeyboardKey.NUM2)
					{
						SetBasicColour(1, 2, 2);
					}
					if (key == EKeyboardKey.NUM3)
					{
						SetBasicColour(1, 2, 3);
					}
					if (key == EKeyboardKey.NUM4)
					{
						SetBasicColour(1, 2, 4);
					}
					if (key == EKeyboardKey.NUM5)
					{
						SetBasicColour(1, 2, 5);
					}
					if (key == EKeyboardKey.NUM6)
					{
						SetBasicColour(1, 2, 6);
					}
					if (key == EKeyboardKey.NUM7)
					{
						SetBasicColour(1, 2, 7);
					}
					if (key == EKeyboardKey.NUM8)
					{
						SetBasicColour(1, 2, 8);
					}
					if (key == EKeyboardKey.NUM9)
					{
						SetBasicColour(1, 2, 9);
					}
				}
				else
				if (Plugin.ColourStage == 2)
				{
					if (key == EKeyboardKey.NUM0)
					{
						SetBasicColour(2, 3, 0);
					}
					if (key == EKeyboardKey.NUM1)
					{
						SetBasicColour(2, 3, 1);
					}
					if (key == EKeyboardKey.NUM2)
					{
						SetBasicColour(2, 3, 2);
					}
					if (key == EKeyboardKey.NUM3)
					{
						SetBasicColour(2, 3, 3);
					}
					if (key == EKeyboardKey.NUM4)
					{
						SetBasicColour(2, 3, 4);
					}
					if (key == EKeyboardKey.NUM5)
					{
						SetBasicColour(2, 3, 5);
					}
					if (key == EKeyboardKey.NUM6)
					{
						SetBasicColour(2, 3, 6);
					}
					if (key == EKeyboardKey.NUM7)
					{
						SetBasicColour(2, 3, 7);
					}
					if (key == EKeyboardKey.NUM8)
					{
						SetBasicColour(2, 3, 8);
					}
					if (key == EKeyboardKey.NUM9)
					{
						SetBasicColour(2, 3, 9);
					}
				}
				if (Plugin.ColourStage == 3)
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
							trueColor.b
						});
					}
					Plugin.UpdatePreviewColoursFloat(trueColor.r, trueColor.g, trueColor.b);
					Plugin.UpdateText();
					Colour1 = 0f;
					Colour2 = 0f;
					Colour3 = 0f;
					FixedColour1 = 0f;
					FixedColour2 = 0f;
					FixedColour3 = 0f;
					Plugin.Page = 0;
					Plugin.ColourStage = 0;
				}
				UpdateScreen();
			}

			if (Plugin.Page == 2)
			{
				if (Plugin.ColourStage2 == 0)
				{
					if (key == EKeyboardKey.NUM0)
					{
						SetBasicColour2(0, 1, 0);
					}
					if (key == EKeyboardKey.NUM1)
					{
						SetBasicColour2(0, 1, 1);
					}
					if (key == EKeyboardKey.NUM2)
					{
						SetBasicColour2(0, 1, 2);
					}
					if (key == EKeyboardKey.NUM3)
					{
						SetBasicColour2(0, 1, 3);
					}
					if (key == EKeyboardKey.NUM4)
					{
						SetBasicColour2(0, 1, 4);
					}
					if (key == EKeyboardKey.NUM5)
					{
						SetBasicColour2(0, 1, 5);
					}
					if (key == EKeyboardKey.NUM6)
					{
						SetBasicColour2(0, 1, 6);
					}
					if (key == EKeyboardKey.NUM7)
					{
						SetBasicColour2(0, 1, 7);
					}
					if (key == EKeyboardKey.NUM8)
					{
						SetBasicColour2(0, 1, 8);
					}
					if (key == EKeyboardKey.NUM9)
					{
						SetBasicColour2(0, 1, 9);
					}
				}
				else
				if (Plugin.ColourStage2 == 1)
				{
					if (key == EKeyboardKey.NUM0)
					{
						SetBasicColour2(1, 2, 0);
					}
					if (key == EKeyboardKey.NUM1)
					{
						SetBasicColour2(1, 2, 1);
					}
					if (key == EKeyboardKey.NUM2)
					{
						SetBasicColour2(1, 2, 2);
					}
					if (key == EKeyboardKey.NUM3)
					{
						SetBasicColour2(1, 2, 3);
					}
					if (key == EKeyboardKey.NUM4)
					{
						SetBasicColour2(1, 2, 4);
					}
					if (key == EKeyboardKey.NUM5)
					{
						SetBasicColour2(1, 2, 5);
					}
					if (key == EKeyboardKey.NUM6)
					{
						SetBasicColour2(1, 2, 6);
					}
					if (key == EKeyboardKey.NUM7)
					{
						SetBasicColour2(1, 2, 7);
					}
					if (key == EKeyboardKey.NUM8)
					{
						SetBasicColour2(1, 2, 8);
					}
					if (key == EKeyboardKey.NUM9)
					{
						SetBasicColour2(1, 2, 9);
					}
				}
				else
				if (Plugin.ColourStage2 == 2)
				{
					if (key == EKeyboardKey.NUM0)
					{
						SetBasicColour2(2, 3, 0);
					}
					if (key == EKeyboardKey.NUM1)
					{
						SetBasicColour2(2, 3, 1);
					}
					if (key == EKeyboardKey.NUM2)
					{
						SetBasicColour2(2, 3, 2);
					}
					if (key == EKeyboardKey.NUM3)
					{
						SetBasicColour2(2, 3, 3);
					}
					if (key == EKeyboardKey.NUM4)
					{
						SetBasicColour2(2, 3, 4);
					}
					if (key == EKeyboardKey.NUM5)
					{
						SetBasicColour2(2, 3, 5);
					}
					if (key == EKeyboardKey.NUM6)
					{
						SetBasicColour2(2, 3, 6);
					}
					if (key == EKeyboardKey.NUM7)
					{
						SetBasicColour2(2, 3, 7);
					}
					if (key == EKeyboardKey.NUM8)
					{
						SetBasicColour2(2, 3, 8);
					}
					if (key == EKeyboardKey.NUM9)
					{
						SetBasicColour2(2, 3, 9);
					}
				}
				if (Plugin.ColourStage2 == 3)
				{
					ConvertMonkeToRGB(_Colour1, "R");
					ConvertMonkeToRGB(_Colour2, "G");
					ConvertMonkeToRGB(_Colour3, "B");
					Plugin.Page = 3;
				}
				UpdateScreen();
			}

		}
		public void ConvertMonkeToRGB(float ColourToChange, string Change)
		{
			float ColorToActuallySetForFucksSakeMyCodeIsSoMessy;
			if (ColourToChange == 0)
			{
				ColorToActuallySetForFucksSakeMyCodeIsSoMessy = 0;
				//return 0;
			}
			else
			if (ColourToChange == 1)
			{
				ColorToActuallySetForFucksSakeMyCodeIsSoMessy = 28;
				//return 28;
			}
			else
			if (ColourToChange == 2)
			{
				ColorToActuallySetForFucksSakeMyCodeIsSoMessy = 57;
				//return 57;
			}
			else
			if (ColourToChange == 3)
			{
				ColorToActuallySetForFucksSakeMyCodeIsSoMessy = 85;
				//return 85;
			}
			else
			if (ColourToChange == 4)
			{
				ColorToActuallySetForFucksSakeMyCodeIsSoMessy = 113;
				//return 113;
			}
			else
			if (ColourToChange == 5)
			{
				ColorToActuallySetForFucksSakeMyCodeIsSoMessy = 142;
				//return 142;
			}
			else
			if (ColourToChange == 6)
			{
				ColorToActuallySetForFucksSakeMyCodeIsSoMessy = 170;
				//return 170;
			}
			else
			if (ColourToChange == 7)
			{
				ColorToActuallySetForFucksSakeMyCodeIsSoMessy = 198;
				//return 198;
			}
			else
			if (ColourToChange == 8)
			{
				ColorToActuallySetForFucksSakeMyCodeIsSoMessy = 227;
				//return 227;
			}
			else
			if (ColourToChange == 9)
			{
				ColorToActuallySetForFucksSakeMyCodeIsSoMessy = 255;
				//return 255;
			}
			else
			{
				ColorToActuallySetForFucksSakeMyCodeIsSoMessy = 255;
				//return 255;
			}

			if (Change == "R")
			{
				RGBRed = ColorToActuallySetForFucksSakeMyCodeIsSoMessy;
			}
			else
			if (Change == "G")
			{
				RGBGreen = ColorToActuallySetForFucksSakeMyCodeIsSoMessy;
			}
			else
			if (Change == "B")
			{
				RGBBlue = ColorToActuallySetForFucksSakeMyCodeIsSoMessy;
			}
			//return ColourThatChanged;
		}

		public void SetBasicColour(int ColourStage1, int ColourStage2, int ColourToSet)
		{
			if (Plugin.ColourStage == 0)
			{
				Colour1 = ColourToSet;
			}
			else
				if (Plugin.ColourStage == 1)
			{
				Colour2 = ColourToSet;
			}
			else
				if (Plugin.ColourStage == 2)
			{
				Colour3 = ColourToSet;
			}

			if (Plugin.ColourStage == ColourStage1)
			{
				Plugin.ColourStage = ColourStage2;
			}

			FixedColour1 = (float)Colour1 / 9f;
			FixedColour2 = (float)Colour2 / 9f;
			FixedColour3 = (float)Colour3 / 9f;

		}

		public void SetBasicColour2(int ColourStage1, int ColourStage2, int ColourToSet)
		{
			if (Plugin.ColourStage2 == 0)
			{
				_Colour1 = ColourToSet;
			}
			else
			if (Plugin.ColourStage2 == 1)
			{
				_Colour2 = ColourToSet;
			}
			else
			if (Plugin.ColourStage2 == 2)
			{
				_Colour3 = ColourToSet;
			}

			if (Plugin.ColourStage2 == ColourStage1)
			{
				Plugin.ColourStage2 = ColourStage2;
			}

			_FixedColour1 = (float)Colour1 / 9f;
			_FixedColour2 = (float)Colour2 / 9f;
			_FixedColour3 = (float)Colour3 / 9f;

		}

	}
}
