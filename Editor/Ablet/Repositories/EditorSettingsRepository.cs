using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Ablet.Repositories
{
    public enum NdmfInteropMode
    {
        None = 0,
        AbletOnNdmf = 1,
        NdmfOnAblet = 2
    }

    [Serializable]
    public class EditorSettings
    {
        [SerializeField] bool applyOnPlay = true;
        [SerializeField] NdmfInteropMode ndmfInteropMode = NdmfInteropMode.AbletOnNdmf;
        [SerializeField] bool preferAblet = false;

        public bool ApplyOnPlay
        {
            get => applyOnPlay;
            set => applyOnPlay = value;
        }

        public bool IsNdmfOnAblet => NdmfInteropMode is NdmfInteropMode.NdmfOnAblet; 
        public bool IsAbletOnNdmf => NdmfInteropMode is NdmfInteropMode.AbletOnNdmf;

        public NdmfInteropMode NdmfInteropMode
        {
            get => ndmfInteropMode;
            set => ndmfInteropMode = value;
        }

        public bool PreferAblet
        {
            get => preferAblet;
            set => preferAblet = value;
        }
    }

    public class EditorSettingsRepository
    {
        public static readonly EditorSettingsRepository Instance = new EditorSettingsRepository();

        const string SettingsPath = "ProjectSettings/Packages/net.kaikoga.ablet/settings.json";

        EditorSettings _value;

        public EditorSettings Value => _value ?? Load();
        EditorSettings Load()
        {
            try
            {
                var json = File.ReadAllText(SettingsPath, Encoding.UTF8);
                _value = JsonUtility.FromJson<EditorSettings>(json);
            }
            catch
            {
                _value = new EditorSettings();
            }
            return _value;
        }

        public void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
            File.WriteAllText(SettingsPath, JsonUtility.ToJson(_value), Encoding.UTF8);
        }
    }
}
