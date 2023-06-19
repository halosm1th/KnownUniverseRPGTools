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
    internal class DrawSubsector
    {
        //Dotnet Framework stuff

        private static readonly int xSize = 1050;
        private static readonly int ySize = 1500;
        private static readonly int HEIGHT = 140;
        private static readonly float WIDTH = HexWidth(HEIGHT);
        //private static readonly Image? GRID = CreateGrid();

        private static readonly SolidBrush Brush = new SolidBrush(Color.Black);
        
        private readonly KURpgSubsector _knownUniverseSubsectorToDraw;

        public DrawSubsector(KURpgSubsector knownUniverseSubsectorToDraw)
        {
            _knownUniverseSubsectorToDraw = knownUniverseSubsectorToDraw;
        }

        public Image GenerateImage(bool printSubImages = false, string path = "", bool highVersian = false)
        {
            Console.WriteLine($"starting to draw Subector: {_knownUniverseSubsectorToDraw.Name}");
            Image subsector = CreateGrid(_knownUniverseSubsectorToDraw);
            DrawWorlds(subsector, highVersian);
            Console.WriteLine($"Finished drawing Subector: {_knownUniverseSubsectorToDraw.Name}");

            if (printSubImages)
            {
                subsector.SaveAsPng(path + " " + _knownUniverseSubsectorToDraw.Name + ".png");
            }
            return subsector;
        }

        private static readonly int fontSize = 18;
        private static readonly Font systemFont = SystemFonts.CreateFont("Segoe UI Emoji", fontSize);
        private FontCollection fonts = new FontCollection();

        private void DrawWorlds(Image subsector, bool highVersian = false)
        {
            fonts.AddSystemFonts();
            
            Font fontWorld;
            
            if (highVersian) fontWorld = SystemFonts.CreateFont("High Versian", fontSize);
            else fontWorld = systemFont;

            for (var row = 0; row < _knownUniverseSubsectorToDraw.XSize; row++)
            for (var col = 0; col < _knownUniverseSubsectorToDraw.YSize; col++)
                //Get HexCoords

                if (_knownUniverseSubsectorToDraw.IsFilledSystem(row, col))
                {
                    var world =
                        _knownUniverseSubsectorToDraw.GetFilledSystem(row, col);
                    DrawSystemName(col, row, fontSize, HEIGHT, WIDTH, fontWorld, subsector, Brush, world);
                    DrawUniversalWorldProfile(HEIGHT, col, row, WIDTH, systemFont, subsector, Brush, world);

                    //DrawSystemStation(subsector, row, col, fontSize, world, fontRest, brush);
                }
        }

        private static void DrawSystemStation(Image subsector, int row, int col, int fontSize, KURpgFilledSystem? world,
            Font? Font, SolidBrush? brush)
        {
            //Get Text Coords
            var y = (float)(HEIGHT / 2) + row * HEIGHT;
            if (col % 2 == 1) y += HEIGHT / 2;
            y += fontSize * 1.5f;

            var text = $"{world?.GetTradeCodesDisplay()}";

            var x = col * (WIDTH * 0.75f);
            if (Font != null)
            {
                x += WIDTH / 2 - text.Length * Font.Size / 2.0f;
                //x += Font.Size;

                subsector.Mutate(ctx => ctx.DrawText(text, Font, brush, new PointF(x, y)));
            }
        }

        private void DrawSystemName(int row, int col, int fontSize, int height, float width, Font font, Image graphics,
            SolidBrush brush, KURpgFilledSystem? world)
        {
            var text = world.Name ?? "";

            if (text.Length > 15) text = text.Substring(0, 15);

            var y = (fontSize * 2 + row * height)+SPACER;
            if (col % 2 == 1) y += height / 2;

            var x = (col * (width * 0.75f))+SPACER;
            x += width / 2 - text.Length/2; //* Font.Size / 4.5f;

            TextOptions options = new(font)
            {
                Origin = new PointF(x, y), // Set the rendering origin.
                TabWidth = 8, // A tab renders as 8 spaces wide
                WrappingLength = 100, // Greater than zero so we will word wrap at 100 pixels wide
                HorizontalAlignment = HorizontalAlignment.Center // Right align
            };
            graphics.Mutate(ctx => ctx.DrawText(options,text,brush));
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
            SolidBrush brush, KURpgFilledSystem? travellerWorld)
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

        public bool PRINTER_FRIENDLY = true;
        
        private Image? CreateGrid(KURpgSubsector subsector)
        {
            Image? retImage = null;
            var fontSize = 18;
            using (var grid = new Image<Rgb24>(xSize, ySize))
            {
                var brush = new SolidBrush(Color.White);
                var BlackBrush = new SolidBrush(Color.Black);
                var font = SystemFonts.CreateFont("Arial", fontSize);
                grid.Mutate(x => x.Fill(PRINTER_FRIENDLY? brush : BlackBrush, new Rectangle(0, 0, xSize, ySize)));

                for (var row = 0; row < 10; row++)
                for (var col = 0; col < 8; col++)
                {
                    var points = HexToPoints(HEIGHT, row, col);
                    var system = subsector.Subsector[(col, row)];
                    if ( !PRINTER_FRIENDLY && system is KURpgEmptySystem)
                    {
                        grid.Mutate(x => x.FillPolygon(Color.Black,points));
                    }

                    else{
                    if(!PRINTER_FRIENDLY) grid.Mutate(x => x.FillPolygon(Color.White,points));
                    grid.Mutate(x => x.DrawPolygon(Color.Black, 2, points));
                    if (!PRINTER_FRIENDLY && system is KURpgFilledSystem && ((KURpgFilledSystem) system).GetTradeCodesDisplay()
                        .Contains("💳"))
                    {
                        grid.Mutate(x => x.FillPolygon(Color.Gold, points));
                    }
                    else if (!PRINTER_FRIENDLY && system is KURpgFilledSystem filledSystem && filledSystem.GetTradeCodesDisplay().Contains("🚜"))
                    {
                        grid.Mutate(x => x.FillPolygon(Color.LawnGreen, points));
                    }else if ( !PRINTER_FRIENDLY &&system is KURpgFilledSystem factSys && factSys.GetTradeCodesDisplay().Contains("🏭"))
                    {
                        grid.Mutate(x => x.FillPolygon(Color.LightGrey, points));

                    }else if ( !PRINTER_FRIENDLY &&system is KURpgFilledSystem resSys && resSys.GetTradeCodesDisplay().Contains("⚒"))
                    {
                        grid.Mutate(x => x.FillPolygon(Color.SlateGray, points));

                    }else if ( !PRINTER_FRIENDLY &&system is KURpgFilledSystem gasSys && gasSys.GetTradeCodesDisplay().Contains("⛽"))
                    {
                        grid.Mutate(x => x.FillPolygon(Color.Salmon, points));

                    }else if ( !PRINTER_FRIENDLY &&system is KURpgFilledSystem sdySys && sdySys.GetTradeCodesDisplay().Contains("🔬"))
                    {
                        grid.Mutate(x => x.FillPolygon(Color.Wheat, points));

                    }
                    else if (!PRINTER_FRIENDLY && system is KURpgFilledSystem && ((KURpgFilledSystem) system).GetTradeCodesDisplay()
                             .Contains("📀"))
                    {
                        grid.Mutate(x => x.FillPolygon(Color.Blue, points));
                    }else if ( !PRINTER_FRIENDLY &&system is KURpgFilledSystem gesSys && gesSys.GetTradeCodesDisplay().Contains("🪐"))
                    {
                        grid.Mutate(x => x.FillPolygon(Color.Orange, points));

                    }else if ( !PRINTER_FRIENDLY &&system is KURpgFilledSystem highpop && highpop.GetTradeCodesDisplay().Contains("👨‍👩‍👧‍👦"))
                    {
                        grid.Mutate(x => x.FillPolygon(Color.Cyan, points));

                    }else if ( !PRINTER_FRIENDLY &&system is KURpgFilledSystem inhabit && inhabit.GetTradeCodesDisplay().Contains("🌎"))
                    {
                        grid.Mutate(x => x.FillPolygon(Color.SkyBlue, points));

                    }
                    

                    DrawLocationText(row, col, fontSize, HEIGHT, WIDTH, font, grid, BlackBrush);
                    }
                }

                retImage = grid.Clone();
            }


            return retImage;
        }
    }
}