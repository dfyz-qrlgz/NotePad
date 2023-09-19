using NotePadCsharp;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;



namespace NotePadCsharp
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Note> notes;
        private Note selectedNote;
        private DateTime selectedDate;
        private string dataFilePath = "notes.json";

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            selectedDate = DateTime.Today;
            datePicker.SelectedDate = selectedDate;

            LoadNotes();
        }

        private void LoadNotes()
        {
            if (File.Exists(dataFilePath))
            {
                notes = SerializationHelper.Deserialize<ObservableCollection<Note>>(dataFilePath);
            }
            else
            {
                notes = new ObservableCollection<Note>();
            }

            RefreshNoteListBox();
        }

        private void SaveNotes()
        {
            SerializationHelper.Serialize(notes, dataFilePath);
        }

        private void RefreshNoteListBox()
        {
            noteListBox.ItemsSource = notes.Where(note => note.Date.Date == selectedDate.Date);
            selectedNote = null;
        }

        private void ClearFields()
        {
            titleTextBox.Text = "";
            descriptionTextBox.Text = "";
        }

        private void AddNote_Click(object sender, RoutedEventArgs e)
        {
            Note newNote = new Note
            {
                Title = titleTextBox.Text,
                Description = descriptionTextBox.Text,
                Date = selectedDate
            };

            notes.Add(newNote);
            SaveNotes();
            ClearFields();
            RefreshNoteListBox();
        }

        private void UpdateNote_Click(object sender, RoutedEventArgs e)
        {
            if (selectedNote != null)
            {
                selectedNote.Title = titleTextBox.Text;
                selectedNote.Description = descriptionTextBox.Text;
                SaveNotes();
                ClearFields();
                RefreshNoteListBox();
            }
        }

        private void DeleteNote_Click(object sender, RoutedEventArgs e)
        {
            if (selectedNote != null)
            {
                notes.Remove(selectedNote);
                SaveNotes();
                ClearFields();
                RefreshNoteListBox();
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDate = datePicker.SelectedDate ?? DateTime.Today;
            RefreshNoteListBox();
            ClearFields();
        }

        private void NoteListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedNote = noteListBox.SelectedItem as Note;

            if (selectedNote != null)
            {
                titleTextBox.Text = selectedNote.Title;
                descriptionTextBox.Text = selectedNote.Description;
            }
        }
    }
}
