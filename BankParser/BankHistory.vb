Imports System.IO
Imports System.Xml.XPath

Public Class BankHistory


    Private ReadOnly motes As Integer() = {22572, 22573, 22574, 22575, 22576, 22577, 22578}
    Private ReadOnly primals As Integer() = {22451, 22452, 21884, 21886, 22457, 22456, 21885}

    Private ReadOnly crystalizeds As Integer() = {37705, 37703, 37704, 37702, 37701, 37700}
    Private ReadOnly eternals As Integer() = {35622, 35627, 35625, 36860, 35624, 35623}

    Private m_money As IList(Of MoneyTransaction) = New List(Of MoneyTransaction)()
    Private m_items As IList(Of ItemTransaction) = New List(Of ItemTransaction)()


    Public Sub New(ByVal dataLocation As String)
        Dim di As New DirectoryInfo(dataLocation)
        For Each f As FileInfo In di.GetFiles("*.xml")
            parseFile(f.FullName)
        Next
    End Sub

    Private Sub parseFile(ByVal f As String)
        Dim fs As New FileStream(f, FileMode.Open, FileAccess.Read, FileShare.Read)
        Dim doc As New XPathDocument(fs)

        Dim nav As XPathNavigator = doc.CreateNavigator()
        For Each entry As XPathNavigator In nav.Select("/page/guildBank/banklogs/banklog")
            Dim money As Integer = entry.SelectSingleNode("@money").ValueAsInt
            If money = 0 Then
                Dim tran As New ItemTransaction(entry)
                If tran.TransactionType = TransactionType.General Then
                    Dim q = From i In m_items Where i.Occured = tran.Occured And i.ItemId = tran.ItemId And i.Count = tran.Count And i.Character = tran.Character Select i
                    If q.Count() = 0 Then
                        m_items.Add(tran)
                    End If
                End If
            Else
                Dim tran As New MoneyTransaction(entry)
                If (tran.TransactionType = TransactionType.General) Or tran.TransactionType = TransactionType.Repairs Then
                    m_money.Add(tran)
                End If
            End If
        Next

        fs.Dispose()
    End Sub

    Public Function GetNetMoneyFlow() As IDictionary(Of String, Integer)
        Dim data As New Dictionary(Of String, Integer)()
        For Each m As MoneyTransaction In m_money
            If data.ContainsKey(m.Character) Then
                data(m.Character) = data(m.Character) + m.Amount
            Else
                data(m.Character) = m.Amount
            End If
        Next
        Return data
    End Function

    Public Function GetNetMoteFlow() As IDictionary(Of String, Integer)
        Dim moteTransactions As New Dictionary(Of String, Integer)
        For Each i As ItemTransaction In Me.m_items
            If i.Name.Contains("Earth") Then
                Continue For
            End If
            If motes.Contains(i.ItemId) Then
                If moteTransactions.ContainsKey(i.Character) Then
                    moteTransactions(i.Character) = moteTransactions(i.Character) + i.Count
                Else
                    moteTransactions(i.Character) = i.Count
                End If
            End If

            If primals.Contains(i.ItemId) Then
                If moteTransactions.ContainsKey(i.Character) Then
                    moteTransactions(i.Character) = moteTransactions(i.Character) + (i.Count * 10)
                Else
                    moteTransactions(i.Character) = (i.Count * 10)
                End If
            End If
        Next
        Return moteTransactions
    End Function

    Public Function GetNetCrystalizedFlow() As IDictionary(Of String, Integer)
        Dim crystalTransactions As New Dictionary(Of String, Integer)
        For Each i As ItemTransaction In Me.m_items
            If i.Name.Contains("Earth") Then
                Continue For
            End If
            If crystalizeds.Contains(i.ItemId) Then
                If crystalTransactions.ContainsKey(i.Character) Then
                    crystalTransactions(i.Character) = crystalTransactions(i.Character) + i.Count
                Else
                    crystalTransactions(i.Character) = i.Count
                End If
            End If

            If eternals.Contains(i.ItemId) Then
                If crystalTransactions.ContainsKey(i.Character) Then
                    crystalTransactions(i.Character) = crystalTransactions(i.Character) + (i.Count * 10)
                Else
                    crystalTransactions(i.Character) = (i.Count * 10)
                End If
            End If
        Next
        Return crystalTransactions
    End Function

    Public Function GetNetItemFlow(ByVal itemId As Integer) As IDictionary(Of String, Integer)
        Dim itemTransactions As New Dictionary(Of String, Integer)
        For Each i As ItemTransaction In Me.m_items
            If i.ItemId = itemId Then
                If itemTransactions.ContainsKey(i.Character) Then
                    itemTransactions(i.Character) = itemTransactions(i.Character) + i.Count
                Else
                    itemTransactions(i.Character) = i.Count
                End If
            End If
        Next
        Return itemTransactions
    End Function

    Public Function GetNetConvertibleItemFlow(ByVal bigItem As Integer, ByVal smallItem As Integer, ByVal smallPerBig As Integer) As IDictionary(Of String, Integer)
        Dim itemTransactions As New Dictionary(Of String, Integer)
        For Each i As ItemTransaction In Me.m_items
            If i.ItemId = smallItem Then
                If itemTransactions.ContainsKey(i.Character) Then
                    itemTransactions(i.Character) = itemTransactions(i.Character) + i.Count
                Else
                    itemTransactions(i.Character) = i.Count
                End If
            End If

            If i.ItemId = bigItem Then
                If itemTransactions.ContainsKey(i.Character) Then
                    itemTransactions(i.Character) = itemTransactions(i.Character) + (i.Count * smallPerBig)
                Else
                    itemTransactions(i.Character) = (i.Count * 10)
                End If
            End If
        Next
        Return itemTransactions
    End Function

    Public Function GetLastestTransactionDate() As DateTime
        Dim d As Date = Date.MinValue
        For Each t In Me.m_items
            If t.Occured > d Then
                d = t.Occured
            End If
        Next
        Return d
    End Function

    Public Function GetFirstTransactionDate() As DateTime
        Dim d As Date = Date.MaxValue
        For Each t In Me.m_items
            If t.Occured < d Then
                d = t.Occured
            End If
        Next
        Return d
    End Function

End Class
