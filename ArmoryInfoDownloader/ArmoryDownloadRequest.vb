Imports Austin.Net

Public Class ArmoryDownloadRequest
    Inherits DownloadRequest

    Public Sub New(ByVal address As Uri)
        MyBase.New(address)
        AddHeader("Accept", "application/xml")
    End Sub

    Public Overrides ReadOnly Property UserAgent() As String
        Get
            Return "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.12) Gecko/20101026 Firefox/3.6.12"
        End Get
    End Property
End Class
