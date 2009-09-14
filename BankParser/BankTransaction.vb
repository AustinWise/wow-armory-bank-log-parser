Imports System.Xml.XPath

Public Class BankTransaction

    Public Sub New(ByVal nav As XPathNavigator)
        Me.m_occured = FromUnixTime(nav.SelectSingleNode("@ts").ValueAsLong)

        'if a character trasnfers off the realm, their transactions will become unknown
        Dim unknownAttr = nav.SelectSingleNode("@unknown")
        If unknownAttr IsNot Nothing AndAlso unknownAttr.ValueAsInt = 1 Then
            Me.m_character = "~~Unknown~~"
        Else
            Me.m_character = nav.SelectSingleNode("@player").Value
        End If

        Me.m_rawType = nav.SelectSingleNode("@type").ValueAsInt

        Select Case RawType
            Case 1
                Me.m_transactionType = TransactionType.General
            Case 2
                Me.m_transactionType = TransactionType.General
            Case 3
                Me.m_transactionType = TransactionType.Move
            Case 4
                Me.m_transactionType = TransactionType.General
            Case 5
                Me.m_transactionType = TransactionType.General
            Case 6
                Me.m_transactionType = TransactionType.Repairs
            Case 7
                Me.m_transactionType = TransactionType.Move
            Case 8
                Me.m_transactionType = TransactionType.ForTab
            Case 9
                Me.m_transactionType = TransactionType.BuyingTab
        End Select
    End Sub

    Private m_occured As DateTime
    Public ReadOnly Property Occured() As DateTime
        Get
            Return Me.m_occured
        End Get
    End Property

    Private m_character As String
    Public ReadOnly Property Character() As String
        Get
            Return Me.m_character
        End Get
    End Property

    Private m_transactionType As TransactionType = TransactionType.Unknown
    Public ReadOnly Property TransactionType() As TransactionType
        Get
            Return Me.m_transactionType
        End Get
    End Property

    Private m_rawType As Integer
    Protected ReadOnly Property RawType() As Integer
        Get
            Return Me.m_rawType
        End Get
    End Property


    Private Shared ReadOnly unixTimeEpoc As New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    Private Shared Function FromUnixTime(ByVal ts As Long) As DateTime
        Return unixTimeEpoc.AddMilliseconds(ts).ToLocalTime()
    End Function

End Class
