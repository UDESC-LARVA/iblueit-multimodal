using Ibit.Core.Serial;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PitacoValueText : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private SerialControllerPitaco _serialController;

    private void Awake()
    {
        if (_text is null)
            throw new ArgumentNullException(nameof(_text));

        if (_serialController is null)
            throw new ArgumentNullException(nameof(SerialControllerPitaco));

        _serialController.OnSerialMessageReceived += (str) =>
        {
            var pitacoValue = double.TryParse(str, out var parsed) ? parsed : 0.0;
            _text.text = pitacoValue.ToString();
        };
    }
}