using System;
using System.Text;
using KnownUniversePoliticsGameWebApp.Data;
using KnownUniversePoliticsGameWebApp.Data.Politics_Game;
using KUP_Simple_Sector_Generator;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace TravellerMapSystem.Tools
{
    public class KUPDrawSubsector
    {
        //Dotnet Framework stuff

        public static readonly int xSize = 1010;
        public static readonly int ySize = 1475;
        public static readonly int HEIGHT = 140;
        public static int SPACER = 15;

        private static readonly float WIDTH = HexWidth(HEIGHT);
        //private static readonly Image? GRID = CreateGrid();

        private static readonly SolidBrush WorldTextBrush = new SolidBrush(Color.Black);
        
        private readonly KUPSubsector _knownUniverseSubsectorToDraw;
        private KnownUniversePoliticsGame PoliticsGame;

        public KUPDrawSubsector(KUPSubsector knownUniverseSubsectorToDraw, KnownUniversePoliticsGame politicsGame)
        {
            _knownUniverseSubsectorToDraw = knownUniverseSubsectorToDraw;
            PoliticsGame = politicsGame;
        }

        public Image GenerateImage(bool printSubImages = false, string path = "", bool whiteBackground = false)
        {
            Console.WriteLine($"starting to draw Subector: {_knownUniverseSubsectorToDraw.Name}");
            Image subsector = CreateGrid(_knownUniverseSubsectorToDraw, whiteBackground);
            DrawWorlds(subsector, false);
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
                    DrawSystemName(col, row, fontSize, HEIGHT, WIDTH, fontWorld, subsector, WorldTextBrush, world);
                    DrawUniversalWorldProfile(HEIGHT, col, row, WIDTH, systemFont, subsector, WorldTextBrush, world);
                    DrawShips(HEIGHT, row, col, WIDTH, subsector, WorldTextBrush, world);
                    //DrawSystemStation(subsector, row, col, fontSize, world, fontRest, brush);
                }
        }

        private void DrawShips(int height, int col, int row, float width,
            Image subsector, SolidBrush worldTextBrush, KUPFilledSystem world)
        {

            int shipCount = 0;
            int xMod = 0;
            foreach (var ship in PoliticsGame.AssetsInPlay
                         .Where(x => x.Location == world?.SystemsPrimaryStation?.PrimaryStationAsset?.Location)
                         .OfType<KUPCombatAsset>())
            {
                var y = (fontSize * 2 + row * height) + (SPACER*2);
                if (col % 2 == 1) y += height / 2;
                xMod = 23 * shipCount;
                
                var x = (col * (width * 0.75f));
                x += width / 2; //* Font.Size / 4.5f;
                x += xMod;

                var shipPoints = new PointF[]
                {
                    new(x, y),
                    new(x + 20, y + 10),
                    new(x - 20, y + 10),
                };

                subsector.Mutate(x =>
                    x.DrawPolygon(Color.Black, 5, shipPoints));
                subsector.Mutate(x =>
                    x.FillPolygon(GetFactionColour(ship.Controller.FactionType), shipPoints));
                shipCount++;
            }
        }

        private Color GetFactionColour(FactionType controllingFaction)
        {

            if (controllingFaction == FactionType.Imperial1)
            {
                return Color.Red;
            }
            else if (controllingFaction == FactionType.Imperial2)
            {
                return Color.DarkRed;
            }
            else if (controllingFaction == FactionType.Imperial3)
            {
                return Color.IndianRed;
            }

            else if (controllingFaction == FactionType.Vers1)
            {
                return Color.Yellow;
            }
            else if (controllingFaction == FactionType.Vers2)
            {
                return Color.DarkGoldenrod;
            }
            else if (controllingFaction == FactionType.Vers3)
            {
                return Color.PaleGoldenrod;
            }

            if (controllingFaction == FactionType.UFE1)
            {
                return Color.LightBlue;
            }
            if (controllingFaction == FactionType.UFE2)
            {
                return Color.MidnightBlue;
            } 
            if (controllingFaction == FactionType.UFE3)
            {
                return Color.DarkBlue;
            }

            else if (controllingFaction == FactionType.XiaoMing1)
            {
                return Color.Purple;
            }
            else if (controllingFaction == FactionType.XiaoMing1)
            {
                return Color.RebeccaPurple;
            }
            else if (controllingFaction == FactionType.XiaoMing1)
            {
                return Color.MediumPurple;
            }

            else if (controllingFaction == FactionType.Deutchria1)
            {
                return Color.LightGrey;
            }
            else if (controllingFaction == FactionType.Deutchri2)
            {
                return Color.DarkGray;
            }
            else if (controllingFaction == FactionType.Deutchria3)
            {
                return Color.SlateGray;
            }

            else if (controllingFaction == FactionType.Bank)
            {
                return Color.ForestGreen;
            }
            else if (controllingFaction == FactionType.GM)
            {
                return Color.LightGreen;
            }
            else if (controllingFaction == FactionType.Food)
            {
                return Color.SeaGreen;
            }

            return Color.White;
        }

        private void DrawSystemName(int row, int col, int fontSize, int height, float width, Font font, Image graphics,
            SolidBrush brush, KUPFilledSystem? world)
        {
            var text = world.Name ?? "";

            if (text.Length > 15) text = text.Substring(0, 15);

            var y = (fontSize * 2 + row * height)+SPACER;
            if (col % 2 == 1) y += height / 2;

            var x = (col * (width * 0.75f));
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
            Image graphics, SolidBrush brush, int drawX, int drawY)
        {
            var text = $"{drawY} {drawX}";

            var y = (fontSize + row * height)+(SPACER);
            if (col % 2 == 1) y += height / 2;

            var x = (col * (width * 0.75f))+(SPACER);
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
            SolidBrush brush, KUPFilledSystem? travellerWorld)
        {
            //Get Text Coords
            var y = (height / 2 + row * height) + SPACER;
            if (col % 2 == 1) y += height / 2;

            var text = "$"+ travellerWorld.MoneyIncome().ToString()
                .Replace("\n","")
                .Replace(" ","");
            

        var utf8Text = Utf8Encoder.GetString(Utf8Encoder.GetBytes(text));
            
            var x = (col * (width * 0.75f));
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

        private static PointF[] HexToPoints(float height, int row, int Col)
        {
            var width = HexWidth(height);
            var y = (height / 2);
            float x = 0;

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
        
        private Image? CreateGrid(KUPSubsector subsector, bool whiteBackground)
        {
            Image? retImage = null;
            var fontSize = 18;
            using (var grid = new Image<Rgba64>(xSize, ySize))
            {
                var brush = new SolidBrush(Color.White);
                var font = SystemFonts.CreateFont("Arial", fontSize);
                grid.Mutate(x => x.Fill(new SolidBrush(Color.Transparent)));

                for (var row = 0; row < 10; row++)
                for (var col = 0; col < 8; col++)
                {
                    var points = HexToPoints(HEIGHT, row, col);
                    var system = subsector.Subsector[(col, row)];
                    if (!whiteBackground && system is KupEmptySystem)
                    {
                        grid.Mutate(x => x.FillPolygon(Color.Transparent, points));
                    }
                    
                    if (system is KUPFilledSystem)
                    {
                        var controllingFaction = ((KUPFilledSystem) system).SysetmAsset.Controller?.FactionType ?? FactionType.Unclaimed;

                        if (  controllingFaction == FactionType.Unclaimed)
                        {
                            grid.Mutate(x => x.FillPolygon(Color.White, points));
                        }
                        
                        if (  controllingFaction == FactionType.Imperial1)
                        {
                            grid.Mutate(x => x.FillPolygon(Color.Red, points));
                        }else if  (  controllingFaction == FactionType.Imperial2)
                        {
                            grid.Mutate(x => x.FillPolygon(Color.DarkRed, points));
                        }else if  (  controllingFaction == FactionType.Imperial3)
                        {
                            grid.Mutate(x => x.FillPolygon(Color.IndianRed, points));
                        }
                        
                        else if  (  controllingFaction == FactionType.Vers1)
                        {
                            grid.Mutate(x => x.FillPolygon(Color.Gold, points));
                        }else if  (  controllingFaction == FactionType.Vers2)
                        {
                            grid.Mutate(x => x.FillPolygon(Color.DarkGoldenrod, points));
                        }else if  (  controllingFaction == FactionType.Vers3)
                        {
                            grid.Mutate(x => x.FillPolygon(Color.PaleGoldenrod, points));
                        }

                        else if (  controllingFaction == FactionType.UFE1)
                        {
                            grid.Mutate(x => x.FillPolygon(Color.LightBlue, points));
                        }else if (  controllingFaction == FactionType.UFE2)
                        {
                            grid.Mutate(x => x.FillPolygon(Color.CornflowerBlue, points));
                        }else if (  controllingFaction == FactionType.UFE3)
                        {
                            grid.Mutate(x => x.FillPolygon(Color.AliceBlue, points));
                        }
                        
                        else if (  controllingFaction == FactionType.XiaoMing1)
                        {
                            grid.Mutate(x => x.FillPolygon(Color.Purple, points));
                        }else if (  controllingFaction == FactionType.XiaoMing1)
                        {
                            grid.Mutate(x => x.FillPolygon(Color.RebeccaPurple, points));
                        }else if (  controllingFaction == FactionType.XiaoMing1)
                        {
                            grid.Mutate(x => x.FillPolygon(Color.MediumPurple, points));
                        }
                        
                        else if (  controllingFaction == FactionType.Deutchria1)
                        {
                            grid.Mutate(x => x.FillPolygon(Color.LightGrey, points));
                        }else if (  controllingFaction == FactionType.Deutchri2)
                        {
                            grid.Mutate(x => x.FillPolygon(Color.DarkGray, points));
                        }else if (  controllingFaction == FactionType.Deutchria3)
                        {
                            grid.Mutate(x => x.FillPolygon(Color.SlateGray, points));
                        }
                        
                        else if (  controllingFaction == FactionType.Bank)
                        {
                            grid.Mutate(x => x.FillPolygon(Color.ForestGreen, points));
                        }else if (  controllingFaction == FactionType.GM)
                        {
                            grid.Mutate(x => x.FillPolygon(Color.LightGreen, points));
                        }else if (  controllingFaction == FactionType.Food)
                        {
                            grid.Mutate(x => x.FillPolygon(Color.SeaGreen, points));
                        }
                        
                        grid.Mutate(x => x.DrawPolygon(Color.Black, 2, points));
                        DrawLocationText(row, col, fontSize, HEIGHT, WIDTH, font, grid,
                            WorldTextBrush,_knownUniverseSubsectorToDraw.GetSystem(col,row).DisplayY
                            , _knownUniverseSubsectorToDraw.GetSystem(col,row).DisplayX);
                    }
                }

                retImage = grid.Clone();
            }


            return retImage;
        }
    }
}