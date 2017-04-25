﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Notepad3000
{
    public class RichEditBoxExtended : Windows.UI.Xaml.Controls.RichEditBox
    {
        public static readonly DependencyProperty RtfTextProperty =
            DependencyProperty.Register(
            "RtfText", typeof(string), typeof(RichEditBoxExtended),
            new PropertyMetadata(default(string), RtfTextPropertyChanged));

        private bool _lockChangeExecution;

        public RichEditBoxExtended()
        {
            TextChanged += RichEditBoxExtended_TextChanged;
        }

        public string RtfText
        {
            get { return (string)GetValue(RtfTextProperty); }
            set { SetValue(RtfTextProperty, value); }
        }

        private void RichEditBoxExtended_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!_lockChangeExecution)
            {
                _lockChangeExecution = true;
                string text;
                Document.GetText(Windows.UI.Text.TextGetOptions.None, out text);
                if (string.IsNullOrWhiteSpace(text))
                {
                    RtfText = "";
                }
                else
                {
                    Document.GetText(Windows.UI.Text.TextGetOptions.None, out text);
                    RtfText = text;
                }
                _lockChangeExecution = false;
            }
        }

        private static void RtfTextPropertyChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var rtb = dependencyObject as RichEditBoxExtended;
            if (rtb == null) return;
            if (!rtb._lockChangeExecution)
            {
                rtb._lockChangeExecution = true;
                rtb.Document.SetText(Windows.UI.Text.TextSetOptions.None, rtb.RtfText);
                rtb._lockChangeExecution = false;
            }
        }
    }
   
}
