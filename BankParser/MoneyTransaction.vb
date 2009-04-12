Imports System.Xml.XPath

Public Class MoneyTransaction
    Inherits BankTransaction

    Public Sub New(ByVal nav As XPathNavigator)
        MyBase.New(nav)
        Dim money As Integer = nav.SelectSingleNode("@money").ValueAsInt

        Select Case Me.TransactionType
            Case BankParser.TransactionType.General
                If Me.RawType = 4 Then
                    Me.m_amount = money
                ElseIf Me.RawType = 5 Then
                    Me.m_amount = -1 * money
                Else
                    Throw New Exception("Wired money things.")
                End If
            Case BankParser.TransactionType.ForTab
                Me.m_amount = -1 * money
            Case BankParser.TransactionType.Repairs
                Me.m_amount = -1 * money
            Case BankParser.TransactionType.BuyingTab
                'money has already been taken out
                Me.m_amount = 0
        End Select
    End Sub

    Private m_amount As Integer
    ''' <summary>
    ''' The amount and direction of copper in regard to the guild bank.
    ''' A negative value indicates withdrawl, postive indicates deposit.
    ''' </summary>
    Public ReadOnly Property Amount() As Integer
        Get
            Return Me.m_amount
        End Get
    End Property
End Class
