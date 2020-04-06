using Ibit.Core.Serial;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CintaValueText : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private SerialControllerCinta _serialController;

    private void Awake()
    {
        if (_text is null)
            throw new ArgumentNullException(nameof(_text));

        if (_serialController is null)
            throw new ArgumentNullException(nameof(SerialControllerCinta));

        _serialController.OnSerialMessageReceived += (str) =>
        {
            var cintaValue = double.TryParse(str, out var parsed) ? parsed : 0.0;
            _text.text = cintaValue.ToString();
        };
    }
}