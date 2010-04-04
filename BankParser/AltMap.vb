Imports System.IO

Public Class AltMap

    Private theMap As New Dictionary(Of String, String)

    ''' <summary>
    ''' Creates a new instance of the AltMap class.
    ''' </summary>
    ''' <param name="altFile">The path to a file of alts.  If it does not exist, there will be no mapping of alts.</param>
    ''' <remarks>
    ''' The format of the file is one person per line.  On each line
    ''' there is the name of the person followed by a comma, followed
    ''' by a comma separated list of character names.
    ''' </remarks>
    Public Sub New(ByVal altFile As String)
        If Not File.Exists(altFile) Then
            Return
        End If
        For Each line In File.ReadAllLines(altFile)
            Dim split As String() = line.Split(","c)
            For i As Integer = 1 To split.Length - 1
                theMap.Add(split(i), split(0))
            Next
        Next
    End Sub

    Public Function MapName(ByVal charName As String)
        If (theMap.ContainsKey(charName)) Then
            Return theMap(charName)
        End If
        Return charName
    End Function
End Class
