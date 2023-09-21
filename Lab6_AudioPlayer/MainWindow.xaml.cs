using System;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;


namespace Lab6_AudioPlayer
{
    public partial class MainWindow : Window
    {
        private MediaPlayer mediaPlayer;
        private List<string> playlist;

        public MainWindow()
        {
            InitializeComponent();

            mediaPlayer = new MediaPlayer();
            playlist = new List<string>();
        }
        
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (playlist.Count > 0)
            {
                if (mediaPlayer.Source == null)
                {
                    mediaPlayer.Open(new Uri(playlist[0]));
                }

                mediaPlayer.Play();
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            mediaPlayer.Close();
        }

        private void AddFilesButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Audio Files|*.mp3;*.wav";

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    playlist.Add(filename);
                    PlaylistListBox.Items.Add(System.IO.Path.GetFileName(filename));
                }
            }
        }

        private void SavePlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Playlist Files|*.txt";

            if (saveFileDialog.ShowDialog() == true)
            {
                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                {
                    foreach (string filename in playlist)
                    {
                        writer.WriteLine(filename);
                    }
                }
            }
        }

        private void OpenPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Playlist Files|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                playlist.Clear();
                PlaylistListBox.Items.Clear();

                using (StreamReader reader = new StreamReader(openFileDialog.FileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        playlist.Add(line);
                        PlaylistListBox.Items.Add(System.IO.Path.GetFileName(line));
                    }
                }
            }
        }

        private int currentTrackIndex = -1; // Индекс текущей композиции

        private void PreviousTrackButton_Click(object sender, RoutedEventArgs e)
        {
            if (playlist.Count > 0)
            {
                // Уменьшаем индекс текущей композиции на 1
                currentTrackIndex--;

                // Если индекс становится меньше 0, переходим к последней композиции
                if (currentTrackIndex < 0)
                {
                    currentTrackIndex = playlist.Count - 1;
                }

                // Обновляем выбранную композицию в ListBox
                PlaylistListBox.SelectedIndex = currentTrackIndex;

                // Выполняем действия по переключению композиции
                string selectedTrack = playlist[currentTrackIndex];
                mediaPlayer.Open(new Uri(selectedTrack));
                mediaPlayer.Play();
            }
        }

        private void NextTrackButton_Click(object sender, RoutedEventArgs e)
        {
            if (playlist.Count > 0)
            {
                // Увеличиваем индекс текущей композиции на 1
                currentTrackIndex++;

                // Если индекс выходит за пределы списка, переходим к первой композиции
                if (currentTrackIndex >= playlist.Count)
                {
                    currentTrackIndex = 0;
                }

                // Обновляем выбранную композицию в ListBox
                PlaylistListBox.SelectedIndex = currentTrackIndex;

                // Выполняем действия по переключению композиции
                string selectedTrack = playlist[currentTrackIndex];
                mediaPlayer.Open(new Uri(selectedTrack));
                mediaPlayer.Play();
            }
        }

    }
}
