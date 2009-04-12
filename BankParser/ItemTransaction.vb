Imports System.Xml.XPath

Public Class ItemTransaction
    Inherits BankTransaction

    Public Sub New(ByVal nav As XPathNavigator)
        MyBase.New(nav)
        Dim item As XPathNavigator = nav.SelectSingleNode("item")
        Me.m_name = item.SelectSingleNode("@name").Value
        Me.m_itemId = item.SelectSingleNode("@id").ValueAsInt
        Me.m_count = item.SelectSingleNode("@count").ValueAsInt

        Select Case Me.TransactionType
            Case BankParser.TransactionType.General
                If Me.RawType = 1 Then
                    'deposit
                ElseIf Me.RawType = 2 Then
                    Me.m_count = Me.m_count * -1
                Else
                    Throw New Exception("Weird stuff if going on.")
                End If
            Case BankParser.TransactionType.Move
                Me.m_count = 0
            Case Else
                Throw New Exception("Weird stuff is going on.")
        End Select
    End Sub

    Private m_name As String
    Public ReadOnly Property Name() As String
        Get
            Return Me.m_name
        End Get
    End Property

    Private m_itemId As Integer
    Public ReadOnly Property ItemId() As Integer
        Get
            Return Me.m_itemId
        End Get
    End Property

    Private m_count As Integer
    ''' <summary>
    ''' The number of items, positive if deposited, negitive if withdrawn.
    ''' </summary>
    Public ReadOnly Property Count() As Integer
        Get
            Return Me.m_count
        End Get
    End Property
End Class
