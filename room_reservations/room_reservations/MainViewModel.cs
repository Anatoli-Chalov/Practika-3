using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace HotelBooking
{
    public class MainViewModel
    {
        public ObservableCollection<Booking> Bookings { get; set; }
        public Booking CurrentBooking { get; set; }
        public string[] RoomTypes { get; set; }

        public ICommand AddBookingCommand { get; }
        public ICommand LoadImageCommand { get; }

        public MainViewModel()
        {
            Bookings = new ObservableCollection<Booking>();
            CurrentBooking = new Booking();
            RoomTypes = new[] { "Одноместный", "Двухместный", "Люкс", "Президентский" };

            AddBookingCommand = new RelayCommand(AddBooking);
            LoadImageCommand = new RelayCommand(LoadImage);
        }

        private void AddBooking(object parameter)
        {
            if (CurrentBooking.CheckOutDate <= CurrentBooking.CheckInDate)
            {
                MessageBox.Show("Дата выезда должна быть позже даты заезда!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Расчет стоимости (заменил switch на if-else для C# 7.3)
            int days = (CurrentBooking.CheckOutDate - CurrentBooking.CheckInDate).Days;
            CurrentBooking.TotalPrice = CalculatePrice(CurrentBooking.RoomType, days);

            Bookings.Add(CurrentBooking);
            CurrentBooking = new Booking();
        }

        private decimal CalculatePrice(string roomType, int days)
        {
            if (roomType == "Одноместный")
                return 2000 * days;
            else if (roomType == "Двухместный")
                return 3500 * days;
            else if (roomType == "Люкс")
                return 6000 * days;
            else if (roomType == "Президентский")
                return 10000 * days;
            else
                return 0;
        }

        private void LoadImage(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var imagePath = openFileDialog.FileName;
                    var bitmap = new BitmapImage(new Uri(imagePath));
                    CurrentBooking.Image = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);
    }
}