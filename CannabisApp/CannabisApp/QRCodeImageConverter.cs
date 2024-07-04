using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using QRCoder;

namespace CannabisApp.Converters
{
    public class QRCodeImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string code = value as string;
                if (code != null)
                {
                    // Générer le code QR
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
                    BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
                    byte[] qrCodeBytes = qrCode.GetGraphic(20); // Taille du QR Code

                    // Convertir les bytes en BitmapImage
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = new MemoryStream(qrCodeBytes);
                    bitmapImage.EndInit();

                    return bitmapImage;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la conversion du code QR en image : " + ex.Message);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
