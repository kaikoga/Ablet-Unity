using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using JetBrains.Annotations;
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
    [SuppressMessage("ReSharper", "RedundantDefaultMemberInitializer")]
    public class EditorSettingsValue
    {
        [SerializeField] int serializedVersion = 0;
        [SerializeField] bool autoOpenConsoleWindow = true;
        [SerializeField] bool applyOnPlay = true;
        [SerializeField] bool applyOnPlatformBuild = true;
        [SerializeField] NdmfInteropMode ndmfInteropMode = NdmfInteropMode.AbletOnNdmf;
        [SerializeField] bool preferAblet = false;
        [SerializeField] bool abletPreferNdmf = true;

        internal const int CurrentSerializedVersion = 1;

        [UsedImplicitly]
        public static EditorSettingsValue DefaultNonNdmf => new EditorSettingsValue
        {
            serializedVersion = CurrentSerializedVersion,
            autoOpenConsoleWindow = true,
            applyOnPlay = true,
            applyOnPlatformBuild = true,
            ndmfInteropMode = NdmfInteropMode.None,
            preferAblet = true,
            abletPreferNdmf = false,
        };

        [UsedImplicitly]
        public static EditorSettingsValue DefaultPreferAblet => new EditorSettingsValue
        {
            serializedVersion = CurrentSerializedVersion,
            autoOpenConsoleWindow = true,
            applyOnPlay = true,
            applyOnPlatformBuild = true,
            ndmfInteropMode = NdmfInteropMode.NdmfOnAblet,
            preferAblet = true,
            abletPreferNdmf = false,
        };

        [UsedImplicitly]
        public static EditorSettingsValue DefaultPreferNdmf => new EditorSettingsValue
        {
            serializedVersion = CurrentSerializedVersion,
            autoOpenConsoleWindow = false,
            applyOnPlay = false,
            applyOnPlatformBuild = false,
            ndmfInteropMode = NdmfInteropMode.AbletOnNdmf,
            preferAblet = false,
            abletPreferNdmf = true,
        };

        public void Validate()
        {
            if (abletPreferNdmf)
            {
                applyOnPlay = false;
                applyOnPlatformBuild = false;
                ndmfInteropMode = NdmfInteropMode.AbletOnNdmf;
                preferAblet = false;
            }
        }

        internal int SerializedVersion => serializedVersion;

        public bool AutoOpenConsoleWindow
        {
            get => autoOpenConsoleWindow;
            set => autoOpenConsoleWindow = value;
        }

        public bool ApplyOnPlay
        {
            get => applyOnPlay;
            set => applyOnPlay = value;
        }

        public bool ApplyOnPlatformBuild
        {
            get => applyOnPlatformBuild;
            set => applyOnPlatformBuild = value;
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

        public bool AbletPreferNdmf
        {
            get => abletPreferNdmf;
            set => abletPreferNdmf = value;
        }
    }

    public class EditorSettingsRepository
    {
        public static readonly EditorSettingsRepository Instance = new EditorSettingsRepository();

        const string SettingsPath = "ProjectSettings/Packages/net.kaikoga.ablet/settings.json";

        EditorSettingsValue? _value;

        public event Action? OnChanged;

        public EditorSettingsValue Value => _value ?? Load();
        EditorSettingsValue Load()
        {
            try
            {
                var json = File.ReadAllText(SettingsPath, Encoding.UTF8);
                _value = JsonUtility.FromJson<EditorSettingsValue>(json);
            }
            catch
            {
                _value = new EditorSettingsValue();
            }
            if (_value.SerializedVersion != EditorSettingsValue.CurrentSerializedVersion)
            {
#if ABLET_NDMF
                _value = EditorSettingsValue.DefaultPreferNdmf;
#else
                _value = EditorSettingsValue.DefaultNonNdmf;
#endif
            }
            return _value;
        }

        public void Save(EditorSettingsValue value)
        {
            value.Validate();
            _value = value;
            Save();
        }

        public void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
            File.WriteAllText(SettingsPath, JsonUtility.ToJson(_value), Encoding.UTF8);
            OnChanged?.Invoke();
        }
    }
}
