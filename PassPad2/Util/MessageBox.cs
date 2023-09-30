using System;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace PassPad.Util;

public enum MessageBoxButton { OK, OKCancel, YesNo, YesNoCancel }
public enum MessageBoxImage { Error, Information, Warning, Question }

public static class MessageBox
{
    public static void Show(string text, string caption, MessageBoxButton button, MessageBoxImage icon)
    {
        var mbbutton = button switch
        {
            MessageBoxButton.OK => ButtonEnum.Ok,
            MessageBoxButton.OKCancel => ButtonEnum.OkCancel,
            MessageBoxButton.YesNo => ButtonEnum.YesNo,
            MessageBoxButton.YesNoCancel => ButtonEnum.YesNoCancel,
            _ => throw new Exception(),
        };
        
        var mbicon = icon switch
        {
            MessageBoxImage.Error => Icon.Error,
            MessageBoxImage.Information => Icon.Info,
            MessageBoxImage.Warning => Icon.Warning,
            MessageBoxImage.Question => Icon.Question,
            _ => throw new Exception(),
        };

        var box = MessageBoxManager.GetMessageBoxStandard(caption, text, mbbutton, mbicon);

        box.ShowAsync();
    }
}