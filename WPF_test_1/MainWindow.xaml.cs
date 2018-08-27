using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Win32;

namespace WPF_test_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///

    public partial class MainWindow : Window
    {
        private SoundPlayer soundPlayCaramelldancen;

        public MainWindow()
        {
            InitializeComponent();
            InitializeSound(); //Start player
        }

        private void InitializeSound()
        {
            soundPlayCaramelldancen = new SoundPlayer(); //Create a new instance of the class Soundplayer
            soundPlayCaramelldancen.Stream = Properties.Resources.Caramelldansen; //Set a audiofile
            soundPlayCaramelldancen.PlayLooping(); //Start play music endless;
        }

        private void LoadFromFileButton_Click(object sender, RoutedEventArgs e) /*The method that read text from file,
                                                                                while you pressing this button and select file from file explorer dialog*/
        {
            OpenFileDialog fileDialog = new OpenFileDialog();  //Create a new instance of the class OpenFileDilalog
            fileDialog.CheckFileExists = true; // Make sure the dialog checks for existence of the selected file
            fileDialog.Filter = "TXT files (*.txt)|*.txt"; //Alow selection txt file only
            fileDialog.DefaultExt = ".txt";
            if (fileDialog.ShowDialog() == true) //Activate the file selection dialog. 
            {
                InputStringTextBox.Text += File.ReadAllText(fileDialog.FileName, Encoding.GetEncoding(1251)); /*Reading text from file using specific encoding, 
                                                                                                             that make posible correct read russian chars,
                                                                                                             then write string into InputStringTextBox*/
            }
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)//The method that processes the click on the "execute" button
        {
            StringProcessor stringProcessor = new StringProcessor();

            if (MainWindowForm.MinHeight != 500)
            {
                MainGrid.RowDefinitions[2].Height = new GridLength(100); //Makes visible output list box
                MainWindowForm.MinHeight = 500; //Increases the minimum height of the main window
            }

            if (CountNumCharRadioButton.IsChecked == true)
            {
                RefreshStringValues(stringProcessor.CountNumChar(InputStringTextBox.Text)); //Counts the number of characters
            }

            if (CountNumWordsRadioButton.IsChecked == true)
            {
                RefreshStringValues(stringProcessor.CountNumWords(InputStringTextBox.Text)); //Counts the number of words
            }

            if (RepСharRadioButton.IsChecked == true)
            {
                RefreshStringValues(stringProcessor.ReplaceChars(InputStringTextBox.Text, OldCharTextBox.Text[0], NewCharTextBox.Text[0])); //Replace characters
            }
        }

        private void InputStringTextBox_PreviewDragEnter(object sender, DragEventArgs e) /*A method that keeps track 
                                                                                        of the cursor occurrence event 
                                                                                        with a drag-and-drop file in 
                                                                                        the InputStringTextBox*/
        {
            bool isCorrect = true; //Create a flag

            if (e.Data.GetDataPresent(DataFormats.FileDrop, true) == true)
            {
                string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true); //Get filenames from droppedfiles
                foreach (string filename in filenames)
                {
                    if (File.Exists(filename) == false) //Check exists of file
                    {
                        isCorrect = false;
                        break;
                    }
                    FileInfo info = new FileInfo(filename); //Get info of file
                    if (info.Extension != ".txt") //Check extension of file
                    {
                        isCorrect = false;
                        break;
                    }
                }
            }
            if (isCorrect == true) //Make possible drop correct files
                e.Effects = DragDropEffects.All;
            else
                e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        private void InputStringTextBox_Drop(object sender, DragEventArgs e) /*A method that keeps track 
                                                                            of the cursor occurrence event
                                                                            with a drag-and-drop file in 
                                                                            the InputStringTextBox*/
        {
            string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true); //Get filenames from droppedfiles
            foreach (string filename in filenames)
                InputStringTextBox.Text += File.ReadAllText(filename); //Add text from file to InputStringTextBox
            e.Handled = true;
        }

        private void RefreshStringValues(String processedText)
        {
            String[] listBoxStrings = new String[OutputListBox.Items.Count]; //Create buffer list of string of list box
            for (int count = 0; count < listBoxStrings.Length; count++)
            {
                listBoxStrings[count] = Convert.ToString(OutputListBox.Items[count]); //Fills the buffer array with strings of list box
            }
            OutputListBox.Items.Clear(); //Clear list box items
            OutputListBox.Items.Add(processedText); //Adding new items that contain characters count
            for (int count = 0; count < listBoxStrings.Length; count++)
            {
                OutputListBox.Items.Add(listBoxStrings[count]); //Add items from buffer list
            }
        }
    }

    public class StringProcessor
    {
        private MainWindow mainWindow = new MainWindow();

        public String CountNumChar(String inputText) //The method that counts the number of characters
        {
            String processedText = "Characters count: " + Convert.ToString(inputText.Length); //Adding new items that contain characters count
            return processedText;
        }

        public String CountNumWords(String inputText) //The method that counts the number of words
        {
            String[] words = inputText.Split(' ', ',', '.', '\n', '\t', '\r'); //Splitting string from InputStringTextBox to array of string
            String processedText = "Words count: " + Convert.ToString(words.Length); //Adding new items that contain characters count
            return processedText;
        }

        public String ReplaceChars(String inputText, char oldChar, char newChar) //The method that replace characters
        {
            String processedText = "String after repalce: " + inputText.Replace(oldChar, newChar); //Adding new items that contain changed string
            return processedText;
        }
    }
}
