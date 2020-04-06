using Ibit.Core.Serial;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ManoValueText : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private SerialControllerMano _serialController;

    private void Awake()
    {
        if (_text is null)
            throw new ArgumentNullException(nameof(_text));

        if (_serialController is null)
            throw new ArgumentNullException(nameof(SerialControllerMano));

        _serialController.OnSerialMessageReceived += (str) =>
        {
            var manoValue = double.TryParse(str, out var parsed) ? parsed : 0.0;
            _text.text = manoValue.ToString();
        };
    }
}