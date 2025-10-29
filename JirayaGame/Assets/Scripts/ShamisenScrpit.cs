using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShamisenScriptMelodia : MonoBehaviour
{
    [SerializeField] AudioSource shamisenAudioSource;

    // Diccionario de notas semitonos desde La4
    private Dictionary<string, int> noteOffsets = new Dictionary<string, int>()
    {
        { "do4", -9 }, { "do#4", -8 },
        { "re4", -7 }, { "re#4", -6 },
        { "mi4", -5 },
        { "fa4", -4 }, { "fa#4", -3 },
        { "sol4", -2 }, { "sol#4", -1 },
        { "la4", 0 }, { "la#4", 1 },
        { "si4", 2 },
        { "do5", 3 }, { "do#5", 4 },
        { "re5", 5 }, { "re#5", 6 },
        { "mi5", 7 }, { "fa5", 8 },
        { "fa#5", 9 }, { "sol5", 10 },
        { "sol#5", 11 }, { "la5", 12 }
    };

    //Melodia
    [SerializeField]
    string[] melodia = new string[]
    {
        "do4","do4","do4","re#4",
        "do4", "do4","do4","do4","re#4",
        "do4","fa#4","fa4",
        "re#4","fa4"
    };

    [SerializeField] float duracionNota = 0.3f;

    private Dictionary<KeyCode, string> keyNoteMap = new Dictionary<KeyCode, string>()
    {
        { KeyCode.A, "do4" },
        { KeyCode.W, "do#4" },
        { KeyCode.S, "re4" },
        { KeyCode.E, "re#4" },
        { KeyCode.D, "mi4" },
        { KeyCode.F, "fa4" },
        { KeyCode.T, "fa#4" },
        { KeyCode.G, "sol4" },
        { KeyCode.Y, "sol#4" },
        { KeyCode.H, "la4" },
        { KeyCode.U, "la#4" },
        { KeyCode.J, "si4" },
        { KeyCode.K, "do5" }
    };

    void Start()
    {
        StartCoroutine(TocarMelodia());
    }

    void Update()
    {
        foreach (var kvp in keyNoteMap)
        {
            if (Input.GetKeyDown(kvp.Key))
                PlayNoteByName(kvp.Value);
        }
    }

    private IEnumerator TocarMelodia()
    {
        foreach (string nota in melodia)
        {
            PlayNoteByName(nota);
            yield return new WaitForSeconds(duracionNota);
        }
    }

    public void PlayNoteByName(string noteName)
    {
        if (!noteOffsets.ContainsKey(noteName.ToLower()))
        {
            Debug.LogWarning($"Nota no encontrada: {noteName}");
            return;
        }

        int semitoneOffset = noteOffsets[noteName.ToLower()];
        shamisenAudioSource.pitch = Mathf.Pow(2f, semitoneOffset / 12f);
        shamisenAudioSource.Play();
    }
}
