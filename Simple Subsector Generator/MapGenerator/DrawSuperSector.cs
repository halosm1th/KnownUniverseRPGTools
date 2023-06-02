using System;
using System.Text;
using Simple_Subsector_Generator;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace TravellerMapSystem.Tools
{
    internal class DrawSuperSector
    {
        //Dotnet Framework stuff

        private static readonly int xSize = (1050*4)*4;
        private static readonly int ySize = (1500*4)*4;
        private static readonly int HEIGHT = 140;
        private static readonly float WIDTH = HexWidth(HEIGHT);
        private static readonly Image GRID = CreateGrid();

        private readonly KURPGSuperSector _knownUniverseSuperSectorToDraw;

        public DrawSuperSector(KURPGSuperSector knownUniverseSuperSectorToDraw)
        {
            _knownUniverseSuperSectorToDraw = knownUniverseSuperSectorToDraw;
        }

        public Image GenerateSectorImage(bool highVersian = false)
        {
            Image subsector = GRID.CloneAs<Rgb24>();
            for (int x = 0; x < _knownUniverseSuperSectorToDraw.Sectors.GetLength(0); x++)
            {
                for (int y = 0; y < _knownUniverseSuperSectorToDraw.Sectors.GetLength(1); y++)
                {
                    var imageToAdd = new DrawSector(_knownUniverseSuperSectorToDraw.Sectors[x,y]);
                    var image = imageToAdd.GenerateSectorImage();
                    var placeX = (1050*4) * x;
                    var placeY = (1500*4) * y;
                    var location = new Point(placeX, placeY);
                    subsector.Mutate(i => i.DrawImage(image,location,1));
                }   
            }
            return subsector;
        }

        private static void DrawLocationText(int row, int col, int fontSize, int height, float width, Font Font,
            Image graphics, SolidBrush brush)
        {
            var text = $"{col + 1} {row + 1}";

            var y = (fontSize + row * height)+(SPACER+15);
            if (col % 2 == 1) y += height / 2;

            var x = (col * (width * 0.75f))+(SPACER+15);
            x += width / 2 - text.Length/2; //* Font.Size / 4.5f;
            
            TextOptions options = new(Font)
            {
                Origin = new PointF(x, y-10), // Set the rendering origin.
                TabWidth = 8, // A tab renders as 8 spaces wide
                WrappingLength = 100, // Greater than zero so we will word wrap at 100 pixels wide
                HorizontalAlignment = HorizontalAlignment.Center // Right align
            };
            graphics.Mutate(ctx => ctx.DrawText(options,text,brush));
        }
        
        private static readonly Encoding Utf8Encoder = Encoding.GetEncoding(
            "UTF-8",
            new EncoderReplacementFallback(string.Empty),
            new DecoderExceptionFallback()
        );

        private static void DrawUniversalWorldProfile(int height, int row, int col, float width, Font Font,
            Image graphics,
            SolidBrush brush, KURpgFilledSystem travellerWorld)
        {
            //Get Text Coords
            var y = (height / 2 + row * height)+20;
            if (col % 2 == 1) y += height / 2;

            var text = travellerWorld.USPDisplay()
                .Replace("\n","")
                .Replace(" ","");
            

        var utf8Text = Utf8Encoder.GetString(Utf8Encoder.GetBytes(text));
            
            var x = (col * (width * 0.75f))+20;
            x += width / 2 - (text.Length/2); //* Font.Size/10;
            x += Font.Size;
            
            TextOptions options = new(Font)
            {
                Origin = new PointF(x, y), // Set the rendering origin.
                TabWidth = 8, // A tab renders as 8 spaces wide
                WrappingLength = 110, // Greater than zero so we will word wrap at 100 pixels wide
                HorizontalAlignment = HorizontalAlignment.Center // Right align
            };
            graphics.Mutate(ctx => ctx.DrawText(options,utf8Text,brush));
        }

        private static int SPACER = 15;
        
        private static PointF[] HexToPoints(float height, int row, int Col)
        {
            var width = HexWidth(height);
            var y = (height / 2) + 20;
            float x = 20;

            y += row * height;
            if (Col % 2 == 1) y += height / 2;

            x += Col * (width * 0.75f);

            return new[]
            {
                new(x, y),
                new PointF(x + width * 0.25f, y - height / 2),
                new PointF(x + width * 0.75f, y - height / 2),
                new PointF(x + width, y),
                new PointF(x + width * 0.75f, y + height / 2),
                new PointF(x + width * 0.25f, y + height / 2)
            };
        }

        private static float HexWidth(float height)
        {
            return (float)(4 * (height / 2 / Math.Sqrt(3)));
        }

        private static Image CreateGrid()
        {
            Image retImage = null;
            var fontSize = 18;
            using (var grid = new Image<Rgb24>(xSize, ySize))
            {
                var brush = new SolidBrush(Color.White);
                var BlackBrush = new SolidBrush(Color.Black);
                var font = SystemFonts.CreateFont("Arial", fontSize);
                grid.Mutate(x => x.Fill(brush, new Rectangle(0, 0, xSize, ySize)));

                for (var row = 0; row < 10; row++)
                for (var col = 0; col < 8; col++)
                {
                    var points = HexToPoints(HEIGHT, row, col);
                    grid.Mutate(x => x.DrawPolygon(Color.Black, 2, points));


                    DrawLocationText(row, col, fontSize, HEIGHT, WIDTH, font, grid, BlackBrush);
                }

                retImage = grid.Clone();
            }


            return retImage;
        }
    }
}