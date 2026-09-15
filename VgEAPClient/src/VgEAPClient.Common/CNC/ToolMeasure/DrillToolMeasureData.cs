// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.CNC.ToolMeasurement;

public class DrillToolMeasureData : NotifyPropertyChangedBase
{
    private ToleranceValue _diameterTol = new();
    private ToleranceValue _lengthTol = new();
    private ToleranceValue _runoutTol = new();

    public ToleranceValue DiameterTol
    {
        get => _diameterTol;
        set
        {
            _diameterTol = value;
            OnPropertyChanged();
        }
    }

    public ToleranceValue LengthTol
    {
        get => _lengthTol;
        set
        {
            _lengthTol = value;
            OnPropertyChanged();
        }
    }

    public ToleranceValue RunoutTol
    {
        get => _runoutTol;
        set
        {
            _runoutTol = value;
            OnPropertyChanged();
        }
    }
}

public class ToleranceValue : NotifyPropertyChangedBase
{
    private string _checked = string.Empty;
    private string _negValue = string.Empty;
    private string _posValue = string.Empty;

    public string Checked
    {
        get => _checked;
        set
        {
            _checked = value;
            OnPropertyChanged();
        }
    }

    public string NegValue
    {
        get => _negValue;
        set
        {
            _negValue = value;
            OnPropertyChanged();
        }
    }

    public string PosValue
    {
        get => _posValue;
        set
        {
            _posValue = value;
            OnPropertyChanged();
        }
    }
}
