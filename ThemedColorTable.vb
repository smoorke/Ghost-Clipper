Public Class ThemedColorTable : Inherits ProfessionalColorTable

    Public Shared darkMode As Boolean = False

    Public Overrides ReadOnly Property ToolStripBorder As Color
        Get
            Return If(darkMode, Color.Gray, MyBase.ToolStripBorder)
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripDropDownBackground As Color
        Get
            Return If(darkMode, Color.Black, MyBase.ToolStripDropDownBackground)
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripGradientBegin As Color
        Get
            Return If(darkMode, Color.Black, MyBase.ToolStripGradientBegin)
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripGradientEnd As Color
        Get
            Return If(darkMode, Color.Black, MyBase.ToolStripGradientEnd)
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripGradientMiddle As Color
        Get
            Return If(darkMode, Color.Black, MyBase.ToolStripGradientMiddle)
        End Get
    End Property

    Public Overrides ReadOnly Property ImageMarginGradientBegin As Color
        Get
            Return If(darkMode, Color.Black, MyBase.ImageMarginGradientBegin)
        End Get
    End Property
    Public Overrides ReadOnly Property ImageMarginGradientMiddle As Color
        Get
            Return If(darkMode, Color.Black, MyBase.ImageMarginGradientMiddle)
        End Get
    End Property

    Public Overrides ReadOnly Property ImageMarginGradientEnd As Color
        Get
            Return If(darkMode, Color.Black, MyBase.ImageMarginGradientEnd)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemSelected As Color
        Get
            Return If(darkMode, Color.DarkGray, MyBase.MenuItemSelected)
        End Get
    End Property

End Class
