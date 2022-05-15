using BepInEx;
using Bepinject;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace DevColourInterface
{
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInDependency("tonimacaroni.computerinterface", "1.4.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        //for new assetloading mod makers, make sure your file is an embedded resource in the properties!!

        public static int Page = 0;
        public static int ColourStage = 0;
        public static int ColourStage2 = 0;
        public static GameObject colourPreview;
        public static string fileLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static void SaveFile(float red, float green, float blue, float redAlt, float greenAlt, float blueAlt)
        {
            string path = fileLocation + "\\SavedColours\\" + System.DateTime.Now.ToString("yyyy_MM_dd-mm_ss") + ".txt";

            string content = $"Information generated with DevColourInterface mod (discord.gg/monkemod)\n\n{(redAlt)} ▶ {(red)}\n{(greenAlt)} ▶ {(green)}\n{(blueAlt)} ▶ {(blue)}";
            File.WriteAllText(path, content);
        }

        public static void UpdateText()
        {
            colourPreview.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = $"{PlayerPrefs.GetFloat("redValue") * 9f}";
            colourPreview.transform.GetChild(4).GetChild(1).GetComponent<Text>().text = $"{PlayerPrefs.GetFloat("greenValue") * 9f}";
            colourPreview.transform.GetChild(4).GetChild(2).GetComponent<Text>().text = $"{PlayerPrefs.GetFloat("blueValue") * 9f}";
            colourPreview.transform.GetChild(4).GetChild(3).GetComponent<Text>().text = $"{PlayerPrefs.GetFloat("redValue") * 9f}{PlayerPrefs.GetFloat("greenValue") * 9f}{PlayerPrefs.GetFloat("blueValue") * 9f}";
        }

        public static void UpdatePreviewColours()
        {
            colourPreview.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(PlayerPrefs.GetFloat("redValue"), 0, 0);
            colourPreview.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = new Color(0, PlayerPrefs.GetFloat("greenValue"), 0);
            colourPreview.transform.GetChild(2).GetComponent<MeshRenderer>().material.color = new Color(0, 0, PlayerPrefs.GetFloat("blueValue"));
            colourPreview.transform.GetChild(3).GetComponent<MeshRenderer>().material.color = new Color(PlayerPrefs.GetFloat("redValue"), PlayerPrefs.GetFloat("greenValue"), PlayerPrefs.GetFloat("blueValue"));
        }

        public static void UpdatePreviewColoursFloat(float red, float green, float blue)
        {
            colourPreview.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(red, 0, 0);
            colourPreview.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = new Color(0, green, 0);
            colourPreview.transform.GetChild(2).GetComponent<MeshRenderer>().material.color = new Color(0, 0, blue);
            colourPreview.transform.GetChild(3).GetComponent<MeshRenderer>().material.color = new Color(red, green, blue);
        }

        void Awake()
        {
            Zenjector.Install<MainInstaller>().OnProject();
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("DevColourInterface.Resources.preview");
            AssetBundle bundle = AssetBundle.LoadFromStream(str);
            GameObject colourPreviewGameObject = bundle.LoadAsset<GameObject>("ColourPreview");
            colourPreview = Instantiate(colourPreviewGameObject);

            colourPreview.transform.GetChild(0).gameObject.AddComponent<ViewText>();
            colourPreview.transform.GetChild(1).gameObject.AddComponent<ViewText>();
            colourPreview.transform.GetChild(2).gameObject.AddComponent<ViewText>();
            colourPreview.transform.GetChild(3).gameObject.AddComponent<ViewText>();

            colourPreview.transform.position = new Vector3(-69.16f, 12.2799f, -82.8401f);
            colourPreview.transform.GetChild(4).transform.rotation = Quaternion.Euler(0f, 280.9588f, 0f);
            colourPreview.transform.GetChild(4).GetChild(0).GetComponent<Text>().enabled = false;
            colourPreview.transform.GetChild(4).GetChild(1).GetComponent<Text>().enabled = false;
            colourPreview.transform.GetChild(4).GetChild(2).GetComponent<Text>().enabled = false;
            colourPreview.transform.GetChild(4).GetChild(3).GetComponent<Text>().enabled = false;

            UpdatePreviewColours();
            UpdateText();
        }

        void Update()
        {
            /* Code here runs every frame when the mod is enabled */
        }
    }
}
