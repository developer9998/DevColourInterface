using BepInEx;
using Bepinject;
using System;
using System.IO;
using System.Text;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using ComputerInterface;

namespace DevColourInterface
{
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInDependency("tonimacaroni.computerinterface", "1.4.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        //for new assetloading mod makers, make sure your file is an embedded resource in the properties!!

        public static Plugin instance;
        public int Page = 0;
        public int ColourStage = 0;
        public int ColourStage2 = 0;
        public GameObject colourPreview;
        public GameObject colourPreviewIgloo;
        public string fileLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        void Awake()
        {
            instance = this;
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

            colourPreviewIgloo = Instantiate(colourPreviewGameObject);
            colourPreviewIgloo.transform.GetChild(0).gameObject.AddComponent<ViewText>();
            colourPreviewIgloo.transform.GetChild(1).gameObject.AddComponent<ViewText>();
            colourPreviewIgloo.transform.GetChild(2).gameObject.AddComponent<ViewText>();
            colourPreviewIgloo.transform.GetChild(3).gameObject.AddComponent<ViewText>();

            colourPreviewIgloo.transform.position = new Vector3(-27.25f, 18.25f, - 94.3101f);
            colourPreviewIgloo.transform.GetChild(4).transform.rotation = Quaternion.Euler(0f, 313.8699f, 0f);
            colourPreviewIgloo.transform.GetChild(4).GetChild(0).GetComponent<Text>().enabled = false;
            colourPreviewIgloo.transform.GetChild(4).GetChild(1).GetComponent<Text>().enabled = false;
            colourPreviewIgloo.transform.GetChild(4).GetChild(2).GetComponent<Text>().enabled = false;
            colourPreviewIgloo.transform.GetChild(4).GetChild(3).GetComponent<Text>().enabled = false;

            UpdatePreviewColours();
            UpdateText();
        }

        public void SaveFile(float red, float green, float blue, float redAlt, float greenAlt, float blueAlt)
        {

            DirectoryInfo directory = new DirectoryInfo(fileLocation + "\\ColourData");

            if (!Directory.Exists(directory.ToString()))
            {
                Directory.CreateDirectory(directory.ToString());
            }

            FileInfo file = new FileInfo(fileLocation + "\\ColourData\\" + "ColourData_" + DateTime.Now.ToString("yyyy_MM_dd-mm_ss") + ".txt");

            if (!File.Exists(file.ToString()))
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("Data generated with DevColourInterface mod (discord.gg/monkemod)").AppendLines(1);
                sb.AppendLine($"{redAlt} ▶ {red}");
                sb.AppendLine($"{greenAlt} ▶ {green}");
                sb.AppendLine($"{blueAlt} ▶ {blue}");
                sb.Append("You can help with the mod by submitting feedback in my Discord: https://discord.gg/tNKt8xwrcx");
                string content = sb.ToString();
                //string content = $"Information generated with DevColourInterface mod (discord.gg/monkemod)\n\n{(redAlt)} ▶ {(red)}\n{(greenAlt)} ▶ {(green)}\n{(blueAlt)} ▶ {(blue)}\n\nYou can help with the mod by submitting feedback in my Discord: https://discord.gg/tNKt8xwrcx";
                File.WriteAllText(file.ToString(), content);
            }

            //string path = fileLocation + "\\SavedColours\\" + System.DateTime.Now.ToString("yyyy_MM_dd-mm_ss") + ".txt";

            //string content = $"Information generated with DevColourInterface mod (discord.gg/monkemod)\n\n{(redAlt)} ▶ {(red)}\n{(greenAlt)} ▶ {(green)}\n{(blueAlt)} ▶ {(blue)}\n\nYou can help with the mod by submitting feedback in my Discord: https://discord.gg/tNKt8xwrcx";
            //File.WriteAllText(path, content);
        }

        public void UpdatePreviewColoursFloat(float red, float green, float blue)
        {
            colourPreview.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(red, 0, 0);
            colourPreview.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = new Color(0, green, 0);
            colourPreview.transform.GetChild(2).GetComponent<MeshRenderer>().material.color = new Color(0, 0, blue);
            colourPreview.transform.GetChild(3).GetComponent<MeshRenderer>().material.color = new Color(red, green, blue);

            colourPreviewIgloo.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(red, 0, 0);
            colourPreviewIgloo.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = new Color(0, green, 0);
            colourPreviewIgloo.transform.GetChild(2).GetComponent<MeshRenderer>().material.color = new Color(0, 0, blue);
            colourPreviewIgloo.transform.GetChild(3).GetComponent<MeshRenderer>().material.color = new Color(red, green, blue);
        }

        public void UpdateText()
        {
            colourPreview.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = $"{PlayerPrefs.GetFloat("redValue") * 9f}";
            colourPreview.transform.GetChild(4).GetChild(1).GetComponent<Text>().text = $"{PlayerPrefs.GetFloat("greenValue") * 9f}";
            colourPreview.transform.GetChild(4).GetChild(2).GetComponent<Text>().text = $"{PlayerPrefs.GetFloat("blueValue") * 9f}";
            colourPreview.transform.GetChild(4).GetChild(3).GetComponent<Text>().text = $"{PlayerPrefs.GetFloat("redValue") * 9f}{PlayerPrefs.GetFloat("greenValue") * 9f}{PlayerPrefs.GetFloat("blueValue") * 9f}";

            colourPreviewIgloo.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = $"{PlayerPrefs.GetFloat("redValue") * 9f}";
            colourPreviewIgloo.transform.GetChild(4).GetChild(1).GetComponent<Text>().text = $"{PlayerPrefs.GetFloat("greenValue") * 9f}";
            colourPreviewIgloo.transform.GetChild(4).GetChild(2).GetComponent<Text>().text = $"{PlayerPrefs.GetFloat("blueValue") * 9f}";
            colourPreviewIgloo.transform.GetChild(4).GetChild(3).GetComponent<Text>().text = $"{PlayerPrefs.GetFloat("redValue") * 9f}{PlayerPrefs.GetFloat("greenValue") * 9f}{PlayerPrefs.GetFloat("blueValue") * 9f}";
        }

        public void UpdatePreviewColours()
        {
            colourPreview.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(PlayerPrefs.GetFloat("redValue"), 0, 0);
            colourPreview.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = new Color(0, PlayerPrefs.GetFloat("greenValue"), 0);
            colourPreview.transform.GetChild(2).GetComponent<MeshRenderer>().material.color = new Color(0, 0, PlayerPrefs.GetFloat("blueValue"));
            colourPreview.transform.GetChild(3).GetComponent<MeshRenderer>().material.color = new Color(PlayerPrefs.GetFloat("redValue"), PlayerPrefs.GetFloat("greenValue"), PlayerPrefs.GetFloat("blueValue"));

            colourPreviewIgloo.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(PlayerPrefs.GetFloat("redValue"), 0, 0);
            colourPreviewIgloo.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = new Color(0, PlayerPrefs.GetFloat("greenValue"), 0);
            colourPreviewIgloo.transform.GetChild(2).GetComponent<MeshRenderer>().material.color = new Color(0, 0, PlayerPrefs.GetFloat("blueValue"));
            colourPreviewIgloo.transform.GetChild(3).GetComponent<MeshRenderer>().material.color = new Color(PlayerPrefs.GetFloat("redValue"), PlayerPrefs.GetFloat("greenValue"), PlayerPrefs.GetFloat("blueValue"));
        }

        public class ViewText : MonoBehaviour
        {
            void Start()
            {
                gameObject.layer = 18;
            }

            void Update()
            {
                gameObject.transform.Rotate(20 * Time.deltaTime, 20 * Time.deltaTime, 20 * Time.deltaTime);
            }

            public void VibrateHand(string objectName)
            {
                GorillaTagger.Instance.StartVibration(objectName != "RightHandTriggerCollider", GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
            }

            private void OnTriggerEnter(Collider other)
            {
                VibrateHand(other.transform.name);
                SetText("RPreview", 0, true);
                SetText("GPreview", 1, true);
                SetText("BPreview", 2, true);
                SetText("Preview", 3, true);
            }

            private void OnTriggerExit(Collider other)
            {
                SetText("RPreview", 0, false);
                SetText("GPreview", 1, false);
                SetText("BPreview", 2, false);
                SetText("Preview", 3, false);
            }

            void SetText(string thename, int getChild, bool setEnable)
            {
                if (gameObject.name == thename)
                {
                    gameObject.transform.parent.GetChild(4).GetChild(getChild).gameObject.GetComponent<Text>().enabled = setEnable;
                }
            }

        }
    }
}
