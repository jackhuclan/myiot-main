// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace VegaUI;
public class UISwitch : Control
{
    public delegate void OnValueChanged(object sender, bool value);
    public delegate void OnCancelEventArgs(object sender, CancelEventArgs e);

    [Description("状态值修改后事件")]
    [Category("VegaUI")]
    public event OnValueChanged? ValueChanged;
    [Description("打开状态修改后事件")]
    [Category("VegaUI")]
    public event EventHandler? ActiveChanged;
    [Description("打开状态修改前事件")]
    [Category("VegaUI")]
    public event OnCancelEventArgs? ActiveChanging;

    #region 变量
    private UICornerRadiusSides _radiusSides = UICornerRadiusSides.All;
    private ToolStripStatusLabelBorderSides _rectSides = ToolStripStatusLabelBorderSides.None;
    private int radius = 5;
    private int baseRadius = 5;
    private int rectSize = 1;
    private bool showText = false;
    private bool showRect = true;
    private bool showFill = true;
    protected Color plainColor = Color.Blue;
    protected Color fillColor = Color.White;
    protected Color fillHoverColor = Color.Blue;
    protected Color fillPressColor = Color.Blue;
    protected Color fillSelectedColor = Color.Blue;
    protected Color fillDisableColor = Color.Blue;
    protected Color fillReadOnlyColor = Color.Blue;
    protected Color frameColor;
    protected Color rectColor = Color.Lime;
    protected Color rectHoverColor = Color.Blue;
    protected Color rectPressColor = Color.Blue;
    protected Color rectSelectedColor = Color.Blue;
    protected Color rectDisableColor = Color.FromArgb(173, 178, 181);
    protected Color rectReadOnlyColor = Color.Blue;
    protected Color foreColor = Color.White;
    protected Color foreHoverColor = Color.Blue;
    protected Color forePressColor = Color.Blue;
    protected Color foreSelectedColor = Color.Blue;
    protected Color foreDisableColor = Color.Blue;
    protected Color foreReadOnlyColor = Color.Blue;
    [Browsable(false)]
    public bool IsHover;
    [Browsable(false)]
    public bool IsPress;
    protected bool selected;
    protected bool isReadOnly;
    protected bool lightStyle;
    private ContentAlignment textAlign = ContentAlignment.MiddleCenter;
    private UISwitchShape switchShape = UISwitchShape.Round;
    private bool activeValue;
    private string activeText = "开";
    private string inActiveText = "关";
    private Color inActiveColor = Color.Gray;
    private bool useDoubleClick;
    #endregion

    #region 属性

    [Description("是否启用双击事件")]
    [Category("VegaUI")]
    [DefaultValue(false)]
    public bool UseDoubleClick
    {
        get
        {
            return useDoubleClick;
        }
        set
        {
            if (useDoubleClick != value)
            {
                useDoubleClick = value;
                SetStyle(ControlStyles.StandardDoubleClick, useDoubleClick);
            }
        }
    }


    [Description("圆角角度")]
    [Category("VegaUI")]
    [DefaultValue(5)]
    public int Radius
    {
        get
        {
            return radius;
        }
        set
        {
            if (radius != value)
            {
                baseRadius = (radius = Math.Max(0, value));
                Invalidate();
            }
        }
    }


    [DefaultValue(UICornerRadiusSides.All)]
    [Description("圆角显示位置")]
    [Category("VegaUI")]
    public UICornerRadiusSides RadiusSides
    {
        get
        {
            return _radiusSides;
        }
        set
        {
            _radiusSides = value;
            OnRadiusSidesChange();
            Invalidate();
        }
    }


    [Description("边框宽度")]
    [Category("VegaUI")]
    [DefaultValue(1)]
    public int RectSize
    {
        get
        {
            return rectSize;
        }
        set
        {
            int num = value;
            if (num > 2)
            {
                num = 2;
            }

            if (num < 1)
            {
                num = 1;
            }

            if (rectSize != num)
            {
                rectSize = num;
                Invalidate();
            }
        }
    }

    protected bool ShowFill
    {
        get
        {
            return showFill;
        }
        set
        {
            if (showFill != value)
            {
                showFill = value;
                Invalidate();
            }
        }
    }

    protected bool ShowRect
    {
        get
        {
            return showRect;
        }
        set
        {
            if (showRect != value)
            {
                showRect = value;
                Invalidate();
            }
        }
    }

    [Description("是否显示文字")]
    [Category("VegaUI")]
    [DefaultValue(false)]
    protected bool ShowText
    {
        get
        {
            return showText;
        }
        set
        {
            if (showText != value)
            {
                showText = value;
                Invalidate();
            }
        }
    }


    [DefaultValue(ToolStripStatusLabelBorderSides.All)]
    [Description("边框显示位置")]
    [Category("VegaUI")]
    public ToolStripStatusLabelBorderSides RectSides
    {
        get
        {
            return _rectSides;
        }
        set
        {
            _rectSides = value;
            OnRectSidesChange();
            Invalidate();
        }
    }

    [Description("是否显示激活状态颜色")]
    [Category("VegaUI")]
    [DefaultValue(false)]
    public bool ShowFocusColor { get; set; }


    [Description("文字对齐方向")]
    [Category("VegaUI")]
    [DefaultValue(ContentAlignment.MiddleCenter)]
    public ContentAlignment TextAlign
    {
        get
        {
            return textAlign;
        }
        set
        {
            if (textAlign != value)
            {
                textAlign = value;
                Invalidate();
            }
        }
    }

    [DefaultValue(false)]
    [Description("是否只读")]
    [Category("VegaUI")]
    public bool ReadOnly { get; set; }

    [Description("开关形状")]
    [Category("VegaUI")]
    [DefaultValue(UISwitchShape.Round)]
    public UISwitchShape SwitchShape
    {
        get
        {
            return switchShape;
        }
        set
        {
            switchShape = value;
            Invalidate();
        }
    }

    [Description("字体颜色")]
    [Category("VegaUI")]
    [DefaultValue(typeof(Color), "White")]
    public override Color ForeColor
    {
        get
        {
            return foreColor;
        }
        set
        {
            SetForeColor(value);
        }
    }

    [DefaultValue(false)]
    [Description("是否打开")]
    [Category("VegaUI")]
    public bool Active
    {
        get
        {
            return activeValue;
        }
        set
        {
            if (!ReadOnly && activeValue != value)
            {
                activeValue = value;
                ValueChanged?.Invoke(this, value);
                ActiveChanged?.Invoke(this, new EventArgs());
                Invalidate();
            }
        }
    }

    [DefaultValue("开")]
    [Description("打开文字")]
    [Category("VegaUI")]
    public string ActiveText
    {
        get
        {
            return activeText;
        }
        set
        {
            activeText = value;
            Invalidate();
        }
    }

    [DefaultValue("关")]
    [Description("关闭文字")]
    [Category("VegaUI")]
    public string InActiveText
    {
        get
        {
            return inActiveText;
        }
        set
        {
            inActiveText = value;
            Invalidate();
        }
    }

    [DefaultValue(typeof(Color), "Gray")]
    [Description("关闭颜色")]
    [Category("VegaUI")]
    public Color InActiveColor
    {
        get
        {
            return inActiveColor;
        }
        set
        {
            inActiveColor = value;
            Invalidate();
        }
    }

    [Description("填充颜色")]
    [Category("VegaUI")]
    [DefaultValue(typeof(Color), "White")]
    public Color ButtonColor
    {
        get
        {
            return fillColor;
        }
        set
        {
            SetFillColor(value);
        }
    }

    [Description("打开颜色")]
    [Category("VegaUI")]
    [DefaultValue(typeof(Color), "Lime")]
    public Color ActiveColor
    {
        get
        {
            return rectColor;
        }
        set
        {
            SetRectColor(value);
        }
    }

    [Description("不可用颜色")]
    [Category("VegaUI")]
    [DefaultValue(typeof(Color), "173, 178, 181")]
    public Color DisabledColor
    {
        get
        {
            return rectDisableColor;
        }
        set
        {
            SetRectDisableColor(value);
        }
    }


    #endregion

    #region 方法

    protected virtual void OnRadiusSidesChange()
    {
    }

    protected virtual void OnRectSidesChange()
    {
    }

    protected Color GetRectColor()
    {
        Color result = frameColor;
        if (IsHover)
        {
            result = rectHoverColor;
        }

        if (IsPress)
        {
            result = rectPressColor;
        }

        if (selected)
        {
            result = rectSelectedColor;
        }

        if (ShowFocusColor && Focused)
        {
            result = rectHoverColor;
        }

        if (isReadOnly)
        {
            result = rectReadOnlyColor;
        }

        if (!base.Enabled)
        {
            return rectDisableColor;
        }

        return result;
    }
    protected Color GetForeColor()
    {
        Color result = (lightStyle ? rectColor : foreColor);
        if (IsHover)
        {
            result = foreHoverColor;
        }

        if (IsPress)
        {
            result = forePressColor;
        }

        if (selected)
        {
            result = foreSelectedColor;
        }

        if (ShowFocusColor && Focused)
        {
            result = foreHoverColor;
        }

        if (isReadOnly)
        {
            result = foreReadOnlyColor;
        }

        if (!base.Enabled)
        {
            return foreDisableColor;
        }

        return result;
    }
    protected Color GetFillColor()
    {
        Color result = (lightStyle ? plainColor : fillColor);
        if (IsHover)
        {
            result = fillHoverColor;
        }

        if (IsPress)
        {
            result = fillPressColor;
        }

        if (selected)
        {
            result = fillSelectedColor;
        }

        if (ShowFocusColor && Focused)
        {
            result = fillHoverColor;
        }

        if (isReadOnly)
        {
            result = fillReadOnlyColor;
        }

        if (!base.Enabled)
        {
            return fillDisableColor;
        }

        return result;
    }

    protected void SetForeColor(Color value)
    {
        if (foreColor != value)
        {
            foreColor = value;
            Invalidate();
        }
    }
    protected void SetRectColor(Color value)
    {
        if (rectColor != value)
        {
            rectColor = value;
            Invalidate();
        }
    }

    protected void SetFillColor(Color value)
    {
        if (fillColor != value)
        {
            fillColor = value;
            Invalidate();
        }
    }

    protected void SetRectDisableColor(Color color)
    {
        if (rectDisableColor != color)
        {
            rectDisableColor = color;
            Invalidate();
        }
    }


    private void PaintRectDisableSides(Graphics g)
    {
        bool value = RectSides.GetValue(ToolStripStatusLabelBorderSides.Left);
        bool value2 = RectSides.GetValue(ToolStripStatusLabelBorderSides.Top);
        bool value3 = RectSides.GetValue(ToolStripStatusLabelBorderSides.Right);
        bool value4 = RectSides.GetValue(ToolStripStatusLabelBorderSides.Bottom);
        bool value5 = RadiusSides.GetValue(UICornerRadiusSides.LeftTop);
        bool value6 = RadiusSides.GetValue(UICornerRadiusSides.LeftBottom);
        bool value7 = RadiusSides.GetValue(UICornerRadiusSides.RightTop);
        bool value8 = RadiusSides.GetValue(UICornerRadiusSides.RightBottom);
        if (RadiusSides > UICornerRadiusSides.None && Radius > 0)
        {
            if (!value && !value6 && !value5)
            {
                g.DrawLine(GetFillColor(), RectSize - 1, 0, RectSize - 1, base.Height, smooth: false, RectSize);
            }

            if (!value2 && !value7 && !value5)
            {
                g.DrawLine(GetFillColor(), 0, RectSize - 1, base.Width, RectSize - 1, smooth: false, RectSize);
            }

            if (!value3 && !value7 && !value8)
            {
                g.DrawLine(GetFillColor(), base.Width - 1, 0, base.Width - 1, base.Height, smooth: false, RectSize);
            }

            if (!value4 && !value6 && !value8)
            {
                g.DrawLine(GetFillColor(), 0, base.Height - 1, base.Width, base.Height - 1, smooth: false, RectSize);
            }
        }
    }


    private void OnPaintRect(Graphics g, GraphicsPath path)
    {
        radius = Math.Min(radius, Math.Min(base.Width, base.Height));
        if (RadiusSides == UICornerRadiusSides.None || Radius == 0)
        {
            bool value = RectSides.GetValue(ToolStripStatusLabelBorderSides.Left);
            bool value2 = RectSides.GetValue(ToolStripStatusLabelBorderSides.Top);
            bool value3 = RectSides.GetValue(ToolStripStatusLabelBorderSides.Right);
            bool value4 = RectSides.GetValue(ToolStripStatusLabelBorderSides.Bottom);
            if (value)
            {
                g.DrawLine(GetRectColor(), RectSize - 1, 0, RectSize - 1, base.Height, smooth: false, RectSize);
            }

            if (value2)
            {
                g.DrawLine(GetRectColor(), 0, RectSize - 1, base.Width, RectSize - 1, smooth: false, RectSize);
            }

            if (value3)
            {
                g.DrawLine(GetRectColor(), base.Width - 1, 0, base.Width - 1, base.Height, smooth: false, RectSize);
            }

            if (value4)
            {
                g.DrawLine(GetRectColor(), 0, base.Height - 1, base.Width, base.Height - 1, smooth: false, RectSize);
            }
        }
        else
        {
            g.DrawPath(GetRectColor(), path, smooth: true, RectSize);
            PaintRectDisableSides(g);
        }
    }
    private void OnPaintFill(Graphics g, GraphicsPath path)
    {
        Color color = (Active ? ActiveColor : InActiveColor);
        if (!base.Enabled)
        {
            color = rectDisableColor;
        }

        if (SwitchShape == UISwitchShape.Round)
        {
            Rectangle rect = new Rectangle(0, 0, base.Width - 1, base.Height - 1);
            using GraphicsPath path2 = rect.CreateTrueRoundedRectanglePath(rect.Height);
            g.FillPath(color, path2);
            int width = base.Width - 3 - 1 - 3 - (rect.Height - 6);
            if (!Active)
            {
                g.FillEllipse(fillColor.IsValid() ? fillColor : Color.White, 3, 3, rect.Height - 6, rect.Height - 6);
                g.DrawString(InActiveText, Font, fillColor.IsValid() ? fillColor : Color.White, new Rectangle(3 + rect.Height - 6, 0, width, rect.Height), ContentAlignment.MiddleCenter);
            }
            else
            {
                g.FillEllipse(fillColor.IsValid() ? fillColor : Color.White, base.Width - 3 - 1 - (rect.Height - 6), 3, rect.Height - 6, rect.Height - 6);
                g.DrawString(ActiveText, Font, fillColor.IsValid() ? fillColor : Color.White, new Rectangle(3, 0, width, rect.Height), ContentAlignment.MiddleCenter);
            }
        }

        if (SwitchShape == UISwitchShape.Square)
        {
            Rectangle rect2 = new Rectangle(0, 0, base.Width - 1, base.Height - 1);
            g.FillRoundRectangle(color, rect2, Radius);
            int width2 = base.Width - 3 - 1 - 3 - (rect2.Height - 6);
            if (!Active)
            {
                g.FillRoundRectangle(fillColor.IsValid() ? fillColor : Color.White, 3, 3, rect2.Height - 6, rect2.Height - 6, Radius);
                g.DrawString(InActiveText, Font, fillColor.IsValid() ? fillColor : Color.White, new Rectangle(3 + rect2.Height - 6, 0, width2, rect2.Height), ContentAlignment.MiddleCenter);
            }
            else
            {
                g.FillRoundRectangle(fillColor.IsValid() ? fillColor : Color.White, base.Width - 3 - 1 - (rect2.Height - 6), 3, rect2.Height - 6, rect2.Height - 6, Radius);
                g.DrawString(ActiveText, Font, fillColor.IsValid() ? fillColor : Color.White, new Rectangle(3, 0, width2, rect2.Height), ContentAlignment.MiddleCenter);
            }
        }


    }

    private void OnPaintFore(Graphics g, GraphicsPath path)
    {
        Rectangle rect = new Rectangle(base.Padding.Left, base.Padding.Top, base.Width - base.Padding.Left - base.Padding.Right, base.Height - base.Padding.Top - base.Padding.Bottom);
        g.DrawString(Text, Font, GetForeColor(), rect, TextAlign);
    }


    protected void SetStyleFlags(bool supportTransparent = true, bool selectable = true, bool resizeRedraw = false)
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
        SetStyle(ControlStyles.DoubleBuffer, value: true);
        SetStyle(ControlStyles.UserPaint, value: true);
        SetStyle(ControlStyles.OptimizedDoubleBuffer, value: true);
        if (supportTransparent)
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, value: true);
        }

        if (selectable)
        {
            SetStyle(ControlStyles.Selectable, value: true);
        }

        if (resizeRedraw)
        {
            SetStyle(ControlStyles.ResizeRedraw, value: true);
        }

        base.DoubleBuffered = true;
        UpdateStyles();
    }

    private void ActiveChange()
    {
        CancelEventArgs cancelEventArgs = new CancelEventArgs();
        if (this.ActiveChanging != null)
        {
            this.ActiveChanging?.Invoke(this, cancelEventArgs);
        }

        if (!cancelEventArgs.Cancel)
        {
            Active = !Active;
        }
    }
    #endregion 

    public UISwitch()
    {
        SetStyleFlags();
        base.Height = 29;
        base.Width = 75;
        ShowText = false;
        ShowRect = false;
        inActiveColor = Color.Gray;
        fillColor = Color.White;
        rectColor = Color.Lime;
        fillColor = Color.White;
        rectDisableColor = Color.FromArgb(173, 178, 181);
    }

    protected override void OnClick(EventArgs e)
    {
        ActiveChange();
        base.OnClick(e);
    }

    protected override void OnDoubleClick(EventArgs e)
    {
        if (!UseDoubleClick)
        {
            ActiveChange();
            base.OnClick(e);
        }
        else
        {
            base.OnDoubleClick(e);
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (!base.Visible || base.Width <= 0 || base.Height <= 0 || base.IsDisposed)
        {
            return;
        }

        using GraphicsPath path = new Rectangle(0, 0, base.Width - 1, base.Height - 1).CreateRoundedRectanglePath(radius, RadiusSides, RectSize);
        if (ShowFill && fillColor.IsValid())
        {
            OnPaintFill(e.Graphics, path);
        }

        if (ShowRect)
        {
            OnPaintRect(e.Graphics, path);
        }

        if (ShowText)
        {
            OnPaintFore(e.Graphics, path);
        }
        base.OnPaint(e);
    }

}
