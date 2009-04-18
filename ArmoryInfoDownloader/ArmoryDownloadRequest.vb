Imports Austin.Net

Public Class ArmoryDownloadRequest
    Inherits DownloadRequest

    Public Sub New(ByVal address As Uri)
        MyBase.New(address)
        AddHeader("Accept", "application/xml")
    End Sub

    Public Overrides ReadOnly Property UserAgent() As String
        Get
            Return "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.0.8) Gecko/2009032609 Firefox/3.0.8"
        End Get
    End Property
End Class
