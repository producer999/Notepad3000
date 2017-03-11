﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Notepad3000;
using Windows.UI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Notepad3000
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public bool textChanged = false;
        public bool isCurrentFileSaved = false;

        public bool isCtrlKeyPressed = false;

        public static string isCurrentFileSavedString = "";

        StorageFile CurrentFile = null;


        public MainPage()
        {
            this.InitializeComponent();
        }

        public void updateTitle()
        {
            var appview = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            if (CurrentFile == null)
            {
                appview.Title = "Untitled" + isCurrentFileSavedString;
            }
            else
            {
                appview.Title = CurrentFile.Path + isCurrentFileSavedString;
            }
        }

        public async Task<bool> Confirm(string content, string title, string ok)
        {
            bool result = false;
            MessageDialog dialog = new MessageDialog(content, title);
            dialog.Commands.Add(new UICommand(ok, new UICommandInvokedHandler((cmd) => result = true)));
            await dialog.ShowAsync();
            return result;

        }

        public async Task<bool> Confirm(string content, string title, string ok, string cancel)
        {
            bool result = false;
            MessageDialog dialog = new MessageDialog(content, title);
            dialog.Commands.Add(new UICommand(ok, new UICommandInvokedHandler((cmd) => result = true)));
            dialog.Commands.Add(new UICommand(cancel, new UICommandInvokedHandler((cmd) => result = false)));
            await dialog.ShowAsync();
            return result;

        }
        public async Task<string> Confirm(string content, string title, string save, string dontsave, string cancel)
        {
            string result = "";
            MessageDialog dialog = new MessageDialog(content, title);
            dialog.Commands.Add(new UICommand(save, new UICommandInvokedHandler((cmd) => result = "save")));
            dialog.Commands.Add(new UICommand(dontsave, new UICommandInvokedHandler((cmd) => result = "dontsave")));
            dialog.Commands.Add(new UICommand(cancel, new UICommandInvokedHandler((cmd) => result = "cancel")));
            await dialog.ShowAsync();
            return result;

        }

        public async Task<bool> OpenFile()
        {
            try
            {
                FileOpenPicker picker = new FileOpenPicker();
                picker.SuggestedStartLocation = PickerLocationId.Desktop;
                picker.FileTypeFilter.Add(".txt");
                StorageFile file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    MainTextBox.Document.SetText(Windows.UI.Text.TextSetOptions.None, await FileIO.ReadTextAsync(file));
                    textChanged = false;
                    CurrentFile = file;
                    isCurrentFileSaved = true;
                    isCurrentFileSavedString = "";
                    updateTitle();
                    

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                await Confirm("Error in opening file.\n\nOpenFile()", "Something went wrong.", "OK");
                return false;
            }
        }

        public async Task<bool> SaveCurrentFile()
        {
            if(CurrentFile != null && isCurrentFileSaved == false)
            {
                try
                {
                    CachedFileManager.DeferUpdates(CurrentFile);

                    string text = "";
                    MainTextBox.Document.GetText(Windows.UI.Text.TextGetOptions.UseCrlf, out text);

                    await FileIO.WriteTextAsync(CurrentFile, text);
                    FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(CurrentFile);

                    isCurrentFileSaved = true;
                    textChanged = false;
                    isCurrentFileSavedString = "";
                    updateTitle();

                    return true;
                }
                catch(Exception ex)
                {
                    string exception = ex.ToString();
                    await Confirm("Error in saving file.\n\nSaveCurrentFile()\n\n" + exception, "Something went wrong.", "OK");

                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SaveNewFile()
        {
            if (CurrentFile == null && isCurrentFileSaved == false)
            {
                try
                {
                    FileSavePicker picker = new FileSavePicker();
                    picker.SuggestedStartLocation = PickerLocationId.Desktop;
                    picker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
                    picker.DefaultFileExtension = ".txt";
                    picker.SuggestedFileName = "Document";

                    StorageFile file = await picker.PickSaveFileAsync();

                   

                    if (file != null)
                    {
                        CachedFileManager.DeferUpdates(file);

                        string text = "";
                        MainTextBox.Document.GetText(Windows.UI.Text.TextGetOptions.UseCrlf, out text);

                        await FileIO.WriteTextAsync(file, text);
                        FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);

                        isCurrentFileSaved = true;
                        textChanged = false;
                        CurrentFile = file;
                        isCurrentFileSavedString = "";
                        updateTitle();

                        return true;
                    }
                    return false; 
                }
                catch
                {
                    return await Confirm("Error in saving file.\n\nSaveNewFile()", "Something went wrong.", "OK");
                }
            }
            else
            {
                return false;
            }
        }

        private void TabKeyDown(object sender, KeyRoutedEventArgs e)
       {
            if (e.Key != Windows.System.VirtualKey.Control && e.Key != Windows.System.VirtualKey.Shift && !isCtrlKeyPressed)
            {
                
                if (e.Key == Windows.System.VirtualKey.Tab)
                {
                    var reb = (RichEditBox)sender;
                    reb.Document.Selection.TypeText("\t");

                    textChanged = true;
                    isCurrentFileSaved = false;
                    if (isCurrentFileSavedString == "")
                    {
                        isCurrentFileSavedString = "*";
                        updateTitle();
                    }

                    e.Handled = true;
                }
                else
                {
                    textChanged = true;
                    isCurrentFileSaved = false;
                    if (isCurrentFileSavedString == "")
                    {
                        isCurrentFileSavedString = "*";
                        updateTitle();
                    }
                }
            }
        }

        private void MenuButtonClicked(object sender, RoutedEventArgs e)
        {
            var menu = (Button)sender;
            var fly = (MenuFlyout)menu.Flyout;

            fly.ShowAt(menu, new Point(0, menu.ActualHeight));
        }

        private async void NewClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isCurrentFileSaved == false && textChanged == true)
                {
                    string result = await Confirm("Do you want to save the current file?", "File not saved - Save changes?", "Save", "Don't Save", "Cancel");

                    if (result == "save")
                    {
                        if (CurrentFile != null)
                        {
                            await SaveCurrentFile();

                            MainTextBox.Document.SetText(Windows.UI.Text.TextSetOptions.None, "");
                            textChanged = false;
                            isCurrentFileSaved = false;
                            CurrentFile = null;
                            isCurrentFileSavedString = "";
                            updateTitle();
                        }
                        else
                        {
                            if (await SaveNewFile())
                            {
                                MainTextBox.Document.SetText(Windows.UI.Text.TextSetOptions.None, "");
                                textChanged = false;
                                isCurrentFileSaved = false;
                                CurrentFile = null;
                                isCurrentFileSavedString = "";
                                updateTitle();
                            }
                        }
                    }
                    else if (result == "dontsave")
                    {
                        MainTextBox.Document.SetText(Windows.UI.Text.TextSetOptions.None, "");
                        textChanged = false;
                        isCurrentFileSaved = false;
                        CurrentFile = null;
                        isCurrentFileSavedString = "";
                        updateTitle();
                    }
                    else
                    {

                    }

                }
                else
                {
                    MainTextBox.Document.SetText(Windows.UI.Text.TextSetOptions.None, "");
                    isCurrentFileSaved = false;
                    textChanged = false;
                    CurrentFile = null;
                    isCurrentFileSavedString = "";
                    updateTitle();
                }
            }
            catch
            {
                await Confirm("Error in creting new file.\n\nNewClicked()", "Something went wrong.", "OK");
            }     
        }

        private async void AboutClicked(object sender, RoutedEventArgs e)
        {
            await Confirm("\n\u00A9 2017 The Architect\n\nVersion: 0.01.05 3/10/2017", "About Notepad3000", "OK"); 
        }

        private async void OpenClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if(isCurrentFileSaved == false && textChanged == true)
                {
                    string result = await Confirm("Do you want to save the current file?", "File not saved - Save changes?", "Save", "Don't Save", "Cancel");

                    if (result == "save")
                    {
                        if (CurrentFile != null)
                        {
                            await SaveCurrentFile();

                            await OpenFile();
                        }
                        else
                        {
                            if (await SaveNewFile())
                            {
                                await OpenFile();
                            }
                        }
                    }
                    else if (result == "dontsave") 
                    {
                        await OpenFile();
                    }
                    else
                    {

                    }                   
                }
                else
                {
                    await OpenFile();
                }
            }
            catch
            {
                await Confirm("Error in opening file.\n\nOpenClicked()", "Something went wrong.", "OK");
            }
        }

        private async void CloseClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isCurrentFileSaved == false && textChanged == true)
                {
                    string result = await Confirm("Do you want to save the current file?", "File not saved - Save changes?", "Save", "Don't Save", "Cancel");

                    if (result == "save")
                    {
                        if (CurrentFile != null)
                        {
                            await SaveCurrentFile();

                            MainTextBox.Document.SetText(Windows.UI.Text.TextSetOptions.None, "");
                            textChanged = false;
                            isCurrentFileSaved = false;
                            CurrentFile = null;
                            isCurrentFileSavedString = "";
                            updateTitle();
                        }
                        else
                        {
                            if (await SaveNewFile())
                            {
                                MainTextBox.Document.SetText(Windows.UI.Text.TextSetOptions.None, "");
                                textChanged = false;
                                isCurrentFileSaved = false;
                                CurrentFile = null;
                                isCurrentFileSavedString = "";
                                updateTitle();
                            }
                        }
                    }
                    else if (result == "dontsave")
                    {
                        MainTextBox.Document.SetText(Windows.UI.Text.TextSetOptions.None, "");
                        textChanged = false;
                        isCurrentFileSaved = false;
                        CurrentFile = null;
                        isCurrentFileSavedString = "";
                        updateTitle();
                    }
                    else
                    {

                    }

                }
                else
                {
                    MainTextBox.Document.SetText(Windows.UI.Text.TextSetOptions.None, "");
                    isCurrentFileSaved = false;
                    textChanged = false;
                    CurrentFile = null;
                    isCurrentFileSavedString = "";
                    updateTitle();
                }
            }
            catch
            {
                await Confirm("Error in closing file.\n\nCloseClicked()", "Something went wrong.", "OK");
            }
        }

        private async void SaveClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (textChanged == true && isCurrentFileSaved == false && CurrentFile == null)
                {
                    await SaveNewFile();
                }
                else
                {
                    if(CurrentFile != null)
                    {
                        await SaveCurrentFile();
                    }
                    else
                    {
                        await SaveNewFile();
                    }
                }

            }
            catch
            {
                await Confirm("Error in saving file.\n\nSaveClicked()", "Something went wrong.", "OK");
            }
        }

        private async void SaveAsClicked(object sender, RoutedEventArgs e)
        {
            try
            {
             
                    FileSavePicker picker = new FileSavePicker();
                    picker.SuggestedStartLocation = PickerLocationId.Desktop;
                    picker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
                    picker.DefaultFileExtension = ".txt";
                    picker.SuggestedFileName = "Document";

                    StorageFile file = await picker.PickSaveFileAsync();

                    if (file != null)
                    {
                        CachedFileManager.DeferUpdates(file);

                        string text = "";
                        MainTextBox.Document.GetText(Windows.UI.Text.TextGetOptions.UseCrlf, out text);

                        await FileIO.WriteTextAsync(file, text);
                        FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                        isCurrentFileSaved = true;
                        textChanged = false;
                        CurrentFile = file;
                        isCurrentFileSavedString = "";
                        updateTitle();

                    }
                
            }
            catch
            {
                await Confirm("Error in saving file.\n\nSaveAsClicked()", "Something went wrong.", "OK");
            }
        }

        private async void KeyPressed(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Control)
            {
                isCtrlKeyPressed = true;
            }
            else if (isCtrlKeyPressed)
            {
                if (e.Key == Windows.System.VirtualKey.S)
                {
                    if(CurrentFile != null)
                    {
                        await SaveCurrentFile();
                    }
                    else
                    {
                        await SaveNewFile();
                    }
                }
            }   
        }

        private void KeyLifted(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.Control)
            {
                isCtrlKeyPressed = false;
            }
        }

        private async void ExitClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isCurrentFileSaved == false && textChanged == true)
                {
                    string result = await Confirm("Do you want to save the current file?", "File not saved - Save changes?", "Save", "Don't Save", "Cancel");

                    if (result == "save")
                    {
                        if (CurrentFile != null)
                        {
                            await SaveCurrentFile();

                            Application.Current.Exit();
                        }
                        else
                        {
                            if (await SaveNewFile())
                            {
                                Application.Current.Exit();
                            }
                        }
                    }
                    else if (result == "dontsave")
                    {
                        Application.Current.Exit();
                    }
                    else
                    {

                    }

                }
                else
                {
                    Application.Current.Exit();
                }
            }
            catch
            {
                await Confirm("Error in closing application.\n\nExitClicked()", "Something went wrong.", "OK");
                Application.Current.Exit();
            }
            
        }
    }
}
