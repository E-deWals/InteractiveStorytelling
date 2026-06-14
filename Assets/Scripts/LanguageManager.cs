using TMPro;
using UnityEngine;
using Yarn.Unity;

public class LanguageManager : MonoBehaviour
{
    [Header("UI Setup")]
    public TMP_Dropdown languageDropdown;
    public TMP_Dropdown voiceDropdown;

    [Header("Text Languages")]
    public string[] supportedTextLanguages = { "en", "nl", "fr" };

    private BuiltinLocalisedLineProvider lineProvider;
    private DialogueRunner dialogueRunner;
    private FMODLineProvider fmodLineProvider;

    private void Start()
    {
        lineProvider = FindAnyObjectByType<BuiltinLocalisedLineProvider>();
        dialogueRunner = FindAnyObjectByType<DialogueRunner>();
        fmodLineProvider = FindAnyObjectByType<FMODLineProvider>();

        if (lineProvider == null)
            Debug.LogWarning("BuiltinLocalisedLineProvider not found in scene!");
        if (dialogueRunner == null)
            Debug.LogWarning("DialogueRunner not found in scene!");
        if (fmodLineProvider == null)
            Debug.LogWarning("FMODLineProvider not found in scene!");

        if (languageDropdown != null)
            languageDropdown.onValueChanged.AddListener(OnTextLanguageChanged);

        if (voiceDropdown != null)
        {
            // Sync the dropdown to whatever language is already set on the component
            voiceDropdown.value = (int)fmodLineProvider.CurrentVoiceLanguage;
            voiceDropdown.onValueChanged.AddListener(OnVoiceLanguageChanged);
        }
    }

    // ?? Text language ????????????????????????????????????????????????????????

    public void SetTextLanguage(string languageCode)
    {
        if (lineProvider == null) return;

        lineProvider.LocaleCode = languageCode;
        Debug.Log($"Text language switched to: {languageCode}");

        // Restart the current node so the new locale takes effect immediately
        if (dialogueRunner != null && dialogueRunner.IsDialogueRunning)
        {
            string currentNode = dialogueRunner.Dialogue.CurrentNode;
            dialogueRunner.Stop();
            dialogueRunner.StartDialogue(currentNode);
        }
    }

    private void OnTextLanguageChanged(int index)
    {
        if (index >= 0 && index < supportedTextLanguages.Length)
            SetTextLanguage(supportedTextLanguages[index]);
    }

    // ?? Voice language ???????????????????????????????????????????????????????

    public void SetVoiceLanguage(FMODLineProvider.VoiceLanguage lang)
    {
        if (fmodLineProvider == null) return;

        fmodLineProvider.SetVoiceLanguage(lang);
        Debug.Log($"Voice language switched to: {lang}");
    }

    private void OnVoiceLanguageChanged(int index)
    {
        // The dropdown index maps directly to the VoiceLanguage enum (EN = 0, ALT = 1)
        if (System.Enum.IsDefined(typeof(FMODLineProvider.VoiceLanguage), index))
            SetVoiceLanguage((FMODLineProvider.VoiceLanguage)index);
    }

}
