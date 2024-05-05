using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using ZXing;
using ZXing.Common;
using ZXing.PDF417;

class Program
{
    static void Main(string[] args)
    {
       
        string name = "John Doe";
        string ssn = "123-45-6789";
        string dodId = "1234567890";  // Fake DOD ID number

        // Convert data to binary
        string binaryName = StringToBinary(name);
        string binarySsn = StringToBinary(ssn);
        string binaryDodId = Convert.ToString(Convert.ToInt32(dodId), 2).PadLeft(40, '0');  // Convert DOD ID to binary (40 bits)

        // Combine binary data
        string combinedData = binaryName + binarySsn + binaryDodId;

        // Compress combined data
        byte[] compressedData = CompressString(combinedData);

        // Generate PDF417 barcode
        Bitmap barcode = GeneratePDF417Barcode(compressedData);

        // Save barcode as an image
        barcode.Save("pdf417_barcode.png");

        Console.WriteLine("PDF417 barcode generated and saved as 'pdf417_barcode.png'.");
    }

    static string StringToBinary(string input)
    {
        string result = "";
        foreach (char c in input)
        {
            result += Convert.ToString(c, 2).PadLeft(8, '0');
        }
        return result;
    }

    static byte[] CompressString(string input)
    {
        using (MemoryStream outputStream = new MemoryStream())
        {
            using (DeflateStream deflateStream = new DeflateStream(outputStream, CompressionMode.Compress))
            {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
                deflateStream.Write(inputBytes, 0, inputBytes.Length);
            }
            return outputStream.ToArray();
        }
    }

    static Bitmap GeneratePDF417Barcode(byte[] data)
    {
        PDF417Writer pdf417Writer = new PDF417Writer();

        // Specify the character set as UTF-8
        IDictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>
    {
        { EncodeHintType.CHARACTER_SET, "UTF-8" }
    };

        BitMatrix bitMatrix = pdf417Writer.encode(System.Text.Encoding.UTF8.GetString(data), BarcodeFormat.PDF_417, 500, 500, hints);
        Bitmap barcodeImage = new Bitmap(bitMatrix.Width, bitMatrix.Height);
        for (int x = 0; x < bitMatrix.Width; x++)
        {
            for (int y = 0; y < bitMatrix.Height; y++)
            {
                barcodeImage.SetPixel(x, y, bitMatrix[x, y] ? Color.Black : Color.White);
            }
        }
        return barcodeImage;
    }


}
