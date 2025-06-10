using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HotelBooking
{
    public class Booking
    {
        public Guid Guid { get; set; }
        public string FullName { get; set; }
        public string RoomType { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string ImagePath { get; set; }
        public decimal TotalPrice { get; set; }

        [NonSerialized]
        private ImageSource _image;

        [System.Xml.Serialization.XmlIgnore]
        public ImageSource Image
        {
            get
            {
                if (_image == null && !string.IsNullOrEmpty(ImagePath))
                {
                    try
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(ImagePath);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        _image = bitmap;
                    }
                    catch { }
                }
                return _image;
            }
            set
            {
                _image = value;
            }
        }

        public Booking()
        {
            Guid = Guid.NewGuid();
        }
    }
}