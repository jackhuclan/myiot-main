// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Drawing.Drawing2D;

namespace VegaUI;
public static class Helper
{
    public static bool GetValue(this UICornerRadiusSides sides, UICornerRadiusSides side)
    {
        return (sides & side) == side;
    }

    public static bool GetValue(this ToolStripStatusLabelBorderSides sides, ToolStripStatusLabelBorderSides side)
    {
        return (sides & side) == side;
    }

    public static bool IsValid(this Color color)
    {
        return !color.IsNullOrEmpty();
    }

    public static Pen Pen(this Color color, float size = 1f)
    {
        return new Pen(color, size);
    }

    public static bool IsNullOrEmpty(this Color color)
    {
        if (!(color == Color.Empty))
        {
            return color == Color.Transparent;
        }

        return true;
    }

    public static SolidBrush Brush(this Color color)
    {
        return new SolidBrush(color);
    }

    public static bool IsNullOrEmpty(this string s)
    {
        return string.IsNullOrEmpty(s);
    }

    public static GraphicsPath Path(this Point[] points)
    {
        GraphicsPath graphicsPath = new GraphicsPath();
        graphicsPath.Reset();
        graphicsPath.AddPolygon(points);
        return graphicsPath;
    }
    public static GraphicsPath GraphicsPath(this Rectangle rect)
    {
        return new Point[5]
        {
            new Point(rect.Left, rect.Top),
            new Point(rect.Right, rect.Top),
            new Point(rect.Right, rect.Bottom),
            new Point(rect.Left, rect.Bottom),
            new Point(rect.Left, rect.Top)
        }.Path();
    }

    public static GraphicsPath CreateRoundedRectanglePath(this Rectangle rect, int radius, UICornerRadiusSides radiusSides, int lineSize = 1)
    {
        if (radiusSides == UICornerRadiusSides.None || radius == 0)
        {
            return rect.GraphicsPath();
        }

        bool value = radiusSides.GetValue(UICornerRadiusSides.LeftTop);
        bool value2 = radiusSides.GetValue(UICornerRadiusSides.LeftBottom);
        bool value3 = radiusSides.GetValue(UICornerRadiusSides.RightTop);
        bool value4 = radiusSides.GetValue(UICornerRadiusSides.RightBottom);
        return rect.CreateRoundedRectanglePath(radius, value, value3, value4, value2, lineSize);
    }

    public static GraphicsPath CreateRoundedRectanglePath(this Rectangle rect, int radius, bool cornerLeftTop = true, bool cornerRightTop = true, bool cornerRightBottom = true, bool cornerLeftBottom = true, int lineSize = 1)
    {
        GraphicsPath graphicsPath = new GraphicsPath();
        if (UIStyles.GlobalRectangle || (!cornerLeftTop && !cornerRightTop && !cornerRightBottom && !cornerLeftBottom) || radius <= 0)
        {
            graphicsPath = rect.GraphicsPath();
        }
        else
        {
            radius *= lineSize;
            if (cornerLeftTop)
            {
                graphicsPath.AddArc(rect.X, rect.Y, radius, radius, 180f, 90f);
            }
            else
            {
                graphicsPath.AddLine(new Point(rect.X, rect.Y + 1), new Point(rect.X, rect.Y));
            }

            if (cornerRightTop)
            {
                graphicsPath.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270f, 90f);
            }
            else
            {
                graphicsPath.AddLine(new Point(rect.X + rect.Width - 1, rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }

            if (cornerRightBottom)
            {
                graphicsPath.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0f, 90f);
            }
            else
            {
                graphicsPath.AddLine(new Point(rect.X + rect.Width, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
            }

            if (cornerLeftBottom)
            {
                graphicsPath.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90f, 90f);
            }
            else
            {
                graphicsPath.AddLine(new Point(rect.X + 1, rect.Y + rect.Height), new Point(rect.X, rect.Y + rect.Height));
            }

            graphicsPath.CloseFigure();
        }

        return graphicsPath;
    }

    public static GraphicsPath CreateTrueRoundedRectanglePath(this Rectangle rect, int radius, int lineSize = 1)
    {
        GraphicsPath graphicsPath = new GraphicsPath();
        radius *= lineSize;
        graphicsPath.AddArc(rect.X, rect.Y, radius, radius, 180f, 90f);
        graphicsPath.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270f, 90f);
        graphicsPath.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0f, 90f);
        graphicsPath.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90f, 90f);
        graphicsPath.CloseFigure();
        return graphicsPath;
    }

    public static void DrawString(this Graphics g, string text, Font font, Color color, Rectangle rect, ContentAlignment alignment, int offsetX = 0, int offsetY = 0)
    {
        if (!text.IsNullOrEmpty())
        {
            rect.Offset(offsetX, offsetY);
            Size size = TextRenderer.MeasureText(text, font);
            int x = 0;
            int y = 0;
            switch (alignment)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    x = rect.Left + 1;
                    break;
                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    x = rect.Left + (rect.Width - size.Width) / 2;
                    break;
                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    x = rect.Left + rect.Width - size.Width - 1;
                    break;
            }

            switch (alignment)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopRight:
                    y = rect.Top + 1;
                    break;
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    y = rect.Top + (rect.Height - size.Height) / 2;
                    break;
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    y = rect.Top + rect.Height - size.Height - 1;
                    break;
            }

            TextRenderer.DrawText(g, text, font, new Point(x, y), color);
        }
    }

    public static void DrawLine(this Graphics g, Color color, int x1, int y1, int x2, int y2, bool smooth = false, float penWidth = 1f)
    {
        g.Smooth(smooth);
        using Pen pen = color.Pen(penWidth);
        g.DrawLine(pen, x1, y1, x2, y2);
        g.Smooth(smooth: false);
    }

    public static void DrawPath(this Graphics g, Color color, GraphicsPath path, bool smooth = true, float penWidth = 1f)
    {
        g.Smooth(smooth);
        using Pen pen = color.Pen(penWidth);
        g.DrawPath(pen, path);
        g.Smooth(smooth: false);
    }

    public static void FillPath(this Graphics g, Color color, GraphicsPath path, bool smooth = true)
    {
        g.Smooth(smooth);
        using SolidBrush brush = color.Brush();
        g.FillPath(brush, path);
        g.Smooth(smooth: false);
    }

    public static void FillEllipse(this Graphics g, Color color, int left, int top, int width, int height, bool smooth = true)
    {
        g.FillEllipse(color, new Rectangle(left, top, width, height), smooth);
    }

    public static Graphics Smooth(this Graphics g, bool smooth = true)
    {
        if (smooth)
        {
            g.SetHighQuality();
        }
        else
        {
            g.SetDefaultQuality();
        }

        return g;
    }

    public static Graphics SetHighQuality(this Graphics g)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.CompositingQuality = CompositingQuality.HighQuality;
        return g;
    }

    public static Graphics SetDefaultQuality(this Graphics g)
    {
        g.SmoothingMode = SmoothingMode.Default;
        g.InterpolationMode = InterpolationMode.Default;
        g.CompositingQuality = CompositingQuality.Default;
        return g;
    }

    public static void FillEllipse(this Graphics g, Color color, Rectangle rect, bool smooth = true)
    {
        g.Smooth(smooth);
        using SolidBrush brush = color.Brush();
        g.FillEllipse(brush, rect);
        g.Smooth(smooth: false);
    }

    public static void FillRoundRectangle(this Graphics g, Color color, Rectangle rect, int cornerRadius, bool smooth = true)
    {
        if (!UIStyles.GlobalRectangle && cornerRadius > 0)
        {
            using (GraphicsPath path = rect.CreateRoundedRectanglePath(cornerRadius))
            {
                g.FillPath(color, path, smooth);
                return;
            }
        }

        g.FillRectangle(color, rect, smooth);
    }

    public static void FillRectangle(this Graphics g, Color color, Rectangle rect, bool smooth = false)
    {
        g.Smooth(smooth);
        using SolidBrush brush = color.Brush();
        g.FillRectangle(brush, rect);
        g.Smooth(smooth: false);
    }

    public static void FillRoundRectangle(this Graphics g, Color color, int left, int top, int width, int height, int cornerRadius, bool smooth = true)
    {
        g.FillRoundRectangle(color, new Rectangle(left, top, width, height), cornerRadius, smooth);
    }

}


public static class UIStyles
{
    public static bool GlobalRectangle { get; set; }
}
