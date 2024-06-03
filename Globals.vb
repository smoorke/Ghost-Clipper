Module Globals

    Public cursorShow As Boolean

    Public Sub pointer(show As Boolean)
        ShowCursor(show) 'this only works because of GWL_HWNDPARENT
        cursorShow = show
    End Sub

End Module
