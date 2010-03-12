Module Program

    Private ReadOnly dataLocation As String = "D:\bankdata\"

    Sub Main()
        Console.WriteLine("Please wait while bank data is loaded from '{0}'.", dataLocation)
        Dim alts As New AltMap(dataLocation + "alts.txt")
        Dim bank As New BankHistory(alts, dataLocation, True)

        Console.WriteLine("Bank data contains data ranging from {0} to {1}", bank.GetFirstTransactionDate(), bank.GetLastestTransactionDate())

        helpMessage()
        Console.WriteLine("Please push a key to select an option:")

        While True
            Dim gotAKey As Boolean = False
            Select Case Console.ReadKey().Key
                Case ConsoleKey.Q
                    Return
                Case ConsoleKey.H
                    gotAKey = True
                    Console.WriteLine()
                    helpMessage()
                Case ConsoleKey.M
                    gotAKey = True
                    Console.WriteLine()
                    Dim formatted = From kvp In bank.GetNetMoneyFlow() Select New KeyValuePair(Of String, String)(kvp.Key, formatMoney(kvp.Value))
                    writeKvps(formatted)
                Case ConsoleKey.P
                    gotAKey = True
                    Console.WriteLine()
                    Console.WriteLine("Motes and primals are normalized into motes.  Earths are not counted.")
                    writeKvps(bank.GetNetMoteFlow())
                Case ConsoleKey.E
                    gotAKey = True
                    Console.WriteLine()
                    Console.WriteLine("Crystalized's and eternals are normalized into crystalized's.  Earths are not counted.")
                    writeKvps(bank.GetNetCrystalizedFlow())
                Case ConsoleKey.I
                    gotAKey = True
                    Console.WriteLine()
                    Console.Write("Please enter the number of the item (can be found in the wowhead url): ")
                    Dim id As Integer
                    While Not Integer.TryParse(Console.ReadLine(), id)
                        Console.WriteLine("Invalid input.  Please make sure you are typing just the number: ")
                    End While
                    writeKvps(bank.GetNetItemFlow(id))

            End Select
            If gotAKey Then
                Console.WriteLine()
                Console.Write("Please push a key to select an option: ")
            End If
        End While
    End Sub

    Private Sub helpMessage()
        Console.WriteLine(vbTab & "q to quit.")
        Console.WriteLine(vbTab & "h for this help message again.")
        Console.WriteLine(vbTab & "m for net money flow.")
        Console.WriteLine(vbTab & "p for net primal/mote flow.")
        Console.WriteLine(vbTab & "e for net eternal/crystalized flow.")
        Console.WriteLine(vbTab & "i to get the net flow of a specific item.")
    End Sub

    Private Sub writeKvps(Of K, V)(ByVal kvps As IEnumerable(Of KeyValuePair(Of K, V)))
        Dim longest As Integer = 0
        For Each kvp In kvps
            Dim l = kvp.Key.ToString().Length
            If l > longest Then
                longest = l
            End If
        Next
        For Each kvp In kvps
            Dim key = kvp.Key.ToString()
            Console.Write(key)
            For i = 0 To longest - key.Length
                Console.Write(" ")
            Next
            Console.WriteLine(kvp.Value)
        Next
    End Sub

    Private Function formatMoney(ByVal raw As Integer) As String
        Dim isNeg As Boolean = (raw < 0)
        raw = Math.Abs(raw)
        Dim copper As Integer = raw Mod 100
        raw = Math.Floor(raw / 100.0F)
        Dim silver As Integer = raw Mod 100
        raw = Math.Floor(raw / 100.0F)
        Dim gold As Integer = raw
        Return String.Format("{0}{1}.{2}.{3}", IIf(isNeg, "-"c, String.Empty), gold, silver, copper)
    End Function

End Module
